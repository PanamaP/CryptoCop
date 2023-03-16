using Cryptocop.Software.API.Models.InputModels;
using Cryptocop.Software.API.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Cryptocop.Software.API.Controllers
{
    [Authorize]
    [Route("api/orders")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService;

        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        [HttpGet]
        [Route("")]
        public IActionResult GetOrders()
        {
            return Ok(_orderService.GetOrders(User.Identity.Name));
        }

        [HttpPost]
        [Route("")]
        public IActionResult CreateNewOrder([FromBody] OrderInputModel newOrder)
        {
            if (newOrder.AddressId.Equals(null)){
                return BadRequest("Please insert an AddressId AND PaymentCardId");
            }
            var orderCreated = _orderService.CreateNewOrder(User.Identity.Name, newOrder);
            if (!orderCreated)
            {
                return BadRequest("Error with creating order, Make sure you have a valid address, a payment card, the Id's are correct and that the cart isn't empty");
            }
            return StatusCode(201);
        }
    }
}