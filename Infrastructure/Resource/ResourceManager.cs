using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Resource
{
    public class ResourceManager : IResourceManager
    {
        private IResourceProvider provider;
        public ResourceManager(IResourceProvider provider)
        {
            this.provider = provider;
        }
        public void Delete(string resourceKey)
        {
            provider.Delete(resourceKey);
        }

        public async Task DeleteAsync(string resourceKey)
        {
            await  provider.DeleteAsync(resourceKey);
        }

        public Stream Get(string resourceKey)
        {
            return provider.Get(resourceKey);
        }

        public async Task<Stream> GetAsync(string resourceKey)
        {
            return await provider.GetAsync(resourceKey);
        }

        public List<ResourceMetaData> List()
        {
            return provider.List();
        }

        public void Put(string resourceKey, Stream stream, ResourceMetaData metaData = null)
        {
            provider.Put(resourceKey, stream, metaData);
        }

        public void Put(string resourceKey, string filePath, ResourceMetaData metaData = null)
        {
            provider.Put(resourceKey, filePath, metaData);
        }

        public async Task PutAsync(string resourceKey, Stream stream, ResourceMetaData metaData = null)
        {
            await provider.PutAsync(resourceKey, stream, metaData);
        }

        public async Task PutAsync(string resourceKey, string filePath, ResourceMetaData metaData = null)
        {
            await provider.PutAsync(resourceKey, filePath, metaData);
        }
    }
}
