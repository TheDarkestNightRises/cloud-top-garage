using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;
namespace UserService.Models;

public class StrongPasswordValidation : ValidationAttribute
{
    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {

        var password = value as string;

        if (string.IsNullOrEmpty(password))
            return new ValidationResult("The password is required.");

        // Check for at least 8 characters
        if (password.Length < 8)
            return new ValidationResult("The password must have at least 8 characters.");

        // Check for at least one digit
        if (!Regex.IsMatch(password, @"\d+"))
            return new ValidationResult("The password must contain at least one digit.");
        return ValidationResult.Success;
    }
}