using System.Web.Http;
using Newtonsoft.Json.Serialization;
using Owin;
using Sso.Configs;

namespace Sso
{
    public class Startup
    {
        #region Methods

        // This code configures Web API. The Startup class is specified as a type
        // parameter in the WebApp.Start method.
        public void Configuration(IAppBuilder appBuilder)
        {
            // Configure Web API for self-host. 
            var httpConfiguration = new HttpConfiguration();

            // Use routing attribute.
            httpConfiguration.MapHttpAttributeRoutes();
            
            // Route navigation configuration.
            httpConfiguration.Routes.MapHttpRoute(
                "DefaultApi",
                "api/{controller}/{id}",
                new {id = RouteParameter.Optional}
            );

            // Use camel-cased json formatter.
            var formatters = httpConfiguration.Formatters;
            var jsonFormatter = formatters.JsonFormatter;
            jsonFormatter.UseDataContractJsonSerializer = false;
            var settings = jsonFormatter.SerializerSettings;
            settings.ContractResolver = new CamelCasePropertyNamesContractResolver();

            // Register inversion of control.
            FilterConfig.Register(httpConfiguration);
            IocConfig.Register(appBuilder, httpConfiguration);
            
            // Register web API module.
            appBuilder.UseWebApi(httpConfiguration);
        }

        #endregion
    }
}