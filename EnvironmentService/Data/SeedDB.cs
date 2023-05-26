using System;
using System.Linq;
using EnvironmentService.Data;
using EnvironmentService.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

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
                new Garage { },
            };


            var indoorEnvironments = new List<IndoorEnvironment>
            {
                new IndoorEnvironment
                {
                    Name = "Environment 1",
                    Garage = garages[0],
                    IndoorEnvironmentSettings = new IndoorEnvironmentSettings
                    {
                        Co2Limit = 100,
                        TemperatureLimit = 25,
                        HumidityLimit = 80,
                        LightLimit = 500,
                        LightOn = true,
                        MacAddress = 120,
                        LoRaWANURL = "wss://iotnet.cibicom.dk/app?token=vnoUBwAAABFpb3RuZXQuY2liaWNvbS5ka54Zx4fqYp5yzAQtnGzDDUw=",
                        Port = 1,
                        Eui = "0004A30B00251001"
                    }
                }
            };

            var stats = new List<Stat>
            {
                new Stat
                {
                    IndoorEnvironment = indoorEnvironments[0],
                    Temperature = 23.5f,
                    Humidity = 75.2f,
                    CO2 = 95.0f,
                    Time = DateTime.Now
                },
                new Stat
                {
                    IndoorEnvironment = indoorEnvironments[0],
                    Temperature = 24.1f,
                    Humidity = 76.8f,
                    CO2 = 105.3f,
                    Time = DateTime.Now.AddMinutes(-30)
                }
            };

            context.Garages.AddRange(garages);
            context.IndoorEnvironments.AddRange(indoorEnvironments);
            context.Stats.AddRange(stats);
            context.SaveChanges();
        }
        else
        {
            Console.WriteLine("--> We already have data");
        }
    }
}
