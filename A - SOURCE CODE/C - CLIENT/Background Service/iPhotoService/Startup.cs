using Newtonsoft.Json.Serialization;
using Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

namespace iPhotoService
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
                new { id = RouteParameter.Optional }
            );

            // Use camel-cased json formatter.
            var formatters = httpConfiguration.Formatters;
            var jsonFormatter = formatters.JsonFormatter;
            jsonFormatter.UseDataContractJsonSerializer = false;
            var settings = jsonFormatter.SerializerSettings;
            settings.ContractResolver = new CamelCasePropertyNamesContractResolver();
            
            // Register web API module.
            appBuilder.UseWebApi(httpConfiguration);
        }

        #endregion
    }
}
