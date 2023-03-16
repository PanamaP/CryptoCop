using Cryptocop.Software.API.Models.InputModels;
using Cryptocop.Software.API.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Cryptocop.Software.API.Controllers
{
    [Authorize]
    [Route("api/account")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService _accountService;
        private readonly ITokenService _tokenService;

        public AccountController(IAccountService accountService, ITokenService tokenService)
        {
            _accountService = accountService;
            _tokenService = tokenService;
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("signin")]
        public IActionResult SignIn([FromBody] LoginInputModel login)
        {
            var user = _accountService.AuthenticateUser(login);
            if (user == null){return Unauthorized();}
            var token = _tokenService.GenerateJwtToken(user);
            return Ok(token);
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("register")]
        public IActionResult Register([FromBody] RegisterInputModel register)
        {
            var user = _accountService.CreateUser(register);
            if (user == null){
                return BadRequest($"An Account with email {register.Email} already exists");
            }
            return Ok(user);
        }

        [HttpGet]
        [Route("userinfo")]
        public IActionResult GetUserInfo()
        {
            var claims = User.Claims.Select(c => new
            {
                Type = c.Type,
                Value = c.Value
            });
            return Ok(claims);
        }

        [HttpGet]
        [Route("signout")]
        public IActionResult SignOut()
        {
            //  retrive Token id from claim and blacklist token
            int.TryParse(User.Claims.FirstOrDefault(c => c.Type == "tokenId").Value, out var tokenId);
            _accountService.Logout(tokenId);
            return NoContent();
        }
    }
}