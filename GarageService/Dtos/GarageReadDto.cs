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
    public LocationReadDto Location { get; set; }

    [Required]
    public UserReadDto User { get; set; }

    public IEnumerable<CarReadDto> Cars { get; set; }


    // public override string ToString()
    // {
    //     return $"{Id} vailable slots: {AvailableSlots}, User: {User}, Cars: {string.Join(",", Cars)}";
    // }
}