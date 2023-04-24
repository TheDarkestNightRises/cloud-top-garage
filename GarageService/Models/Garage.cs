using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
namespace GarageService.Models;

public class Garage
{
    public int Id { get; set; }
    [Required]
    public string Name { get; set; }
    [Required]
    public uint Capacity { get; set; }
    [NotMapped]
    public uint SlotsUsed { get; set; }
    public User User { get; set; }
    public List<Car> Cars { get; set; }
}