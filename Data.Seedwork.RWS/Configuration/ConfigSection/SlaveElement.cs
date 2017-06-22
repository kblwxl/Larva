using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Seedwork.RWS.Configuration.ConfigSection
{
    public class SlaveElement : ConfigurationElement
    {
        public override bool IsReadOnly()
        {
            return false;
        }
        internal const string ConnectionStringPropertyName = "connectionStringName";

        [DescriptionAttribute("The ConnectionStringName.")]
        [ConfigurationPropertyAttribute(ConnectionStringPropertyName, IsRequired = true, IsKey = true, IsDefaultCollection = false)]
        public virtual string ConnectionStringName
        {
            get
            {
                return ((string)(base[ConnectionStringPropertyName]));
            }
            set
            {
                base[ConnectionStringPropertyName] = value;
            }
        }
    }
}
