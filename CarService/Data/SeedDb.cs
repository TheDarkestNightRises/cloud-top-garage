using System;
using System.Linq;
using CarService.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

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
            var engines = new List<Engine>
        {
            new Engine { Size = 2.5, FuelType = "Gasoline", PowerHP = 203, TorqueNM = 250 },
            new Engine { Size = 1.5, FuelType = "Gasoline", PowerHP = 174, TorqueNM = 220 },
            new Engine { Size = 5.0, FuelType = "Gasoline", PowerHP = 395, TorqueNM = 400 },
            new Engine { Size = 0.0, FuelType = "Electric", PowerHP = 283, TorqueNM = 416 },
            new Engine { Size = 6.2, FuelType = "Gasoline", PowerHP = 490, TorqueNM = 637 }
        };



            var cars = new List<Car>
        {
            new Car { Name = "Toyota Camry", Description = "Midsize sedan", Manufacturer = "Toyota", Model = "Camry", Year = 2022, Seats = 5, Garage = garages[0], Image = images[0] },
            new Car { Name = "Honda Civic", Description = "Compact car", Manufacturer = "Honda", Model = "Civic", Year = 2022, Seats = 5, Garage = garages[0], Image = images[0] },
            new Car { Name = "Ford F-150", Description = "Full-size pickup truck", Manufacturer = "Ford", Model = "F-150", Year = 2022, Seats = 6, Garage = garages[1], Image = images[0] },
            new Car { Name = "Tesla Model 3", Description = "Electric sedan", Manufacturer = "Tesla", Model = "Model 3", Year = 2022, Seats = 5, Garage = garages[2], Image = images[0] },
            new Car { Name = "Chevrolet Corvette", Description = "Sports car", Manufacturer = "Chevrolet", Model = "Corvette", Year = 2022, Seats = 2, Garage = garages[2], Image = images[0] }
        };

            for (int i = 0; i < cars.Count; i++)
            {
                cars[i].Engine = engines[i];
            }
            context.Engines.AddRange(engines);
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
