using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using Sso.Middlewares;

namespace Sso.Configs
{
    public class FilterConfig
    {
        #region Methods

        public static void Register(HttpConfiguration httpConfiguration)
        {
            // Authentication middleware registration.
            httpConfiguration.Filters.Add(new BearerAuthenticationMiddleware());
        }

#endregion
    }
}
