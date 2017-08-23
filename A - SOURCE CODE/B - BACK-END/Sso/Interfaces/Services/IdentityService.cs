using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using JWT;
using SharedService.Interfaces;

namespace Sso.Interfaces.Services
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
            // Construct url.
            var url = $"https://www.googleapis.com/oauth2/v3/tokeninfo?id_token={idToken}";

            using (var httpClient = new HttpClient())
            {
                return await httpClient.GetAsync(url);
            }
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
        #endregion
    }
}
