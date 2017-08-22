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

        #endregion
    }
}
