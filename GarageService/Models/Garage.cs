namespace GarageService.Models;

public class Garage
{
    public int Id { get; set; }
    public string Name { get; set; }
    public uint Capacity { get; set; }
    public User Owner { get; set; }
    public List<Car> Cars { get; set; }
}