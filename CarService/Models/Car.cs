using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CarService.Models;

public class Car
{
    public int Id { get; set; }

    [Required]
    public string Name { get; set; }

    [Required]
    public string Description { get; set; }
    [Required]
    public string Manufacturer { get; set; }
    [Required]
    public string Model { get; set; }
    [Required]
    public int Year { get; set; }
    [Required]
    public int Seats { get; set; }
    public Image Image { get; set; }
    //public Engine Engine { get; set; }

    public Garage Garage { get; set; }

    public override string ToString()
    {
        string garageId = Garage != null ? Garage.Id.ToString() : "null";
        return $"Car [Id={Id}, Name={Name}, Description={Description}, GarageId={garageId}]";
    }
}