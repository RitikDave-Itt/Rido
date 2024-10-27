using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rido.Common.Models.Responses
{
    public class VehicleDto
    {
        public double Price { get; set; }
        public string? EstimatedTime { get; set; }

        public VehicleDto() { }
        public VehicleDto(int price, string estimatedTime = null)
        {
            Price = price;
            EstimatedTime = estimatedTime;
        }
    }

}
