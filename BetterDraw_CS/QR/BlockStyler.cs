using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using QR.Drawing.Graphic;
using QR.Drawing.Data;
using QR.Drawing.Util;
using System.Drawing;

namespace QR.Drawing.Graphic
{
    class BlockStyler : Styler
    {
        protected Dictionary<PatternElems, string> PatternDirs;
        //Construction
        public BlockStyler(int canvas_length, float margin, MarginMode margin_mode) : base(canvas_length, margin, margin_mode)
        {
            InitDefaultPatterns();
        }
        public BlockStyler(int canvas_length, float margin, MarginMode margin_mode, string json_path)
            :base(canvas_length, margin, margin_mode, json_path)
        {

        }

        //Private Methods
        //Protected Methods
        public void InitStyle(string folder,
            string center_img, 
            string single_border_img, 
            string end_border_img, 
            string elbow_border_img,
            string path_border_img,
            string t_border_img,
            string corner_border_img)
        {
            string[] imgs = new string[7]
            {
                center_img,
                single_border_img,
                end_border_img,
                elbow_border_img,
                path_border_img,
                t_border_img,
                corner_border_img
            };
            ImportPatterns(folder, imgs);
        }
        protected void InitDefaultPatterns()
        {
            string[] paths = new string[7]
            {
                @"center.png",
                @"single_border.png",
                @"end_border.png",
                @"elbow_border.png",
                @"path_border.png",
                @"t_border.png",
                @"corner_border.png"
            };
            ImportPatterns(@"StrokePatterns", paths);
        }
        /// <summary>
        /// Update every cell's Role, Direction, Corner keys based on current state.
        /// </summary>
        protected void UpdateCellsMarks()
        {
            var cells = from c in Matrix.CellMatrix.Cast<DataCell>() where c.Color == CellColor.BLACK select c;
            foreach (var c in cells)
            {
                if (c.IsSinglePoint())
                {
                    c.Marks.Add(Keys.Role, Values.Single);
                    c.Marks.Add(Keys.Direction, Values.Null);
                    c.Marks.Add(Keys.Corner, Values.Null);
                }
                else if (c.IsEndPoint())
                {
                    c.Marks.Add(Keys.Role, Values.EndPoint);
                    c.Marks.Add(Keys.Corner, Values.Null);
                    if ((c.AroundColorInfo() & 4) == 4)
                    {
                        c.Marks.Add(Keys.Direction, Values.Left);
                    }
                    else if ((c.AroundColorInfo() & 8) == 8)
                    {
                        c.Marks.Add(Keys.Direction, Values.Up);
                    }
                    else if ((c.AroundColorInfo() & 1) == 1)
                    {
                        c.Marks.Add(Keys.Direction, Values.Right);
                    }
                    else if ((c.AroundColorInfo() & 2) == 2)
                    {
                        c.Marks.Add(Keys.Direction, Values.Down);
                    }
                }
                else if (c.IsPathNode())
                {
                    c.Marks.Add(Keys.Role, Values.Path);
                    c.Marks.Add(Keys.Corner, Values.Null);
                    if ((c.AroundColorInfo() & 2) == 2)
                    {
                        c.Marks.Add(Keys.Direction, Values.Vertical);
                    }
                    else if ((c.AroundColorInfo() & 1) == 1)
                    {
                        c.Marks.Add(Keys.Direction, Values.Horizontal);
                    }
                }
                else if (c.IsElbowNode())
                {
                    c.Marks.Add(Keys.Role, Values.Elbow);
                    int info = c.AroundColorInfo();
                    if ((info & 1) == 0 && (info & 2) == 0)
                    {
                        c.Marks.Add(Keys.Direction, Values.LeftUp);
                        if ((info & 64) == 0)
                        {
                            c.Marks.Add(Keys.Corner, Values.RightDown);
                        }
                        else
                        {
                            c.Marks.Add(Keys.Corner, Values.Null);
                        }
                    }
                    if ((info & 4) == 0 && (info & 2) == 0)
                    {
                        c.Marks.Add(Keys.Direction, Values.RightUp);
                        if ((info & 128) == 0)
                        {
                            c.Marks.Add(Keys.Corner, Values.LeftDown);
                        }
                        else
                        {
                            c.Marks.Add(Keys.Corner, Values.Null);
                        }
                    }
                    if ((info & 1) == 0 && (info & 8) == 0)
                    {
                        c.Marks.Add(Keys.Direction, Values.LeftDown);
                        if ((info & 32) == 0)
                        {
                            c.Marks.Add(Keys.Corner, Values.RightUp);
                        }
                        else
                        {
                            c.Marks.Add(Keys.Corner, Values.Null);
                        }
                    }
                    if ((info & 4) == 0 && (info & 8) == 0)
                    {
                        c.Marks.Add(Keys.Direction, Values.RightDown);
                        if ((info & 16) == 0)
                        {
                            c.Marks.Add(Keys.Corner, Values.LeftUp);
                        }
                        else
                        {
                            c.Marks.Add(Keys.Corner, Values.Null);
                        }
                    }
                }
                else if (c.IsTCenter())
                {
                    c.Marks.Add(Keys.Role, Values.TCenter);
                    c.Marks.Add(Keys.Corner, "!");  //init
                    int info = c.AroundColorInfo();
                    if ((info & 1) == 0)
                    {
                        c.Marks.Add(Keys.Direction, Values.Left);
                        if ((info & 32) == 0)
                        {
                            c.Marks[Keys.Corner] += Values.RightUp;
                        }
                        if ((info & 64) == 0)
                        {
                            c.Marks[Keys.Corner] += Values.RightDown;
                        }
                    }
                    if ((info & 2) == 0)
                    {
                        c.Marks.Add(Keys.Direction, Values.Up);
                        if ((info & 128) == 0)
                        {
                            c.Marks[Keys.Corner] += Values.LeftDown;
                        }
                        if ((info & 64) == 0)
                        {
                            c.Marks[Keys.Corner] += Values.RightDown;
                        }
                    }
                    if ((info & 4) == 0)
                    {
                        c.Marks.Add(Keys.Direction, Values.Right);
                        if ((info & 16) == 0)
                        {
                            c.Marks[Keys.Corner] += Values.LeftUp;
                        }
                        if ((info & 128) == 0)
                        {
                            c.Marks[Keys.Corner] += Values.LeftDown;
                        }
                    }
                    if ((info & 8) == 0)
                    {
                        c.Marks.Add(Keys.Direction, Values.Down);
                        if ((info & 32) == 0)
                        {
                            c.Marks[Keys.Corner] += Values.RightUp;
                        }
                        if ((info & 16) == 0)
                        {
                            c.Marks[Keys.Corner] += Values.LeftUp;
                        }
                    }
                }
                else if (c.IsCrossCenter())
                {
                    c.Marks.Add(Keys.Role, Values.CrossCenter);
                    c.Marks.Add(Keys.Direction, Values.Null);
                    c.Marks.Add(Keys.Corner, "!"); // init
                    int info = c.AroundColorInfo();
                    if ((info & 16) == 0)
                    {
                        c.Marks[Keys.Corner] += Values.LeftUp;
                    }
                    if ((info & 32) == 0)
                    {
                        c.Marks[Keys.Corner] += Values.RightUp;
                    }
                    if ((info & 64) == 0)
                    {
                        c.Marks[Keys.Corner] += Values.RightDown;
                    }
                    if ((info & 128) == 0)
                    {
                        c.Marks[Keys.Corner] += Values.LeftDown;
                    }
                }
            }
        }

