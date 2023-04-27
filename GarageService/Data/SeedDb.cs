using System;
using System.Linq;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using GarageService.Models;

namespace GarageService.Data;
public static class PrepDb
{
    public static void PrepPopulation(IApplicationBuilder app, bool isProd)
    {
        using (var serviceScope = app.ApplicationServices.CreateScope())
        {
            SeedData(serviceScope.ServiceProvider.GetService<AppDbContext>(), isProd);
        }
    }

    private static void SeedData(AppDbContext context, bool isProd)
    {
        if (isProd)
        {
            Console.WriteLine("--> Attempting to apply migrations...");
            try
            {
                context.Database.Migrate();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"--> Could not run migrations: {ex.Message}");
            }
        }

        if (!context.Garages.Any())
        {
            var car1 = new Car { };
            var car2 = new Car {  };
            var car3 = new Car {  };
            var car4 = new Car {  };
            var car5 = new Car {  };

            var user1 = new User { };
            var user2 = new User { };

            var location1 = new Location { Latitude = 55.862656, Longitude = 9.837616 };
            var location2 = new Location { Latitude = 44.863337, Longitude = 13.857887 };
            var location3 = new Location { Latitude = 44.420792, Longitude = 26.095582 };


            // Creating some garage instances
            var garage1 = new Garage { Name = "Main Garage", Capacity = 5, Location = location1, User = user1, Cars = new List<Car> { car1, car2 } };
            var garage2 = new Garage { Name = "Secondary", Capacity = 3, Location = location2, Cars = new List<Car> { car3 } };
            var garage3 = new Garage { Name = "Waffle", Capacity = 7, Location = location3, Cars = new List<Car> { car4, car5 } };

            // Adding the garage and car instances to the database context
            context.Cars.AddRange(new List<Car> { car1, car2, car3, car4, car5 });
            context.Users.AddRange(user1, user2);
            context.Locations.AddRange(location1, location2, location3);
            context.Garages.AddRange(new List<Garage> { garage1, garage2, garage3 });
            
            context.SaveChanges();
        }
        else
        {
            Console.WriteLine("--> We already have data");
        }
    }
}
