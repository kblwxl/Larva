using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.UnitOfWork
{
    public interface IUnitOfWork : IActiveUnitOfWork, IUnitOfWorkCompleteHandle
    {
        string Id { get; }
        IUnitOfWork Outer { get; set; }
        void Begin(UnitOfWorkOptions options);
    }
}
