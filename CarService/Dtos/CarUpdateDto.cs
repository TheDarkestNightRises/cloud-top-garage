using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CarService.Dtos;

public class CarUpdateDto
{
    public int Id { get; set; }
    public GarageDto Garage { get; set; }
}