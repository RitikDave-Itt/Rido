using Rido.Common.Models.Types;
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
        private const int SedanRatePerKm = 15;       
        private const int SuvRatePerKm = 18;         
        private const int CoupeRatePerKm = 13;       
        private const int VanRatePerKm = 15;         
        private const int AutoRikshawRatePerKm = 10;        
        private const int MotorcycleRatePerKm = 8;       
        private const int OtherRatePerKm = 15;

        public decimal CalculateDistance(LocationType pickup , LocationType destinition)
        {
            const double EarthRadius = 6371.0; 

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

            return (decimal)(EarthRadius * c);    
        }

        public int CalculateFare(LocationType pickup, LocationType destinition ,VehicleType vehicle)
        {

            var distanceInKm = CalculateDistance(pickup,destinition);

            int ratePerKm = GetRatePerKm(vehicle);
            return (int) distanceInKm * ratePerKm;
        }

        public List<FareType> FareList(LocationType pickup, LocationType destination)
        {
            decimal distance = CalculateDistance(pickup, destination);
            var fareEstimates = new List<FareType>();

            foreach (VehicleType vehicle in Enum.GetValues(typeof(VehicleType)))
            {
                int fare = (int)(distance * GetRatePerKm(vehicle));    
                fareEstimates.Add(new FareType { Vehicle= vehicle.ToString() , FarePrice = fare});
            }

            return fareEstimates;
        }


        public int GetRatePerKm(VehicleType vehicleType)
        {
            return vehicleType switch
            {
                VehicleType.Sedan => SedanRatePerKm,
                VehicleType.SUV => SuvRatePerKm,
                VehicleType.Coupe => CoupeRatePerKm,
                VehicleType.Van => VanRatePerKm,
                VehicleType.AutoRikshaw => AutoRikshawRatePerKm,
                VehicleType.Motorcycle => MotorcycleRatePerKm,
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
