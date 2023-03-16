using Cryptocop.Software.API.Models.Exceptions;
using Cryptocop.Software.API.Models.Extensions;
using Cryptocop.Software.API.Models.InputModels;
using Cryptocop.Software.API.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Cryptocop.Software.API.Controllers
{
    [Authorize]
    [Route("api/addresses")]
    [ApiController]
    public class AddressController : ControllerBase
    {
        private readonly IAddressService _addressService;

        public AddressController(IAddressService addressService)
        {
            _addressService = addressService;
        }

        [HttpGet]
        [Route("")]
        public IActionResult GetAllAddresses()
        {
            return Ok(_addressService.GetAllAddresses(User.Identity.Name));
        }

        [HttpPost]
        [Route("")]
        public IActionResult AddAddress([FromBody] AddressInputModel address)
        {
            if(!ModelState.IsValid){
                throw new ModelFormatException(ModelState.RetrieveErrorString());
            }
            _addressService.AddAddress(User.Identity.Name, address); 
            
            return StatusCode(201);
        }

        [HttpDelete]
        [Route("{addressId}")]
        public IActionResult DeleteAddress(int addressId)
        {
            var deleteAddress = _addressService.DeleteAddress(User.Identity.Name, addressId);
            if (!deleteAddress){
                return BadRequest($"Address with id {addressId} doesn't exist.");
            }
            return NoContent();
        }
    }
}