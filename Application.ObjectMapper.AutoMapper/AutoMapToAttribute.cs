using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.ObjectMapper.AutoMapper
{
    public class AutoMapToAttribute : AutoMapAttribute
    {
        internal override AutoMapDirection Direction
        {
            get
            {
                return AutoMapDirection.To;
            }
        }
        public AutoMapToAttribute(params Type[] targerType)
            :base(targerType)
        {

        }
    }
}
