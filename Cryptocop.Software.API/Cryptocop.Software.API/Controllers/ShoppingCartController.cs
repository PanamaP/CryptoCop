using Cryptocop.Software.API.Models.InputModels;
using Cryptocop.Software.API.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Cryptocop.Software.API.Controllers
{
    [Authorize]
    [Route("api/cart")]
    [ApiController]
    public class ShoppingCartController : ControllerBase
    {
        private readonly IShoppingCartService _shoppingCartService;

        public ShoppingCartController(IShoppingCartService shoppingCartService)
        {
            _shoppingCartService = shoppingCartService;
        }

        [HttpGet]
        [Route("")]
        public IActionResult GetCartItems()
        {
            return Ok(_shoppingCartService.GetCartItems(User.Identity.Name));
        }

        [HttpPost]
        [Route("")]
        public async Task<IActionResult> AddCartItem([FromBody] ShoppingCartItemInputModel shoppingCartItemItem)
        {
            if (shoppingCartItemItem.ProductIdentifier == null) {return BadRequest("ProductIdentifier can't be null");}
            var createdItem = await _shoppingCartService.AddCartItem(User.Identity.Name, shoppingCartItemItem);
            if (!createdItem){
                return BadRequest($"There was an error fetching price for '{shoppingCartItemItem.ProductIdentifier}'. Make sure identifier is correct");
            }
            return StatusCode(201);
        }

        [HttpDelete]
        [Route("{id}")]
        public IActionResult RemoveCartItem(int id)
        {
            var deletedItem = _shoppingCartService.RemoveCartItem(User.Identity.Name, id);
            if (!deletedItem){
                return BadRequest($"Item with id {id} doesn't exist in your cart");
            }
            return StatusCode(204);
        }

        [HttpPatch]
        [Route("{id}")]
        public IActionResult UpdateCartItemQuantity([FromBody] ShoppingCartItemInputModel input, int id)
        {
            var updatedItem = _shoppingCartService.UpdateCartItemQuantity(User.Identity.Name, id, (float) input.Quantity);
            if (!updatedItem){
                return BadRequest($"Item with id {id} doesn't exist in your cart");
            }
            return StatusCode(204);
        }

        [HttpDelete]
        [Route("")]
        public IActionResult ClearCart()
        {
            _shoppingCartService.ClearCart(User.Identity.Name);
            return StatusCode(204);
        }

    }
}