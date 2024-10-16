﻿using Rido.Common.Models.Types;
using Rido.Data.DTOs;
using Rido.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rido.Services.Interfaces
{
    public interface  IRideService
    {
        Task<RideRequest> CreateRideRequest(RideRequestDto rideRequestDto);

        Task<List<RideRequest>> GetRideRequestByLocation(LocationType location);

        Task<bool> CancelRideByUser(string rideRequestId);

        Task<bool> CancelRideByDriver(string rideRequestId);

        Task<RideRequest> AssignRideDriver(string rideRequestId);
    }
}