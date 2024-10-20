using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rido.Data.DataTypes
{
    public class AddressType
    {
        public string? Name { get; set; }        
        public string? HouseNumber { get; set; }     
        public string? Road { get; set; }     
        public string? Neighbourhood { get; set; }     
        public string? Suburb { get; set; }   
        public string? Island { get; set; }   
        public string? City { get; set; }        
        public string? County { get; set; }  
        public string? State { get; set; }      
        public string? StateCode { get; set; }     
        public string? Postcode { get; set; }    
        public string? Country { get; set; }   
        public string? Country_code { get; set; }         

        public AddressType(string? name, string? houseNumber, string? road, string? neighbourhood, string? suburb,
                          string? island, string? city, string? county, string? state, string? stateCode,
                          string? postcode, string? country, string? country_code)
        {
            Name = name;
            HouseNumber = houseNumber;
            Road = road;
            Neighbourhood = neighbourhood;
            Suburb = suburb;
            Island = island;
            City = city;
            County = county;
            State = state;
            StateCode = stateCode;
            Postcode = postcode;
            Country = country;
            Country_code = country_code;
        }
        public AddressType() { }
    }

}
