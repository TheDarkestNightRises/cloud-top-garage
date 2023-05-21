using System.ComponentModel.DataAnnotations;
using UserService.Models;

namespace UserService.Dtos;
public class UserAuthDto
{
    [Required]
    [EmailAddress(ErrorMessage = "Invalid email address.")]
    public string Email { get; set; }
    [StrongPasswordValidation(ErrorMessage = "The password must be strong and meet the specified requirements.")]
    public string Password { get; set; }
}


