using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using Web.Seedwork.Mvc.Controllers;

namespace Web.Seedwork.Mvc
{
    [Infrastructure.Modules.DependsOn(
        typeof(Infrastructure.KernelModule),
        typeof(WebSeedworkModule))]
    public class WebSeedworkMvcModule : Infrastructure.Modules.Module
    {
        public override void PreInitialize()
        {
            IocManager.AddConventionalRegistrar(new ControllerConventionalRegistrar());
        }
        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(System.Reflection.Assembly.GetExecutingAssembly());
            ControllerBuilder.Current.SetControllerFactory(new WindsorControllerFactory(IocManager));
        }
    }
}
