using System.Collections.Generic;
using System.Linq;
using Cryptocop.Software.API.Models.Dtos;
using Cryptocop.Software.API.Models.Entities;
using Cryptocop.Software.API.Models.InputModels;
using Cryptocop.Software.API.Repositories.Contexts;
using Cryptocop.Software.API.Repositories.Interfaces;

namespace Cryptocop.Software.API.Repositories.Implementations
{
    public class AddressRepository : IAddressRepository
    {
        private readonly CryptocopDbContext _dbContext;

        public AddressRepository(CryptocopDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public void AddAddress(string email, AddressInputModel address)
        {
            var user = _dbContext.Users.FirstOrDefault(a => a.Email == email);
            if (user == null){return;}

            var nextId = _dbContext.Addresses.Count() + 1;
            _dbContext.Add(new Address{
                Id = nextId,
                UserId = user.Id,
                StreetName = address.StreetName,
                HouseNumber = address.HouseNumber,
                ZipCode = address.ZipCode,
                Country = address.Country,
                City = address.City
            });

            _dbContext.SaveChanges();
        }

        public IEnumerable<AddressDto> GetAllAddresses(string email)
        {
            return _dbContext.Addresses
            .Where(a => a.User.Email == email)
            .Select(a => new AddressDto {
                Id = a.Id,
                StreetName = a.StreetName,
                HouseNumber = a.HouseNumber,
                ZipCode = a.ZipCode,
                Country = a.Country,
                City = a.City
            })
            .ToList();
        }

        public bool DeleteAddress(string email,int addressId)
        {

            var entitiy = _dbContext.Addresses.FirstOrDefault(a => a.User.Email == email && a.Id == addressId);
            if (entitiy == null){return false;}

            _dbContext.Addresses.Remove(entitiy);
            _dbContext.SaveChanges();
            return true;
        }
    }
}