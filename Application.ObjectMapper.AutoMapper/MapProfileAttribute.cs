using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.ObjectMapper.AutoMapper
{
    [AttributeUsage(AttributeTargets.Class,AllowMultiple =false,Inherited =false)]
    public class MapProfileAttribute : Attribute
    {
        public Type[] ProviderTypes { get; private set; }
        public MapProfileAttribute(params Type[] providerTypes)
        {
            ProviderTypes = providerTypes;
        }
    }
}
