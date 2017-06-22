using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Resource
{
    
    public partial class AliResourceSection : global::System.Configuration.ConfigurationSection
    {

        
        internal const string AliResourceSectionSectionName = "aliResourceSection";

        
        internal const string AliResourceSectionSectionPath = "aliResourceSection";

        
        public static AliResourceSection Instance
        {
            get
            {
                return ((AliResourceSection)(global::System.Configuration.ConfigurationManager.GetSection(AliResourceSection.AliResourceSectionSectionPath)));
            }
        }
        
        internal const string XmlnsPropertyName = "xmlns";

        
        [global::System.Configuration.ConfigurationPropertyAttribute(AliResourceSection.XmlnsPropertyName, IsRequired = false, IsKey = false, IsDefaultCollection = false)]
        public string Xmlns
        {
            get
            {
                return ((string)(base[AliResourceSection.XmlnsPropertyName]));
            }
        }
        
        public override bool IsReadOnly()
        {
            return false;
        }
        
        internal const string EndPointPropertyName = "endPoint";

        
        [global::System.ComponentModel.DescriptionAttribute("The EndPoint.")]
        [global::System.Configuration.ConfigurationPropertyAttribute(AliResourceSection.EndPointPropertyName, IsRequired = true, IsKey = false, IsDefaultCollection = false,DefaultValue = "http://oss-cn-beijing.aliyuncs.com")]
        public virtual string EndPoint
        {
            get
            {
                return ((string)(base[AliResourceSection.EndPointPropertyName]));
            }
            set
            {
                base[AliResourceSection.EndPointPropertyName] = value;
            }
        }
        
        internal const string AccessKeyIdPropertyName = "accessKeyId";

        
        [global::System.ComponentModel.DescriptionAttribute("The AccessKeyId.")]
        [global::System.Configuration.ConfigurationPropertyAttribute(AliResourceSection.AccessKeyIdPropertyName, IsRequired = false, IsKey = false, IsDefaultCollection = false)]
        public virtual string AccessKeyId
        {
            get
            {
                return ((string)(base[AliResourceSection.AccessKeyIdPropertyName]));
            }
            set
            {
                base[AliResourceSection.AccessKeyIdPropertyName] = value;
            }
        }
        
        internal const string AccessKeySecretPropertyName = "accessKeySecret";

       
        [global::System.ComponentModel.DescriptionAttribute("The AccessKeySecret.")]
        [global::System.Configuration.ConfigurationPropertyAttribute(AliResourceSection.AccessKeySecretPropertyName, IsRequired = false, IsKey = false, IsDefaultCollection = false)]
        public virtual string AccessKeySecret
        {
            get
            {
                return ((string)(base[AliResourceSection.AccessKeySecretPropertyName]));
            }
            set
            {
                base[AliResourceSection.AccessKeySecretPropertyName] = value;
            }
        }
       
        internal const string BucketNamePropertyName = "bucketName";

        
        [global::System.ComponentModel.DescriptionAttribute("The BucketName.")]
        [global::System.Configuration.ConfigurationPropertyAttribute(AliResourceSection.BucketNamePropertyName, IsRequired = false, IsKey = false, IsDefaultCollection = false)]
        public virtual string BucketName
        {
            get
            {
                return ((string)(base[AliResourceSection.BucketNamePropertyName]));
            }
            set
            {
                base[AliResourceSection.BucketNamePropertyName] = value;
            }
        }
        
        internal const string ResourcePrefixPropertyName = "resourcePrefix";

        
        [global::System.ComponentModel.DescriptionAttribute("The ResourcePrefix.")]
        [global::System.Configuration.ConfigurationPropertyAttribute(AliResourceSection.ResourcePrefixPropertyName, IsRequired = false, IsKey = false, IsDefaultCollection = false)]
        public virtual string ResourcePrefix
        {
            get
            {
                return ((string)(base[AliResourceSection.ResourcePrefixPropertyName]));
            }
            set
            {
                base[AliResourceSection.ResourcePrefixPropertyName] = value;
            }
        }
        
    }
}
