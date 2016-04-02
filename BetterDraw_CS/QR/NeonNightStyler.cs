using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Drawing;
using QR.Drawing.Graphic;
using QR.Drawing.Data;
using QR.Drawing.Util;

namespace QR.Drawing.Graphic
{
    class NeonNightStyler : Styler
    {
        protected string YelloCircle { get; set; }
        protected string RedCircle { get; set; }
        protected string Bean { get; set; }
        protected string Umbrella { get; set; }
        protected string Bulb { get; set; }
        protected string Wave { get; set; }
        protected string Heart { get; set; }
        protected string Pointer { get; set; }
        protected string Bike { get; set; }
        protected string Brand { get; set; }

        //construction
        /// <summary>
        /// In this style, margin must be mesured as cell, the margin mode will be MarginMode.CELL automatically.
        /// </summary>
        /// <param name="canvas_length">The length of the canvas width and height.</param>
        /// <param name="margin">How much cells width as the margin width.</param>
        /// <param name="json_path">Json information to draw based.</param>
        public NeonNightStyler(int canvas_length, int margin, string json_path)
            : base(canvas_length, (float)margin, MarginMode.CELL, json_path)
        { }

        //Protected Methods
        protected bool MarkEyeWhites(DataCell cell)
        {
            Random rand = new Random(DateTime.Now.Millisecond);
            if (cell.IsEye() && cell.Color == CellColor.WHITE)
            {
                cell.Marks.Add(Keys.Role, Values.YellowCircle);

                if (cell.UpCell().Color != cell.DownCell().Color)
                {

                    double rand_value = rand.NextDouble();
                    if (rand_value > 0.5)
                    {
                        cell.Marks[Keys.Role] += Values.RedDashedCircle;
                    }
                }

                return true;
            }
            else { return false; }
        }

