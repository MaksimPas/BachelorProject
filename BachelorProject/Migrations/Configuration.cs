namespace BachelorProject.Migrations
{
    using BachelorProject.Models;
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<BachelorProject.Models.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(BachelorProject.Models.ApplicationDbContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data.

            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));
            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));
            string roleName1 = "new_user";
            string roleName2 = "worker";
            string roleName3 = "admin";
            if (!roleManager.RoleExists(roleName1))
            {
                roleManager.Create(new IdentityRole(roleName1));
            }

            if (!roleManager.RoleExists(roleName2))
            {
                roleManager.Create(new IdentityRole(roleName2));
            }
            if (!roleManager.RoleExists(roleName3))
            {
                roleManager.Create(new IdentityRole(roleName3));
            }

            var adminUser = userManager.FindByEmail("admin@rodekors.com");
            
            if (adminUser == null)
            {
                userManager.Create(
                    new ApplicationUser
                    {
                        Email = "admin@rodekors.com",
                        FirstName = "Micah",
                        LastName = "Lehmann",
                        UserName = "admin@rodekors.com"
                    },
                     "admin123"
                    ) ;
            }

            IdentityResult result;
            result = userManager.AddToRole(userManager.FindByEmail("admin@rodekors.com").Id, roleName3);

            //Seeding equipment
            //NB! Don't need to specify ID upon inserting due to IDENTITY!
            //HOWEVER ID need to be specified upon updating the row. 
            context.Equipments.AddOrUpdate(new Equipment { Id = 1, NameAndType = "Hansker S"}) ;
            context.Equipments.AddOrUpdate(new Equipment { Id = 2, NameAndType = "Hansker M"});
            context.SaveChanges();

            //Seeding some depot records
            context.DepotRecords.AddOrUpdate(new DepotRecord { Id = 1, EquipmentCodeId = 1, ExpirationDate = new DateTime(2020, 3, 14), QuantityOriginal = 50, QuantityLeft = 30 });
            context.DepotRecords.AddOrUpdate(new DepotRecord { Id = 2, EquipmentCodeId = 1, ExpirationDate = new DateTime(2020, 6, 14), QuantityOriginal = 100, QuantityLeft = 30 });
            context.DepotRecords.AddOrUpdate(new DepotRecord { Id = 3, EquipmentCodeId = 2, ExpirationDate = new DateTime(2020, 6, 14), QuantityOriginal = 100, QuantityLeft = 30 });
            context.SaveChanges();

        }
    }

    public static class Operations
    {

    }
}
