using System;
using System.Linq;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using EnvironmentService.Models;
using EnvironmentService.Data;

namespace EnvironmentService.Data;
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

        if (!context.IndoorEnvironments.Any())
        {
            Console.WriteLine("--> Seeding Data...");
            var garages = new List<Garage>
            {
                new Garage { Id = 1 },
            };

            var indoorEnvironments = new List<IndoorEnvironment>
            {
                new IndoorEnvironment { Name = "Environment 1", Garage = garages[0], MacAddress = 120,LorraWanURL="wss://iotnet.cibicom.dk/app?token=vnoUBwAAABFpb3RuZXQuY2liaWNvbS5ka54Zx4fqYp5yzAQtnGzDDUw=" },
            };

            context.Garages.AddRange(garages);
            context.IndoorEnvironments.AddRange(indoorEnvironments);
            context.SaveChanges();
        }
        else
        {
            Console.WriteLine("--> We already have data");
        }
    }
}
