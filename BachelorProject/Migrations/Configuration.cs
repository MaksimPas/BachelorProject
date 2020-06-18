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

            context.Equipments.AddOrUpdate(new Equipment { Id = 1, NameAndType = "Kompresjonsbandasje 8 cm" });
            context.Equipments.AddOrUpdate(new Equipment { Id = 2, NameAndType = "Kompresjonsbandasje 10 cm" });
            context.Equipments.AddOrUpdate(new Equipment { Id = 3, NameAndType = "Kompresjonsbandasje 12 cm" });
            context.Equipments.AddOrUpdate(new Equipment { Id = 4, NameAndType = "Støttebandasje 8 cm" });
            context.Equipments.AddOrUpdate(new Equipment { Id = 5, NameAndType = "Enkeltmannspakke 7,5 cm" });
            context.Equipments.AddOrUpdate(new Equipment { Id = 6, NameAndType = "Enkeltmannspakke 18 cm" });
            context.Equipments.AddOrUpdate(new Equipment { Id = 7, NameAndType = "Gasbind 8 cm" });
            context.Equipments.AddOrUpdate(new Equipment { Id = 8, NameAndType = "Kompress stor (10x20)" });
            context.Equipments.AddOrUpdate(new Equipment { Id = 9, NameAndType = "Kompress liten (6x5)" });
            context.Equipments.AddOrUpdate(new Equipment { Id = 10, NameAndType = "Kompress høyabsorberende" });
            context.Equipments.AddOrUpdate(new Equipment { Id = 11, NameAndType = "Hansker S" });
            context.Equipments.AddOrUpdate(new Equipment { Id = 12, NameAndType = "Hansker M" });
            context.Equipments.AddOrUpdate(new Equipment { Id = 13, NameAndType = "Hansker L" });
            context.Equipments.AddOrUpdate(new Equipment { Id = 14, NameAndType = "Hansker XL" });
            context.Equipments.AddOrUpdate(new Equipment { Id = 15, NameAndType = "Sprøyte 1mL" });
            context.Equipments.AddOrUpdate(new Equipment { Id = 16, NameAndType = "Sprøyte 5mL" });
            context.Equipments.AddOrUpdate(new Equipment { Id = 17, NameAndType = "Plaster (1stk)" });
            context.Equipments.AddOrUpdate(new Equipment { Id = 18, NameAndType = "Plaster (rull)" });
            context.Equipments.AddOrUpdate(new Equipment { Id = 19, NameAndType = "Plaster Mepore (8x10)" });
            context.Equipments.AddOrUpdate(new Equipment { Id = 20, NameAndType = "Plaster Gnagsår" });
            context.Equipments.AddOrUpdate(new Equipment { Id = 21, NameAndType = "Plaster Melfix (uten kompress)" });
            context.Equipments.AddOrUpdate(new Equipment { Id = 22, NameAndType = "Teip" });


            context.SaveChanges();

            //Seeding some depot records
            context.DepotRecords.AddOrUpdate(new DepotRecord { Id = 1, EquipmentCodeId = 1, DateOfRecord = DateTime.Today, ExpirationDate = new DateTime(2020, 3, 14), QuantityOriginal = 50, QuantityLeft = 30, Information = "tester info" });
            context.DepotRecords.AddOrUpdate(new DepotRecord { Id = 2, EquipmentCodeId = 1, DateOfRecord = DateTime.Today, ExpirationDate = new DateTime(2020, 6, 14), QuantityOriginal = 100, QuantityLeft = 30, Information = "tester info" });
            context.DepotRecords.AddOrUpdate(new DepotRecord { Id = 3, EquipmentCodeId = 2, DateOfRecord = DateTime.Today, ExpirationDate = new DateTime(2020, 6, 14), QuantityOriginal = 100, QuantityLeft = 30, Information = "tester info" });
            context.DepotRecords.AddOrUpdate(new DepotRecord { Id = 4, EquipmentCodeId = 2, DateOfRecord = DateTime.Today, ExpirationDate = new DateTime(2020, 6, 14), QuantityOriginal = 100, QuantityLeft = 30, Information = "tester info" });
            context.DepotRecords.AddOrUpdate(new DepotRecord { Id = 5, EquipmentCodeId = 3, DateOfRecord = DateTime.Today, ExpirationDate = new DateTime(2020, 6, 14), QuantityOriginal = 100, QuantityLeft = 30, Information = "tester info" });
            context.DepotRecords.AddOrUpdate(new DepotRecord { Id = 6, EquipmentCodeId = 3, DateOfRecord = DateTime.Today, ExpirationDate = new DateTime(2020, 6, 14), QuantityOriginal = 100, QuantityLeft = 30, Information = "tester info" });
            context.DepotRecords.AddOrUpdate(new DepotRecord { Id = 7, EquipmentCodeId = 4, DateOfRecord = DateTime.Today, ExpirationDate = new DateTime(2020, 6, 14), QuantityOriginal = 100, QuantityLeft = 30, Information = "tester info" });
            context.DepotRecords.AddOrUpdate(new DepotRecord { Id = 8, EquipmentCodeId = 22, DateOfRecord = DateTime.Today, ExpirationDate = null, QuantityOriginal = 100, QuantityLeft = 30, Information = "tester info" });
            context.DepotRecords.AddOrUpdate(new DepotRecord { Id = 9, EquipmentCodeId = 22, DateOfRecord = DateTime.Today, ExpirationDate = null, QuantityOriginal = 100, QuantityLeft = 30, Information = "tester info" });
            context.DepotRecords.AddOrUpdate(new DepotRecord { Id = 10, EquipmentCodeId = 6, DateOfRecord = DateTime.Today, ExpirationDate = new DateTime(2020, 6, 14), QuantityOriginal = 100, QuantityLeft = 30, Information = "tester info" });
            context.DepotRecords.AddOrUpdate(new DepotRecord { Id = 11, EquipmentCodeId = 7, DateOfRecord = DateTime.Today, ExpirationDate = new DateTime(2020, 6, 14), QuantityOriginal = 100, QuantityLeft = 30, Information = "tester info" });
            context.DepotRecords.AddOrUpdate(new DepotRecord { Id = 12, EquipmentCodeId = 8, DateOfRecord = DateTime.Today, ExpirationDate = new DateTime(2020, 6, 14), QuantityOriginal = 100, QuantityLeft = 30, Information = "tester info" });
            context.DepotRecords.AddOrUpdate(new DepotRecord { Id = 13, EquipmentCodeId = 9, DateOfRecord = DateTime.Today, ExpirationDate = new DateTime(2020, 6, 14), QuantityOriginal = 100, QuantityLeft = 30, Information = "tester info" });
            context.DepotRecords.AddOrUpdate(new DepotRecord { Id = 14, EquipmentCodeId = 10, DateOfRecord = DateTime.Today, ExpirationDate = new DateTime(2020, 6, 14), QuantityOriginal = 100, QuantityLeft = 30, Information = "tester info" });
            context.DepotRecords.AddOrUpdate(new DepotRecord { Id = 21, EquipmentCodeId = 17, DateOfRecord = DateTime.Today, ExpirationDate = new DateTime(2020, 6, 14), QuantityOriginal = 100, QuantityLeft = 30, Information = "tester info" });
            context.DepotRecords.AddOrUpdate(new DepotRecord { Id = 22, EquipmentCodeId = 18, DateOfRecord = DateTime.Today, ExpirationDate = new DateTime(2020, 6, 14), QuantityOriginal = 100, QuantityLeft = 30, Information = "tester info" });
            context.DepotRecords.AddOrUpdate(new DepotRecord { Id = 23, EquipmentCodeId = 19, DateOfRecord = DateTime.Today, ExpirationDate = new DateTime(2020, 6, 14), QuantityOriginal = 100, QuantityLeft = 30, Information = "tester info" });
            context.DepotRecords.AddOrUpdate(new DepotRecord { Id = 24, EquipmentCodeId = 20, DateOfRecord = DateTime.Today, ExpirationDate = new DateTime(2020, 6, 14), QuantityOriginal = 100, QuantityLeft = 30, Information = "tester info" });
            context.DepotRecords.AddOrUpdate(new DepotRecord { Id = 25, EquipmentCodeId = 21, DateOfRecord = DateTime.Today, ExpirationDate = new DateTime(2020, 6, 14), QuantityOriginal = 100, QuantityLeft = 30, Information = "tester info" });
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
