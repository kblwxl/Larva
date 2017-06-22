using Infrastructure.Dependency;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http.Controllers;
using System.Web.Http.Dispatcher;

namespace Web.Seedwork.Api
{
    public class ApiControllerActivator : IHttpControllerActivator
    {
        private readonly IIocResolver iocResolver;
        public ApiControllerActivator(IIocResolver iocResolver)
        {
            this.iocResolver = iocResolver;
        }
        public IHttpController Create(HttpRequestMessage request, HttpControllerDescriptor controllerDescriptor, Type controllerType)
        {
            var controllerWrapper = iocResolver.ResolveAsDisposable<IHttpController>(controllerType);
            request.RegisterForDispose(controllerWrapper);
            return controllerWrapper.Object;
        }
    }
}
