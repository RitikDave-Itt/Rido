using Rido.Common.Models.Requests;
using Rido.Common.Models.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rido.Data.DataTypes;

namespace Rido.Services.Interfaces
{
    public interface ILocationServices
    {
        Task<List<NearbyLocation>> GetNearbyLocationsAsync(GetNearbyLocationRequestDto dto);

        Task<ReverseGeocodeResponseDto> GetAddressFromCoordinatesAsync(LocationType location);
    }
}
