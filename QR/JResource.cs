using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QR.Drawing.Data
{
    class JsonResource
    {
        //propeties
        public string Message { get; set; }
        public string Version { get; set; }
        public string Mask { get; set; }
        public bool[,] Matrix { get; set; }
        public int Size { get; set; }
    }
}
