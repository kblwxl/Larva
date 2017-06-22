using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.FileConverter
{
    public class BookMarker
    {
        public string MarkerName { get;  set; }
        public string ReplaceText { get;  set; }

        public BookMarker()
            :this(string.Empty,string.Empty)
        { }
        public BookMarker(string name,string replaceText)
        {
            this.MarkerName = name;
            this.ReplaceText = replaceText;
        }
    }
}
