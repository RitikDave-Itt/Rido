using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Options;
using Newtonsoft.Json.Linq;
using Rido.Common.Models.Requests;
using Rido.Common.Models.Responses;
using Rido.Common.Models.Types;
using Rido.Common.Secrets;


namespace Rido.Services
{
    public  class LocationUtils 
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly string _key ;


        public LocationUtils(IHttpClientFactory httpClientFactory,IOptions<LocationSecrets> locationSecrets)
        {
            _httpClientFactory = httpClientFactory;
            _key = locationSecrets.Value.ApiKey;
        }

        public async Task<List<NearbyLocation>> GetNearbyLocationsAsync(GetNearbyLocationRequestDto requestDto)
        {
            var client = _httpClientFactory.CreateClient("location");


            string requestUri = $"autocomplete?key={_key}&q={Uri.EscapeDataString(requestDto.Destination)},{requestDto.City},{requestDto.County},{requestDto.State},{requestDto.Postcode},{requestDto.County},{requestDto.Country_code}";

            HttpResponseMessage response = await client.GetAsync(requestUri);
            response.EnsureSuccessStatusCode();

            string jsonResponse = await response.Content.ReadAsStringAsync();
            JArray nearbyLocations = JArray.Parse(jsonResponse);

            List<NearbyLocation> locations = nearbyLocations
                .Select(location => new NearbyLocation
                {
                    Name = location["display_name"]?.ToString(),
                    Latitude = location["lat"]?.ToString(),
                    Longitude = location["lon"]?.ToString(),
                    Type = location["type"]?.ToString()
                })
                .ToList();

            return locations;
        }

        public async Task<ReverseGeocodeResponseDto> GetAddressFromCoordinatesAsync(LocationType location)
        {
            var client = _httpClientFactory.CreateClient("location");

            string requestUri = $"reverse?key={_key}&lat={location.Latitude}&lon={location.Longitude}&format=json";

            HttpResponseMessage response = await client.GetAsync(requestUri);
            response.EnsureSuccessStatusCode();

            string jsonResponse = await response.Content.ReadAsStringAsync();
            JObject locationData = JObject.Parse(jsonResponse);

            var displayName = locationData["display_name"];
            var address = locationData["address"];
            var AddressObj = address.ToObject<AddressType>();

            return new ReverseGeocodeResponseDto { DisplayName = locationData["display_name"]?.ToString(), Address = AddressObj };




        }





    }
}
