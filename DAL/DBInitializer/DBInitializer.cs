using BAL.model;
using DAL.Data;
using DAL.DBInitializer;
using DAL.models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Utility;

namespace DALProject.DBInitializer
{
    public class DBInitializer : IDBInitializer
    {
        private readonly UserManager<AppUser> userManager;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly BookFilghtsDbContext db;

        public DBInitializer(UserManager<AppUser> userManager,
            RoleManager<IdentityRole> roleManager,
            BookFilghtsDbContext db)
        {
            this.userManager = userManager;
            this.roleManager = roleManager;
            this.db = db;
        }

        public async Task Initialize()
        {
            // migration if they are not applied
            try
            {
                if (db.Database.GetPendingMigrations().Count() > 0)
                {
                    db.Database.Migrate();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("DB Initialization error: " + ex.Message);
            }


            if (!db.Airlines.Any())
            {
                db.Airlines.AddRange(new List<Airline>
                {
                    new Airline { Name = "EgyptAir", Code = "EGY" },
                    new Airline { Name = "Emirates", Code = "EMR" },
                    new Airline { Name = "Qatar Airways", Code = "QTR" },
                });
                db.SaveChanges();
            }

           
            if (!db.Airports.Any())
            {
                db.Airports.AddRange(new List<Airport>
                {
                    new Airport { Name = "Cairo International Airport", Code = "CAI" },
                    new Airport { Name = "Dubai International Airport", Code = "DXB" },
                    new Airport { Name = "Doha International Airport", Code = "DOH" },
                    new Airport { Name = "Istanbul Airport", Code = "IST" },
                });
                db.SaveChanges();
            }


            if (!db.Airplanes.Any())
            {
                var egyptAirId = db.Airlines.FirstOrDefault(a => a.Name == "EgyptAir")?.Id ?? 1;

                db.Airplanes.AddRange(new List<Airplane>
    {
        new Airplane { Model = "Boeing 737", SeatCapacity = 180, AirlineId = egyptAirId },
        new Airplane { Model = "Airbus A320", SeatCapacity = 150, AirlineId = egyptAirId },
        new Airplane { Model = "Boeing 777", SeatCapacity = 396, AirlineId = egyptAirId },
        new Airplane { Model = "Airbus A350", SeatCapacity = 300, AirlineId = egyptAirId }
    });
                db.SaveChanges();
            }


            // create roles if they are not applied

            if (!await roleManager.RoleExistsAsync(SD.Customer))
            {
                // Create CurrentRole
                await roleManager.CreateAsync(new IdentityRole(SD.Customer));
                await roleManager.CreateAsync(new IdentityRole(SD.Admin));

                // if roles are created , then we will create admin User as well
                var result = await userManager.CreateAsync(new AppUser
                {
                    UserName = "admin@gmail.com",
                    Email = "admin@gmail.com",
                    FirstName = "admin",
                    PhoneNumber = "1234567890",
                }, "Admin123*");


                if (result.Succeeded)
                {
                    AppUser user = await userManager.FindByEmailAsync("admin@gmail.com");
                    await userManager.AddToRoleAsync(user, SD.Admin);
                }
            }

            return;
        }
    }
}
