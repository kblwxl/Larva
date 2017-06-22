using Aliyun.OSS;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Resource
{
    public class AliResourceProvider : IResourceProvider
    {
        private OssClient client;
        private ILogger logger;

        public AliResourceProvider(ILogger logger)
        {
            client = new OssClient(
                AliResourceSection.Instance.EndPoint,
                AliResourceSection.Instance.AccessKeyId,
                AliResourceSection.Instance.AccessKeySecret
                );
            this.logger = logger;
        }

        public void Delete(string resourceKeyWithPrefix)
        {
            try
            {
                client.DeleteObject(AliResourceSection.Instance.BucketName,  resourceKeyWithPrefix);
            }
            catch(Exception error)
            {
                logger.Fatal(string.Format("删除键为{0}的资源发生异常:", resourceKeyWithPrefix), error);
            }
        }

        public Task DeleteAsync(string resourceKey)
        {
            return Task.Run(() => Delete(resourceKey));
        }

        public Stream Get(string resourceKeyWithPrefix)
        {
            try
            {
                var obj = client.GetObject(AliResourceSection.Instance.BucketName,  resourceKeyWithPrefix);
                return obj.Content;
            }
            catch(Exception error)
            {
                logger.Fatal(string.Format("获取键为{0}的资源发生异常:", resourceKeyWithPrefix), error);
                throw error;
            }
            
        }

        public Task<Stream> GetAsync(string resourceKey)
        {
            return Task<Stream>.FromResult(Get(resourceKey));
        }

        public List<ResourceMetaData> List()
        {
            try
            {
                var listObjectsRequest = new ListObjectsRequest(AliResourceSection.Instance.BucketName);
                var result = client.ListObjects(listObjectsRequest);
                return result.ObjectSummaries.Select(p =>
                    new ResourceMetaData
                    {
                        ResourceKey = p.Key,
                        ContentLength = p.Size
                    }
                ).ToList();
            }
            catch (Exception error)
            {
                logger.Fatal("获取资源发生异常:", error);
                throw error;
            }

        }

        public void Put(string resourceKey, Stream stream, ResourceMetaData metaData = null)
        {
            try
            {
                if (metaData == null)
                {
                    var metadata = new ObjectMetadata();
                    metadata.CacheControl = "No-Cache";
                    metadata.ContentType = "text/html";
                    client.PutBigObject(AliResourceSection.Instance.BucketName, AliResourceSection.Instance.ResourcePrefix + resourceKey, stream, metadata);
                }
                else
                {
                    ObjectMetadata omd = metaData.ToObjectMetadata();

                    client.PutBigObject(AliResourceSection.Instance.BucketName, AliResourceSection.Instance.ResourcePrefix + resourceKey, stream, omd);
                }
            }
            catch (Exception error)
            {
                logger.Fatal(string.Format("添加键为{0}的资源发生异常:", resourceKey), error);
                throw error;
            }

        }

        public void Put(string resourceKey, string filePath, ResourceMetaData metaData = null)
        {
            try
            {
                if (metaData == null)
                {
                    var metadata = new ObjectMetadata();
                    metadata.CacheControl = "No-Cache";
                    metadata.ContentType = "text/html";
                    client.PutBigObject(AliResourceSection.Instance.BucketName, AliResourceSection.Instance.ResourcePrefix + resourceKey, filePath,metadata);
                }
                else
                {
                    ObjectMetadata omd = metaData.ToObjectMetadata();

                    client.PutBigObject(AliResourceSection.Instance.BucketName, AliResourceSection.Instance.ResourcePrefix + resourceKey, filePath, omd);
                }
            }
            catch (Exception error)
            {
                logger.Fatal(string.Format("添加键为{0}的资源发生异常:", resourceKey), error);
                throw error;
            }

        }

        public async Task PutAsync(string resourceKey, Stream stream, ResourceMetaData metaData = null)
        {
            await Task.Run(()=>Put(resourceKey, stream, metaData));
        }

        public async Task PutAsync(string resourceKey, string filePath, ResourceMetaData metaData = null)
        {
            await Task.Run(() => Put(resourceKey, filePath, metaData));
        }

        
    }
    public static class ResourceMetaDataExtension
    {
        public static ObjectMetadata ToObjectMetadata(this ResourceMetaData instance)
        {
            ObjectMetadata omd = new ObjectMetadata();
            omd.CacheControl = instance.CacheControl;
            omd.ContentType = instance.ContentType;
            return omd;
        }
    }
}
