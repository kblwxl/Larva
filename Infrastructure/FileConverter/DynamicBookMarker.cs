using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.FileConverter
{
    public class DynamicBookMarker
    {
        public string StartBookMarker { get; set; }
        public bool HasHeader { get; set; }
        public RowSetting HeaderSetting { get; set; }
        public RowSetting BodySetting { get; set; }
        public List<double> CellWidths { get; set; }
        public string[,] Datas { get; set; }

        public static DynamicBookMarker UseDefaultStyle(string startBookMarker,List<double> cellWidths,string[,] datas)
        {
            DynamicBookMarker retValue = new DynamicBookMarker();
            retValue.StartBookMarker = startBookMarker;
            retValue.HasHeader = true;
            retValue.CellWidths = cellWidths;
            retValue.Datas = datas;
            retValue.HeaderSetting = new RowSetting
            {
                BackgroundColor = System.Drawing.Color.FromArgb(93, 93, 93),
                FontColor = System.Drawing.Color.White,
                FontName = "Songti SC",
                FontSize = 8,
                Bold = true
            };
            retValue.BodySetting = new RowSetting
            {
                BackgroundColor = System.Drawing.Color.White,
                FontColor = System.Drawing.Color.Black,
                FontName = "微软雅黑",
                FontSize = 6,
                Bold = false
            };
            return retValue;
            
        }


        public class RowSetting
        {
            public Color BackgroundColor { get; set; }
            public Color FontColor { get; set; }
            public string FontName { get; set; }
            public int FontSize { get; set; }
            public bool Bold { get; set; }
        }
        
    }

    
}
