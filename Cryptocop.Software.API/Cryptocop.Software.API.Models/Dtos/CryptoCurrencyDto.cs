
using Newtonsoft.Json;

namespace Cryptocop.Software.API.Models.Dtos
{
    public class CryptoCurrencyDto
    {
        [JsonProperty("id")]
        public string Id { get; set; }
        [JsonProperty("symbol")]
        public string Symbol { get; set; }
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("slug")]
        public string Slug { get; set; }
        [JsonProperty("price_usd")]
        public float? PriceInUsd { get; set; }
        [JsonProperty("project_details")]
        public string ProjectDetails { get; set; }

    }
}