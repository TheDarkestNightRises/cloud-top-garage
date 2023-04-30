using System.ComponentModel.DataAnnotations;

namespace CarService.Models;

public class Engine
{
    public int Id { get; set; }

    [Required(ErrorMessage = "The engine size is required.")]
    [Range(0.1, 10.0, ErrorMessage = "The engine size must be between {1} and {2}.")]
    public double Size { get; set; }

    [Required(ErrorMessage = "The fuel type is required.")]
    [StringLength(50, MinimumLength = 1, ErrorMessage = "The fuel type must be between {2} and {1} characters.")]
    public string FuelType { get; set; }

    [Required(ErrorMessage = "The engine power is required.")]
    [Range(1, 1000, ErrorMessage = "The engine power must be between {1} and {2} horsepower.")]
    public int PowerHP { get; set; }

    [Required(ErrorMessage = "The engine torque is required.")]
    [Range(1, 1000, ErrorMessage = "The engine torque must be between {1} and {2} Newton meters.")]
    public int TorqueNM { get; set; }
}
