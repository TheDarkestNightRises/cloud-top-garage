using System.ComponentModel.DataAnnotations;

namespace GarageService.Dtos;

public class GarageCreateDto
{
    [Required]
    public string Name { get; set; }
    [Required]
    [Range(1, int.MaxValue, ErrorMessage = "Capacity must be a positive number.")]
    public int Capacity { get; set; }
    public LocationCreateDto Location { get; set; }
    [Required]
    public UserReadDto User { get; set; }
}