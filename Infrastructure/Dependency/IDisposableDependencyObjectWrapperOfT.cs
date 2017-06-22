using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Dependency
{
    public interface IDisposableDependencyObjectWrapper<out T> : IDisposable
    {
        
        T Object { get; }
    }
}
