namespace EnvironmentService.Dtos;

public class IndoorEnvironmentSettingsReadDto
{
    public int Id { get; set; }
    public int Co2Limit { get; set; }
    public int TemperatureLimit { get; set; }
    public int HumidityLimit { get; set; }
    public int LightLimit { get; set; }
    public bool LightOn { get; set; }
    public int MacAddress { get; set; }
    public string LoRaWANURL { get; set; }
}