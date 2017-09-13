using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;
using DbModel.Models.Entities;
using JWT;
using Newtonsoft.Json;
using Sso.Enumerations;
using Sso.Interfaces.Services;
using Sso.Models.Identity;

namespace Sso.Services
{
    public class IdentityService : IIdentityService
    {
        #region Properties

        /// <summary>
        /// Jwt encoder.
        /// </summary>
        private readonly IJwtEncoder _jwtEncoder;

        /// <summary>
        /// Jwt decoder.
        /// </summary>
        private readonly IJwtDecoder _jwtDecoder;

        /// <summary>
        /// Url which is for requesting facebook profile.
        /// </summary>
        private const string FacebookProfileUrl = "https://graph.facebook.com/me";

        /// <summary>
        /// Url which is for requesting facebook profile picture.
        /// </summary>
        private const string FacebookUserPictureUrl = "https://graph.facebook.com/v2.10/{0}/picture";


        #endregion

        #region Constructors

        /// <summary>
        /// Initiate
        /// </summary>
        /// <param name="jwtEncoder"></param>
        /// <param name="jwtDecoder"></param>
        public IdentityService(IJwtEncoder jwtEncoder, IJwtDecoder jwtDecoder)
        {
            _jwtEncoder = jwtEncoder;
            _jwtDecoder = jwtDecoder;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Using id token to find account information.
        /// </summary>
        /// <param name="idToken"></param>
        /// <returns></returns>
        public async Task<HttpResponseMessage> FindGoogleIdentity(string idToken)
        {
            using (var httpClient = new HttpClient())
            {
                // Construct url.
                var url = $"https://www.googleapis.com/oauth2/v3/tokeninfo?id_token={idToken}";
                return await httpClient.GetAsync(url);
            }
        }

        /// <summary>
        /// Using id token to obtain facebook information.
        /// </summary>
        /// <param name="idToken"></param>
        /// <returns></returns>
        public async Task<HttpResponseMessage> FindFacebookIdentity(string idToken)
        {
            using (var httpClient = new HttpClient())
            {
                var url = $"{FacebookProfileUrl}?fields=id,email,name,picture&access_token={idToken}";
                return await httpClient.GetAsync(url);
            }
        }

        /// <summary>
        /// Find facebook profile picture url.
        /// </summary>
        /// <returns></returns>
        /// <param name="userId"></param>
        /// <param name="height">The height of this picture in pixels.</param>
        /// <param name="size">The size of this picture</param>
        /// <param name="width">The width of this picture in pixels</param>
        public string FindFacebookProfileImage(string userId, int? height, FacebookProfilePictureSize? size, int? width)
        {
            var parameters = new Dictionary<string, string>();
            if (height != null)
                parameters.Add("height", height.Value.ToString());

            if (size != null)
                parameters.Add("type", size.ToString());

            if (width != null)
                parameters.Add("width", width.ToString());

            // Url construction.
            var url = string.Format(FacebookUserPictureUrl, userId);
            if (parameters.Count > 0)
                url = $"{url}?{string.Join(",", parameters.Select(x => $"{x.Key}={x.Value}"))}";

            return url;
        }
        /// <summary>
        /// Encode jwt from claims and secret.
        /// </summary>
        /// <param name="claims"></param>
        /// <param name="secret"></param>
        /// <returns></returns>
        public string EncodeJwt(Dictionary<string, string> claims, string secret)
        {
            return _jwtEncoder.Encode(claims, secret);
        }

        /// <summary>
        /// Decode jwt using secret.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="jwt"></param>
        /// <param name="secret"></param>
        /// <returns></returns>
        public T DecodeJwt<T>(string jwt, string secret)
        {
            return _jwtDecoder.DecodeToObject<T>(jwt, secret, false);
        }
        
        /// <inheritdoc />
        /// <summary>
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public Account FindRequestIdentity(HttpRequestMessage request)
        {
            // Invalid request.
            if (request == null)
                return null;
            
            // Properties are not valid.
            if (request.Properties == null || !request.Properties.ContainsKey(ClaimTypes.Actor))
                return null;

            return (Account) request.Properties[ClaimTypes.Actor];
        }

        #endregion
    }
}
