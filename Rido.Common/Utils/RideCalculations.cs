using Rido.Common.Models.Responses;
using Rido.Data.DataTypes;
using Rido.Data.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rido.Common.Utils
{
    public class RideCalculations
    {
        private const double SedanRatePerKm = 15;       
        private const double SuvRatePerKm = 18;         
        private const double CoupeRatePerKm = 13;       
        private const double VanRatePerKm = 15;         
        private const double AutoRikshawRatePerKm = 10;        
        private const double BikeRatePerKm = 8;       
        private const double OtherRatePerKm = 15;

        public double CalculateDistance(LocationType pickup , LocationType destinition)
        {
            const double EarthRadius = 6371.0f; 

            double lat1 = Convert.ToDouble(pickup.Latitude);
            double lon1 = Convert.ToDouble(pickup.Longitude);

            double lat2 = Convert.ToDouble(destinition.Latitude);

            double lon2 = Convert.ToDouble(destinition.Longitude);

            double dLat = ToRadians(lat2 - lat1);
            double dLon = ToRadians(lon2 - lon1);
            double a =
                Math.Sin(dLat / 2) * Math.Sin(dLat / 2) +
                Math.Cos(ToRadians(lat1)) * Math.Cos(ToRadians(lat2)) *
                Math.Sin(dLon / 2) * Math.Sin(dLon / 2);
            double c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));

            return EarthRadius * (double) c;    
        }

        public double[] CalculateFare(double distanceInKm, VehicleType vehicle)
        {


            double ratePerKm = GetRatePerKm(vehicle);
            return [distanceInKm * ratePerKm , distanceInKm*ratePerKm+15];
        }

        public FareListResponseDto FareList(LocationType pickup, LocationType destination)
        {
            double distance = CalculateDistance(pickup, destination);
            var fareList = new FareListResponseDto();

            fareList.Bike = new VehicleDto()
            {
                Price = distance * BikeRatePerKm,
                
            };
            fareList.AutoRikshaw = new VehicleDto()
            {
                Price = distance * AutoRikshawRatePerKm,

            };
            fareList.Coupe = new VehicleDto()
            {
                Price = distance * CoupeRatePerKm,

            };

            fareList.Sedan = new VehicleDto()
            {
                Price = distance * SedanRatePerKm,

            };
            fareList.SUV = new VehicleDto()
            {
                Price = distance * SuvRatePerKm,

            };
            fareList.Van = new VehicleDto()
            {
                Price = distance * VanRatePerKm,

            };
            fareList.Other = new VehicleDto()
            {
                Price = distance * OtherRatePerKm,

            };


            return fareList;
        }


        public double GetRatePerKm(VehicleType vehicleType)
        {
            return vehicleType switch
            {
                VehicleType.Sedan => SedanRatePerKm,
                VehicleType.SUV => SuvRatePerKm,
                VehicleType.Coupe => CoupeRatePerKm,
                VehicleType.Van => VanRatePerKm,
                VehicleType.AutoRikshaw => AutoRikshawRatePerKm,
                VehicleType.Bike => BikeRatePerKm,
                VehicleType.Other => OtherRatePerKm,
                _ => throw new ArgumentException("Invalid vehicle type.")
            };
        }
        private double ToRadians(double angle)
        {
            return angle * Math.PI / 180.0;
        }
    }
}
