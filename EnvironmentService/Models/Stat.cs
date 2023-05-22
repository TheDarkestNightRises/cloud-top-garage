namespace EnvironmentService.Models;

public class Stat
{
    public int Id { get; set; }
    public IndoorEnvironment IndoorEnvironment { get; set; }
    public float Temperature { get; set; }
    public float Humidity { get; set; }
    public float CO2 { get; set; }
    public DateTime Time { get; set; } = DateTime.Now;
}
