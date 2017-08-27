using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Sso.Models.Identity
{
    public class Facebook
    {
        #region Properties

        /// <summary>
        /// Email address (should be unique value)
        /// </summary>
        [JsonProperty("email")]
        public string Email { get; set; }

        /// <summary>
        /// Display name of facebook
        /// </summary>
        [JsonProperty("name")]
        public string Name { get; set; }

        /// <summary>
        /// Profile picture.
        /// </summary>
        [JsonProperty("picture")]
        public JObject Picture { get; set; }

        #endregion
    }
}