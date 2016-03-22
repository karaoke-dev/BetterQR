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
            string j_path = @"Resources/json_file.json";
            //Data.DataMatrix dm = new Data.DataMatrix(21);
            //bool[,] info = dm.MatrixColorInfo;
            //Traverse.PrintMatirx<bool>(info);

            //Styler st = new Styler(1000, 50, MarginMode.PIXEL, j_path);
            //st.InitStyle(@"Patterns", "t_p.png", "t_bg.jpg");
            //st.Draw();
            //st.Display(800, 800);
            //st.Save(@"Patterns/2.png");

            //BlockStyler bs = new BlockStyler(1000, 50, MarginMode.PIXEL, j_path);
            //bs.InitBlockStyle("StrokePatterns", "center.png", "single_border.png", "end_border.png", "elbow_border.png", "path_border.png", "t_border.png", "corner_border.png");
            //bs.Draw();
            //bs.Display(800, 800);
            //bs.Save(@"Patterns/b.png");

            BarStyler bs = new BarStyler(1000, 50, MarginMode.PIXEL, j_path);
            bs.InitBarStyle("BarPatterns", "canvas.png", "eye.png", "b3.png", "b4.png", "s1.png", "s2.png");
            bs.Draw();
            bs.Display(800, 800);
            bs.Save(@"Patterns/bars.png");
        }
    }
}
