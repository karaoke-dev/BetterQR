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
    public class BarStyler : Styler
    {
        private string[] singleImages;
        private string[] eyeImages;
        private string[] barImages;
        private int[] barLengths;

        //private Functions
        private void initBarStyler()
        {
            singleImages = new string[2];
            eyeImages = new string[3];
            barLengths = new int[2] { 3, 4 };
            barImages = new string[barLengths.Length];
        }

        //protected
        /// <summary>
        /// Mark All BLACK Cells, 
        /// Left is the bar left head,
        /// Up is the bar up head, 
        /// Marked is the bar's body,
        /// Single is the not in bar.
        /// Unmarked cells is the WHITE cell.
        /// </summary>
        protected void updateCellMarks()
        {
            Random rand_length = new Random();
            bool markable = true;

            foreach(DataCell cell in Matrix.CellMatrix)
            {
                //reset markable
                markable = true;

                if (!cell.IsEye() &&
                    !cell.Marks.ContainsKey(Keys.Role) &&
                    cell.Color == CellColor.BLACK)
                {
                    //Random Bar Length
                    int bar_length_index = rand_length.Next(2);
                    int bar_length = barLengths[bar_length_index];

                    DataCell currCell = cell;
                    //Judge if could horizontal draw bar
                    for(int i = 0; i < bar_length; i++)
                    {
                        if (currCell == null || currCell.IsEye() || currCell.Color != cell.Color || currCell.Marks.ContainsKey(Keys.Role))
                        {
                            markable = false;
                            break;
                        }
                        currCell = currCell.RightCell();
                    }
                    if (markable)
                    {
                        //mark horizontal bar
                        cell.Marks.Add(Keys.Role, Values.Left);
                        cell.Marks.Add(Keys.Count, bar_length.ToString());
                        currCell = cell.RightCell();
                        for(int i = 0; i < bar_length-1; i++)
                        {
                            currCell.Marks.Add(Keys.Role, Values.Marked);
                            currCell = currCell.RightCell();
                        }
                    }
                    else
                    {
                        markable = true;
                        currCell = cell;
                        for(int i = 0; i < bar_length; i++)
                        {
                            if(currCell == null || currCell.IsEye() || currCell.Color != cell.Color || currCell.Marks.ContainsKey(Keys.Role))
                            {
                                markable = false;
                                break;
                            }
                            currCell = currCell.DownCell();
                        }
                        if (markable)
                        {
                            cell.Marks.Add(Keys.Role, Values.Up);
                            cell.Marks.Add(Keys.Count, bar_length.ToString());
                            currCell = cell.DownCell();
                            for(int i = 0; i < bar_length-1; i++)
                            {
                                currCell.Marks.Add(Keys.Role, Values.Marked);
                                currCell = currCell.DownCell();
                            }
                        }
                        else
                        {
                            cell.Marks.Add(Keys.Role, Values.Single);
                        }
                    }
                }
            }
        }

        //public fucntions
        /// <summary>
        /// Only for DEMO
        /// </summary>
        /// <param name="folder"></param>
        /// <param name="eye_image"></param>
        /// <param name="bar_image1"></param>
        /// <param name="bar_image2"></param>
        /// <param name="single_image1"></param>
        /// <param name="single_image2"></param>
        public void InitStyle(string folder, string canvas_image, string eye_image, string bar_image1, string bar_image2, string single_image1, string single_image2)
        {
            MergeLayers(new Bitmap(folder + @"/" + canvas_image));
            eyeImages[0] = folder + @"/" + eye_image;
            eyeImages[1] = folder + @"/" + eye_image;
            eyeImages[2] = folder + @"/" + eye_image;
            barImages[0] = folder + @"/" + bar_image1;
            barImages[1] = folder + @"/" + bar_image2;
            singleImages[0] = folder + @"/" + single_image1;
            singleImages[1] = folder + @"/" + single_image2;
        }

        public override void Draw()
        {
            updateCellMarks();

            //Init layers
            Bitmap layer_black = NewLayer();
            Bitmap layer_eyes = NewIntegerPixelLayer();
            Bitmap layer_single = NewIntegerPixelLayer();
            Bitmap[] layer_bars = new Bitmap[barLengths.Length];
            Bitmap layer_background = NewLayer();
            Bitmap layer_canvas = NewLayer();
            Graphics paint = null;
            for(int i = 0; i < layer_bars.Length; i++)
            {
                layer_bars[i] = NewIntegerPixelLayer();
            }

            //draw eyes
            paint = Graphics.FromImage(layer_eyes);
            Bitmap image_lu = new Bitmap(eyeImages[0]);
            Bitmap image_ru = new Bitmap(eyeImages[1]);
            Bitmap image_ld = new Bitmap(eyeImages[2]);
            paint.DrawImage(image_lu, 
                GetEyeRectangle(EyePosition.LEFT_UP),
                new Rectangle(0, 0, image_lu.Width, image_lu.Height),
                GraphicsUnit.Pixel);
            paint.DrawImage(image_ru,
                GetEyeRectangle(EyePosition.RIGHT_UP),
                new Rectangle(0, 0, image_ru.Width, image_ru.Height),
                GraphicsUnit.Pixel);
            paint.DrawImage(image_ld,
                GetEyeRectangle(EyePosition.LEFT_DOWN),
                new Rectangle(0, 0, image_ld.Width, image_ld.Height),
                GraphicsUnit.Pixel);

            //draw bars
            for(int i = 0; i < layer_bars.Length; i++)
            {
                //horizontal
                paint = Graphics.FromImage(layer_bars[i]);
                int length = barLengths[i];
                Bitmap bar_image = new Bitmap(barImages[i]);
                var bar_h = from c in Matrix.CellMatrix.Cast<DataCell>()
                            where c.Marks.ContainsKey(Keys.Role) && c.Marks[Keys.Role] == Values.Left && int.Parse(c.Marks[Keys.Count]) == length
                            select c;
                foreach(var cell in bar_h)
                {
                    Rectangle cell_rect = GetCellRectangle(cell.Position.Row, cell.Position.Column);
                    Rectangle bar_rect = new Rectangle(cell_rect.X, cell_rect.Y, IntStep.Width * length, IntStep.Height);
                    paint.DrawImage(bar_image, bar_rect, new Rectangle(0, 0, bar_image.Width, bar_image.Height), GraphicsUnit.Pixel);
                }

                //verticle
                var bar_v = from c in Matrix.CellMatrix.Cast<DataCell>()
                            where c.Marks.ContainsKey(Keys.Role) && c.Marks[Keys.Role] == Values.Up && int.Parse(c.Marks[Keys.Count]) == length
                            select c;
                bar_image.RotateFlip(RotateFlipType.Rotate90FlipNone);
                foreach (var cell in bar_v)
                {
                    Rectangle cell_rect = GetCellRectangle(cell.Position.Row, cell.Position.Column);
                    Rectangle bar_rect = new Rectangle(cell_rect.X, cell_rect.Y, IntStep.Width, IntStep.Height * length);
                    paint.DrawImage(bar_image, bar_rect, new Rectangle(0, 0, bar_image.Width, bar_image.Height), GraphicsUnit.Pixel);
                }
            }

            //draw singles
            paint = Graphics.FromImage(layer_single);
            Random single_rand = new Random();
            Bitmap[] single_images = new Bitmap[singleImages.Length];
            for(int i = 0; i < single_images.Length; i++)
            {
                single_images[i] = new Bitmap(singleImages[i]);
            }
            var singles = from c in Matrix.CellMatrix.Cast<DataCell>() where c.Marks.ContainsKey(Keys.Role) && c.Marks[Keys.Role] == Values.Single select c;
            foreach(var cell in singles)
            {
                int img_index = single_rand.Next(singleImages.Length);
                paint.DrawImage(single_images[img_index],
                    GetCellRectangle(cell.Position.Row, cell.Position.Column),
                    new Rectangle(0, 0, single_images[img_index].Width, single_images[img_index].Height),
                    GraphicsUnit.Pixel);
            }
            //draw black_layer
            paint = Graphics.FromImage(layer_black);
            paint.DrawImage(layer_eyes,
                new RectangleF(CodePosition.X, CodePosition.Y, CodeSize.Width, CodeSize.Height),
                new Rectangle(0, 0, layer_eyes.Width, layer_eyes.Height),
                GraphicsUnit.Pixel);
            paint.DrawImage(layer_single,
                new RectangleF(CodePosition.X, CodePosition.Y, CodeSize.Width, CodeSize.Height),
                new Rectangle(0, 0, layer_single.Width, layer_single.Height),
                GraphicsUnit.Pixel);
            for(int i = 0; i < layer_bars.Length; i++)
            {
                paint.DrawImage(layer_bars[i],
                    new RectangleF(CodePosition.X, CodePosition.Y, CodeSize.Width, CodeSize.Height),
                    new Rectangle(0, 0, layer_bars[i].Width, layer_bars[i].Height),
                    GraphicsUnit.Pixel);
            }

            //draw background
            paint = Graphics.FromImage(layer_background);
            paint.FillRectangle(
                new SolidBrush(Default.WHITE),
                new RectangleF(CodePosition.X, CodePosition.Y, CodeSize.Width, CodeSize.Height));

            //draw canvas
            paint = Graphics.FromImage(layer_canvas);
            paint.FillRectangle(new SolidBrush(Default.BG_COLOR),
                new RectangleF(0, 0, CanvasSize.Width, CanvasSize.Height));

            //Merge layers
            MergeLayers( layer_black);
        }

        //construction
        public BarStyler(int canvas_length, float margin, MarginMode margin_mode) : base(canvas_length, margin, margin_mode)
        {
            initBarStyler();
        }
        public BarStyler(int canvas_length, float margin, MarginMode margin_mode, string json_path) : base(canvas_length, margin, margin_mode, json_path)
        {
            initBarStyler();
        }
    }
}
