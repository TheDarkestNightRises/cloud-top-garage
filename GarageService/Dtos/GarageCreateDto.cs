using System.ComponentModel.DataAnnotations;

namespace GarageService.Dtos;

public class GarageCreateDto
{
    [Required]
    public string Name { get; set; }
    [Required]
    public int Capacity { get; set; }
    public LocationCreateDto Location { get; set; }
    [Required]
    public UserReadDto User { get; set; }
}