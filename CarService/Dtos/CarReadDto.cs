using System.ComponentModel.DataAnnotations;

namespace CarService.Dtos;

public class CarReadDto
{
    public int Id { get; set; }

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
    public EngineReadDto Engine { get; set; }

    public GarageDto Garage { get; set; }
}
