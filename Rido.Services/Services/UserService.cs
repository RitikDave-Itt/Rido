using Microsoft.AspNetCore.Identity;
using Rido.Model.Attributes;
using Rido.Common.Utils;
using Rido.Data.Entities;
using Rido.Model.Enums;
using Rido.Data.Repositories.Interfaces;
using Rido.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rido.Services
{
    public class UserService : BaseService<User> ,IUserService
    {

        public UserService(IServiceProvider serviceProvider):base(serviceProvider) { }



        public async Task<dynamic> GetUser(string id = null)
        {
            var userId = id ?? GetCurrentUserId();

            var user = await _repository.FindAsync(
                u => u.Id == userId,
                u=>u.DriverData            
            );

            if (user == null)
            {
                return null;
            }

          
            
                var response = new
                {
                    user = new
                    {
                        FirstName = user.FirstName,
                        LastName = user.LastName,
                        Email = user.Email,
                        Gender = user.Gender.ToString(),
                        Role = user.Role.ToString(),
                       
                    },
                  
                };

                return response;
            
        }

    }
}
