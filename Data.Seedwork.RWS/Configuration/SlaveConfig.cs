using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Seedwork.RWS.Configuration
{
    public class SlaveConfig 
    {
        public string ConnectionStringName { get; internal set; }
        public bool Offline { get; internal set; }
    }
}
