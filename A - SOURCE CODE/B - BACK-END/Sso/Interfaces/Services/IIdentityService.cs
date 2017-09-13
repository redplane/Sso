using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using DbModel.Models.Entities;
using Sso.Enumerations;
using Sso.Models.Identity;

namespace Sso.Interfaces.Services
{
    public interface IIdentityService
    {
        #region Methods

        /// <summary>
        /// Using id token to find account information.
        /// </summary>
        /// <param name="idToken"></param>
        /// <returns></returns>
        Task<HttpResponseMessage> FindGoogleIdentity(string idToken);

        /// <summary>
        /// Using id token to find facebook profile information.
        /// </summary>
        /// <param name="idToken"></param>
        /// <returns></returns>
        Task<HttpResponseMessage> FindFacebookIdentity(string idToken);

        /// <summary>
        /// Find facebook profile picture url.
        /// </summary>
        /// <returns></returns>
        /// <param name="userId">User index</param>
        /// <param name="height">The height of this picture in pixels.</param>
        /// <param name="size">The size of this picture</param>
        /// <param name="width">The width of this picture in pixels</param>
        string FindFacebookProfileImage(string userId, int? height, FacebookProfilePictureSize? size, int? width);

        /// <summary>
        /// Encode jwt from claims and secret.
        /// </summary>
        /// <param name="claims"></param>
        /// <param name="secret"></param>
        /// <returns></returns>
        string EncodeJwt(Dictionary<string, string> claims, string secret);

        /// <summary>
        /// Decode jwt using secret.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="jwt"></param>
        /// <param name="secret"></param>
        /// <returns></returns>
        T DecodeJwt<T>(string jwt, string secret);

        /// <summary>
        /// Find identity from request.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Account FindRequestIdentity(HttpRequestMessage request);
        
        #endregion
    }
}