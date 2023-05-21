using System.ComponentModel.DataAnnotations;
using UserService.Models;

namespace UserService.Dtos;
public class UserCreateDto
{
    [Required]
    public string Name { get; set; }
    [Required]
    [EmailAddress(ErrorMessage = "Invalid email address.")]
    public string Email { get; set; }
    [StrongPasswordValidation(ErrorMessage = "The password must be strong and meet the specified requirements.")]
    public string Password { get; set; }
    [Range(1, int.MaxValue, ErrorMessage = "The age must be bigger than 0.")]
    public int Age { get; set; }
    public string Phone { get; set; }
}


