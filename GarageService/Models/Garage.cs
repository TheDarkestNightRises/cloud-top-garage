using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
namespace GarageService.Models;

public class Garage
{
    public int Id { get; set; }
    [Required]
    public string Name { get; set; }
    [Required]
    public int Capacity { get; set; }

    public int AvailableSlots
    {
        get { return Capacity - Cars?.Count ?? 0; }
        private set { }
    }

    public User User { get; set; }
    public List<Car> Cars { get; set; }

    public override string ToString()
    {
        return $"Available slots: {AvailableSlots}, User: {User}, Cars: {string.Join(",", Cars)}";
    }
}