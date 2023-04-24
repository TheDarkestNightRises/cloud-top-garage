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
            var car1 = new Car {};
            var car2 = new Car {};
            var car3 = new Car {};
            var car4 = new Car {};
            var car5 = new Car {};

            var user1 = new User {};
            var user2 = new User {};

            // Creating some garage instances
            var garage1 = new Garage { Name = "Main Garage", Capacity = 5, Owner = user1, Cars = new List<Car> { car1, car2 } };
            var garage2 = new Garage { Name = "Secondary", Capacity = 3, Owner = user1, Cars = new List<Car> { car3 }};
            var garage3 = new Garage { Name = "Waffle", Capacity = 7, Owner = user2, Cars = new List<Car> { car4, car5 } };

            // Adding the garage and car instances to the database context
            context.Cars.AddRange(new List<Car> { car1, car2, car3, car4, car5 });
            context.Users.AddRange(user1, user2);
            context.Garages.AddRange(new List<Garage> { garage1, garage2, garage3 });
            context.SaveChanges();
        }
        else
        {
            Console.WriteLine("--> We already have data");
        }
    }
}
