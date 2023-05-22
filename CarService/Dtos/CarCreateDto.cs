namespace CarService.Dtos;

using System.ComponentModel.DataAnnotations;
using CarService.Models;

public class CarCreateDto
{
    [Required]
    public string Name { get; set; }
    [Required]
    public string Description { get; set; }
    [Required]
    public string Manufacturer { get; set; }
    [Required]
    public string Model { get; set; }
    [Required]
    public int Year { get; set; }
    [Required]
    public int Seats { get; set; }
    [Required]
    public GarageDto Garage { get; set; }
    public EngineCreateDto Engine { get; set; }
}
