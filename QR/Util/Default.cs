using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Drawing;

namespace QR.Drawing.Util
{
    public class Default
    {
        public static readonly Color WHITE = Color.White;
        public static readonly Color BLACK = Color.Black;
        public static readonly Color BG_COLOR = Color.White;
        public static readonly Color CANVAS_COLOR = Color.White;
        public static readonly Color WINDOW_BG = Color.LightBlue;
        public static readonly string WINDOW_TITLE = "QR Code Display";

        public static readonly bool[,] MATRIX_COLOR_INFO = new bool[,]
        {
            {true,true,true,true,true,true,true,false,true,true,true,false,false,false,true,true,true,true,true,true,true},
            {true,false,false,false,false,false,true,false,false,true,false,true,false,false,true,false,false,false,false,false,true},
            {true,false,true,true,true,false,true,false,true,false,true,true,true,false,true,false,true,true,true,false,true},
            {true,false,true,true,true,false,true,false,false,false,false,true,true,false,true,false,true,true,true,false,true},
            {true,false,true,true,true,false,true,false,true,true,false,false,false,false,true,false,true,true,true,false,true},
            {true,false,false,false,false,false,true,false,false,false,false,true,true,false,true,false,false,false,false,false,true},
            {true,true,true,true,true,true,true,false,true,false,true,false,true,false,true,true,true,true,true,true,true},
            {false,false,false,false,false,false,false,false,true,true,true,true,false,false,false,false,false,false,false,false,false},
            {false,false,false,false,false,true,true,false,false,false,true,true,false,false,true,false,true,false,true,false,true},
            {true,false,false,true,true,true,false,false,true,true,false,true,true,true,true,false,true,false,false,false,true},
            {true,true,true,true,true,false,true,false,true,false,true,true,true,true,false,false,true,true,false,true,false},
            {false,false,false,true,false,true,false,false,false,false,false,false,true,false,true,false,true,false,true,true,true},
            {true,true,false,true,false,true,true,true,false,true,true,false,false,true,true,false,false,true,true,false,false},
            {false,false,false,false,false,false,false,false,true,false,true,false,false,false,true,true,false,true,false,false,false},
            {true,true,true,true,true,true,true,false,false,false,false,false,false,true,false,true,false,false,false,true,false},
            {true,false,false,false,false,false,true,false,true,false,true,true,true,false,false,true,true,true,false,false,false},
            {true,false,true,true,true,false,true,false,false,true,true,true,false,true,false,true,false,false,true,false,false},
            {true,false,true,true,true,false,true,false,false,false,true,false,true,true,false,false,false,false,true,false,false},
            {true,false,true,true,true,false,true,false,false,false,false,true,false,false,true,false,false,false,true,true,true},
            {true,false,false,false,false,false,true,false,false,true,false,true,false,true,false,false,true,false,true,false,false},
            {true,true,true,true,true,true,true,false,false,false,false,true,true,true,true,false,true,true,true,true,false}
        };
    }
}
