using Data.Seedwork.EF;
using Data.Seedwork.EF.Utils;
using EntityFramework.DynamicFilters;
using Infrastructure;
using Infrastructure.Dependency;
using Infrastructure.Extensions;
using Infrastructure.Reflection;
using Infrastructure.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Seedwork.RWS
{
    public class EFDirectUnitOfWork : IDirectUnitOfWork
    {
        private readonly ISlaveConnectionStringResolver connectionStringResolver;
        protected IDictionary<string, DbContext> ActiveDbContexts { get; private set; }
        private readonly IDbContextResolver dbContextResolver;
        private readonly IIocResolver iocResolver;
        private readonly IUnitOfWorkDefaultOptions defaultOptions;

        private List<DataFilterConfiguration> filters;

        private bool _succeed;
        private Exception _exception;

        public EFDirectUnitOfWork(
            ISlaveConnectionStringResolver connectionStringResolver,
            IDbContextResolver dbContextResolver,
            IIocResolver iocResolver,
            IUnitOfWorkDefaultOptions defaultOptions
            )
        {
            this.connectionStringResolver = connectionStringResolver;
            this.dbContextResolver = dbContextResolver;
            this.iocResolver = iocResolver;
            this.defaultOptions = defaultOptions;

            Id = Guid.NewGuid().ToString("N");
            filters = defaultOptions.Filters.ToList();
            ActiveDbContexts = new Dictionary<string, DbContext>();

        }
        public IReadOnlyList<DataFilterConfiguration> Filters
        {
            get
            {
                return filters.ToImmutableList();
            }
        }

        public string Id { get; private set; }

        public bool IsDisposed { get; private set; }

        public UnitOfWorkOptions Options { get; private set; }

        public IUnitOfWork Outer
        {
            get
            {
                return null;
            }

            set
            {
                
            }
        }

        public event EventHandler Completed;
        public event EventHandler Disposed;
        public event EventHandler<UnitOfWorkFailedEventArgs> Failed;

        public void Begin(UnitOfWorkOptions options)
        {
            Options = options;
            SetFilters(options.FilterOverrides);
        }
        private void SetFilters(List<DataFilterConfiguration> filterOverrides)
        {
            for (var i = 0; i < filters.Count; i++)
            {
                var filterOverride = filterOverrides.FirstOrDefault(f => f.FilterName == filters[i].FilterName);
                if (filterOverride != null)
                {
                    filters[i] = filterOverride;
                }
            }


        }

        public void Complete()
        {
            try
            {
                CompleteUow();
                _succeed = true;
                OnCompleted();
            }
            catch (Exception ex)
            {
                _exception = ex;
                throw;
            }
        }

        private void CompleteUow()
        {
            SaveChanges();
            DisposeUow();
        }

        public async Task CompleteAsync()
        {
            try
            {
                await CompleteUowAsync();
                _succeed = true;
                OnCompleted();
            }
            catch (Exception ex)
            {
                _exception = ex;
                throw;
            }
        }

        private async Task CompleteUowAsync()
        {
            await SaveChangesAsync();
            
            DisposeUow();
        }

        private void DisposeUow()
        {
            foreach (var context in ActiveDbContexts.Values)
            {
                context.Dispose();
                iocResolver.Release(context);
            }
            ActiveDbContexts.Clear();
        }

        public IDisposable DisableFilter(params string[] filterNames)
        {
            var disabledFilters = new List<string>();

            foreach (var filterName in filterNames)
            {
                var filterIndex = GetFilterIndex(filterName);
                if (filters[filterIndex].IsEnabled)
                {
                    disabledFilters.Add(filterName);
                    filters[filterIndex] = new DataFilterConfiguration(filterName, false);
                }
            }

            disabledFilters.ForEach(ApplyDisableFilter);

            return new DisposeAction(() => EnableFilter(disabledFilters.ToArray()));
        }

        public void Dispose()
        {
            if (IsDisposed)
            {
                return;
            }

            IsDisposed = true;

            if (!_succeed)
            {
                OnFailed(_exception);
            }

            DisposeUow();
            OnDisposed();
        }

        public IDisposable EnableFilter(params string[] filterNames)
        {
            var enabledFilters = new List<string>();

            foreach (var filterName in filterNames)
            {
                var filterIndex = GetFilterIndex(filterName);
                if (!filters[filterIndex].IsEnabled)
                {
                    enabledFilters.Add(filterName);
                    filters[filterIndex] = new DataFilterConfiguration(filterName, true);
                }
            }

            enabledFilters.ForEach(ApplyEnableFilter);

            return new DisposeAction(() => DisableFilter(enabledFilters.ToArray()));
        }

        public TDBContext GetOrCreateDbContext<TDBContext>() 
            where TDBContext : DbContext
        {
            var connectionString = connectionStringResolver.GetNameOrConnectionString();

            var dbContextKey = typeof(TDBContext).FullName + "#" + connectionString;

            DbContext dbContext;
            if (!ActiveDbContexts.TryGetValue(dbContextKey, out dbContext))
            {

                dbContext = dbContextResolver.Resolve<TDBContext>(connectionString) ;

                ((IObjectContextAdapter)dbContext).ObjectContext.ObjectMaterialized += (sender, args) =>
                {
                    ObjectContext_ObjectMaterialized(dbContext, args);
                };

                foreach (var filter in Filters)
                {
                    if (filter.IsEnabled)
                    {
                        dbContext.EnableFilter(filter.FilterName);
                    }
                    else
                    {
                        dbContext.DisableFilter(filter.FilterName);
                    }

                    foreach (var filterParameter in filter.FilterParameters)
                    {
                        if (TypeHelper.IsFunc<object>(filterParameter.Value))
                        {
                            dbContext.SetFilterScopedParameterValue(filter.FilterName, filterParameter.Key, (Func<object>)filterParameter.Value);
                        }
                        else
                        {
                            dbContext.SetFilterScopedParameterValue(filter.FilterName, filterParameter.Key, filterParameter.Value);
                        }
                    }
                }

                ActiveDbContexts[dbContextKey] = dbContext;
            }

            return (TDBContext)dbContext;
        }
        private static void ObjectContext_ObjectMaterialized(DbContext dbContext, ObjectMaterializedEventArgs e)
        {
            var entityType = ObjectContext.GetObjectType(e.Entity.GetType());
            var previousState = dbContext.Entry(e.Entity).State;

            DateTimePropertyInfoHelper.NormalizeDatePropertyKinds(e.Entity, entityType);

            dbContext.Entry(e.Entity).State = previousState;
        }

        public bool IsFilterEnabled(string filterName)
        {
            return GetFilter(filterName).IsEnabled;
        }

        public void SaveChanges()
        {
            foreach (var dbContext in ActiveDbContexts.Values)
            {
                SaveChangesInDbContext(dbContext);
            }
        }

        private void SaveChangesInDbContext(DbContext dbContext)
        {
            dbContext.SaveChanges();
        }

        public async Task SaveChangesAsync()
        {
            foreach (var dbContext in ActiveDbContexts.Values)
            {
                await SaveChangesInDbContextAsync(dbContext);
            }
        }

        private async Task SaveChangesInDbContextAsync(DbContext dbContext)
        {
            await dbContext.SaveChangesAsync();
        }

        public IDisposable SetFilterParameter(string filterName, string parameterName, object value)
        {
            var filterIndex = GetFilterIndex(filterName);

            var newfilter = new DataFilterConfiguration(filters[filterIndex].FilterName,filters[filterIndex].IsEnabled);

            //Store old value
            object oldValue = null;
            var hasOldValue = newfilter.FilterParameters.ContainsKey(parameterName);
            if (hasOldValue)
            {
                oldValue = newfilter.FilterParameters[parameterName];
            }

            newfilter.FilterParameters[parameterName] = value;

            filters[filterIndex] = newfilter;

            ApplyFilterParameterValue(filterName, parameterName, value);

            return new DisposeAction(() =>
            {
                //Restore old value
                if (hasOldValue)
                {
                    SetFilterParameter(filterName, parameterName, oldValue);
                }
            });
        }
        private DataFilterConfiguration GetFilter(string filterName)
        {
            var filter = filters.FirstOrDefault(f => f.FilterName == filterName);
            if (filter == null)
            {
                throw new Exception("Unknown filter name: " + filterName + ". Be sure this filter is registered before.");
            }

            return filter;
        }

        private int GetFilterIndex(string filterName)
        {
            var filterIndex = filters.FindIndex(f => f.FilterName == filterName);
            if (filterIndex < 0)
            {
                throw new Exception("Unknown filter name: " + filterName + ". Be sure this filter is registered before.");
            }

            return filterIndex;
        }
        protected void ApplyDisableFilter(string filterName)
        {
            foreach (var activeDbContext in ActiveDbContexts.Values)
            {
                activeDbContext.DisableFilter(filterName);
            }
        }

        protected void ApplyEnableFilter(string filterName)
        {
            foreach (var activeDbContext in ActiveDbContexts.Values)
            {
                activeDbContext.EnableFilter(filterName);
            }
        }

        protected void ApplyFilterParameterValue(string filterName, string parameterName, object value)
        {
            foreach (var activeDbContext in ActiveDbContexts.Values)
            {
                if (TypeHelper.IsFunc<object>(value))
                {
                    activeDbContext.SetFilterScopedParameterValue(filterName, parameterName, (Func<object>)value);
                }
                else
                {
                    activeDbContext.SetFilterScopedParameterValue(filterName, parameterName, value);
                }
            }
        }
        protected virtual void OnCompleted()
        {
            Completed.InvokeSafely(this);
        }

        /// <summary>
        /// Called to trigger <see cref="Failed"/> event.
        /// </summary>
        /// <param name="exception">Exception that cause failure</param>
        protected virtual void OnFailed(Exception exception)
        {
            Failed.InvokeSafely(this, new UnitOfWorkFailedEventArgs(exception));
        }

        /// <summary>
        /// Called to trigger <see cref="Disposed"/> event.
        /// </summary>
        protected virtual void OnDisposed()
        {
            Disposed.InvokeSafely(this);
        }
    }
}
