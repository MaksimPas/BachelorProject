using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using BachelorProject.CustomAttributes;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace BachelorProject.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit https://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime DateOfRecord { get; set; } //not null
        public ICollection<LogRecord> LogRecords { get; set; }
    }

    class ApplicationRole : IdentityRole
    {
        public ApplicationRole() : base()
        {

        }
        public ApplicationRole(string name) : base(name)
        {

        }
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        //specify connection name in web.config here
        public ApplicationDbContext(): base("DefaultConnection", throwIfV1Schema: false)
        {
        }
        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }
        public DbSet<Equipment> Equipments { get; set; }
        public DbSet<DepotRecord> DepotRecords  { get; set; }
        public DbSet<LogRecord> LogRecords { get; set; }
    }
}