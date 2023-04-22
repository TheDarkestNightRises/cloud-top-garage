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
           
            // var cars = new List<Car>
            // {
            //     new Car { Id = 1 },
            //     new Car { Id = 2 },
            //     new Car { Id = 3 },
            //     new Car { Id = 4 },
            //     new Car { Id = 5 }
            // };

            //  var garages = new List<Garage>
            // {
            //  new Garage { Id = 1 },
            //  new Garage { Id = 2 },
            //  new Garage { Id = 3 }
            // };
            var car1 = new Car { Id = 1 };
            var car2 = new Car { Id = 2 };
            var car3 = new Car { Id = 3 };
            var car4 = new Car { Id = 4 };
            var car5 = new Car { Id = 5 };

            var user1 = new User { Id = 1};
            var user2 = new User { Id = 2};

            // Creating some garage instances
            var garage1 = new Garage { Id = 1, Name = "Main Garage", Owner = user1, Cars = new List<Car> { car1, car2 } };
            var garage2 = new Garage { Id = 2, Name = "Secondary", Owner = user1, Cars = new List<Car> { car3 }};
            var garage3 = new Garage { Id = 3, Name = "Waffle", Owner = user2, Cars = new List<Car> { car4, car5 } };

            // Adding the garage and car instances to the database context
            context.Cars.AddRange(new List<Car> { car1, car2, car3, car4, car5 });
            context.Garages.AddRange(new List<Garage> { garage1, garage2, garage3 });
            context.SaveChanges();
        }
        else
        {
            Console.WriteLine("--> We already have data");
        }
    }
}
