namespace EnvironmentService.Dtos;
public class IndoorEnvironmentSettingsUpdateDto
{
    public int Co2Limit { get; set; }
    public float TemperatureLimit { get; set; }
    public float HumidityLimit { get; set; }
    public float LightLimit { get; set; }
    public bool LightOn { get; set; }
}
