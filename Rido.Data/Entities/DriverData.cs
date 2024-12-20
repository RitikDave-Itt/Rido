﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using AutoMapper.Configuration.Annotations;
using Rido.Model.Enums;

namespace Rido.Data.Entities
{
    public class DriverData
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string UserId { get; set; }       
        public string LicenseType { get; set; }        
        public string LicenseNumber { get; set; }    

        public VehicleType VehicleType { get; set; }     
        public string VehicleRegistrationNumber { get; set; }      
        public string VehicleModel { get; set; }     
        public string VehicleMake { get; set; }        

        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        public User User { get; set; }

    }
}
