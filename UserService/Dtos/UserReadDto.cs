using System.ComponentModel.DataAnnotations;
using UserService.Models;
namespace UserService.Dtos;
public class UserReadDto
{

    public int Id { get; set; }
    [Required]
    public string Name { get; set; }
    [Required]
    [EmailAddress(ErrorMessage = "Invalid email address.")]
    public string Email { get; set; }
    [Range(1, int.MaxValue, ErrorMessage = "The age must be bigger than 0.")]
    public int Age { get; set; }
   
    public string Phone { get; set; }
}


