using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Seedwork.RWS.Configuration.ConfigSection
{
    [ConfigurationCollectionAttribute(typeof(SlaveElement), CollectionType = ConfigurationElementCollectionType.BasicMapAlternate, AddItemName = SlaveElementPropertyName)]
    public class SlaveElementCollection : ConfigurationElementCollection
    {
        internal const string SlaveElementPropertyName = "slave";

        public override global::System.Configuration.ConfigurationElementCollectionType CollectionType
        {
            get
            {
                return global::System.Configuration.ConfigurationElementCollectionType.BasicMapAlternate;
            }
        }
        protected override string ElementName
        {
            get
            {
                return SlaveElementPropertyName;
            }
        }
        protected override bool IsElementName(string elementName)
        {
            return (elementName == SlaveElementPropertyName);
        }
        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((SlaveElement)(element)).ConnectionStringName;
        }
        protected override ConfigurationElement CreateNewElement()
        {
            return new SlaveElement();
        }
        public SlaveElement this[int index]
        {
            get
            {
                return ((SlaveElement)(base.BaseGet(index)));
            }
        }
        public SlaveElement this[object connectionString]
        {
            get
            {
                return ((SlaveElement)(base.BaseGet(connectionString)));
            }
        }
        public void Add(SlaveElement slaveElement)
        {
            base.BaseAdd(slaveElement);
        }
        public void Remove(SlaveElement slaveElement)
        {
            base.BaseRemove(this.GetElementKey(slaveElement));
        }
        public SlaveElement GetItemAt(int index)
        {
            return ((SlaveElement)(base.BaseGet(index)));
        }
        public SlaveElement GetItemByKey(string connectionString)
        {
            return ((SlaveElement)(base.BaseGet(((object)(connectionString)))));
        }
        public override bool IsReadOnly()
        {
            return false;
        }
    }
}
