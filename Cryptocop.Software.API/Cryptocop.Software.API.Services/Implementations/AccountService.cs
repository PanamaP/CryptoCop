using System;
using Cryptocop.Software.API.Models.Dtos;
using Cryptocop.Software.API.Models.InputModels;
using Cryptocop.Software.API.Repositories.Interfaces;
using Cryptocop.Software.API.Services.Interfaces;

namespace Cryptocop.Software.API.Services.Implementations
{
    public class AccountService : IAccountService
    {
        private readonly IJwtTokenService _tokenService;
        private readonly IUserRepository _userRepository;

        public AccountService(IJwtTokenService tokenService, IUserRepository userRepository)
        {
            _tokenService = tokenService;
            _userRepository = userRepository;
        }

        public UserDto CreateUser(RegisterInputModel inputModel)
        {
            var user = _userRepository.CreateUser(inputModel);
            if (user == null){return null;}
            return user;
        }

        public UserDto AuthenticateUser(LoginInputModel loginInputModel)
        {
            return _userRepository.AuthenticateUser(loginInputModel);
        }

        public void Logout(int tokenId)
        {
            _tokenService.VoidToken(tokenId);
        }
    }
}