using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Resource
{
    public class ResourceMetaData
    {
        public string ResourceKey { get; set; }
        public string CacheControl { get; set; }
        public string ContentType { get; set; }
        public long ContentLength { get; set; }
        public DateTime ExpirationTime { get; set; }
    }
}
