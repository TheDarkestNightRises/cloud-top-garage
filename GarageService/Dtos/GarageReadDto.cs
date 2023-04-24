using System.ComponentModel.DataAnnotations;

namespace GarageService.Dtos;

public class GarageReadDto
{
    public int Id { get; set; }

    [Required]
    public string Name { get; set; }

    [Required]
    public int Capacity { get; set; }

    [Required]
    public int AvailableSlots { get; set; }

    [Required]
    public UserReadDto User { get; set; }

    public IEnumerable<CarReadDto> Cars { get; set; }
}