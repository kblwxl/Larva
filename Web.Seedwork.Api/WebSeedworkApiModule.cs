using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http.Dispatcher;
using Web.Seedwork.Api.Configuration;
using Web.Seedwork.Api.Filters;

namespace Web.Seedwork.Api
{
    [Infrastructure.Modules.DependsOn(
        typeof(WebSeedworkModule))]
    public class WebSeedworkApiModule : Infrastructure.Modules.Module
    {
        public override void PreInitialize()
        {
            IocManager.AddConventionalRegistrar(new ApiControllerConventionalRegistrar());
            IocManager.Register<IWebApiModuleConfiguration, WebApiModuleConfiguration>();
        }
        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(Assembly.GetExecutingAssembly());
            var httpConfiguration = IocManager.Resolve<IWebApiModuleConfiguration>().HttpConfiguration;
            httpConfiguration.Services.Replace(typeof(IHttpControllerActivator), new ApiControllerActivator(IocManager));
            httpConfiguration.Filters.Add(IocManager.Resolve<LoggingExceptionFilterAttribute>());

        }
        public override void PostInitialize()
        {
            Configuration.Modules.WebApiModule().HttpConfiguration.EnsureInitialized();
        }
    }
}
