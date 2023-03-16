using System;
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
    public class OrderRepository : IOrderRepository
    {
        private readonly CryptocopDbContext _dbContext;

        public OrderRepository(CryptocopDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IEnumerable<OrderDto> GetOrders(string email)
        {
            var user = _dbContext.Users.FirstOrDefault(a => a.Email == email);
            if (user == null){return null;}


            var allOrders =  _dbContext.Orders
            .Where(o => o.Email == email)
            .Select(o => new OrderDto
            {
                Id = o.id,
                Email = o.Email,
                FullName = o.FullName,
                StreetName = o.StreetName,
                HouseNumber = o.HouseNumber,
                ZipCode = o.ZipCode,
                Country = o.Country,
                City = o.City,
                CardHolderName = o.CardHolderName,
                CreditCard = o.MaskedCreditCard,
                OrderDate = o.OrderDate.ToString(),
                TotalPrice = o.TotalPrice,
                OrderItems = _dbContext.OrderItems
                .Where(a => a.OrderId == o.id)
                .Select(a => new OrderItemDto{
                    Id = a.Id,
                    ProductIdentifier = a.ProductIdentifier,
                    Quantity = a.Quantity,
                    UnitPrice = a.UnitPrice,
                    TotalPrice = a.TotalPrice
                }).ToList()
            });

            return allOrders;
        }

        public OrderDto CreateNewOrder(string email, OrderInputModel order)
        {
            var user = _dbContext.Users.FirstOrDefault(a => a.Email == email);
            if (user == null){return null;}

            var address = _dbContext.Addresses.FirstOrDefault(a => a.UserId == user.Id && a.Id == order.AddressId);
            if (address == null){return null;}

            var paymentCard = _dbContext.PaymentCards.FirstOrDefault(p => p.User.Email == email && p.Id == order.PaymentCardId);
            if (paymentCard == null){return null;}

            var cartItems = _dbContext.ShoppingCartItems
            .Where(s => s.ShoppingCart.User.Email == email)
            .Select(s => new ShoppingCartItemDto{
                Id = s.Id,
                ProductIdentifier = s.ProductIdentifier,
                Quantity = s.Quantity,
                UnitPrice = s.UnitPrice,
                TotalPrice = s.Quantity * s.UnitPrice
            });
           

            float totalPrice = 0;
            foreach (var item in cartItems)
            {
                totalPrice += item.UnitPrice * item.Quantity;
            }
            if (totalPrice <= 0){return null;}

            var maskedCard = PaymentCardHelper.MaskPaymentCard(paymentCard.CardNumber);
            var nextId = _dbContext.Orders.Count() + 1;
            var dateNow = DateTime.UtcNow;
            var dateFormatForOrderDto = dateNow.Day + "." + dateNow.Month + "." + dateNow.Year;

            _dbContext.Orders.Add(new Order{
                id = nextId,
                Email = user.Email,
                FullName = user.FullName,
                StreetName = address.StreetName,
                HouseNumber = address.HouseNumber,
                ZipCode = address.ZipCode,
                Country = address.Country,
                City = address.City,
                CardHolderName = paymentCard.CardHolderName,
                MaskedCreditCard = maskedCard,
                OrderDate = dateNow,
                TotalPrice = totalPrice
            });

            // writes Order items to database
            foreach (var orderitem in cartItems)
            {
                var newOrderItem = new OrderItem{
                    OrderId = nextId,
                    ProductIdentifier = orderitem.ProductIdentifier,
                    Quantity = orderitem.Quantity,
                    UnitPrice = orderitem.UnitPrice,
                    TotalPrice = orderitem.TotalPrice
                };
                _dbContext.OrderItems.Add(newOrderItem);
            }
            _dbContext.SaveChanges();


            return new OrderDto{
                Id = nextId,
                Email = user.Email,
                FullName = user.FullName,
                StreetName = address.StreetName,
                HouseNumber = address.HouseNumber,
                ZipCode = address.ZipCode,
                Country = address.Country,
                City = address.City,
                CardHolderName = paymentCard.CardHolderName,
                CreditCard = paymentCard.CardNumber,
                OrderDate = dateFormatForOrderDto,
                TotalPrice = totalPrice,
                OrderItems = _dbContext.OrderItems
                .Where(o => o.OrderId == nextId)
                .Select(c => new OrderItemDto{
                    Id = c.Id,
                    ProductIdentifier = c.ProductIdentifier,
                    Quantity = c.Quantity,
                    UnitPrice = c.UnitPrice,
                    TotalPrice = c.Quantity * c.UnitPrice
                }).ToList()
            };
        }
    }
}