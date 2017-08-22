using SharedService.Interfaces;
using System.Net.Http;
using System.Threading.Tasks;

namespace SharedService.Services
{
    public class IdentityService : IIdentityService
    {
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
    }
}
