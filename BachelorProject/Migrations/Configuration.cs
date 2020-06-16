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
            string roleName4 = "subAdmin";
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
            if (!roleManager.RoleExists(roleName4))
            {
                roleManager.Create(new IdentityRole(roleName4));
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
                        PhoneNumber = "44444444",
                        UserName = "admin@rodekors.com",
                        DateOfRecord = DateTime.Today
                    },
                     "admin123"
                    );
            }

            IdentityResult result = userManager.AddToRole(userManager.FindByEmail("admin@rodekors.com").Id, roleName3);

            //Seeding equipment
            //NB! Don't need to specify ID upon inserting due to IDENTITY!
            //HOWEVER ID need to be specified upon updating the row. 

            context.Equipments.AddOrUpdate(new Equipment { Id = 1, NameAndType = "Bondasje (kompresjon)" });
            context.Equipments.AddOrUpdate(new Equipment { Id = 2, NameAndType = "Sprøyte 10mL" });
            context.Equipments.AddOrUpdate(new Equipment { Id = 3, NameAndType = "Hansker S" });
            context.Equipments.AddOrUpdate(new Equipment { Id = 4, NameAndType = "Hansker M" });
            context.Equipments.AddOrUpdate(new Equipment { Id = 5, NameAndType = "Bondasje B" });
            context.Equipments.AddOrUpdate(new Equipment { Id = 6, NameAndType = "Bondasje A" });

            context.SaveChanges();

            //Seeding some depot records
            context.DepotRecords.AddOrUpdate(new DepotRecord { Id = 1, EquipmentCodeId = 1, DateOfRecord = DateTime.Today, ExpirationDate = new DateTime(2020, 3, 14), QuantityOriginal = 50, QuantityLeft = 30, Information = "tester info" });
            context.DepotRecords.AddOrUpdate(new DepotRecord { Id = 2, EquipmentCodeId = 1, DateOfRecord = DateTime.Today, ExpirationDate = new DateTime(2020, 6, 14), QuantityOriginal = 100, QuantityLeft = 30, Information = "tester info" });
            context.DepotRecords.AddOrUpdate(new DepotRecord { Id = 3, EquipmentCodeId = 2, DateOfRecord = DateTime.Today, ExpirationDate = new DateTime(2020, 6, 14), QuantityOriginal = 100, QuantityLeft = 30, Information = "tester info" });
            context.DepotRecords.AddOrUpdate(new DepotRecord { Id = 4, EquipmentCodeId = 2, DateOfRecord = DateTime.Today, ExpirationDate = null, QuantityOriginal = 100, QuantityLeft = 30, Information = "tester info" });
            context.DepotRecords.AddOrUpdate(new DepotRecord { Id = 5, EquipmentCodeId = 2, DateOfRecord = DateTime.Today, ExpirationDate = null, QuantityOriginal = 100, QuantityLeft = 30, Information = "tester info" });
            context.SaveChanges();

            //seeding some log records
            string userId = userManager.FindByEmail("admin@rodekors.com").Id;
            context.LogRecords.AddOrUpdate(new LogRecord { Id = 1, UserId = userId, DateOfRecord = new DateTime(2020, 3, 13, 14, 14, 14), Action = LogAction.FORBRUK, InfoMessage = "Seeding init test record" });
            context.LogRecords.AddOrUpdate(new LogRecord { Id = 2, UserId = userId, DateOfRecord = new DateTime(2020, 3, 13, 14, 14, 15), Action = LogAction.FORBRUK, InfoMessage = "Seeding init test record" });
            context.LogRecords.AddOrUpdate(new LogRecord { Id = 3, UserId = userId, DateOfRecord = new DateTime(2020, 3, 13, 14, 14, 16), Action = LogAction.FORBRUK, InfoMessage = "Seeding init test record" });
            context.SaveChanges();
        }
    }

    public static class Operations
    {

    }
}
