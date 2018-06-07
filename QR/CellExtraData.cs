using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QR.Drawing.Data
{
    public class CellExtraData
    {
        public Roles Role { get; set; }
    }// CellExtraData

    public enum Roles
    {
        HEAD,
        PATH,
        ELBOW,
        TAIL,
        BLOCK
    }
}
