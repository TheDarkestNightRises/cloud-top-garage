namespace EnvironmentService.Dtos;
public class StatReadDto
{
    public int Id { get; set; }
    public IndoorEnvironmentReadDto IndoorEnvironment { get; set; }
    public float Temperature { get; set; }
    public float Humidity { get; set; }
    public float CO2 { get; set; }
}
