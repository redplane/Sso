using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http.Filters;
using Autofac.Integration.WebApi;
using JWT;
using log4net;
using Sso.Models.Identity;

namespace Sso.Middlewares
{
    public class BearerAuthenticationMiddleware : IAuthenticationFilter
    {
        #region Constructors

        /// <summary>
        ///     Initiate middleware instance with default logging.
        /// </summary>
        public BearerAuthenticationMiddleware()
        {
            Log = LogManager.GetLogger(typeof(BearerAuthenticationMiddleware));
        }

        #endregion

        #region Properties

        /// <summary>
        ///     Whether multiple authentication is supported or not.
        /// </summary>
        public bool AllowMultiple => false;

        /// <summary>
        ///     Provider which provides functions to analyze and validate token.
        /// </summary>
        public JwtSetting JwtSetting { get; set; }

        /// <summary>
        /// Decoder which is for decoding jwt token.
        /// </summary>
        public IJwtDecoder JwtDecoder { get; set; }

        /// <summary>
        ///     Instance which serves logging process of log4net.
        /// </summary>
        public ILog Log { get; set; }

        #endregion

        #region Methods

        /// <summary>
        ///     Authenticate a request asynchronously.
        /// </summary>
        /// <param name="httpAuthenticationContext"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task AuthenticateAsync(HttpAuthenticationContext httpAuthenticationContext,
            CancellationToken cancellationToken)
        {
            var dependencyScope = httpAuthenticationContext.Request.GetDependencyScope();
            if (JwtSetting == null)
                JwtSetting = (JwtSetting)dependencyScope.GetService(typeof(JwtSetting));

            if (JwtDecoder == null)
                JwtDecoder = (IJwtDecoder)dependencyScope.GetService(typeof(IJwtDecoder));

            // Account has been authenticated before token is parsed.
            // Skip the authentication.
            var principal = httpAuthenticationContext.Principal;
            if (principal != null && principal.Identity != null && principal.Identity.IsAuthenticated)
                return Task.FromResult(0);

            // Search the authorization in the header.
            var authorization = httpAuthenticationContext.Request.Headers.Authorization;

            // Bearer token is detected.
            if (authorization == null)
                return Task.FromResult(0);

            // Scheme is not bearer.
            if (!"Bearer".Equals(authorization.Scheme,
                StringComparison.InvariantCultureIgnoreCase))
                return Task.FromResult(0);

            // Token parameter is not defined.
            var token = authorization.Parameter;
            if (string.IsNullOrWhiteSpace(token))
                return Task.FromResult(0);

            try
            {
                // Decode the token and set to claim. The object should be in dictionary.
                var claimPairs = JwtDecoder.DecodeToObject<Dictionary<string, string>>(token,
                    JwtSetting.Key, false);

                var claimIdentity = new ClaimsIdentity(null, JwtSetting.Name);
                foreach (var key in claimPairs.Keys)
                    claimIdentity.AddClaim(new Claim(key, claimPairs[key]));

                // Authenticate the request.
                httpAuthenticationContext.Principal = new ClaimsPrincipal(claimIdentity);
            }
            catch (Exception exception)
            {
                // Suppress error.
                Log.Error(exception.Message, exception);
            }

            return Task.FromResult(0);


        }

        /// <summary>
        ///     Callback which is called after the authentication which to handle the result.
        /// </summary>
        /// <param name="httpAuthenticationChallengeContext"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task ChallengeAsync(HttpAuthenticationChallengeContext httpAuthenticationChallengeContext,
            CancellationToken cancellationToken)
        {
            return Task.FromResult(0);
        }
        
        #endregion
    }
}