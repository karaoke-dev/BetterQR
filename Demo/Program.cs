using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Drawing;
using QR.Drawing.Util;
using QR.Drawing.Graphic;

namespace Demo
{
    class Program
    {
        static void Main(string[] args)
        {
            string j_path = @"Resources/json_file.json";
            //Data.DataMatrix dm = new Data.DataMatrix(21);
            //bool[,] info = dm.MatrixColorInfo;
            //Traverse.PrintMatirx<bool>(info);

            //BasicStyler st = new BasicStyler(1000, 50, MarginMode.PIXEL, j_path);
            //st.InitStyle(@"Patterns", "point.png", "t_bg.jpg");
            //st.Draw();
            //st.Display(800, 800);
            //st.Save(@"Patterns/2.png");

            //BlockStyler bs = new BlockStyler(1000, 50, MarginMode.PIXEL, j_path);
            //bs.InitStyle("StrokePatterns", "center.png", "single_border.png", "end_border.png", "elbow_border.png", "path_border.png", "t_border.png", "corner_border.png");
            //bs.Draw();
            //bs.Display(800, 800);
            //bs.Save(@"Patterns/b.png");

            //BarStyler bs = new BarStyler(1000, 50, MarginMode.PIXEL, j_path);
            //bs.InitStyle("BarPatterns", "canvas.png", "eye.png", "b3.png", "b4.png", "s1.png", "s2.png");
            //bs.Draw();
            //bs.Display(800, 800);
            //bs.Save(@"Patterns/bars.png");

            //TetrisStyler ts = new TetrisStyler(800, 1.5f, MarginMode.CELL, j_path);
            //ts.InitStyle("TetrisPatterns", "point.png", "top_sample.png", "bottom_sample.png", Color.FromArgb(255,156, 161, 159));
            //ts.Draw();
            //ts.Display();
            //ts.Save(@"patterns/Tetris.png");

            //BlueNightStyler bs = new BlueNightStyler(800, 50, MarginMode.PIXEL, j_path);
            //bs.InitStyle(@"Patterns", "blue_point.png");
            //bs.Draw();
            //bs.Display();
            //bs.Save("patterns/BlueNight.png");

            //NeonNightStyler ns = new NeonNightStyler(800, 2, j_path);
            //ns.InitStyle("Neon", "blue_point.png", "red_circle.png", "bean.png", "umbrella.png", "bulb.png", "wave.png", "heart.png", "pointer.png", "bike.png", "brand.png");
            //ns.Draw();
            //ns.Display();

            CascadeStyler cs = new CascadeStyler(800, 2, j_path);
            cs.InitStyle("Neon", "cas1.png", "cas2.png", "cas3.png", "cas4.png");
            cs.Draw();
            cs.Display();
        }
    }
}
