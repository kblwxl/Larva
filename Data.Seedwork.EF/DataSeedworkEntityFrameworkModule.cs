
using Castle.MicroKernel.Registration;
using Data.Seedwork.EF.Repositories;
using Data.Seedwork.EF.Uow;
using Infrastructure;
using Infrastructure.Collections.Extensions;
using Infrastructure.Dependency;
using Infrastructure.Modules;
using Infrastructure.Reflection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Data.Seedwork.EF
{
    [DependsOn(typeof(KernelModule))]
    public class DataSeedworkEntityFrameworkModule : Infrastructure.Modules.Module
    {
        public ILogger Logger { get; set; }

        private readonly ITypeFinder _typeFinder;
        public DataSeedworkEntityFrameworkModule(ITypeFinder typeFinder)
        {
            _typeFinder = typeFinder;
            Logger = NullLogger.Instance;
        }
        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(Assembly.GetExecutingAssembly());

            IocManager.IocContainer.Register(
                Component.For(typeof(IDbContextProvider<>))
                    .ImplementedBy(typeof(UnitOfWorkDbContextProvider<>))
                    .LifestyleTransient()
                );

            RegisterGenericRepositoriesAndMatchDbContexes();
        }
        private void RegisterGenericRepositoriesAndMatchDbContexes()
        {
            var dbContextTypes =
                _typeFinder.Find(type =>
                    type.IsPublic &&
                    !type.IsAbstract &&
                    type.IsClass &&
                    typeof(BaseDbContext).IsAssignableFrom(type)
                    );

            if (dbContextTypes.IsNullOrEmpty())
            {
                Logger.Warning("未发现从BaseDbContext派生出来的类.");
                return;
            }

            using (var repositoryRegistrar = IocManager.ResolveAsDisposable<EntityFrameworkGenericRepositoryRegistrar>())
            {
                foreach (var dbContextType in dbContextTypes)
                {
                    repositoryRegistrar.Object.RegisterForDbContext(dbContextType, IocManager);
                }
            }

            using (var dbContextMatcher = IocManager.ResolveAsDisposable<IDbContextTypeMatcher>())
            {
                dbContextMatcher.Object.Populate(dbContextTypes);
            }
        }
    }
}
