using System;
using System.Configuration;
using System.Data.Entity;
using System.Web.Http;
using System.Web.Http.Filters;
using Autofac;
using Autofac.Integration.WebApi;
using Database.Models.Contexts;
using JWT;
using JWT.Algorithms;
using JWT.Serializers;
using Owin;
using Sso.Controllers;
using Sso.Interfaces.Repositories;
using Sso.Interfaces.Services;
using Sso.Models.Identity;
using Sso.Repositories;
using Sso.Services;
using Sso.Middlewares;

namespace Sso.Configs
{
    public class IocConfig
    {
        #region Methods

        /// <summary>
        ///     Register list of inversion of controls.
        /// </summary>
        /// <param name="app"></param>
        /// <param name="httpConfiguration"></param>
        public static void Register(IAppBuilder app, HttpConfiguration httpConfiguration)
        {
            // Initiate container builder to register dependency injection.
            var containerBuilder = new ContainerBuilder();

            #region Controllers & hubs

            // Controllers & hubs
            containerBuilder.RegisterApiControllers(typeof(Startup).Assembly);

            #endregion

            #region Unit of work & Database context

            // Initiate token serializer.
            IJwtAlgorithm jwtAlgorithm = new HMACSHA256Algorithm();
            IJsonSerializer jsonSerializer = new JsonNetSerializer();
            IBase64UrlEncoder urlEncoder = new JwtBase64UrlEncoder();
            IDateTimeProvider provider = new UtcDateTimeProvider();
            IJwtValidator jwtValidator = new JwtValidator(jsonSerializer, provider);
            IJwtEncoder jwtEncoder = new JwtEncoder(jwtAlgorithm, jsonSerializer, urlEncoder);
            var jwtDecoder = new JwtDecoder(jsonSerializer, jwtValidator, urlEncoder);

            containerBuilder.RegisterInstance(jwtEncoder).As<IJwtEncoder>();
            containerBuilder.RegisterInstance(jwtDecoder).As<IJwtDecoder>();
            
            // Read token settings.
            var tokenSetting = new JwtSetting();
            tokenSetting.Name = ConfigurationManager.AppSettings["token-name"];
            tokenSetting.Key = ConfigurationManager.AppSettings["token-key"];
            tokenSetting.LifeTime = Convert.ToInt32(ConfigurationManager.AppSettings["token-life-time"]);
            tokenSetting.Type = ConfigurationManager.AppSettings["token-type"];

            // Register token settings into system.
            containerBuilder.RegisterInstance(tokenSetting).As<JwtSetting>() .SingleInstance();
            
            // Database context initialization.
            containerBuilder.RegisterType<RelationalDbContext>().As<DbContext>().InstancePerLifetimeScope();

            // Unit of work registration.
            containerBuilder.RegisterType<UnitOfWork>().As<IUnitOfWork>().InstancePerLifetimeScope();

            // Register services.
            containerBuilder.RegisterType<SystemFileService>().As<ISystemFileService>().InstancePerLifetimeScope();
            containerBuilder.RegisterType<IdentityService>().As<IIdentityService>().InstancePerLifetimeScope();
            containerBuilder.RegisterType<EncryptionService>().As<IEncryptionService>().InstancePerLifetimeScope();
            containerBuilder.RegisterType<SystemTimeService>().As<ISystemTimeService>().InstancePerLifetimeScope();

            #endregion

            #region IoC build

            // Container build.
            containerBuilder.RegisterWebApiFilterProvider(httpConfiguration);
            var container = containerBuilder.Build();

            // Attach DI resolver.
            httpConfiguration.DependencyResolver = new AutofacWebApiDependencyResolver(container);

            // Attach dependency injection into configuration.
            app.UseAutofacMiddleware(container);
            app.UseAutofacWebApi(httpConfiguration);

            #endregion
        }

        #endregion
    }
}