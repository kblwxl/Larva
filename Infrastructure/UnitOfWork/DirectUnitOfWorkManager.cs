using Infrastructure.Dependency;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace Infrastructure.UnitOfWork
{
    internal class DirectUnitOfWorkManager : IDirectUnitOfWorkManager, ITransientDependency
    {
        private readonly IIocResolver iocResolver;
        private readonly ICurrentUnitOfWorkProvider currentUnitOfWorkProvider;
        private readonly IUnitOfWorkDefaultOptions defaultOptions;
        public DirectUnitOfWorkManager(
            IIocResolver iocResolver,
            ICurrentUnitOfWorkProvider currentUnitOfWorkProvider,
            IUnitOfWorkDefaultOptions defaultOptions)
        {
            this.iocResolver = iocResolver;
            this.currentUnitOfWorkProvider = currentUnitOfWorkProvider;
            this.defaultOptions = defaultOptions;
        }
        public IActiveUnitOfWork Current
        {
            get
            {
                return currentUnitOfWorkProvider.Current;
            }
        }

        public IUnitOfWorkCompleteHandle Begin()
        {
            return Begin(new UnitOfWorkOptions());
        }
        public IUnitOfWorkCompleteHandle Begin(TransactionScopeOption scope)
        {
            return Begin(new UnitOfWorkOptions { Scope = scope });
        }
        public IUnitOfWorkCompleteHandle Begin(UnitOfWorkOptions options)
        {
            options.FillDefaultsForNonProvidedOptions(defaultOptions);
            IDirectUnitOfWork uow = iocResolver.Resolve<IDirectUnitOfWork>();
            uow.Completed += (sender, args) =>
            {
                currentUnitOfWorkProvider.Current = null;
            };

            uow.Failed += (sender, args) =>
            {
                currentUnitOfWorkProvider.Current = null;
            };

            uow.Disposed += (sender, args) =>
            {
                iocResolver.Release(uow);
            };

            uow.Begin(options);
            currentUnitOfWorkProvider.Current = uow;
            return uow;
        }

        
    }
}
