using System;
using System.Linq;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using CarService.Models;

namespace CarService.Data;
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

        if (!context.Cars.Any())
        {
            var garages = new List<Garage>
            {
             new Garage { Id = 1 },
             new Garage { Id = 2 },
             new Garage { Id = 3 }
            };

            var cars = new List<Car>
            {
                new Car { Id = 1, Name = "Toyota Camry", Description = "Midsize sedan", Garage = garages[0] },
                new Car { Id = 2, Name = "Honda Civic", Description = "Compact car", Garage = garages[0] },
                new Car { Id = 3, Name = "Ford F-150", Description = "Full-size pickup truck", Garage = garages[1] },
                new Car { Id = 4, Name = "Tesla Model 3", Description = "Electric sedan", Garage = garages[2] },
                new Car { Id = 5, Name = "Chevrolet Corvette", Description = "Sports car", Garage = garages[2] }
            };

            context.Garages.AddRange(garages);
            context.Cars.AddRange(cars);
            context.SaveChanges();
        }
        else
        {
            Console.WriteLine("--> We already have data");
        }
    }
}
