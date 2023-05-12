namespace EnvironmentService.Models;

public class Stat
{
    public int Id { get; set; }
    public IndoorEnvironment IndoorEnvironment { get; set; }
    public float Temparature { get; set; }
    public float Humidity { get; set; }
    public float CO2 { get; set; }
}