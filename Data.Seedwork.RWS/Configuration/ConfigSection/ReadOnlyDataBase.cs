using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Seedwork.RWS.Configuration.ConfigSection
{
    public class ReadOnlyDataBase : ConfigurationSection
    {
        #region Singleton Instance
        
        internal const string ReadOnlyDataBaseSectionName = "readOnlyDataBase";
        internal const string ReadOnlyDataBaseSectionPath = "readOnlyDataBase";

        
        public static ReadOnlyDataBase Instance
        {
            get
            {
                return ((ReadOnlyDataBase)(ConfigurationManager.GetSection(ReadOnlyDataBaseSectionPath)));
            }
        }
        #endregion

        #region Xmlns Property
        
        internal const string XmlnsPropertyName = "xmlns";

        
        [ConfigurationPropertyAttribute(ReadOnlyDataBase.XmlnsPropertyName, IsRequired = false, IsKey = false, IsDefaultCollection = false)]
        public string Xmlns
        {
            get
            {
                return ((string)(base[ReadOnlyDataBase.XmlnsPropertyName]));
            }
        }
        #endregion

        #region IsReadOnly override
        
        public override bool IsReadOnly()
        {
            return false;
        }
        #endregion

        #region SwitchSlaveOnMasterFailed Property
        
        internal const string SwitchSlaveOnMasterFailedPropertyName = "switchSlaveOnMasterFailed";

        
        [DescriptionAttribute("The SwitchSlaveOnMasterFailed.")]
        [ConfigurationPropertyAttribute(SwitchSlaveOnMasterFailedPropertyName, IsRequired = false, IsKey = false, IsDefaultCollection = false, DefaultValue = false)]
        public virtual bool SwitchSlaveOnMasterFailed
        {
            get
            {
                return ((bool)(base[SwitchSlaveOnMasterFailedPropertyName]));
            }
            set
            {
                base[SwitchSlaveOnMasterFailedPropertyName] = value;
            }
        }
        #endregion

        #region SwitchMasterOnSlaveFailed Property
        
        internal const string SwitchMasterOnSlaveFailedPropertyName = "switchMasterOnSlaveFailed";

        
        [DescriptionAttribute("The SwitchMasterOnSlaveFailed.")]
        [ConfigurationPropertyAttribute(SwitchMasterOnSlaveFailedPropertyName, IsRequired = false, IsKey = false, IsDefaultCollection = false, DefaultValue = true)]
        public virtual bool SwitchMasterOnSlaveFailed
        {
            get
            {
                return ((bool)(base[SwitchMasterOnSlaveFailedPropertyName]));
            }
            set
            {
                base[SwitchMasterOnSlaveFailedPropertyName] = value;
            }
        }
        #endregion

        #region HealthCheckIntervalSecond Property
        
        internal const string HealthCheckIntervalSecondPropertyName = "healthCheckIntervalSecond";

        
        [DescriptionAttribute("The HealthCheckIntervalSecond.")]
        [ConfigurationPropertyAttribute(HealthCheckIntervalSecondPropertyName, IsRequired = false, IsKey = false, IsDefaultCollection = false, DefaultValue = 30)]
        public virtual int HealthCheckIntervalSecond
        {
            get
            {
                return ((int)(base[HealthCheckIntervalSecondPropertyName]));
            }
            set
            {
                base[HealthCheckIntervalSecondPropertyName] = value;
            }
        }
        #endregion

        #region SlaveRandomization Property
        
        internal const string SlaveRandomizationPropertyName = "slaveRandomization";

        
        [DescriptionAttribute("The SlaveRandomization.")]
        [ConfigurationPropertyAttribute(SlaveRandomizationPropertyName, IsRequired = false, IsKey = false, IsDefaultCollection = false, DefaultValue = true)]
        public virtual bool SlaveRandomization
        {
            get
            {
                return ((bool)(base[SlaveRandomizationPropertyName]));
            }
            set
            {
                base[SlaveRandomizationPropertyName] = value;
            }
        }
        #endregion

        #region Slaves Property
        
        internal const string SlavesPropertyName = "slaves";

        
        [DescriptionAttribute("The Slaves.")]
        [ConfigurationPropertyAttribute(SlavesPropertyName, IsRequired = false, IsKey = false, IsDefaultCollection = false)]
        public virtual SlaveElementCollection Slaves
        {
            get
            {
                return ((SlaveElementCollection)(base[SlavesPropertyName]));
            }
            set
            {
                base[SlavesPropertyName] = value;
            }
        }
        #endregion
    }
}
