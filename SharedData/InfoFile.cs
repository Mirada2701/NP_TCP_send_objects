using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedData
{
    public class InfoFile
    {
        public string FileName { get; set; }
        public string Extension { get; set; }
        public byte[] Data { get; set; }
    }
}
