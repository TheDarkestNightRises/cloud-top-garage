using System.ComponentModel.DataAnnotations;

namespace Carservice.Dtos;

public class CarReadDto
{
    [Required]
    public int Id { get; set; }

    [Required]
    public string Name { get; set; }

    [Required]
    public string Description { get; set; }
}