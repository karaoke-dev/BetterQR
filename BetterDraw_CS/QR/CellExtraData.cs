using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QR.Drawing.Data
{
    class CellExtraData
    {
        public Roles Role { get; set; }
    }// CellExtraData

    enum Roles
    {
        HEAD,
        PATH,
        ELBOW,
        TAIL,
        BLOCK
    }
}
