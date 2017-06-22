using Infrastructure.Configuration;
using Infrastructure.Dependency;
using Infrastructure.Dependency.Installers;
using Infrastructure.Modules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure
{
    public class Bootstrapper : IDisposable
    {
        public IIocManager IocManager { get; private set; }
        protected bool IsDisposed;
        private IModuleManager _moduleManager;

        public Bootstrapper()
            : this(Dependency.IocManager.Instance)
        {

        }
        public Bootstrapper(IIocManager iocManager)
        {
            IocManager = iocManager;
        }
        public virtual void Initialize()
        {
            IocManager.IocContainer.Install(new CoreInstaller());

            IocManager.Resolve<StartupConfiguration>().Initialize();

            _moduleManager = IocManager.Resolve<IModuleManager>();
            _moduleManager.InitializeModules();
        }
        public void Dispose()
        {
            if (IsDisposed)
            {
                return;
            }

            IsDisposed = true;

            if (_moduleManager != null)
            {
                _moduleManager.ShutdownModules();
            }
        }
    }
}
