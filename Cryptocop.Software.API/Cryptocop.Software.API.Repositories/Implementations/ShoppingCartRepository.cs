using System.Collections.Generic;
using System.Linq;
using Cryptocop.Software.API.Models.Dtos;
using Cryptocop.Software.API.Models.Entities;
using Cryptocop.Software.API.Models.InputModels;
using Cryptocop.Software.API.Repositories.Contexts;
using Cryptocop.Software.API.Repositories.Interfaces;

namespace Cryptocop.Software.API.Repositories.Implementations
{
    public class ShoppingCartRepository : IShoppingCartRepository
    {
        private readonly CryptocopDbContext _dbContext;

        public ShoppingCartRepository(CryptocopDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IEnumerable<ShoppingCartItemDto> GetCartItems(string email)
        {
            var user = _dbContext.Users.FirstOrDefault(a => a.Email == email);
            if (user == null){return null;}

            return _dbContext.ShoppingCartItems
            .Where(s => s.ShoppingCart.User.Email == email)
            .Select(s => new ShoppingCartItemDto{
                Id = s.Id,
                ProductIdentifier = s.ProductIdentifier,
                Quantity = s.Quantity,
                UnitPrice = s.UnitPrice,
                TotalPrice = s.Quantity * s.UnitPrice
            })
            .ToList();

        }

        public void AddCartItem(string email, ShoppingCartItemInputModel shoppingCartItemItem, float priceInUsd)
        {

            var user = _dbContext.Users.FirstOrDefault(a => a.Email == email);
            if (user == null){return;}

            var shoppingCart = _dbContext.ShoppingCarts.FirstOrDefault(s => s.UserId == user.Id);
            var nextId = _dbContext.ShoppingCartItems.Count() + 1;

            _dbContext.Add(new ShoppingCartItem{
                Id = nextId,
                ShoppingCartId = shoppingCart.Id,
                ProductIdentifier = shoppingCartItemItem.ProductIdentifier,
                Quantity = (float) shoppingCartItemItem.Quantity,
                UnitPrice = priceInUsd
            });

            _dbContext.SaveChanges();
        }

        public bool RemoveCartItem(string email, int id)
        {
            var user = _dbContext.Users.FirstOrDefault(a => a.Email == email);
            if (user == null){return false;}

            var shoppingCart = _dbContext.ShoppingCarts.FirstOrDefault(s => s.UserId == user.Id);
            var entitiy = _dbContext.ShoppingCartItems.FirstOrDefault(s => s.Id == id && s.ShoppingCartId == shoppingCart.Id);
            if (entitiy == null){return false;}

            _dbContext.Remove(entitiy);
            _dbContext.SaveChanges();
            return true;
        }

        public bool UpdateCartItemQuantity(string email, int id, float quantity)
        {
            var user = _dbContext.Users.FirstOrDefault(a => a.Email == email);
            if (user == null){return false;}

            var entitiy = _dbContext.ShoppingCartItems.FirstOrDefault(s => s.ShoppingCart.User.Email == email && s.Id == id);
            if (entitiy == null){return false;}

            entitiy.Quantity = quantity;
            _dbContext.SaveChanges();
            return true;
        }

        public void ClearCart(string email)
        {
            var user = _dbContext.Users.FirstOrDefault(a => a.Email == email);
            if (user == null){return;}

            var entities = _dbContext.ShoppingCartItems.Where(s => s.ShoppingCart.User.Email == email);
            if (entities == null){return;}
            
            foreach (var entity in entities)
            {
                _dbContext.Remove(entity);
            }
            _dbContext.SaveChanges();
        }

        public void DeleteCart(string email)
        { //There is nothing that calls this, this would be implemented if user could delete their account
            throw new System.NotImplementedException();
        }
    }
}