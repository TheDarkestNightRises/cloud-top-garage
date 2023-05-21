using System.ComponentModel.DataAnnotations;
namespace UserService.Models;

public class User 
{
    public int Id { get; set; }
    [Required]
    public string Name { get; set; }
    [Required]
    public string Email { get; set; }
    [StrongPasswordValidation(ErrorMessage = "The password must be strong and meet the specified requirements.")]
    public string Password { get; set; }
    public string Role { get; set; }
    [Range(1, int.MaxValue, ErrorMessage = "The value must be bigger than 0.")]
    public int Age { get; set; }
    public string Phone { get; set; }
}


