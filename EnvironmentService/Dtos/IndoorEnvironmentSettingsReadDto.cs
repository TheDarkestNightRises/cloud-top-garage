namespace EnvironmentService.Dtos;

public class IndoorEnvironmentSettingsReadDto
{
    public int Id { get; set; }
    public int Co2Limit { get; set; }
    public float TemperatureLimit { get; set; }
    public float HumidityLimit { get; set; }
    public float LightLimit { get; set; }
    public bool LightOn { get; set; }
    public int MacAddress { get; set; }
    public string LoRaWANURL { get; set; }
}
