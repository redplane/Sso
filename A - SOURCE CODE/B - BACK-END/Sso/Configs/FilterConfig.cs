using System.Web.Http;
using Sso.Filters;
using Sso.Middlewares;

namespace Sso.Configs
{
    public static class FilterConfig
    {
        #region Methods

        public static void Register(HttpConfiguration httpConfiguration)
        {
            // Authentication middleware registration.
            httpConfiguration.Filters.Add(new BearerAuthenticationMiddleware());

            // Global exception filter registration,
            httpConfiguration.Filters.Add(new GlobalExceptionFilter());
        }

        #endregion
    }
}