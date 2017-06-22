using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.UnitOfWork
{
    public interface IDirectUnitOfWork : IUnitOfWork
    {
        //TDBContext GetOrCreateDbContext<TDBContext>();
    }
}