        //Public Methods
        /// <summary>
        /// Import 7 elements path to the PatternDirs Dictionary.
        /// </summary>
        /// <param name="folder"></param>
        /// <param name="patterns">Should in order with CENTER, SINGLE_BORDER, END_BORDER, ELBOW_BORDER, PATH_BORDER, T_BORDER, CORNER_BORDER</param>
        public void ImportPatterns(string folder, string[] patterns)
        {
            PatternDirs = new Dictionary<PatternElems, string>();
            PatternDirs.Add(PatternElems.BASE_PATTERN, folder + @"/" + patterns[0]);
            PatternDirs.Add(PatternElems.SINGLE_BORDER, folder + @"/" + patterns[1]);
            PatternDirs.Add(PatternElems.END_BORDER, folder + @"/" + patterns[2]);
            PatternDirs.Add(PatternElems.ELBOW_BORDER, folder + @"/" + patterns[3]);
            PatternDirs.Add(PatternElems.PATH_BORDER, folder + @"/" + patterns[4]);
            PatternDirs.Add(PatternElems.T_BORDER, folder + @"/" + patterns[5]);
            PatternDirs.Add(PatternElems.CORNER_BORDER, folder + @"/" + patterns[6]);
        }
        public void SetPatternDir(PatternElems elem, string path)
        {
            if (PatternDirs.ContainsKey(elem))
            {
                PatternDirs[elem] = path;
            }
            else
            {
                PatternDirs.Add(elem, path);
            }
        }

