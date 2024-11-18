using AutoMapper;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using System.Collections.Concurrent;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Rido.Web.Hubs
{
    public class ChatHub : Hub
    {
        private static ConcurrentDictionary<string, string> UserConnections = new ConcurrentDictionary<string, string>();

        // Dependencies
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IMapper _mapper;
        private readonly ILogger<ChatHub> _logger;  // Inject the logger

        // Constructor
        public ChatHub(IServiceProvider serviceProvider, ILogger<ChatHub> logger)
        {
            _httpContextAccessor = serviceProvider.GetRequiredService<IHttpContextAccessor>();
            _mapper = serviceProvider.GetRequiredService<IMapper>();
            _logger = logger;  // Assign logger
        }

        // Helper Methods to Extract User Info
        protected string GetCurrentUserId()
        {
            return _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        }

        protected string GetCurrentUserEmail()
        {
            return _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.Email)?.Value;
        }

        protected string GetCurrentUserRole()
        {
            return _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.Role)?.Value;
        }

        // OnConnectedAsync: Called when a new connection is established
        public override Task OnConnectedAsync()
        {
            string userId = GetCurrentUserId();
            _logger.LogInformation($"User connected with ID: {userId}, ConnectionId: {Context.ConnectionId}");

            if (!string.IsNullOrEmpty(userId))
            {
                bool added = UserConnections.TryAdd(userId, Context.ConnectionId);
                if (added)
                {
                    _logger.LogInformation($"User {userId} added to connections.");
                }
                else
                {
                    _logger.LogWarning($"User {userId} already connected.");
                }
            }

            return base.OnConnectedAsync();
        }

        // OnDisconnectedAsync: Called when a connection is disconnected
        public override Task OnDisconnectedAsync(Exception exception)
        {
            string userId = GetCurrentUserId();
            if (!string.IsNullOrEmpty(userId))
            {
                bool removed = UserConnections.TryRemove(userId, out _);
                if (removed)
                {
                    _logger.LogInformation($"User {userId} disconnected.");
                }
                else
                {
                    _logger.LogWarning($"User {userId} not found in connections.");
                }
            }
            else
            {
                _logger.LogWarning("User ID is null or empty during disconnect.");
            }

            if (exception != null)
            {
                _logger.LogError($"Error during disconnect: {exception.Message}");
            }

            return base.OnDisconnectedAsync(exception);
        }

        // ConnectToDriver: Establish a connection between rider and driver
        public async Task ConnectToDriver(string driverId)
        {
            try
            {
                if (UserConnections.TryGetValue(driverId, out string driverConnectionId))
                {
                    _logger.LogInformation($"Rider {GetCurrentUserId()} is connecting to driver {driverId}.");
                    await Clients.Client(driverConnectionId).SendAsync("ReceiveMessage", "Rider wants to chat with you.");
                }
                else
                {
                    _logger.LogWarning($"Driver {driverId} not available for connection.");
                    await Clients.Caller.SendAsync("ErrorMessage", "Driver not available.");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in ConnectToDriver: {ex.Message}");
                await Clients.Caller.SendAsync("ErrorMessage", "An error occurred while connecting to driver.");
            }
        }

        // SendMessageToDriver: Send message from rider to driver
        public async Task SendMessageToDriver(string driverId, string message)
        {
            try
            {
                if (UserConnections.TryGetValue(driverId, out string driverConnectionId))
                {
                    _logger.LogInformation($"Sending message to driver {driverId}: {message}");
                    await Clients.Client(driverConnectionId).SendAsync("ReceiveMessage", message);
                }
                else
                {
                    _logger.LogWarning($"Driver {driverId} not available to receive message.");
                    await Clients.Caller.SendAsync("ErrorMessage", "Driver is currently unavailable.");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in SendMessageToDriver: {ex.Message}");
                await Clients.Caller.SendAsync("ErrorMessage", "An error occurred while sending message to driver.");
            }
        }

        // SendMessageToRider: Send message from driver to rider
        public async Task SendMessageToRider(string riderId, string message)
        {
            try
            {
                if (UserConnections.TryGetValue(riderId, out string riderConnectionId))
                {
                    _logger.LogInformation($"Sending message to rider {riderId}: {message}");
                    await Clients.Client(riderConnectionId).SendAsync("ReceiveMessage", message);
                }
                else
                {
                    _logger.LogWarning($"Rider {riderId} not available to receive message.");
                    await Clients.Caller.SendAsync("ErrorMessage", "Rider is currently unavailable.");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in SendMessageToRider: {ex.Message}");
                await Clients.Caller.SendAsync("ErrorMessage", "An error occurred while sending message to rider.");
            }
        }
    }
}
