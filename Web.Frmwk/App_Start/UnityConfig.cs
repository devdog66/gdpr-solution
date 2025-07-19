using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;
using System.Net.Http;
using System.Web.Mvc;
using Unity;
using Unity.Lifetime;
using Unity.Mvc5;
using Web.Frmwk.App_Start;
using WebUtils.Config;
using WebUtils.Frmwk.Config;
using WebUtils.Frmwk.Logging;
using WebUtils.Frmwk.Security;
using WebUtils.Logging;
using WebUtils.Security;
using WebUtils.Services;
using WebUtils.Standard.ServiceClients;
using WebUtils.Standard.Web;
using WebUtils.Web;

namespace Web.Frmwk
{
    public static class UnityConfig
    {
        public static void RegisterComponents()
        {
			var container = new UnityContainer();

            // register all your components with the container here
            // it is NOT necessary to register your controllers

            // e.g. container.RegisterType<ITestService, TestService>();

            container.RegisterFactory<IMemoryCache>(
            c =>
            {
                var optionsAccessor = new MemoryCacheOptions
                {
                    SizeLimit = 2,
                    TrackStatistics = true
                };
                return new MemoryCache(optionsAccessor);
            }, new ContainerControlledLifetimeManager());

            container.RegisterType<IConsentServiceClient, ConsentServiceClient>();
            container.RegisterType<ILogManager, NLogManagerAdapter>();
            container.RegisterType<IConfigContract, ConfigAdapter>();
            container.RegisterType<IProtectionProvider, ProtectionProvider>();
            container.RegisterType<IVault, FakeVault>();
            container.RegisterType<ICacheFacade, CacheImpl>();

            var services = new ServiceCollection();
            services.AddHttpClient();
            var serviceProvider = services.BuildServiceProvider();
            container.RegisterInstance(serviceProvider.GetService<IHttpClientFactory>());

            DependencyResolver.SetResolver(new UnityDependencyResolver(container));
        }
    }
}