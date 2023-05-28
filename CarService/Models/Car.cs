using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CarService.Models;

public class Car
{
    public int Id { get; set; }

    [Required]
    [StringLength(200, MinimumLength = 1, ErrorMessage = "{0} must be between {2} and {1} characters long.")]

    public string Name { get; set; }

    [Required]
    [StringLength(500, MinimumLength = 0, ErrorMessage = "{0} must be between {2} and {1} characters long.")]
    public string Description { get; set; }

    [Required]
    [StringLength(100, MinimumLength = 1, ErrorMessage = "{0} must be between {2} and {1} characters long.")]
    public string Manufacturer { get; set; }

    [Required]
    [StringLength(100, MinimumLength = 1, ErrorMessage = "{0} must be between {2} and {1} characters long.")]
    public string Model { get; set; }

    [Required(ErrorMessage = "Year is required.")]
    [Range(1800, Int32.MaxValue, ErrorMessage = "{0} must be at least {1}.")]
    public int Year { get; set; }

    [Required(ErrorMessage = "Number of seats is required.")]
    [Range(1, 8, ErrorMessage = "Number of seats must be between {1} and {2}.")]
    public int Seats { get; set; }

    public Image Image { get; set; }
    [Required(ErrorMessage = "Engine must be specified.")]
    public Engine Engine { get; set; }
    [Required(ErrorMessage = "Garage must be spiecified.")]
    public Garage Garage { get; set; }

    public override string ToString()
    {
        string garageId = Garage != null ? Garage.Id.ToString() : "null";
        return $"Car [Id={Id}, Name={Name}, Description={Description}, GarageId={garageId}]";
    }
}
