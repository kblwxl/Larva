using Infrastructure.Dependency;
using Infrastructure.Reflection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Web.Seedwork
{
    public abstract class WebApplicationBase : HttpApplication
    {
        protected Infrastructure.Bootstrapper Bootstrapper { get; private set; }
        public WebApplicationBase()
        {
            Bootstrapper = new Infrastructure.Bootstrapper();
        }
        protected virtual void Application_Start(object sender, EventArgs e)
        {
            Bootstrapper.IocManager.RegisterIfNot<IAssemblyFinder, WebAssemblyFinder>();
            Bootstrapper.Initialize();
        }
        protected virtual void Application_End(object sender, EventArgs e)
        {
            Bootstrapper.Dispose();
        }
        protected virtual void Session_Start(object sender, EventArgs e)
        {

        }

        protected virtual void Session_End(object sender, EventArgs e)
        {

        }
        protected virtual void Application_BeginRequest(object sender, EventArgs e)
        {
            
        }
        protected virtual void Application_EndRequest(object sender, EventArgs e)
        {

        }

        protected virtual void Application_AuthenticateRequest(object sender, EventArgs e)
        {
            
        }

        protected virtual void Application_Error(object sender, EventArgs e)
        {

        }
    }
}
