using System.Text.Json.Serialization;

namespace backend.Models
{
    public class Supplier{
        public Guid Id { get; set; }

        [JsonPropertyName("businessName")]
        public required string BusinessName { get; set; }

        [JsonPropertyName("tradeName")]
        public required string TradeName { get; set; }

        [JsonPropertyName("taxId")]
        public required string TaxId { get; set; } // RUC (11 dígitos)

        [JsonPropertyName("phone")]
        public required string Phone { get; set; }

        [JsonPropertyName("email")]
        public required string Email { get; set; }

        [JsonPropertyName("website")]
        public required string Website { get; set; }

        [JsonPropertyName("address")]
        public required string Address { get; set; }

        [JsonPropertyName("country")]
        public required string Country { get; set; }

        [JsonPropertyName("annualBilling")]
        public decimal AnnualBilling { get; set; }

        [JsonPropertyName("lastEdited")]
        public DateTime LastEdited { get; set; }
    }
}