using Newtonsoft.Json;

namespace SharedService.Models
{
    public class IdentityGoogle
    {
        [JsonProperty("aud")]
        public string Audience { get; set; }
        
        [JsonProperty("sub")]
        public string Subscriber { get; set; }

        [JsonProperty("email")]
        public string Email { get; set; }

        [JsonProperty("email_verified")]
        public string IsAvailable { get; set; }

        [JsonProperty("iss")]
        public string Issuer { get; set; }

        [JsonProperty("iat")]
        public double IssueTime { get; set; }

        [JsonProperty("exp")]
        public double Expiration { get; set; }

        [JsonProperty("name")]
        public string FullName { get; set; }

        [JsonProperty("picture")]
        public string PhotoUrl { get; set; }
        
        [JsonProperty("locale")]
        public string Locale { get; set; }
    }
}