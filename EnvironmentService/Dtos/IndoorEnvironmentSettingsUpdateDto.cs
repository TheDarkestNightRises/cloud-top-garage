namespace EnvironmentService.Dtos;
public class IndoorEnvironmentSettingsUpdateDto
{
    public int Co2Limit { get; set; }
    public int TemperatureLimit { get; set; }
    public int HumidityLimit { get; set; }
    public int LightLimit { get; set; }
    public bool LightOn { get; set; }
}