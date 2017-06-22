using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Caching
{
    [Flags]
    public enum CacheStrategy
    {
        TimeOut=1,
        Manual=2
    }
}