        public override void Draw()
        {
            UpdateCellsMarks();

            Bitmap layer_black = NewLayer();
            Bitmap layer_black_tmp = NewIntegerPixelLayer();
            Bitmap layer_background = NewLayer();
            Bitmap layer_canvas = NewLayer();
            Graphics paint = null;

            Bitmap base_pattern = new Bitmap(PatternDirs[PatternElems.BASE_PATTERN]);

            //draw black cells
            paint = Graphics.FromImage(layer_black_tmp);
            var cells = from c in Matrix.CellMatrix.Cast<DataCell>() where c.Color == CellColor.BLACK select c;
            foreach (var c in cells)
            {
                // img, target_rectF, source_rectF, pixel
                paint.DrawImage(base_pattern,
                    GetCellRectangle(c.Position.Row, c.Position.Column),
                    new RectangleF(0, 0, base_pattern.Width, base_pattern.Height),
                    GraphicsUnit.Pixel);

                string role = c.Marks[Keys.Role];
                if (role == Values.Single)
                {
                    Bitmap curr_img = new Bitmap(PatternDirs[PatternElems.SINGLE_BORDER]);
                    paint.DrawImage(curr_img,
                        GetCellRectangle(c.Position.Row, c.Position.Column),
                        new RectangleF(0, 0, curr_img.Width, curr_img.Height),
                        GraphicsUnit.Pixel);
                }
                else if (role == Values.EndPoint)
                {
                    Bitmap curr_img = new Bitmap(PatternDirs[PatternElems.END_BORDER]);
                    string direction = c.Marks[Keys.Direction];
                    if (direction == Values.Up)
                    {
                        curr_img.RotateFlip(RotateFlipType.Rotate90FlipNone);
                    }
                    else if (direction == Values.Right)
                    {
                        curr_img.RotateFlip(RotateFlipType.Rotate180FlipNone);
                    }
                    else if (direction == Values.Down)
                    {
                        curr_img.RotateFlip(RotateFlipType.Rotate270FlipNone);
                    }
                    paint.DrawImage(curr_img,
                            GetCellRectangle(c.Position.Row, c.Position.Column),
                            new RectangleF(0, 0, curr_img.Width, curr_img.Height),
                            GraphicsUnit.Pixel);
                }
                else if (role == Values.Path)
                {
                    Bitmap curr_img = new Bitmap(PatternDirs[PatternElems.PATH_BORDER]);
                    if (c.Marks[Keys.Direction] == Values.Vertical)
                    {
                        curr_img.RotateFlip(RotateFlipType.Rotate90FlipNone);
                    }
                    paint.DrawImage(curr_img,
                            GetCellRectangle(c.Position.Row, c.Position.Column),
                            new RectangleF(0, 0, curr_img.Width, curr_img.Height),
                            GraphicsUnit.Pixel);
                }
                else if (role == Values.Elbow)
                {
                    //draw border
                    Bitmap curr_img = new Bitmap(PatternDirs[PatternElems.ELBOW_BORDER]);
                    string direction = c.Marks[Keys.Direction];
                    if (direction == Values.RightUp)
                    {
                        curr_img.RotateFlip(RotateFlipType.Rotate90FlipNone);
                    }
                    else if (direction == Values.RightDown)
                    {
                        curr_img.RotateFlip(RotateFlipType.Rotate180FlipNone);
                    }
                    else if (direction == Values.LeftDown)
                    {
                        curr_img.RotateFlip(RotateFlipType.Rotate270FlipNone);
                    }
                    paint.DrawImage(curr_img,
                            GetCellRectangle(c.Position.Row, c.Position.Column),
                            new RectangleF(0, 0, curr_img.Width, curr_img.Height),
                            GraphicsUnit.Pixel);
                    //draw corner
                    curr_img = new Bitmap(PatternDirs[PatternElems.CORNER_BORDER]);
                    string corner = c.Marks[Keys.Corner];
                    if (corner != Values.Null)
                    {
                        if (corner == Values.RightUp)
                        {
                            curr_img.RotateFlip(RotateFlipType.Rotate90FlipNone);
                        }
                        else if (corner == Values.RightDown)
                        {
                            curr_img.RotateFlip(RotateFlipType.Rotate180FlipNone);
                        }
                        else if (corner == Values.LeftDown)
                        {
                            curr_img.RotateFlip(RotateFlipType.Rotate270FlipNone);
                        }
                        paint.DrawImage(curr_img,
                                GetCellRectangle(c.Position.Row, c.Position.Column),
                                new RectangleF(0, 0, curr_img.Width, curr_img.Height),
                                GraphicsUnit.Pixel);
                    }
                }
                else if (role == Values.TCenter)
                {
                    //draw border
                    Bitmap curr_img = new Bitmap(PatternDirs[PatternElems.T_BORDER]);
                    string direction = c.Marks[Keys.Direction];
                    if (direction == Values.Up)
                    {
                        curr_img.RotateFlip(RotateFlipType.Rotate90FlipNone);
                    }
                    else if (direction == Values.Right)
                    {
                        curr_img.RotateFlip(RotateFlipType.Rotate180FlipNone);
                    }
                    else if (direction == Values.Down)
                    {
                        curr_img.RotateFlip(RotateFlipType.Rotate270FlipNone);
                    }
                    paint.DrawImage(curr_img,
                            GetCellRectangle(c.Position.Row, c.Position.Column),
                            new RectangleF(0, 0, curr_img.Width, curr_img.Height),
                            GraphicsUnit.Pixel);
                    //draw corners
                    curr_img = new Bitmap(PatternDirs[PatternElems.CORNER_BORDER]);
                    string corner = c.Marks[Keys.Corner];
                    if (corner != Values.Null)
                    {
                        if (corner.Contains(Values.RightUp))
                        {
                            curr_img.RotateFlip(RotateFlipType.Rotate90FlipNone);
                            paint.DrawImage(curr_img,
                                GetCellRectangle(c.Position.Row, c.Position.Column),
                                new RectangleF(0, 0, curr_img.Width, curr_img.Height),
                                GraphicsUnit.Pixel);
                            curr_img.RotateFlip(RotateFlipType.Rotate270FlipNone);
                        }
                        if (corner.Contains(Values.RightDown))
                        {
                            curr_img.RotateFlip(RotateFlipType.Rotate180FlipNone);
                            paint.DrawImage(curr_img,
                                GetCellRectangle(c.Position.Row, c.Position.Column),
                                new RectangleF(0, 0, curr_img.Width, curr_img.Height),
                                GraphicsUnit.Pixel);
                            curr_img.RotateFlip(RotateFlipType.Rotate180FlipNone);
                        }
                        if (corner.Contains(Values.LeftDown))
                        {
                            curr_img.RotateFlip(RotateFlipType.Rotate270FlipNone);
                            paint.DrawImage(curr_img,
                                GetCellRectangle(c.Position.Row, c.Position.Column),
                                new RectangleF(0, 0, curr_img.Width, curr_img.Height),
                                GraphicsUnit.Pixel);
                            curr_img.RotateFlip(RotateFlipType.Rotate90FlipNone);
                        }
                        if (corner.Contains(Values.LeftUp))
                        {
                            paint.DrawImage(curr_img,
                                GetCellRectangle(c.Position.Row, c.Position.Column),
                                new RectangleF(0, 0, curr_img.Width, curr_img.Height),
                                GraphicsUnit.Pixel);
                        }
                    }
                }
                else if (role == Values.CrossCenter)
                {
                    //draw corners
                    Bitmap curr_img = new Bitmap(PatternDirs[PatternElems.CORNER_BORDER]);
                    string corner = c.Marks[Keys.Corner];
                    if (corner != Values.Null)
                    {
                        if (corner.Contains(Values.LeftUp))
                        {
                            paint.DrawImage(curr_img,
                                GetCellRectangle(c.Position.Row, c.Position.Column),
                                new RectangleF(0, 0, curr_img.Width, curr_img.Height),
                                GraphicsUnit.Pixel);
                        }
                        if (corner.Contains(Values.RightUp))
                        {
                            curr_img.RotateFlip(RotateFlipType.Rotate90FlipNone);
                            paint.DrawImage(curr_img,
                                GetCellRectangle(c.Position.Row, c.Position.Column),
                                new RectangleF(0, 0, curr_img.Width, curr_img.Height),
                                GraphicsUnit.Pixel);
                            curr_img.RotateFlip(RotateFlipType.Rotate270FlipNone);
                        }
                        if (corner.Contains(Values.RightDown))
                        {
                            curr_img.RotateFlip(RotateFlipType.Rotate180FlipNone);
                            paint.DrawImage(curr_img,
                                GetCellRectangle(c.Position.Row, c.Position.Column),
                                new RectangleF(0, 0, curr_img.Width, curr_img.Height),
                                GraphicsUnit.Pixel);
                            curr_img.RotateFlip(RotateFlipType.Rotate180FlipNone);
                        }
                        if (corner.Contains(Values.LeftDown))
                        {
                            curr_img.RotateFlip(RotateFlipType.Rotate270FlipNone);
                            paint.DrawImage(curr_img,
                                GetCellRectangle(c.Position.Row, c.Position.Column),
                                new RectangleF(0, 0, curr_img.Width, curr_img.Height),
                                GraphicsUnit.Pixel);
                            curr_img.RotateFlip(RotateFlipType.Rotate90FlipNone);
                        }
                    }
                }
            }  //draw black cells foreach
            paint = Graphics.FromImage(layer_black);
            paint.DrawImage(layer_black_tmp,
                new RectangleF(CodePosition.X, CodePosition.Y, CodeSize.Width, CodeSize.Height),
                new Rectangle(0, 0, layer_black_tmp.Width, layer_black_tmp.Height),
                GraphicsUnit.Pixel);

            //draw background
            paint = Graphics.FromImage(layer_background);
            paint.FillRectangle(
                new SolidBrush(Default.WHITE),
                new RectangleF(CodePosition.X, CodePosition.Y, CodeSize.Width, CodeSize.Height));

            //draw canvas
            paint = Graphics.FromImage(layer_canvas);
            paint.FillRectangle(new SolidBrush(Default.BG_COLOR),
                new RectangleF(0, 0, CanvasSize.Width, CanvasSize.Height));

            MergeLayers(layer_canvas, layer_background, layer_black);
        }
    }

    enum PatternElems
    {
        BASE_PATTERN,
        SINGLE_BORDER,
        END_BORDER,
        ELBOW_BORDER,
        PATH_BORDER,
        T_BORDER,
        CORNER_BORDER
    }
}
