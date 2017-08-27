using Newtonsoft.Json;

namespace Sso.Models.Identity
{
    public class Google
    {
        #region Properties

        /// <summary>
        /// Token audience.
        /// </summary>
        [JsonProperty("aud")]
        public string Audience { get; set; }

        /// <summary>
        /// Subscriber.
        /// </summary>
        [JsonProperty("sub")]
        public string Subscriber { get; set; }

        /// <summary>
        /// Email account.
        /// </summary>
        [JsonProperty("email")]
        public string Email { get; set; }

        /// <summary>
        /// Whether account is verified or not.
        /// </summary>
        [JsonProperty("is_verified")]
        public bool IsVerified { get; set; }

        /// <summary>
        /// Issuer
        /// </summary>
        [JsonProperty("iss")]
        public string Issuer { get; set; }

        /// <summary>
        /// Time when token was issued.
        /// </summary>
        [JsonProperty("iat")]
        public double IssueTime { get; set; }

        /// <summary>
        /// Time when token is expired.
        /// </summary>
        [JsonProperty("exp")]
        public double Expiration { get; set; }

        /// <summary>
        /// Name of account.
        /// </summary>
        [JsonProperty("name")]
        public string Name { get; set; }

        /// <summary>
        /// Account photo url.
        /// </summary>
        [JsonProperty("picture")]
        public string Photo { get; set; }

        /// <summary>
        /// Locale of account.
        /// </summary>
        [JsonProperty("locale")]
        public string Locale { get; set; }

        #endregion
    }
}