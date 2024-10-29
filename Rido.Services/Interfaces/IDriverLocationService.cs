using Rido.Model.DataTypes;
using Rido.Model.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rido.Services.Interfaces
{
    public interface IDriverLocationService
    {
        Task<string> UpdateLocation(LocationType location,  VehicleType vehicleType);

    }
}
