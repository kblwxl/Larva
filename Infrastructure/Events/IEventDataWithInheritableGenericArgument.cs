using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Events
{
    public interface IEventDataWithInheritableGenericArgument
    {
        object[] GetConstructorArgs();
    }
}
