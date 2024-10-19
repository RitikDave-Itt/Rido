using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using AutoMapper;

namespace Rido.Web.Controllers
{
    [ApiController]
    public abstract class BaseController<T> : ControllerBase
    {
        protected readonly ILogger<T> _logger;
        protected readonly IHttpContextAccessor _httpContextAccessor;
        protected readonly IMapper _mapper;

        protected BaseController(IServiceProvider serviceProvider)
        {
            _logger = serviceProvider.GetRequiredService<ILogger<T>>();
            _httpContextAccessor = serviceProvider.GetRequiredService<IHttpContextAccessor>();
            _mapper = serviceProvider.GetRequiredService<IMapper>();
        }


        protected string GetCurrentUserId()
        {
            return _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        }
    }
}
