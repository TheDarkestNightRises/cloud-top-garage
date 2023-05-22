using System.ComponentModel.DataAnnotations;

namespace EnvironmentService.Dtos;

public class IndoorEnvironmentReadDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public GarageReadDto Garage { get; set; }
    public IndoorEnvironmentSettingsReadDto IndoorEnvironmentSettings { get; set; }
}
