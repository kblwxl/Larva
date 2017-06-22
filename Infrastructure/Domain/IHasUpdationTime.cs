using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Domain
{
    public interface IHasUpdationTime
    {
        DateTime? UpdationTime { get; set; }
    }
}
