using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace Infrastructure.UnitOfWork
{
    public interface IDirectUnitOfWorkManager 
    {
        IActiveUnitOfWork Current { get; }
        IUnitOfWorkCompleteHandle Begin();
        IUnitOfWorkCompleteHandle Begin(TransactionScopeOption scope);
        IUnitOfWorkCompleteHandle Begin(UnitOfWorkOptions options);
    }
}
