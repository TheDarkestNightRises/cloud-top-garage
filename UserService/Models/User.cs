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

    public override bool Equals(object obj)
    {
        if (obj == null || GetType() != obj.GetType())
        {
            return false;
        }

        User otherUser = (User)obj;
        return Id == otherUser.Id &&
            Name == otherUser.Name &&
            Email == otherUser.Email &&
            Password == otherUser.Password &&
            Role == otherUser.Role &&
            Age == otherUser.Age &&
            Phone == otherUser.Phone;
    }

    public override string ToString()
    {
        return $"User [Id={Id}, Name={Name}, Email={Email}, Role={Role}, Age={Age}, Phone={Phone}]";
    }

    public override int GetHashCode()
    {
        // Implement your own GetHashCode logic here if needed
        return base.GetHashCode();
    }
}


