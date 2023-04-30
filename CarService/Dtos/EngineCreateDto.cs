using System.ComponentModel.DataAnnotations;

namespace CarService.Dtos;

public class EngineCreateDto
{
    public double Size { get; set; }
    public string FuelType { get; set; }
    public int PowerHP { get; set; }
    public int TorqueNM { get; set; }
}