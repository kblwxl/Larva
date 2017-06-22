using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.UnitOfWork
{
    public class NullDirectUnitOfWork : IDirectUnitOfWork
    {
        private IUnitOfWork internalUow;
        public NullDirectUnitOfWork(IUnitOfWork uow)
        {
            internalUow = uow;
        }
        public IReadOnlyList<DataFilterConfiguration> Filters
        {
            get
            {
                return internalUow.Filters;
            }
        }

        public string Id
        {
            get
            {
                return internalUow.Id;
            }
        }

        public bool IsDisposed
        {
            get
            {
                return internalUow.IsDisposed;
            }
        }

        public UnitOfWorkOptions Options
        {
            get
            {
                return internalUow.Options;
            }
        }

        public IUnitOfWork Outer
        {
            get
            {
                return internalUow.Outer;
            }

            set
            {
                internalUow.Outer = value;
            }
        }

        public event EventHandler Completed;
        public event EventHandler Disposed;
        public event EventHandler<UnitOfWorkFailedEventArgs> Failed;

        public void Begin(UnitOfWorkOptions options)
        {
            options.IsTransactional = false;
            internalUow.Begin(options);
        }

        public void Complete()
        {
            internalUow.Complete();
        }

        public async Task CompleteAsync()
        {
            await internalUow.CompleteAsync();
        }

        public IDisposable DisableFilter(params string[] filterNames)
        {
            return internalUow.DisableFilter(filterNames);
        }

        public void Dispose()
        {
            internalUow.Dispose();
        }

        public IDisposable EnableFilter(params string[] filterNames)
        {
            return internalUow.EnableFilter(filterNames);
        }

        public TDBContext GetOrCreateDbContext<TDBContext>()
        {
            return ((dynamic)internalUow).GetOrCreateDbContext<TDBContext>();
        }

        public bool IsFilterEnabled(string filterName)
        {
            return internalUow.IsFilterEnabled(filterName);
        }

        public void SaveChanges()
        {
            internalUow.SaveChanges();
        }

        public async Task SaveChangesAsync()
        {
            await internalUow.SaveChangesAsync();
        }

        public IDisposable SetFilterParameter(string filterName, string parameterName, object value)
        {
            return internalUow.SetFilterParameter(filterName, parameterName, value);
        }

        
    }
}