        protected bool MarkEyeLines(DataCell cell)
        {
            if (cell.Position.Row == 7)
            {
                if (cell.Position.Column < 7)
                {
                    cell.Marks.Add(Keys.Role, Values.BeanToRight);
                }
                else if (cell.Position.Column == 7)
                {
                    cell.Marks.Add(Keys.Role, Values.BeanToRightUp);
                }
                else if (cell.Position.Column == cell.RootMatrix.MatrixOrder - 8)
                {
                    cell.Marks.Add(Keys.Role, Values.BeanToLeftUp);
                }
                else if (cell.Position.Column > cell.RootMatrix.MatrixOrder - 8)
                {
                    cell.Marks.Add(Keys.Role, Values.BeanToLeft);
                }
                else
                {
                    return false;
                }
            }
            else if (cell.Position.Row < 7)
            {
                if (cell.Position.Column == 7 || cell.Position.Column == cell.RootMatrix.MatrixOrder - 8)
                {
                    cell.Marks.Add(Keys.Role, Values.BeanToUp);
                }
                else
                {
                    return false;
                }
            }
            else if (cell.Position.Row == cell.RootMatrix.MatrixOrder - 8)
            {
                if (cell.Position.Column < 7)
                {
                    cell.Marks.Add(Keys.Role, Values.BeanToRight);
                }
                else if (cell.Position.Column == 7)
                {
                    cell.Marks.Add(Keys.Role, Values.BeanToRightDown);
                }
                else
                {
                    return false;
                }
            }
            else if (cell.Position.Row > cell.RootMatrix.MatrixOrder - 8)
            {
                if (cell.Position.Column == 7)
                {
                    cell.Marks.Add(Keys.Role, Values.BeanToDown);
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
            return true;
        } //MarkEyeLines

        protected bool MarkUmbrella(DataCell cell)
        {
            if (FeatureCell(cell))
            {
                int feature = cell.Feature(3, 3, FeatureCell);
                if (feature == 191 || feature == 255 || feature == 447 || feature == 511)
                {
                    cell.Marks.Add(Keys.Role, Values.UmbrellaBegin);
                    cell.RightCell().Marks.Add(Keys.Role, Values.Umbrella);
                    cell.RightCell().RightCell().Marks.Add(Keys.Role, Values.Umbrella);
                    cell.DownCell().Marks.Add(Keys.Role, Values.Umbrella);
                    cell.DownCell().RightCell().Marks.Add(Keys.Role, Values.Umbrella);
                    cell.DownCell().RightCell().RightCell().Marks.Add(Keys.Role, Values.Umbrella);
                    cell.DownCell().DownCell().RightCell().Marks.Add(Keys.Role, Values.Umbrella);
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

        protected bool MarkBulb(DataCell cell)
        {
            if (FeatureCell(cell))
            {
                int feature = cell.Feature(3, 2, FeatureCell);
                if (feature == 63)
                {
                    cell.Marks.Add(Keys.Role, Values.BulbBegin);
                    cell.RightCell().Marks.Add(Keys.Role, Values.Bulb);
                    cell.DownCell().Marks.Add(Keys.Role, Values.Bulb);
                    cell.DownCell().RightCell().Marks.Add(Keys.Role, Values.Bulb);
                    cell.DownCell().DownCell().Marks.Add(Keys.Role, Values.Bulb);
                    cell.DownCell().DownCell().RightCell().Marks.Add(Keys.Role, Values.Bulb);
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

        protected bool MarkWave(DataCell cell)
        {
            if (FeatureCell(cell))
            {
                int feature = cell.Feature(1, 5, FeatureCell);
                if (feature == 31)
                {
                    cell.Marks.Add(Keys.Role, Values.WaveBegin);
                    cell.RightCell().Marks.Add(Keys.Role, Values.Wave);
                    cell.RightCell().RightCell().Marks.Add(Keys.Role, Values.Wave);
                    cell.RightCell().RightCell().RightCell().Marks.Add(Keys.Role, Values.Wave);
                    cell.RightCell().RightCell().RightCell().RightCell().Marks.Add(Keys.Role, Values.Wave);
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

        protected bool MarkHeart(DataCell cell)
        {
            if (FeatureCell(cell))
            {
                int feature = cell.Feature(2, 2, FeatureCell);
                if (feature == 15)
                {
                    cell.Marks.Add(Keys.Role, Values.HeartBegin);
                    cell.RightCell().Marks.Add(Keys.Role, Values.Heart);
                    cell.DownCell().Marks.Add(Keys.Role, Values.Heart);
                    cell.DownCell().RightCell().Marks.Add(Keys.Role, Values.Heart);
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

        protected bool MarkPointer(DataCell cell)
        {
            if (FeatureCell(cell))
            {
                int feature = cell.Feature(2, 1, FeatureCell);
                if (feature == 3)
                {
                    cell.Marks.Add(Keys.Role, Values.PointerBegin);
                    cell.DownCell().Marks.Add(Keys.Role, Values.Pointer);
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
        protected bool MarkBike(DataCell cell)
        {
            if (FeatureCell(cell))
            {
                int feature = cell.Feature(1, 2, FeatureCell);
                if (feature == 3)
                {
                    cell.Marks.Add(Keys.Role, Values.BikeBegin);
                    cell.RightCell().Marks.Add(Keys.Role, Values.Bike);
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

        protected void UpdateCellMarks()
        {
            var white_cells = from w in Matrix.CellMatrix.Cast<DataCell>() where w.Color == CellColor.WHITE select w;
            foreach (var cell in white_cells)
            {
                if (MarkEyeWhites(cell)) { continue; }
                if (MarkEyeLines(cell)) { continue; }
            }
            foreach (var cell in white_cells)
            {
                //if (MarkUmbrella(cell)) { continue; }
                //if (MarkBulb(cell)) { continue; }
                //if (MarkWave(cell)) { continue; }
                //if (MarkHeart(cell)) { continue; }
                //if (MarkPointer(cell)) { continue; }
                //if (MarkBike(cell)) { continue; }
                if (!cell.Marks.ContainsKey(Keys.Role))
                {
                    cell.Marks.Add(Keys.Role, Values.Marked);
                }
            }
        }

        /// <summary>
        /// If the cell is white and no marked, feature it.
        /// </summary>
        /// <param name="cell"></param>
        /// <returns></returns>
        protected bool FeatureCell(DataCell cell)
        {
            if (cell.Color == CellColor.WHITE && !cell.Marks.ContainsKey(Keys.Role)) { return true; }
            else { return false; }
        }

        //public methoeds
        public void InitStyle(string folder, string yellow_circle, string red_circle, string bean, string umbrella, string bulb, string wave, string heart, string pointer, string bike, string brand = null)
        {
            YelloCircle = folder + @"/" + yellow_circle;
            RedCircle = folder + @"/" + red_circle;
            Bean = folder + @"/" + bean;
            Umbrella = folder + @"/" + umbrella;
            Bulb = folder + @"/" + bulb;
            Wave = folder + @"/" + wave;
            Heart = folder + @"/" + heart;
            Pointer = folder + @"/" + pointer;
            Bike = folder + @"/" + bike;
            Brand = folder + @"/" + brand;
        }

        public override void Draw()
        {
            UpdateCellMarks();

            Bitmap layer_canvas = NewLayer();
            Bitmap layer_black = NewLayer();
            Bitmap layer_white = NewLayer();
            Bitmap white_tmp = NewIntegerPixelLayer();
            Bitmap layer_brand = NewLayer();
            Bitmap brand_tmp = NewIntegerPixelLayer();
            Graphics paint;

            //draw canvas
            paint = Graphics.FromImage(layer_canvas);
            paint.FillRectangle(new SolidBrush(Default.WHITE), new Rectangle(0, 0, Canvas.Width, Canvas.Height));

            //draw black_layer
            paint = Graphics.FromImage(layer_black);
            paint.FillRectangle(new SolidBrush(Default.BLACK), new RectangleF(CodePosition.X, CodePosition.Y, CodeSize.Width, CodeSize.Height));

            //draw white_layer
            paint = Graphics.FromImage(white_tmp);
            var cells = from c in Matrix.CellMatrix.Cast<DataCell>() where c.Color == CellColor.WHITE select c;
            foreach (var c in cells)
            {
                Random rand = new Random();
                if (c.Marks.ContainsKey(Keys.Role))
                {
                    string val = c.Marks[Keys.Role];

                    //draw eye white
                    if (val.Contains(Values.YellowCircle))
                    {
                        DrawImage(paint, new Bitmap(YelloCircle), c, 1, 1);
                        if (val.Contains(Values.RedDashedCircle))
                        {
                            DrawImage(paint, new Bitmap(RedCircle), c, 1, 1);
                        }
                        continue;
                    }

                    //draw eye line
                    if (val == Values.BeanToRight)
                    {
                        Bitmap bean = new Bitmap(Bean);
                        DrawImage(paint, bean, c, 1, 1);
                        continue;
                    }
                    if (val == Values.BeanToLeft)
                    {
                        Bitmap bean = new Bitmap(Bean);
                        bean.RotateFlip(RotateFlipType.RotateNoneFlipX);
                        DrawImage(paint, bean, c, 1, 1);
                        continue;
                    }
                    if (val == Values.BeanToDown)
                    {
                        Bitmap bean = new Bitmap(Bean);
                        bean.RotateFlip(RotateFlipType.Rotate90FlipNone);
                        DrawImage(paint, bean, c, 1, 1);
                        continue;
                    }
                    if (val == Values.BeanToUp)
                    {
                        Bitmap bean = new Bitmap(Bean);
                        bean.RotateFlip(RotateFlipType.Rotate270FlipNone);
                        DrawImage(paint, bean, c, 1, 1);
                        continue;
                    }
                    if (val == Values.BeanToRightUp)
                    {
                        Bitmap bean = new Bitmap(Bean);
                        Graphics g = Graphics.FromImage(bean);
                        g.RotateTransform(45.0f);
                        DrawImage(paint, bean, c, 1, 1, 0, 0);
                        continue;
                    }
                    if (val == Values.BeanToLeftUp)
                    {
                        Bitmap bean = new Bitmap(Bean);
                        Graphics g = Graphics.FromImage(bean);
                        g.RotateTransform(135.0f);
                        DrawImage(paint, bean, c, 1, 1, 0, 0);
                        continue;
                    }
                    if (val == Values.BeanToRightDown)
                    {
                        Bitmap bean = new Bitmap(Bean);
                        Graphics g = Graphics.FromImage(bean);
                        g.RotateTransform(315.0f);
                        DrawImage(paint, bean, c, 1, 1, 0, 0);
                        continue;
                    }

                    //draw umbrella
                    if (val == Values.UmbrellaBegin)
                    {
                        DrawImage(paint, new Bitmap(Umbrella), c, 3.5f, 3.5f, -0.26f, -0.25f);
                        continue;
                    }

                    //draw bulb
                    if (val == Values.BulbBegin)
                    {
                        DrawImage(paint, new Bitmap(Bulb), c, 3, 3, -0.5f, 0);
                        continue;
                    }

                    //draw wave
                    if (val == Values.WaveBegin)
                    {
                        Bitmap wave = new Bitmap(Wave);
                        DrawFitImage(paint, wave, c, 1, 5, 0, 0);
                        continue;
                    }

                    //draw heart
                    if (val == Values.HeartBegin)
                    {
                        DrawImage(paint, new Bitmap(Heart), c, 2.4f, 2.4f, 0.2f, 0.2f);
                        continue;
                    }

                    //draw pointer
                    if (val == Values.PointerBegin)
                    {
                        DrawImage(paint, new Bitmap(Pointer), c, 2, 2, -0.5f, 0);
                        continue;
                    }

                    //draw bike
                    if (val == Values.BikeBegin)
                    {
                        DrawImage(paint, new Bitmap(Bike), c, 1, 2);
                        continue;
                    }

                    //draw default
                    if (val == Values.Marked)
                    {
                        int r = rand.Next(100);
                        if (r < 2)
                        {
                            DrawImage(paint, new Bitmap(Heart), c, 1, 1);
                        }
                        else
                        {
                            DrawImage(paint, new Bitmap(YelloCircle), c, 1, 1);
                        }
                        continue;
                    }
                }
            }
            paint = Graphics.FromImage(layer_white);
            paint.DrawImage(white_tmp, new RectangleF(CodePosition.X, CodePosition.Y, CodeSize.Width, CodeSize.Height), new Rectangle(0, 0, white_tmp.Width, white_tmp.Height), GraphicsUnit.Pixel);

            //draw brand
            if (Brand != null)
            {
                paint = Graphics.FromImage(brand_tmp);
                float brand_size = (float)(13.0 / 37.0 * Matrix.MatrixOrder);
                float brand_cell_posi = (Matrix.MatrixOrder - brand_size) / 2;
                Bitmap brand_img = new Bitmap(Brand);
                paint.DrawImage(brand_img, new RectangleF(brand_cell_posi * IntStep.Width, brand_cell_posi * IntStep.Height, brand_size * IntStep.Width, brand_size * IntStep.Height), new Rectangle(0, 0, brand_img.Width, brand_img.Height), GraphicsUnit.Pixel);
                paint = Graphics.FromImage(layer_brand);
                paint.DrawImage(brand_tmp, new RectangleF(CodePosition.X, CodePosition.Y, CodeSize.Width, CodeSize.Height), new Rectangle(0, 0, brand_tmp.Width, brand_tmp.Height), GraphicsUnit.Pixel);
            }


            //merge layers
            MergeLayers(layer_canvas, layer_black, layer_white, layer_brand);
        }
    }
}
