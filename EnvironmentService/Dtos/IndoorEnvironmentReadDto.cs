using System.ComponentModel.DataAnnotations;

namespace EnvironmentService.Dtos;

public class IndoorEnvironmentReadDto
{
    public int Id { get; set; }
    [Required]
    public string Name { get; set; }
    public GarageReadDto Garage { get; set; }
    public int MacAddress { get; set; }
    public string LoRaWANURL { get; set; }
}
