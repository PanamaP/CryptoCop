using Cryptocop.Software.API.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Cryptocop.Software.API.Controllers
{
    [Authorize]
    [Route("api/exchanges")]
    [ApiController]
    public class ExchangeController : ControllerBase
    {
        private readonly IExchangeService _exchangeService;

        public ExchangeController(IExchangeService exchangeService)
        {
            _exchangeService = exchangeService;
        }

        [HttpGet]
        [Route("")]
        public async Task<IActionResult> GetExchanges([FromQuery] int pageNumber = 1)
        {
            var exchanges = await _exchangeService.GetExchanges(pageNumber);
            return Ok(exchanges);
        }

    }
}