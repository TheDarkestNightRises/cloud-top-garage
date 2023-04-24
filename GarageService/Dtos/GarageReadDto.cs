using System.ComponentModel.DataAnnotations;

namespace GarageService.Dtos;

public class GarageReadDto
{
    public int Id { get; set; }

    [Required]
    public string Name { get; set; }

    [Required]
    public uint Capacity { get; set; }

    [Required]
    public uint SlotsUsed { get; set; }

    [Required]
    public UserReadDto User { get; set; }

    public IEnumerable<CarReadDto> Cars { get; set; }
}