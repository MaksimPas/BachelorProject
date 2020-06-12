using BachelorProject.CustomAttributes;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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
                Phone = user.PhoneNumber,
                FirstName = user.FirstName,
                LastName = user.LastName,
                DateOfRecord = user.DateOfRecord,
                Role = (await roleManager.FindByIdAsync(userRoleId)).Name
            };

            return createdUserViewModel;
        }

        public static UserViewModel Create(ApplicationUser user, RoleManager<IdentityRole> roleManager)
        {
            var userRoleId = user.Roles.FirstOrDefault().RoleId;
            var createdUserViewModel = new UserViewModel
            {
                UserId = user.Id,
                RoleId = userRoleId,
                Email = user.Email,
                Phone = user.PhoneNumber,
                FirstName = user.FirstName,
                LastName = user.LastName,
                DateOfRecord = user.DateOfRecord,
                Role = roleManager.FindById(userRoleId).Name
            };

            return createdUserViewModel;
        }

        public static async Task<List<UserViewModel>> CreateFromQueryAsListAsync(IEnumerable<ApplicationUser> userList, RoleManager<IdentityRole> roleManager)
        {
            var viewModelList = new List<UserViewModel>();
            foreach (var user in userList)
            {
                viewModelList.Add(await CreateAsync(user, roleManager));
            }

            return viewModelList;
        }

        public string UserId { get; set; }
        public string RoleId { get; set; }

        [Required]
        [Name(ErrorMessage = "Fornavnet kan inneholde kun bokstaver")]
        public string FirstName { get; set; }

        [Required]
        [Name(ErrorMessage = "Etternavnet kan inneholde kun bokstaver")]
        public string LastName { get; set; }

        [Required]
        [EmailAddress(ErrorMessage = "E-post har ugyldig format")]
        public string Email { get; set; }

        [Required]
        [NorwegianPhoneNumber(ErrorMessage = "Telefon har ugyldig format")]
        [Display(Name = "Telefon")]
        public string Phone { get; set; }
        public string Role { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime DateOfRecord { get; set; }

    }
}