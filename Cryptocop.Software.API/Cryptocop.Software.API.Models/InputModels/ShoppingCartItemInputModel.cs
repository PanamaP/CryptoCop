using System.ComponentModel.DataAnnotations;

namespace Cryptocop.Software.API.Models.InputModels
{
    public class ShoppingCartItemInputModel
    {
        // [Required]
        public string ProductIdentifier {get;set;}
        [Required]
        [Range(0.01, float.MaxValue, ErrorMessage = "The value needs to be between 0.01 and 3.4028235e+38")]
        public float? Quantity {get; set;}
    }
}