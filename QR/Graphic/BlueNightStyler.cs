using System.Drawing;
using System.Linq;
using QR.Drawing.Data;

namespace QR.Drawing.Graphic
{
    public class BlueNightStyler : Styler
    {
        protected string blue_pattern;

        public BlueNightStyler(int canvas_length, float margin, MarginMode margin_mode, string json_path)
            : base(canvas_length, margin, margin_mode, json_path)
        {
        }

        public void InitStyle(string folder, string blue)
        {
            blue_pattern = folder + @"/" + blue;
        }

        public override void Draw()
        {
            Bitmap layer_white = NewLayer();
            Bitmap layer_black = NewLayer();
            Bitmap layer_canvas = NewLayer();
            Bitmap white_tmp = NewIntegerPixelLayer();
            Graphics paint = Graphics.FromImage(white_tmp);

            //draw white
            Bitmap blue_img = new Bitmap(blue_pattern);
            var wc = from c in Matrix.CellMatrix.Cast<DataCell>() where c.Color == CellColor.WHITE select c;
            foreach (var c in wc)
            {
                paint.DrawImage(blue_img, GetCellRectangle(c.Position.Row, c.Position.Column),
                    new Rectangle(0, 0, blue_img.Width, blue_img.Height), GraphicsUnit.Pixel);
            }
            paint = Graphics.FromImage(layer_white);
            paint.DrawImage(white_tmp, new RectangleF(CodePosition.X, CodePosition.Y, CodeSize.Width, CodeSize.Height),
                new Rectangle(0, 0, white_tmp.Width, white_tmp.Height), GraphicsUnit.Pixel);

            //draw black
            paint = Graphics.FromImage(layer_black);
            paint.FillRectangle(new SolidBrush(Color.Black), CodePosition.X, CodePosition.Y, CodeSize.Width,
                CodeSize.Height);

            //draw canvas
            paint = Graphics.FromImage(layer_canvas);
            paint.FillRectangle(new SolidBrush(Color.White), 0, 0, Canvas.Width, Canvas.Height);

            MergeLayers(layer_canvas, layer_black, layer_white);
        }
    }
}