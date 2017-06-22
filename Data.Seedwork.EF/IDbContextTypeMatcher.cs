using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Seedwork.EF
{
    public interface IDbContextTypeMatcher
    {
        void Add(Type sourceDbContextType, Type targetDbContextType);

        void Populate(Type[] dbContextTypes);

        Type GetConcreteType(Type sourceDbContextType);
    }
}
