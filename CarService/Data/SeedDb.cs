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

        string currentDirectory = Directory.GetCurrentDirectory();
        Console.WriteLine($"--> Creating new images in {currentDirectory}");

        if (!context.Cars.Any())
        {
            var images = new List<Image>
        {
            new Image
            {
                Data = GetImageData("car.png")
            }
        };

            var garages = new List<Garage>
        {
            new Garage(),
            new Garage(),
            new Garage()
        };

            var cars = new List<Car>
        {
            new Car { Name = "Toyota Camry", Description = "Midsize sedan", Garage = garages[0], Image = images[0] },
            new Car { Name = "Honda Civic", Description = "Compact car", Garage = garages[0], Image = images[0] },
            new Car { Name = "Ford F-150", Description = "Full-size pickup truck", Garage = garages[1], Image = images[0] },
            new Car { Name = "Tesla Model 3", Description = "Electric sedan", Garage = garages[2], Image = images[0] },
            new Car { Name = "Chevrolet Corvette", Description = "Sports car", Garage = garages[2], Image = images[0] }
        };

            context.Images.AddRange(images);
            context.Garages.AddRange(garages);
            context.Cars.AddRange(cars);
            context.SaveChanges();
        }
        else
        {
            Console.WriteLine("--> We already have data");
        }

    }

    public static byte[] GetImageData(string fileName)
    {
        var imagePath = Path.Combine(Directory.GetCurrentDirectory(), "Images", "car.png");
        return File.ReadAllBytes(imagePath);
    }
}
