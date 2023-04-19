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
           
            var cars = new List<Car>
            {
                new Car { Id = 1 },
                new Car { Id = 2 },
                new Car { Id = 3 },
                new Car { Id = 4 },
                new Car { Id = 5 }
            };

             var garages = new List<Garage>
            {
             new Garage { Id = 1 },
             new Garage { Id = 2 },
             new Garage { Id = 3 }
            };

            context.Cars.AddRange(cars);
            context.Garages.AddRange(garages);
            context.SaveChanges();
        }
        else
        {
            Console.WriteLine("--> We already have data");
        }
    }
}
