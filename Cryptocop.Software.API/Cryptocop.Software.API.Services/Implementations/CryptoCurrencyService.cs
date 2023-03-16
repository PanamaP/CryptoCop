using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Cryptocop.Software.API.Models.Dtos;
using Cryptocop.Software.API.Services.Helpers;
using Cryptocop.Software.API.Services.Interfaces;

namespace Cryptocop.Software.API.Services.Implementations
{
    public class CryptoCurrencyService : ICryptoCurrencyService
    {
        private readonly HttpClient _httpClient;

        public CryptoCurrencyService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<IEnumerable<CryptoCurrencyDto>> GetAvailableCryptocurrencies()
        {
            // var response = await _httpClient.GetAsync("");
            // if (!response.IsSuccessStatusCode)
            // {
            //     throw new Exception("There was an error fetching available currencies");
            // } 
            List<string> CryptoFilter = new List<string>{"BTC", "ETH", "USDT", "XMR"};
            var response = await _httpClient.GetAsync("?limit=200&fields=id,name,slug,symbol,metrics/market_data/price_usd,profile/general/overview/project_details");
            response.EnsureSuccessStatusCode();

            var items = await response.DeserializeJsonToList<CryptoCurrencyDto>(true);
            return items.Where(i => CryptoFilter.Contains(i.Symbol));

        }
    }
}