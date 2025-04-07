using System.Text.Json.Serialization;

namespace backend.Models
{
    public class OfacResponse{
        [JsonPropertyName("web")]
        public string? Web { get; set; }

        [JsonPropertyName("name")]
        public string? Name { get; set; }

        [JsonPropertyName("address")]
        public string? Address { get; set; }

        [JsonPropertyName("type")]
        public string? Type { get; set; }

        [JsonPropertyName("program")]
        public string? Program { get; set; }

        [JsonPropertyName("list")]
        public string? List { get; set; }

        [JsonPropertyName("score")]
        public string? Score { get; set; }
    }
}