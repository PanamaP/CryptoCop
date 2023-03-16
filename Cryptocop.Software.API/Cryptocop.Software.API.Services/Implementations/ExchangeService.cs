using System;
using System.Net.Http;
using System.Threading.Tasks;
using Cryptocop.Software.API.Models;
using Cryptocop.Software.API.Models.Dtos;
using Cryptocop.Software.API.Services.Helpers;
using Cryptocop.Software.API.Services.Interfaces;

namespace Cryptocop.Software.API.Services.Implementations
{
    public class ExchangeService : IExchangeService
    {
        private HttpClient _httpClient;

        public ExchangeService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<Envelope<ExchangeDto>> GetExchanges(int pageNumber = 1)
        {
            var response = await _httpClient.GetAsync($"?page={pageNumber}");
            if (!response.IsSuccessStatusCode)
            {
                throw new Exception("There was an error fetching exchanges");
            } 
            
            var responseObject = await HttpResponseMessageExtensions.DeserializeJsonToList<ExchangeDto>(response, true);
            Envelope<ExchangeDto> exchanges = new Envelope<ExchangeDto>();
            exchanges.PageNumber = pageNumber;
            exchanges.Items = responseObject;
            return exchanges;
            

        }
    }
}