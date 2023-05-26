namespace EnvironmentService.Models;

public class IndoorEnvironmentSettings 
{
    public int Id { get; set; }
    public int Co2Limit { get; set; }
    public float TemperatureLimit { get; set; }
    public float HumidityLimit { get; set; }
    public float LightLimit { get; set; }
    public bool LightOn { get; set; }
    public int MacAddress { get; set; }
    public string LoRaWANURL { get; set; }
    public int Port { get; set; }
    public string Eui { get; set; }
}