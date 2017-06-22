using Infrastructure.Dependency;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.Routing;

namespace Web.Seedwork.Mvc.Controllers
{
    public class WindsorControllerFactory : DefaultControllerFactory
    {
        private readonly IIocResolver iocManager;
        public WindsorControllerFactory(IIocResolver iocManager)
        {
            this.iocManager = iocManager;
        }
        public override void ReleaseController(IController controller)
        {
            iocManager.Release(controller);
        }
        protected override IController GetControllerInstance(RequestContext requestContext, Type controllerType)
        {
            if (controllerType == null)
            {
                return base.GetControllerInstance(requestContext, controllerType);
            }

            return iocManager.Resolve<IController>(controllerType);
        }
    }
}
