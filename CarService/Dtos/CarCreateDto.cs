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
    public GarageDto Garage { get; set; }
}