using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Infrastructure.Resource;
using Infrastructure.Dependency;
using System.IO;

namespace Infrastructure.FileConverter
{
    public class FileConverterManager : IFileConverterManager
    {
        private const string sigPrefix = "YY_";
        private List<IFileConverter> fileConverters;
        private IResourceManager resourceManager;
        public List<IFileConverter> FileConverters
        {
            get
            {
                return fileConverters;
            }
        }

        public IResourceManager ResourceManager
        {
            get { return resourceManager; }
        }
        public FileConverterManager(IIocResolver resolver,IResourceManager resourceManager)
        {
            fileConverters = resolver.ResolveAll<IFileConverter>();
            this.resourceManager = resourceManager;
        }
        private IFileConverter SatisfiedBy(string inputFileExt, string outputFileExt)
        {
            IFileConverter retValue = null;
            foreach(var fc in fileConverters)
            {
                if(fc.SupportedInputFileFormat.SupportedFileExt.Contains(inputFileExt.ToUpper()) &&
                    fc.SupportedOutputFileFormat.SupportedFileExt.Contains(outputFileExt.ToUpper()))
                {
                    retValue = fc;
                    break;
                }
            }
            return retValue;
        }
        private string GetFileExt(string filePath)
        {
            try
            {
                if (File.Exists(filePath))
                {
                    FileInfo fi = new FileInfo(filePath);
                    return fi.Extension.Substring(1);
                }
                else
                {
                    return filePath.Substring(filePath.LastIndexOf('.') + 1);
                }

            }
            catch
            {
                throw new Exception(string.Format("无效的文件名称:{0}", filePath));
            }
            
        }
        public string Convert(string inputFilePath,string outputFilePath,string signatureKeyPath=null,string posText=null)
        {
            return Convert(inputFilePath, outputFilePath, null, null, signatureKeyPath, posText);
        }
        public string Convert(string inputFilePath, 
            string outputFilePath, 
            List<BookMarker> bookMarkers, 
            DynamicBookMarker dynamicData,
            string signatureKeyPath = null, 
            string posText = null)
        {
            string inputExt = GetFileExt(inputFilePath);
            string outputExt = GetFileExt(outputFilePath);

            IFileConverter fileConverter = SatisfiedBy(
                inputExt,
                outputExt);

            if (fileConverter == null)
            {
                throw new InvalidOperationException(string.Format("未找到从{0}转到{1}的文件转换器", inputExt, outputExt));
            }
            fileConverter.Convert(inputFilePath, outputFilePath, bookMarkers, dynamicData);
            FileInfo fi = new FileInfo(outputFilePath);
            var resourceKey = fi.Name;
            var finalPath = outputFilePath;

            if (signatureKeyPath != null && posText != null)
            {
                string savePath = Path.Combine(fi.DirectoryName, sigPrefix + fi.Name);
                fileConverter.Signature(outputFilePath, savePath, signatureKeyPath, posText);

                fi = new FileInfo(savePath);
                resourceKey = fi.Name;
                finalPath = savePath;
            }

            resourceManager.Put(resourceKey, File.Open(resourceKey, FileMode.Open));
            File.Delete(finalPath);
            return finalPath;
        }
    }
}
