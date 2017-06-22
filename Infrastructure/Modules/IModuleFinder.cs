using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Modules
{
    public interface IModuleFinder
    {
        ICollection<Type> FindAll();
    }
}
