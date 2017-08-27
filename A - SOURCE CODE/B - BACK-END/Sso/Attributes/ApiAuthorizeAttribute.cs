using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using Autofac;
using Sso.Interfaces.Repositories;

namespace Sso.Attributes
{
    public class ApiAuthorizeAttribute : AuthorizationFilterAttribute
    {
        #region Properties

        /// <summary>
        ///     Autofac lifetime scope.
        /// </summary>
        public ILifetimeScope LifetimeScope { get; set; }

        #endregion

        #region Methods

        /// <summary>
        ///     Override this function for checking whether user is allowed to access function.
        /// </summary>
        /// <param name="httpActionContext"></param>
        /// <returns></returns>
        public override void OnAuthorization(HttpActionContext httpActionContext)
        {
#if ALLOW_ANONYMOUS
            return;
#else
            try
            {
                // Anonymous request is allowed.
                if (IsAllowAnonymousRequest(httpActionContext))
                    return;

                // Search the principle of request.
                var principle = httpActionContext.RequestContext.Principal;

                // Principal is invalid.
                if (principle == null)
                {
                    httpActionContext.Response =
                        httpActionContext.Request.CreateResponse(HttpStatusCode.Unauthorized);
                    return;
                }

                var lifetimeScope = httpActionContext.Request.GetDependencyScope();
                // Search the instance of unit of work.
                var unitOfWork = (IUnitOfWork)lifetimeScope.GetService(typeof(IUnitOfWork));

                #region Principle validation
                
                // Search the identity set in principle.
                var identity = principle.Identity;
                if (identity == null)
                {
                    // Anonymous request is allowed.
                    if (IsAllowAnonymousRequest(httpActionContext))
                        return;

                    httpActionContext.Response =
                        httpActionContext.Request.CreateResponse(HttpStatusCode.Unauthorized);
                    return;
                }

                #endregion

                #region Claim identity

                // Search the claim identity.
                var claimIdentity = (ClaimsIdentity)identity;

                // Claim doesn't contain email.
                var claimEmail = claimIdentity.FindFirst(ClaimTypes.Email);
                if (claimEmail == null || string.IsNullOrEmpty(claimEmail.Value))
                {
                    // Anonymous request is allowed.
                    if (IsAllowAnonymousRequest(httpActionContext))
                        return;

                    httpActionContext.Response =
                        httpActionContext.Request.CreateResponse(HttpStatusCode.Unauthorized);
                    return;
                }

                // Search email in the database.
                var account = unitOfWork.RepositoryAccounts.Search()
                    .FirstOrDefault(
                        x => x.Email.Equals(claimEmail.Value, StringComparison.InvariantCultureIgnoreCase));

                // Account is not found.
                if (account == null)
                {
                    // Anonymous request is allowed.
                    if (IsAllowAnonymousRequest(httpActionContext))
                        return;

                    httpActionContext.Response =
                        httpActionContext.Request.CreateResponse(HttpStatusCode.Unauthorized);
                    return;
                }

                #endregion

                // Insert account information into HttpItem for later use.
                var properties = httpActionContext.Request.Properties;
                if (properties.ContainsKey(ClaimTypes.Actor))
                    properties[ClaimTypes.Actor] = account;
                properties.Add(ClaimTypes.Actor, account);
            }
            catch (Exception exception)
            {
                // Anonymous request is allowed.
                if (IsAllowAnonymousRequest(httpActionContext))
                    return;

                httpActionContext.Response = httpActionContext.Request.CreateErrorResponse(HttpStatusCode.Unauthorized, "");
            }
#endif

        }

        /// <summary>
        ///     Whether method or controller allows anonymous requests or not.
        /// </summary>
        /// <param name="httpActionContext"></param>
        /// <returns></returns>
        private bool IsAllowAnonymousRequest(HttpActionContext httpActionContext)
        {
#if UNAUTHENTICATION_ALLOW
            return true;
#endif
            return httpActionContext.ActionDescriptor.GetCustomAttributes<AllowAnonymousAttribute>().Any()
                   ||
                   httpActionContext.ControllerContext.ControllerDescriptor.GetCustomAttributes<AllowAnonymousAttribute>
                       ().Any();
        }

        #endregion
    }
}