using System.ComponentModel.DataAnnotations;

namespace GarageService.Dtos;

public class GarageReadDto
{
    public int Id { get; set; }

    [Required]
    public string Name { get; set; }
}