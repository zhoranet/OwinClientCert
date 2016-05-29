using System;
using System.Collections.Generic;
using System.Web.Http;
using Microsoft.Extensions.DependencyInjection;
using Owin;
using OwinServer.Auth;
using OwinServer.Controllers;
using OwinServer.Properties;

namespace OwinServer
{
    public class OwinConfig
    {
        public void Configuration(IAppBuilder app)
        {
            app.UseWebApi(Register());
        }

        private static HttpConfiguration Register()
        {
            var services = BuildServiceDependencies();

            var config = new HttpConfiguration
            {
                DependencyResolver = new DefaultDependencyResolver(services.BuildServiceProvider())
            };

            config.Routes.MapHttpRoute("DefaultApi", "{controller}/{id}", new { id = RouteParameter.Optional });
            return config;
        }

        private static ServiceCollection BuildServiceDependencies()
        {
            var services = new ServiceCollection();
            services.AddSingleton<ICertificateRepository>(sp => new CachedCertificateRepository(GetClientCertThumbprints()));
            services.AddTransient<DefaultCertificateValidator>();
            services.AddTransient<IClientCertificateValidator, DefaultCertificateValidator>();
            services.AddTransient<RestrictedController>();
            services.AddTransient<PublicController>();
            return services;
        }

        private static IEnumerable<string> GetClientCertThumbprints()
        {
            return Settings.Default.AllowedCertThumbprints?.Split(new[] {','},
                StringSplitOptions.RemoveEmptyEntries);
        }
    }
}