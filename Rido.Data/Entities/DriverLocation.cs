﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rido.Data.Enums;

namespace Rido.Data.Entities
{
    public class DriverLocation
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();


        public string DriverId { get; set; }

      
        public string Latitude { get; set; }

        public string Longitude { get; set; }

          
        public string Geohash { get; set; }

        public VehicleType VehicleType { get; set; }
    }
}