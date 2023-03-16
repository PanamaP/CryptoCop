using System;
using Cryptocop.Software.API.Models.Dtos;
using Cryptocop.Software.API.Models.InputModels;
using Cryptocop.Software.API.Repositories.Contexts;
using Cryptocop.Software.API.Repositories.Helpers;
using Cryptocop.Software.API.Repositories.Interfaces;
using System.Linq;
using Cryptocop.Software.API.Models.Entities;

namespace Cryptocop.Software.API.Repositories.Implementations
{
    public class UserRepository : IUserRepository
    {
        private readonly CryptocopDbContext _dbContext;
        private readonly ITokenRepository _tokenRepository;

        public UserRepository(CryptocopDbContext dbContext, ITokenRepository tokenRepository)
        {
            _dbContext = dbContext;
            _tokenRepository = tokenRepository;
        }

        public UserDto CreateUser(RegisterInputModel inputModel)
        {
            var userExists = _dbContext.Users.FirstOrDefault(u => u.Email == inputModel.Email);
            if (userExists != null){return null;}

            var nextId = _dbContext.Users.Count() + 1;
            var hashedpass = HashingHelper.HashPassword(inputModel.Password);

            _dbContext.Users.Add(new User{
                Id = nextId,
                FullName = inputModel.FullName,
                Email = inputModel.Email,
                HashedPassword = hashedpass
            });

            //ALSO CREATE A CART FOR THE USER
            var cartId = _dbContext.ShoppingCarts.Count() + 1;
            _dbContext.ShoppingCarts.Add(new ShoppingCart{
                Id = cartId,
                UserId = nextId
            });

            var token = _tokenRepository.CreateNewToken();
            _dbContext.SaveChanges();

            return new UserDto{
                Id = nextId,
                FullName = inputModel.FullName,
                Email = inputModel.Email,
                TokenId = token.Id
            };


        }

        public UserDto AuthenticateUser(LoginInputModel loginInputModel)
        {
            var user = _dbContext.Users.FirstOrDefault(u => 
                u.Email == loginInputModel.Email &&
                u.HashedPassword == HashingHelper.HashPassword(loginInputModel.Password));
            if (user == null){return null;}

            var token = _tokenRepository.CreateNewToken();
            return new UserDto{
                Id = user.Id,
                FullName = user.FullName,
                Email = user.Email,
                TokenId = token.Id
            };
        }
    }
}