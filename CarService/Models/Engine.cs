using System.ComponentModel.DataAnnotations;

namespace CarService.Models;

public class Engine
{
    public int Id { get; set; }
    public double Size { get; set; }
    public string FuelType { get; set; }
    public int PowerHP { get; set; }
    
    public int TorqueNM { get; set; }
}