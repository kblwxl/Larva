using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Resource
{
    public interface IResourceProvider : Dependency.ITransientDependency
    {
        void Put(string resourceKey,string filePath,ResourceMetaData metaData=null);
        void Put(string resourceKey, System.IO.Stream stream, ResourceMetaData metaData = null);
        Task PutAsync(string resourceKey, string filePath, ResourceMetaData metaData = null);
        Task PutAsync(string resourceKey, System.IO.Stream stream, ResourceMetaData metaData = null);

        Stream Get(string resourceKey);
        Task<Stream> GetAsync(string resourceKey);

        void Delete(string resourceKey);
        Task DeleteAsync(string resourceKey);

        List<ResourceMetaData> List();
    }
}
