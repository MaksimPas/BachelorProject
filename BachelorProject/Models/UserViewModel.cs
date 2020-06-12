using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace BachelorProject.Models
{
    public class UserViewModel
    {
        public static async Task<UserViewModel> CreateAsync(ApplicationUser user, RoleManager<IdentityRole> roleManager)
        {
            var userRoleId = user.Roles.FirstOrDefault().RoleId;
            var createdUserViewModel = new UserViewModel
            {
                UserId = user.Id,
                RoleId = userRoleId,
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Role = (await roleManager.FindByIdAsync(userRoleId)).Name
            };

            return createdUserViewModel;
        }
        public string UserId { get; set; }
        public string RoleId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Role { get; set; }

    }
}