using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.FileConverter
{
    public interface IFileConverter : Dependency.ITransientDependency
    {
        /// <summary>
        /// 获取受支持的输入文件类型
        /// </summary>
        SupportdFilePoint SupportedInputFileFormat { get;  }

        /// <summary>
        /// 获取受支持的输出文件类型
        /// </summary>
        SupportdFilePoint SupportedOutputFileFormat { get;  }

        /// <summary>
        /// 根据指定的输入文件扩展名和输出文件扩展名，判断此FileConverter是否支持此种转换
        /// </summary>
        /// <param name="inputFileExt">输入文件扩展名</param>
        /// <param name="outputFileExt">输出文件扩展名</param>
        /// <returns>如支持此种转换，返回True</returns>
        //bool SatisfiedBy(string inputFileExt, string outputFileExt);

        /// <summary>
        /// 转换指定的文件
        /// </summary>
        /// <param name="inputFilePath">源文件完整路径</param>
        /// <param name="outputFilePath">目的文件完整路径</param>
        void Convert(string inputFilePath, string outputFilePath);

        /// <summary>
        /// 转换指定的文件,并替换对应的书签的内容
        /// </summary>
        /// <param name="inputFilePath">源文件完整路径</param>
        /// <param name="outputFilePath">目的文件完整路径</param>
        /// <param name="bookMarkers">要替换的书签的集合</param>
        void Convert(string inputFilePath, string outputFilePath, List<BookMarker> bookMarkers);

        void Convert(string inputFilePath, string outputFilePath, List<BookMarker> bookMarkers, DynamicBookMarker dynamicData);
        void Convert(string inputFilePath, string outputFilePath, DynamicBookMarker dynamicData);

        /// <summary>
        /// 对pdf文件进行电子签章
        /// </summary>
        /// <param name="filePath">原文件完整路径 例 @"D:\合同更改\我的文件.pdf"</param>
        /// <param name="savePath">生成文件保存路径 例 @"D:\合同更改"</param>
        /// <param name="keyPath">key文件完整路径 例  @"D:\合同\key\壹壹金融信息服务（北京）有限公司.key"</param>
        /// <param name="position">定位文字 例 "壹壹财富投资管理有限公司"</param>
        void Signature(string filePath, string savePath, string keyPath, string postext);


    }
}
