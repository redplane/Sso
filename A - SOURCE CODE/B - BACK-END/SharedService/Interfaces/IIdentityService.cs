using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace SharedService.Interfaces
{
    public interface IIdentityService
    {
        #region Methods

        /// <summary>
        /// Find google identity information.
        /// </summary>
        /// <returns></returns>
        Task<HttpResponseMessage> FindGoogleIdentity(string idToken);

        /// <summary>
        /// Encode jwt from claims and secret.
        /// </summary>
        /// <param name="claims"></param>
        /// <param name="secret"></param>
        /// <returns></returns>
        string EncodeJwt(Dictionary<string, string> claims, string secret);

        #endregion
    }
}
