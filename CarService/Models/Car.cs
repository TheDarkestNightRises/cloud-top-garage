using System.ComponentModel.DataAnnotations;

namespace Carservice.Models;

public class Car
{
    [Required]
    public int Id { get; set; }

    [Required]
    public string Name { get; set; }

    [Required]
    public string Description { get; set; }
}