using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.DataObjects
{
    /// <summary>
    /// DTO对象的抽象基类
    /// </summary>
    public abstract class DataObjectBase : IDataObject
    {
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            PropertyInfo[] publicProperties = this.GetType().GetProperties();

            foreach (var pInfo in publicProperties)
            {
                var value = pInfo.GetValue(this);
                if (value == null)
                {
                    continue;
                }
                if (value.GetType().IsArray)
                {
                    object[] array = value as object[];
                    if (array != null)
                    {
                        foreach (var item in array)
                        {
                            if (item != null)
                            {
                                sb.AppendFormat("{0}_", item);
                            }
                        }
                    }
                }
                else
                {
                    sb.AppendFormat("{0}_", value);
                }
            }

            return sb.ToString();
        }
    }
}
