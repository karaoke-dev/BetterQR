using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using QR.Drawing.Data;
using QR.Drawing.Graphic;
using QR.Drawing.Util;
using System.Drawing;
using System.Drawing.Imaging;

namespace QR.Drawing.Graphic
{
    class BasicStyler : Styler
    {
        private string black_pattern;
        private string white_pattern;
        private string background_image;
        private string canvas_image;
        private Color black_color;
        private Color white_color;
        private Color background_color;
        private Color canvas_color;

        //Public Methods
        public BasicStyler(int canvas_length, float margin, MarginMode margin_mode, string json_path)
            :base(canvas_length, margin, margin_mode, json_path)
        {
            //init colors
            black_color = Default.BLACK;
            white_color = Default.WHITE;
            background_color = Default.BG_COLOR;
            canvas_color = Default.CANVAS_COLOR;
        }

        public void InitStyle(string folder, string black, string bg)
        {
            InitStyle(folder, black, null, bg, null);
        }
        public void InitStyle(string folder, string black_pattern_img, string white_pattern_img, string background_img, string canvas_img)
        {
            if (black_pattern_img != null)
            {
                black_pattern = folder + @"/" + black_pattern_img;
            }
            if (white_pattern_img != null)
            {
                white_pattern = folder + @"/" + white_pattern_img;
            }
            if (background_img != null)
            {
                background_image = folder + @"/" + background_img;
            }
            if (canvas_img != null)
            {
                canvas_image = folder + @"/" + canvas_img;
            }
        }

        public override void Draw()
        {
            Bitmap layer_black = NewLayer();
            Bitmap layer_black_tmp = NewIntegerPixelLayer();
            Bitmap layer_white = NewLayer();
            Bitmap layer_white_tmp = NewIntegerPixelLayer();
            Bitmap layer_background = NewLayer();
            Bitmap layer_canvas = NewLayer();
            Graphics paint = null;

            //draw black
            paint = Graphics.FromImage(layer_black_tmp);
            if (black_pattern != null)
            {
                Bitmap pattern_black = new Bitmap(black_pattern);
                var black = from b in Matrix.CellMatrix.Cast<DataCell>() where b.Color == CellColor.BLACK select b;
                foreach (var b in black)
                {
                    paint.DrawImage(pattern_black,
                            GetCellRectangle(b.Position.Row, b.Position.Column),
                            new Rectangle(0, 0, pattern_black.Width, pattern_black.Height),
                            GraphicsUnit.Pixel);
                }
            }
            paint = Graphics.FromImage(layer_black);
            paint.DrawImage(layer_black_tmp,
                new RectangleF(CodePosition.X, CodePosition.Y, CodeSize.Width, CodeSize.Height),
                new Rectangle(0, 0, layer_black_tmp.Width, layer_black_tmp.Height), GraphicsUnit.Pixel);

            //draw white
            paint = Graphics.FromImage(layer_white_tmp);
            if (white_pattern != null)
            {
                Bitmap pattern_white = new Bitmap(white_pattern);
                var white = from w in Matrix.CellMatrix.Cast<DataCell>() where w.Color == CellColor.WHITE select w;
                foreach (var w in white)
                {
                    paint.DrawImage(pattern_white,
                            GetCellRectangle(w.Position.Row, w.Position.Column),
                            new Rectangle(0, 0, pattern_white.Width, pattern_white.Height),
                            GraphicsUnit.Pixel);
                }
            }
            paint = Graphics.FromImage(layer_white);
            paint.DrawImage(layer_white_tmp,
                new RectangleF(CodePosition.X, CodePosition.Y, CodeSize.Width, CodeSize.Height),
                new Rectangle(0, 0, layer_black_tmp.Width, layer_black_tmp.Height), GraphicsUnit.Pixel);

            //draw background
            paint = Graphics.FromImage(layer_background);
            if (background_image != null)
            {
                Bitmap bg_img = new Bitmap(background_image);
                paint.DrawImage(bg_img,
                    new RectangleF(CodePosition.X, CodePosition.Y, CodeSize.Width, CodeSize.Height),
                        new Rectangle(0, 0, bg_img.Width, bg_img.Height), GraphicsUnit.Pixel);
            }

            //draw canvas
            paint = Graphics.FromImage(layer_canvas);
            if (canvas_image != null)
            {
                Bitmap c_img = new Bitmap(canvas_image);
                paint.DrawImage(c_img, new Rectangle(0, 0, CanvasSize.Width, CanvasSize.Height),
                        new Rectangle(0, 0, c_img.Width, c_img.Height),
                        GraphicsUnit.Pixel);
            }
            else
            {
                paint.FillRectangle(new SolidBrush(canvas_color), new Rectangle(0, 0, CanvasSize.Width, CanvasSize.Height));
            }

            //merge layers
            MergeLayers(layer_canvas, layer_background, layer_white, layer_black);
        }
    }
}
