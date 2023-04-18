using System.ComponentModel.DataAnnotations;

namespace CarService.Dtos;

public class CarReadDto
{
    public int Id { get; set; }

    [Required]
    public string Name { get; set; }

    [Required]
    public string Description { get; set; }

    public GarageDto Garage { get; set; }
}