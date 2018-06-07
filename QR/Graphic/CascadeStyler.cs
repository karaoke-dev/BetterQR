using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Drawing;
using QR.Drawing.Graphic;
using QR.Drawing.Util;
using QR.Drawing.Data;

namespace QR.Drawing.Graphic
{
    public class CascadeStyler : NeonNightStyler
    {
        protected string Cas1 { get; set; }
        protected string Cas2 { get; set; }
        protected string Cas3 { get; set; }
        protected string Cas4 { get; set; }

        public CascadeStyler(int canvas_length, int margin, string json_path):base(canvas_length,margin, json_path) { }

        public void InitStyle(string folder, string cas1, string cas2, string cas3, string cas4)
        {
            Cas1 = folder + @"/" + cas1;
            Cas2 = folder + @"/" + cas2;
            Cas3 = folder + @"/" + cas3;
            Cas4 = folder + @"/" + cas4;
        }

        protected bool MarkCas2(DataCell cell)
        {
            if (FeatureCell(cell))
            {
                int feature = cell.Feature(2, 1, FeatureCell);
                if (feature == 3)
                {
                    cell.Marks.Add(Keys.Role, Values.Cas2Begin);
                    cell.DownCell().Marks.Add(Keys.Role, Values.Cas2);
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        protected bool MarkCas3(DataCell cell)
        {
            if (FeatureCell(cell))
            {
                int feature = cell.Feature(3, 1, FeatureCell);
                if (feature == 7)
                {
                    cell.Marks.Add(Keys.Role, Values.Cas3Begin);
                    cell.DownCell().Marks.Add(Keys.Role, Values.Cas3);
                    cell.DownCell().DownCell().Marks.Add(Keys.Role, Values.Cas3);
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        protected bool MarkCas4(DataCell cell)
        {
            if (FeatureCell(cell))
            {
                int feature = cell.Feature(4, 1, FeatureCell);
                if (feature == 15)
                {
                    cell.Marks.Add(Keys.Role, Values.Cas4Begin);
                    cell.DownCell().Marks.Add(Keys.Role, Values.Cas3);
                    cell.DownCell().DownCell().Marks.Add(Keys.Role, Values.Cas3);
                    cell.DownCell().DownCell().DownCell().Marks.Add(Keys.Role, Values.Cas3);
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        protected new void UpdateCellMarks()
        {
            var cells = from c in Matrix.CellMatrix.Cast<DataCell>() where c.Color == CellColor.WHITE select c;
            foreach(var c in cells)
            {
                if (MarkCas4(c)) { continue; }
                if (MarkCas3(c)) { continue; }
                if (MarkCas2(c)) { continue; }
                if (!c.Marks.ContainsKey(Keys.Role))
                {
                    c.Marks.Add(Keys.Role, Values.Marked);
                }
            }
        }

        public override void Draw()
        {
            UpdateCellMarks();

            Bitmap layer_canvas = NewLayer();
            Bitmap layer_black = NewLayer();
            Bitmap layer_white = NewLayer();
            Bitmap white_tmp = NewIntegerPixelLayer();
            Graphics paint;
            
            //draw canvas
            paint = Graphics.FromImage(layer_canvas);
            paint.FillRectangle(new SolidBrush(Default.CANVAS_COLOR), new Rectangle(0, 0, Canvas.Width, Canvas.Height));
            
            //draw black
            paint = Graphics.FromImage(layer_black);
            paint.FillRectangle(new SolidBrush(Default.BLACK), new RectangleF(CodePosition.X, CodePosition.Y, CodeSize.Width, CodeSize.Height));
            
            //draw white
            paint = Graphics.FromImage(white_tmp);
            var cells = from c in Matrix.CellMatrix.Cast<DataCell>() where c.Color == CellColor.WHITE select c;
            foreach(var c in cells)
            {
                if (true)
                {
                    string val = c.Marks[Keys.Role];
                    if (val == Values.Cas4Begin)
                    {
                        DrawImage(paint, new Bitmap(Cas4), c, 4, 1);
                    }
                    if (val == Values.Cas3Begin)
                    {
                        DrawImage(paint, new Bitmap(Cas3), c, 3, 1);
                    }
                    if (val == Values.Cas2Begin)
                    {
                        DrawImage(paint, new Bitmap(Cas2), c, 2, 1);
                    }
                    if (val == Values.Marked)
                    {
                        DrawImage(paint, new Bitmap(Cas1), c, 1, 1);
                    }
                }
            }
            paint = Graphics.FromImage(layer_white);
            paint.DrawImage(white_tmp, new RectangleF(CodePosition.X, CodePosition.Y, CodeSize.Width, CodeSize.Height), new Rectangle(0, 0, white_tmp.Width, white_tmp.Height), GraphicsUnit.Pixel);

            //merge
            MergeLayers(layer_canvas, layer_black, layer_white);
        }
    }
}
