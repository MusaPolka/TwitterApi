using Newtonsoft.Json;

namespace DomainLayer.Entities
{
    public class Product
    {
        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }

        [JsonProperty(PropertyName = "name")]
        public string? Name { get; set; }

        [JsonProperty(PropertyName = "details")]
        public string? Details { get; set; }
    }
}
