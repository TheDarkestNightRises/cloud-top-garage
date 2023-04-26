using System.ComponentModel.DataAnnotations;
using CarService.Models;

namespace GarageService.Models;

public class Car
{
    public int Id { get; set; }

    public string Name { get; set; }

    public string Description { get; set; }

    public Image Image { get; set; }

    public Garage Garage { get; set; }
}