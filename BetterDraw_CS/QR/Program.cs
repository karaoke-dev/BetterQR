using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Drawing;
using QR.Drawing.Util;
using QR.Drawing.Graphic;

namespace QR.Drawing
{
    class Program
    {
        static void Main(string[] args)
        {
            //Data.DataMatrix dm = new Data.DataMatrix(21);
            //bool[,] info = dm.MatrixColorInfo;
            //Traverse.PrintMatirx<bool>(info);

            //Styler st = new Styler(1000, 900);
            //st.Draw(
            //    new TextureBrush(new Bitmap(@"Patterns/t_p.png")),
            //    new TextureBrush(new Bitmap(@"Patterns/t_bg.jpg")),
            //    Default.BG_COLOR);
            //st.Display(800, 800);
            //st.Save(@"Patterns/2.png");

            //BlockStyler bs = new BlockStyler(1000, 50, MarginMode.PIXEL);
            //bs.Draw();
            //bs.Display(800, 800);
            //bs.Save(@"Patterns/b.png");


            BarStyler bs = new BarStyler(1000, 50, MarginMode.PIXEL);
            bs.initDefaultBarStyler("BarPatterns", "canvas.png", "eye.png", "b3.png", "b4.png", "s1.png", "s2.png");
            bs.Draw();
            bs.Display(800, 800);
            bs.Save(@"Patterns/bars.png");


        }
    }
}
