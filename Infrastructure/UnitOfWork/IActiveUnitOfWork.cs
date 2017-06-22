using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.UnitOfWork
{
    public interface IActiveUnitOfWork
    {
        event EventHandler Completed;
        event EventHandler<UnitOfWorkFailedEventArgs> Failed;
        event EventHandler Disposed;
        UnitOfWorkOptions Options { get; }
        IReadOnlyList<DataFilterConfiguration> Filters { get; }
        bool IsDisposed { get; }
        void SaveChanges();
        Task SaveChangesAsync();
        IDisposable DisableFilter(params string[] filterNames);
        IDisposable EnableFilter(params string[] filterNames);
        bool IsFilterEnabled(string filterName);
        IDisposable SetFilterParameter(string filterName, string parameterName, object value);
    }
}
