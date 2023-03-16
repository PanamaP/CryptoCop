using System.ComponentModel.DataAnnotations;

namespace Cryptocop.Software.API.Models.InputModels
{
    public class PaymentCardInputModel
    {
        [Required]
        [MinLength(3, ErrorMessage = "The minimum length should be 3")]
        public string CardHolderName {get;set;}
        [Required]
        [CreditCard(ErrorMessage = "Enter a valid credit card number.")]
        public string CardNumber {get;set;}
        [Range(1,12, ErrorMessage = "Number should be between 1 and 12")]
        public int Month {get;set;}
        [Range(0,99, ErrorMessage = "Number should be between 0 and 99")]
        public int Year {get; set;}
        
    }
}