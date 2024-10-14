using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Geohash;
using Microsoft.AspNetCore.Http;
using Rido.Common.Models.Types;
using Rido.Data.Entities;
using Rido.Data.Enums;
using Rido.Data.Repositories.Interfaces;
using Rido.Services.Interfaces;

namespace Rido.Services
{
    public class DriverLocationService : IDriverLocationService
    {
        private IRepository<DriverLocation> _repository;
        private readonly IHttpContextAccessor _httpContextAccessor;


        public DriverLocationService(IRepository<DriverLocation> repository, IHttpContextAccessor httpContextAccessor) {
        _repository = repository;
            _httpContextAccessor = httpContextAccessor;

        }


        public async Task<string> UpdateLocation(LocationType location, VehicleType vehicleType)
        {
            string driverId = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var geohasher = new Geohasher();
            string geohash = geohasher.Encode(Convert.ToDouble(location.Latitude), Convert.ToDouble(location.Longitude), 5);

            var existingLocation = await _repository.FindAsync(dl => dl.DriverId == driverId && dl.VehicleType == vehicleType);



            if (existingLocation != null)
            {
                existingLocation.Latitude = location.Latitude;
                existingLocation.Longitude = location.Longitude;
                existingLocation.Geohash = geohash;

                await _repository.UpdateAsync(existingLocation);
            }
            else
            {
                DriverLocation newDriverLocation = new DriverLocation
                {
                    DriverId = driverId,
                    Latitude = location.Latitude,
                    Longitude = location.Longitude,
                    Geohash = geohash,
                    VehicleType = vehicleType
                };

                await _repository.AddAsync(newDriverLocation);
            }

            return geohash;           
        }


    }
}
