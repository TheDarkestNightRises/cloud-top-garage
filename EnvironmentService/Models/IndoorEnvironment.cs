namespace EnvironmentService.Models;

public class IndoorEnvironment
{
    public int Id { get; set; }
    public string Name { get; set; }
    public Garage Garage { get; set; }
    public int MacAddress { get; set; }

    public string LorraWanURL { get; set; }
}