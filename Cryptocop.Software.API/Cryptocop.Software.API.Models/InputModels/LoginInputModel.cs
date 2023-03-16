using System.ComponentModel.DataAnnotations;

namespace Cryptocop.Software.API.Models.InputModels
{
    public class LoginInputModel
    {
        [Required]
        [EmailAddress(ErrorMessage = "The email address is not correctly formatted")]
        public string Email {get;set;}
        [Required]
        [DataType(DataType.Password)]
        [MinLength(8, ErrorMessage = "The minimum length should be 8")]
        public string Password {get;set;}
    }
}