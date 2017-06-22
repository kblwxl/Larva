using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Caching
{
    public interface ICachedItemUpdater<T> : Dependency.ITransientDependency
    {
        void UpdateSource(T source);
    }
}
