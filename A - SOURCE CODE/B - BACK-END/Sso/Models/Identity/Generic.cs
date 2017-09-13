using System;
using System.Collections.Generic;
using System.Security.Claims;
using DbModel.Enumerations;
using DbModel.Models.Entities;

namespace Sso.Models.Identity
{
    public class Generic
    {
        #region Properties

        /// <summary>
        /// Email of account.
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// Photo url.
        /// </summary>
        public string Photo { get; set; }

        /// <summary>
        /// Role of account.
        /// </summary>
        public Role Role { get; set; }
        
        /// <summary>
        /// Time when token should be expired.
        /// </summary>
        public double ExpirationTime { get; set; }

        #endregion

        #region Constructor

        /// <summary>
        /// Initiate generic token with default information.
        /// </summary>
        public Generic()
        {
            
        }

        /// <summary>
        ///  Initiate generic token with given information.
        /// </summary>
        /// <param name="email"></param>
        /// <param name="photo"></param>
        /// <param name="role"></param>
        /// <param name="expiration"></param>
        public Generic(string email, string photo, Role role, double expiration)
        {
            Email = email;
            Photo = photo;
            Role = role;
            ExpirationTime = expiration;
        }

        /// <summary>
        /// Initiate generic token using account information.
        /// </summary>
        /// <param name="account"></param>
        public Generic(Account account)
        {
            Email = account.Email;
            Photo = account.PhotoUrl;
            Role = account.Role;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Get claim from object.
        /// </summary>
        /// <returns></returns>
        public Dictionary<string, string> ToClaims()
        {
            var claims = new Dictionary<string, string>();
            claims.Add(ClaimTypes.Email, Email);
            claims.Add(ClaimTypes.Uri, Photo);
            claims.Add(ClaimTypes.Role, $"{Role}");
            claims.Add(ClaimTypes.Expiration, ExpirationTime.ToString("N"));
            return claims;
        }

        #endregion
    }
}