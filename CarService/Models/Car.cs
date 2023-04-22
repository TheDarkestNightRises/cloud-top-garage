using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CarService.Models;

public class Car
{
    [Required]
    public int Id { get; set; }

    [Required]
    public string Name { get; set; }

    [Required]
    public string Description { get; set; }

    public Image Image { get; set; }

    public Garage Garage { get; set; }

    public override string ToString()
    {
        string garageId = Garage != null ? Garage.Id.ToString() : "null";
        return $"Car [Id={Id}, Name={Name}, Description={Description}, GarageId={garageId}]";
    }
}