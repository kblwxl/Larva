using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace Infrastructure.UnitOfWork
{
    public interface IUnitOfWorkDefaultOptions
    {
        TransactionScopeOption Scope { get; set; }


        bool IsTransactional { get; set; }


        TimeSpan? Timeout { get; set; }


        IsolationLevel? IsolationLevel { get; set; }


        IReadOnlyList<DataFilterConfiguration> Filters { get; }


        void RegisterFilter(string filterName, bool isEnabledByDefault);


        void OverrideFilter(string filterName, bool isEnabledByDefault);
    }
}
