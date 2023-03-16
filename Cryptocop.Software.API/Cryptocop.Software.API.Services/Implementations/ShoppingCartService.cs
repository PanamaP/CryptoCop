using Cryptocop.Software.API.Models.Dtos;
using Cryptocop.Software.API.Models.InputModels;
using Cryptocop.Software.API.Repositories.Interfaces;
using Cryptocop.Software.API.Services.Helpers;
using Cryptocop.Software.API.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace Cryptocop.Software.API.Services.Implementations
{
    public class ShoppingCartService : IShoppingCartService
    {
        private readonly IShoppingCartRepository _shoppingCartRepository;
        private readonly HttpClient _httpClient;

        public ShoppingCartService(IShoppingCartRepository shoppingCartRepository, HttpClient httpClient)
        {
            _shoppingCartRepository = shoppingCartRepository;
            _httpClient = httpClient;
        }

        public IEnumerable<ShoppingCartItemDto> GetCartItems(string email)
        {
            return _shoppingCartRepository.GetCartItems(email);
        }

        public async Task<bool> AddCartItem(string email, ShoppingCartItemInputModel shoppingCartItemItem)
        {
            List<string> CryptoFilter = new List<string>{"BTC", "ETH", "USDT", "XMR"};
            var assetKey = shoppingCartItemItem.ProductIdentifier;
            if (CryptoFilter.Contains(assetKey)){return false;}

            var response = await _httpClient.GetAsync($"{assetKey}/metrics?fields=id,symbol,name,market_data/price_usd");
            if (!response.IsSuccessStatusCode){return false;} 

            var responseObject = await HttpResponseMessageExtensions.DeserializeJsonToObject<CryptoCurrencyDto>(response, true);
            _shoppingCartRepository.AddCartItem(email, shoppingCartItemItem, (float) responseObject.PriceInUsd);
            return true;
            
        }

        public bool RemoveCartItem(string email, int id)
        {
            return _shoppingCartRepository.RemoveCartItem(email, id);
        }

        public bool UpdateCartItemQuantity(string email, int id, float quantity)
        {
            return _shoppingCartRepository.UpdateCartItemQuantity(email, id, quantity);
        }

        public void ClearCart(string email)
        {
            _shoppingCartRepository.ClearCart(email);
        }
    }
}
