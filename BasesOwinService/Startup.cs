using Microsoft.Owin;
using Microsoft.Owin.Security.OAuth;
using Owin;
using BaseOwinService;

[assembly: OwinStartup(typeof (Startup))]

namespace BaseOwinService
{
    using System;
    using System.Web.Http;
    using BaseOwinService.Providers;
    using Helpers;
    using Microsoft.Practices.Unity;

    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            var config = new HttpConfiguration();

            app.UseCors(Microsoft.Owin.Cors.CorsOptions.AllowAll);

            ConfigureOAuth(app);
            ConfigureDependencies(config);

            config.Filters.Add(new AuthorizeAttribute());
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute("DefaultAPI",
                "api/{controller}/{id}/{param}",
                new { id = RouteParameter.Optional, param = RouteParameter.Optional });

            app.UseWebApi(config);
        }

        public void ConfigureDependencies(HttpConfiguration config)
        {
            var container = new UnityContainer();
            // TODO: register database
            //container.RegisterType<IDbContext, Database>();
            config.DependencyResolver = new UnityResolver(container);
        }

        public void ConfigureOAuth(IAppBuilder app)
        {
            var oAuthServerOptions = new OAuthAuthorizationServerOptions
            {
                AllowInsecureHttp = true,
                TokenEndpointPath = new PathString("/token"),
                AccessTokenExpireTimeSpan = TimeSpan.FromDays(1),
                Provider = new ApiAuthorizationServerProvider(),
                RefreshTokenProvider = new RefreshTokenProvider()
            };

            // Token Generation
            app.UseOAuthAuthorizationServer(oAuthServerOptions);
            app.UseOAuthBearerAuthentication(new OAuthBearerAuthenticationOptions());
        }
    }
}