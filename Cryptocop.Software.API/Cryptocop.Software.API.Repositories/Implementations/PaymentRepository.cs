using System.Collections.Generic;
using System.Linq;
using Cryptocop.Software.API.Models.Dtos;
using Cryptocop.Software.API.Models.Entities;
using Cryptocop.Software.API.Models.InputModels;
using Cryptocop.Software.API.Repositories.Contexts;
using Cryptocop.Software.API.Repositories.Helpers;
using Cryptocop.Software.API.Repositories.Interfaces;

namespace Cryptocop.Software.API.Repositories.Implementations
{
    public class PaymentRepository : IPaymentRepository
    {
        private readonly CryptocopDbContext _dbContext;

        public PaymentRepository(CryptocopDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public void AddPaymentCard(string email, PaymentCardInputModel paymentCard)
        {
            var user = _dbContext.Users.FirstOrDefault(a => a.Email == email);
            if (user == null){return;}

            var nextId = _dbContext.PaymentCards.Count() + 1;

            _dbContext.PaymentCards.Add(new PaymentCard{
                Id = nextId,
                UserId = user.Id,
                CardHolderName = paymentCard.CardHolderName,
                CardNumber = paymentCard.CardNumber,
                Month = paymentCard.Month,
                Year = paymentCard.Year
            });

            _dbContext.SaveChanges();
        }

        public IEnumerable<PaymentCardDto> GetStoredPaymentCards(string email)
        {
            var user = _dbContext.Users.FirstOrDefault(a => a.Email == email);
            if (user == null){return null;}

            return _dbContext.PaymentCards
            .Where(p => p.User.Email == email)
            .Select(p => new PaymentCardDto{
                Id = p.Id,
                CardHolderName = p.CardHolderName,
                CardNumber = p.CardNumber,
                Month = p.Month,
                Year = p.Year
            }).ToList();
        }
    }
}