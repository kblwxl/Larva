using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Seedwork.EF
{
    public interface IDbContextResolver
    {
        TDbContext Resolve<TDbContext>(string connectionString);
    }
}
