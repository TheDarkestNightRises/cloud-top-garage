using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
namespace GarageService.Models;

public class Garage
{
    public int Id { get; set; }
    [Required]
    [StringLength(50, MinimumLength = 1, ErrorMessage = "Name must be between {2} and {1    } characters.")]
    public string Name { get; set; }
    [Required]
    [Range(1, int.MaxValue, ErrorMessage = "Capacity must be a positive number.")]
    public int Capacity { get; set; }

    public int AvailableSlots
    {
        get { return Capacity - Cars?.Count ?? 0; }
        private set { }
    }

    public Location Location { get; set; }
    public User User { get; set; }
    public List<Car> Cars { get; set; }

    public override string ToString()
    {
        string carsString = Cars != null ? string.Join(",", Cars) : "No Cars";
        return $"{Id} vailable slots: {AvailableSlots}, User: {User}, Cars: {carsString}";
    }

    public override bool Equals(object obj)
    {
        if (obj == null || GetType() != obj.GetType())
        {
            return false;
        }

        Garage otherGarage = (Garage)obj;
        return Id == otherGarage.Id &&
               Name == otherGarage.Name &&
               Capacity == otherGarage.Capacity;
    }


}