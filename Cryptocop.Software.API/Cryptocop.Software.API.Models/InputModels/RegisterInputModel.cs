using System.ComponentModel.DataAnnotations;

namespace Cryptocop.Software.API.Models.InputModels
{
    public class RegisterInputModel
    {
        [Required]
        [EmailAddress(ErrorMessage = "The email address is not correctly formatted")]
        public string Email {get;set;}
        [Required]
        [MinLength(3, ErrorMessage = "The minimum length should be 3")]
        public string FullName {get;set;}
        [Required]
        [DataType(DataType.Password)]
        [MinLength(8, ErrorMessage = "The minimum length should be 8")]
        public string Password {get;set;}
        [Required]
        [MinLength(8, ErrorMessage = "The minimum length should be 8")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Password do not match.")]
        public string PasswordConfirmation {get;set;}
    }
}