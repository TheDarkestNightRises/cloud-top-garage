namespace CarService.Dtos;

using System.ComponentModel.DataAnnotations;
using CarService.Models;

public class CarRegisterDto
{
    [Required]
    public string Name { get; set; }

    [Required]
    public string Description { get; set; }

    public GarageDto GarageDto { get; set; }

}