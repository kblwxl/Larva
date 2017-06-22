using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.FileConverter
{
    public class SupportdFilePoint
    {
        public string Description { get; set; }
        public List<string> SupportedFileExt { get; set; }
        public SupportdFilePoint()
            :this(string.Empty)
        {

        }
       
        public SupportdFilePoint(string description,params string[] supportedFileExt)
        {
            this.Description = description;
            SupportedFileExt = new List<string>(supportedFileExt.Select(p=>p.ToUpper()));

        }
    }
}
