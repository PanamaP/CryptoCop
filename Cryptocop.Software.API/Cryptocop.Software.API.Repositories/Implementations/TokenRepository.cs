using System.Linq;
using Cryptocop.Software.API.Models.Dtos;
using Cryptocop.Software.API.Models.Entities;
using Cryptocop.Software.API.Repositories.Contexts;
using Cryptocop.Software.API.Repositories.Interfaces;

namespace Cryptocop.Software.API.Repositories.Implementations
{
    public class TokenRepository : ITokenRepository
    {
        private readonly CryptocopDbContext _dbContext;

        public TokenRepository(CryptocopDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public JwtTokenDto CreateNewToken()
        {
            var nextId = _dbContext.JwtTokens.Count() + 1;
            var newToken = (new JwtToken{
                Id = nextId,
                Blacklisted = false
            });

            _dbContext.Add(newToken);
            _dbContext.SaveChanges();
            
            return new JwtTokenDto{
                Id = nextId,
                Blacklisted = false
            };
        }

        public bool IsTokenBlacklisted(int tokenId)
        {
            var token = _dbContext.JwtTokens.FirstOrDefault(t => t.Id == tokenId);
            if (token == null){return true;}
            return token.Blacklisted;
        }

        public void VoidToken(int tokenId)
        {
            var token = _dbContext.JwtTokens.FirstOrDefault(t => t.Id == tokenId);
            if (token == null){return;}
            token.Blacklisted = true;
            _dbContext.SaveChanges();
        }
    }
}