using Aspose.Words;
using Aspose.Words.Tables;
using com.kinggrid.pdf;
using com.kinggrid.pdf.executes;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.FileConverter
{
    public class DocToPdfFileConverter : IFileConverter
    {
        private SupportdFilePoint supportedInputFileFormat;
        private SupportdFilePoint supportedOutputFileFormat;

        public DocToPdfFileConverter()
        {
            supportedInputFileFormat = new SupportdFilePoint("word文件", "doc", "docx");
            supportedOutputFileFormat = new SupportdFilePoint("pdf文件", "pdf");
        }
        public SupportdFilePoint SupportedInputFileFormat
        {
            get
            {
                return supportedInputFileFormat;
            }
        }

        public SupportdFilePoint SupportedOutputFileFormat
        {
            get
            {
                return supportedOutputFileFormat;
            }
        }

        public void Convert(string inputFilePath, string outputFilePath)
        {
            Convert(inputFilePath, outputFilePath, null, null);
            
        }

        public void Convert(string inputFilePath, string outputFilePath, DynamicBookMarker dynamicData)
        {

            Convert(inputFilePath, outputFilePath, null, dynamicData);
        }

        public void Convert(string inputFilePath, string outputFilePath, List<BookMarker> bookMarkers)
        {
            Convert(inputFilePath, outputFilePath, bookMarkers, null);
           
        }

        public void Convert(string inputFilePath, string outputFilePath, List<BookMarker> bookMarkers, DynamicBookMarker dynamicData)
        {
            Document document = new Document(inputFilePath);
            if(bookMarkers!=null)
            {
                bookMarkers.ForEach(p => document.Range.Replace(p.MarkerName, p.ReplaceText, false, false));
            }
            if(dynamicData!=null)
            {
                DocumentBuilder docBuilde = new DocumentBuilder(document);
                docBuilde.MoveToBookmark(dynamicData.StartBookMarker);
                var lineCount = dynamicData.Datas.GetLength(0);
                var colCount = dynamicData.Datas.GetLength(1);
                for(int i=0;i<lineCount;i++)
                {
                    for(int j=0;j<colCount;j++)
                    {
                        docBuilde.InsertCell();//添加一个单元格
                        var font = docBuilde.Font;
                        DynamicBookMarker.RowSetting rowSetting;
                        if (dynamicData.HasHeader && i==0)
                        {
                            rowSetting = dynamicData.HeaderSetting;
                            
                        }
                        else
                        {
                            rowSetting = dynamicData.BodySetting;
                        }
                        docBuilde.CellFormat.Shading.BackgroundPatternColor = rowSetting.BackgroundColor;//设置列头单元格背景色
                        font.Color = rowSetting.FontColor;
                        font.Name = rowSetting.FontName;
                        font.Size = rowSetting.FontSize;
                        font.Bold = rowSetting.Bold;

                        docBuilde.CellFormat.Width =dynamicData.CellWidths[j];//获取列头的宽度 
                        docBuilde.CellFormat.Borders.LineStyle = LineStyle.Single;//边框样式
                        docBuilde.CellFormat.Borders.Color = Color.Black;//边框颜色
                        docBuilde.CellFormat.VerticalMerge = CellMerge.None;
                        docBuilde.CellFormat.VerticalAlignment = CellVerticalAlignment.Center;//垂直居中对齐
                        docBuilde.ParagraphFormat.Alignment = ParagraphAlignment.Center;//水平居中对齐
                        docBuilde.Write(dynamicData.Datas[i,j]);//单元格写入数据
                    }
                    docBuilde.EndRow();//添加行结束
                }
            }
            document.Save(outputFilePath, SaveFormat.Pdf);


        }
        

        public void Signature(string filePath, string savePath, string keyPath, string postext)
        {
            //throw new NotImplementedException();

            KGPdfHummer hummer = null;
            FileStream os = null;
            try
            {
                int index = filePath.LastIndexOf('\\');
                string fileName = filePath.Substring(index + 1);       //原始文档路径
                os = new FileStream(savePath , FileMode.Create, FileAccess.ReadWrite); //签章或者数字签名后输出文件路径
                AbstractSign pdfExecute = null;
                /*进行电子签章*/
                hummer = KGPdfHummer.CreateInstance(filePath, null, true, os, true);           //创建文档编辑实例。
                pdfExecute = new PdfElectronicSeal4KG(keyPath, 1, "123456");     //与.key文件对接,签章编号默认为1，密码默认123456
                pdfExecute.SetPagen(hummer.GetNumberOfPages());                              //只在pdf最后一页加载电子签章
                hummer.AddExecute(pdfExecute);
                pdfExecute.SetText(postext);               //根据文字定位
                hummer.DoExecute();                       //执行电子签章任务
                File.Delete(filePath);
                
            }
            catch (Exception e)
            {
                e.ToString();
            }
            finally
            {
                if (os != null) try { os.Close(); }
                    catch { }
                if (hummer != null) hummer.Close();
            }
        }
    }
}
