using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Drawing;
using QR.Drawing.Util;
using QR.Drawing.Data;
using QR.Drawing.Open;
using Newtonsoft.Json;

namespace QR.Drawing.Graphic
{
    class Styler
    {
        private string black_pattern;
        private string white_pattern;
        private string background_image;
        private string canvas_image;
        private Color black_color;
        private Color white_color;
        private Color background_color;
        private Color canvas_color;
        private object b;

        protected DataMatrix Matrix { get; set; }
        protected Size CanvasSize { get; set; }
        protected SizeF CodeSize { get; set; }
        protected Size Step { get; set; } //the min integer step length.
        protected PointF CodePosition { get; set; }
        protected Bitmap Canvas { get; set; }
        protected JsonResource JsonInstance { get; set; }

        //Constructions *************************************************************************************************
        //build by margin
        public Styler(int canvas_length, float margin, MarginMode margin_mode)
            : this(canvas_length, margin, margin_mode, Default.MATRIX_COLOR_INFO, Default.MATRIX_COLOR_INFO.GetLength(0))
        { }
        public Styler(int canvas_length, float margin, MarginMode margin_mode, string json_path)
            : this(canvas_length, canvas_length, margin, margin, margin, margin, margin_mode, json_path)
        { }
        public Styler(int canvas_length, float margin, MarginMode margin_mode, bool[,] info, int order)
            : this(canvas_length, canvas_length, margin, margin, margin, margin, margin_mode, info, order)
        { }
        public Styler(int canvas_width, int canvas_height, float margin, MarginMode margin_mode, bool[,] info, int order)
            : this(canvas_width, canvas_height, margin, margin, margin, margin, margin_mode, info, order)
        { }
        public Styler(int canvas_width, int canvas_height, float margin_horizontal, float margin_vertical, MarginMode margin_mode, bool[,] info, int order)
            : this(canvas_width, canvas_height, margin_horizontal, margin_vertical, margin_horizontal, margin_vertical, margin_mode, info, order)
        { }
        public Styler(int canvas_width, int canvas_height, float left_margin, float top_margin, float right_margin, float down_margin, MarginMode margin_mode, bool[,] info, int order)
        {
            InitStyleWithMarginMode(canvas_width, canvas_height, left_margin, top_margin, right_margin, down_margin, margin_mode, info, order);
        }
        public Styler(int canvas_width, int canvas_height, float left_margin, float top_margin, float right_margin, float down_margin, MarginMode margin_mode, string json_path)
        {
            InitJson(json_path);
            InitStyleWithMarginMode(canvas_width, canvas_height, left_margin, top_margin, right_margin, down_margin, margin_mode, JsonInstance.Matrix, JsonInstance.Size);
        }
        //build by size and position
        public Styler(int canvas_length, float code_length)
            : this(canvas_length, code_length, Default.MATRIX_COLOR_INFO, Default.MATRIX_COLOR_INFO.GetLength(0))
        { }
        public Styler(int canvas_length, float code_length, bool[,] info, int order)
            : this(canvas_length, canvas_length, code_length, code_length, CodePositinoMode.CENTER, info, order)
        { }
        public Styler(int canvas_width, int canvas_height, float code_width, float code_height, bool[,] info, int order)
            : this(canvas_width, canvas_height, code_width, code_height, CodePositinoMode.CENTER, info, order)
        { }
        public Styler(
            int canvas_width, int canvas_height,
            float code_width, float code_height,
            CodePositinoMode mode,
            bool[,] info, int order)
        {
            InitStyleWithPositionMode(canvas_width, canvas_height, code_width, code_height, mode, info, order);
        }
        public Styler(int canvas_width, int canvas_height,
            float code_width, float code_height,
            CodePositinoMode mode,
            string json_path)
        {
            InitJson(json_path);
            InitStyleWithPositionMode(canvas_width, canvas_height, code_width, code_height, mode, JsonInstance.Matrix, JsonInstance.Size);
        }
        public Styler(
            int canvas_width, int canvas_height,
            float code_width, float code_height,
            float posi_x, float posi_y,
            bool[,] info, int order)
        {
            __InitStyle(canvas_width, canvas_height, code_width, code_height, posi_x, posi_y, info, order);
        }
        public Styler(
           int canvas_width, int canvas_height,
           float code_width, float code_height,
           float posi_x, float posi_y,
           string json_path, int order)
        {
            InitJson(json_path);
            __InitStyle(canvas_width, canvas_height, code_width, code_height, posi_x, posi_y, JsonInstance.Matrix, order);
        }

        //Protected Methods
        protected void InitStyleWithMarginMode(int canvas_width, int canvas_height, float left_margin, float top_margin, float right_margin, float down_margin, MarginMode margin_mode, bool[,] info, int order)
        {
            if (margin_mode == MarginMode.PIXEL)
            {
                float code_posi_x = left_margin;
                float code_posi_y = top_margin;
                float code_width = canvas_width - left_margin - right_margin;
                float code_height = canvas_height - top_margin - down_margin;
                __InitStyle(canvas_width, canvas_height, code_width, code_height, code_posi_x, code_posi_y, info, order);
            }
            else if (margin_mode == MarginMode.CELL)
            {
                float cells_number_horizontal = order + left_margin + right_margin;
                float cells_number_vertical = order + top_margin + down_margin;
                float step_horizontal = (float)canvas_width / cells_number_horizontal;
                float step_vertical = (float)canvas_height / cells_number_vertical;
                float code_posi_x = left_margin * step_horizontal;
                float code_posi_y = top_margin * step_vertical;
                float code_width = step_horizontal * order;
                float code_height = step_vertical * order;
                __InitStyle(canvas_width, canvas_height, code_width, code_height, code_posi_x, code_posi_y, info, order);
            }
        }
        protected void InitStyleWithPositionMode(
            int canvas_width, int canvas_height,
            float code_width, float code_height,
            CodePositinoMode mode,
            bool[,] info, int order)
        {
            float x = 0f;
            float y = 0f;
            switch (mode)
            {
                case CodePositinoMode.CENTER:
                    x = (float)(canvas_width - code_width) / 2f;
                    y = (float)(canvas_height - code_height) / 2f;
                    break;
                case CodePositinoMode.CENTER_LEFT:
                    y = (float)(canvas_height - code_height) / 2f;
                    x = y;
                    break;
                case CodePositinoMode.CENTER_RIGHT:
                    y = (float)(canvas_height - code_height) / 2f;
                    x = canvas_width - code_width - y;
                    break;
                case CodePositinoMode.CENTER_UP:
                    x = (float)(canvas_width - code_width) / 2f;
                    y = x;
                    break;
                case CodePositinoMode.CENTER_DOWN:
                    x = (float)(canvas_width - code_width) / 2f;
                    y = canvas_height - code_height - x;
                    break;
                default:
                    break;
            }
            __InitStyle(canvas_width, canvas_height, code_width, code_height, x, y, info, order);
        }
        protected void __InitStyle(int i_canvas_width, int i_canvas_height,
            float i_code_width, float i_code_height,
            float i_posi_x, float i_posi_y,
            bool[,] i_info, int i_order)
        {
            try
            {
                if (i_code_width > i_canvas_width || i_code_height > i_canvas_height)
                {
                    throw new CodeSizeOutOfCanvasException(
                        "Code size out of canvas. Code cannot be drawn completely.");
                }
                else
                {
                    if (i_posi_x + i_code_width > i_canvas_width || i_posi_y + i_code_height > i_canvas_height)
                    {
                        throw new CodePositionIncorrectException(
                            "Your position \"" + i_posi_x.ToString() + ", " + i_posi_y.ToString() +
                            "\" will let code been drawn out of canvas. Please modify the position or the size about.");
                    }
                    else
                    {
                        Matrix = new DataMatrix(i_info, i_order);
                        CanvasSize = new Size(i_canvas_width, i_canvas_height);
                        CodeSize = new SizeF(i_code_width, i_code_height);
                        CodePosition = new PointF(i_posi_x, i_posi_y);
                        UpdateStep();
                        Canvas = new Bitmap(i_canvas_width, i_canvas_height);
                    }
                }

                //init colors
                black_color = Default.BLACK;
                white_color = Default.WHITE;
                background_color = Default.BG_COLOR;
                canvas_color = Default.CANVAS_COLOR;
            }
            catch (CodeSizeOutOfCanvasException e)
            {
                Console.WriteLine(e.Message);
            }
            catch (CodePositionIncorrectException e)
            {
                Console.WriteLine(e.Message);
            }
        }
        protected void __InitStyle(int i_canvas_width, int i_canvas_height,
            float i_code_width, float i_code_height,
            float i_posi_x, float i_posi_y,
            string i_json_path, int i_order)
        {
            InitJson(i_json_path);
            __InitStyle(i_canvas_width, i_canvas_height, i_code_width, i_code_height, i_posi_x, i_posi_y, JsonInstance.Matrix, i_order);
        }
        protected void InitJson(string json_path)
        {
            string j_string = Utils.ReadFileToString(json_path);
            JsonInstance = JsonConvert.DeserializeObject<JsonResource>(j_string);
        }

        protected void UpdateStep()
        {
            try
            {
                if (CodeSize == null)
                {
                    throw new CodeSizeEmptyException(
                        "codeSize is empty. Please initialize it before invoke this method.");
                }
                else if (Matrix == null)
                {
                    throw new MatrixEmptyException(
                        "Matrix is empty. Please initialize it before invoke this method.");
                }
                else
                {
                    int width = (int)Math.Ceiling(CodeSize.Width / Matrix.MatrixOrder);
                    int height = (int)Math.Ceiling(CodeSize.Height / Matrix.MatrixOrder);
                    Step = new Size(width, height);
                }
            }
            catch (CodeSizeEmptyException e) { Console.WriteLine(e.Message); }
            catch (MatrixEmptyException e) { Console.WriteLine(e.Message); }
        }

        /// <summary>
        /// Get a Rectangle with index[row,col].
        /// The actual position and size of the Rectangle is based on the corresponding index of MatrixColorInfo.
        /// </summary>
        /// <param name="row"></param>
        /// <param name="col"></param>
        /// <returns></returns>
        protected Rectangle GetCellRectangle(int row, int col)
        {
            int x = col * Step.Width;
            int y = row * Step.Height;
            return new Rectangle(x, y, Step.Width, Step.Height);
        }
        /// <summary>
        /// Get a Rectangle with EyePosition.
        /// The position is based on the Code inner position and size, not the final position and size in the canvas.
        /// </summary>
        /// <param name="posi"></param>
        /// <returns></returns>
        protected Rectangle GetEyeRectangle(EyePosition posi)
        {
            int x = -1;
            int y = -1;
            int width = Step.Width * 7;
            int height = Step.Height * 7;
            switch (posi)
            {
                case EyePosition.LEFT_UP:
                    x = 0;
                    y = 0;
                    width = Step.Width * 7;
                    height = Step.Height * 7;
                    break;
                case EyePosition.RIGHT_UP:
                    x = Step.Width * (Matrix.MatrixOrder - 7);
                    y = 0;
                    break;
                case EyePosition.LEFT_DOWN:
                    x = 0;
                    y = Step.Height * (Matrix.MatrixOrder - 7);
                    break;
                default:
                    break;
            }
            try
            {
                if (x > -1)
                {
                    return new Rectangle(x, y, width, height);
                }
                else
                {
                    throw (new EyeRectangleCreationException(
                        "Creating Eye Rectangle has some problems..."));
                }
            }
            catch (EyeRectangleCreationException e)
            {
                Console.WriteLine(e.Message);
                return new Rectangle(0, 0, 1, 1);
            }
        }

        protected Bitmap NewLayer()
        {
            return new Bitmap(Canvas.Width, Canvas.Height);
        }
        protected Bitmap NewIntegerPixelLayer()
        {
            int width = Step.Width * Matrix.MatrixOrder;
            int height = Step.Height * Matrix.MatrixOrder;
            return new Bitmap(width, height);
        }

        /// <summary>
        /// Merge layers to the Canvas.
        /// The first layer in the layers will be sorted at the lowest z-index.
        /// </summary>
        /// <param name="layers"></param>
        protected void MergeLayers(params Bitmap[] layers)
        {
            Graphics paint = Graphics.FromImage(Canvas);
            foreach (Bitmap layer in layers)
            {
                paint.DrawImage(layer,
                    new Rectangle(0, 0, Canvas.Width, Canvas.Height),
                    new Rectangle(0, 0, layer.Width, layer.Height),
                    GraphicsUnit.Pixel);
            }
        }

        //Public Methos
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

        public void DrawOrigin()
        {
            Bitmap layer_black = NewLayer();
            Bitmap layer_black_tmp = NewIntegerPixelLayer();
            Bitmap layer_canvas = NewLayer();
            Graphics paint = null;

            //draw black
            paint = Graphics.FromImage(layer_black_tmp);
            Brush black_brush = new SolidBrush(Color.Black);
            var cells = from c in Matrix.CellMatrix.Cast<DataCell>() where c.Color == CellColor.BLACK select c;
            foreach (var c in cells)
            {
                paint.FillRectangle(black_brush, GetCellRectangle(c.Position.Row, c.Position.Column));
            }
            paint = Graphics.FromImage(layer_black);
            paint.DrawImage(layer_black_tmp,
                new RectangleF(CodePosition.X, CodePosition.Y, CodeSize.Width, CodeSize.Height),
                new Rectangle(0, 0, layer_black_tmp.Width, layer_black_tmp.Height),
                GraphicsUnit.Pixel);

            //draw canvas
            paint = Graphics.FromImage(layer_canvas);
            Brush white_brush = new SolidBrush(Color.White);
            paint.FillRectangle(white_brush, new Rectangle(0, 0, Canvas.Width, Canvas.Height));

            //Merge layers
            MergeLayers(layer_canvas, layer_black);
        }
        public virtual void Draw()
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

        public void Draw(Object brush_black, Object brush_background, Object brush_canvas)
        {
            try
            {
                Brush black = null;
                Brush background = null;
                Brush canvas = null;

                if (brush_black is Color)
                {
                    Color c = (Color)brush_black;
                    black = new SolidBrush(c);
                }
                else if (brush_black is Brush)
                {
                    black = (Brush)brush_black;
                }
                else
                {
                    throw new BrushInvalidException(
                        "Is it a brush? Or it is an invalid brush. Your brush type is: " + brush_black.GetType().Name);
                }

                if (brush_background is Color)
                {
                    Color c = (Color)brush_background;
                    background = new SolidBrush(c);
                }
                else if (brush_background is Brush)
                {
                    background = (Brush)brush_background;
                }
                else
                {
                    throw new BrushInvalidException(
                        "Is it a brush? Or it is an invalid brush. Your brush type is: " + brush_background.GetType().Name);
                }

                if (brush_canvas is Color)
                {
                    Color c = (Color)brush_canvas;
                    canvas = new SolidBrush(c);
                }
                else if (brush_canvas is Brush)
                {
                    canvas = (Brush)brush_canvas;
                }
                else
                {
                    throw new BrushInvalidException(
                        "Is it a brush? Or it is an invalid brush. Your brush type is: " + brush_canvas.GetType().Name);
                }

                Draw(black, background, canvas);
            }
            catch (BrushInvalidException e)
            {
                Console.WriteLine(e.Message);
            }
        }
        public void Draw(Brush brush_black, Brush brush_background, Brush brush_canvas)
        {

            try
            {
                //init layers and paint
                Bitmap layer_black = NewLayer();
                Bitmap layer_black_tmp = NewIntegerPixelLayer();
                Bitmap layer_background = NewLayer();
                Bitmap layer_canvas = NewLayer();
                Graphics paint = null;

                //Draw black cells
                paint = Graphics.FromImage(layer_black_tmp);
                var black_cells = from c in Matrix.CellMatrix.Cast<DataCell>() where c.Color == CellColor.BLACK select c;
                foreach (var c in black_cells)
                {
                    if (brush_black is SolidBrush)
                    {
                        paint.FillRectangle(brush_black, GetCellRectangle(c.Position.Row, c.Position.Column));
                    }
                    else if (brush_black is TextureBrush)
                    {
                        TextureBrush b = (TextureBrush)brush_black;
                        paint.DrawImage(b.Image,
                            GetCellRectangle(c.Position.Row, c.Position.Column),
                            new Rectangle(0, 0, b.Image.Width, b.Image.Height),
                            GraphicsUnit.Pixel);
                    }
                    else
                    {
                        throw new BrushInvalidException(
                            "Brush black is invalid. Your type is: " + brush_black.GetType().Name);
                    }
                }
                paint = Graphics.FromImage(layer_black);
                paint.DrawImage(layer_black_tmp,
                    new RectangleF(CodePosition.X, CodePosition.Y, CodeSize.Width, CodeSize.Height),
                    new Rectangle(0, 0, layer_black_tmp.Width, layer_black_tmp.Height),
                    GraphicsUnit.Pixel);

                //Draw background
                paint = Graphics.FromImage(layer_background);
                if (brush_background is SolidBrush)
                {
                    paint.FillRectangle(
                        brush_background,
                        new RectangleF(CodePosition.X, CodePosition.Y, CodeSize.Width, CodeSize.Height));
                }
                else if (brush_background is TextureBrush)
                {
                    TextureBrush b = (TextureBrush)brush_background;
                    paint.DrawImage(
                        b.Image,
                        new RectangleF(CodePosition.X, CodePosition.Y, CodeSize.Width, CodeSize.Height),
                        new RectangleF(0, 0, b.Image.Width, b.Image.Height), GraphicsUnit.Pixel);
                }
                else
                {
                    throw new BrushInvalidException(
                        "Brush background is invalid. Your type is: " + brush_background.GetType().Name);
                }

                //Draw canvas
                paint = Graphics.FromImage(layer_canvas);
                if (brush_canvas is SolidBrush)
                {
                    paint.FillRectangle(
                        brush_canvas,
                        new Rectangle(0, 0, CanvasSize.Width, CanvasSize.Height));
                }
                else if (brush_canvas is TextureBrush)
                {
                    TextureBrush b = (TextureBrush)brush_canvas;
                    paint.DrawImage(
                        b.Image,
                        new Rectangle(0, 0, CanvasSize.Width, CanvasSize.Height),
                        new Rectangle(0, 0, b.Image.Width, b.Image.Height),
                        GraphicsUnit.Pixel);
                }
                else
                {
                    throw new BrushInvalidException(
                        "Brush canvas is invalid. Your type is: " + brush_canvas.GetType().Name);
                }

                //Merge layers
                MergeLayers(new Bitmap[] { layer_canvas, layer_background, layer_black });
            }
            catch (BrushInvalidException e)
            {
                Console.WriteLine(e.Message);
            }
        }

        public virtual void Display(int width = 640, int height = 480)
        {
            OpenDisplay window = new OpenDisplay(Canvas, width, height);
            window.Run();
        }

        public void Save(string path)
        {
            if (Canvas != null)
            {
                Canvas.Save(path);
            }
        }
    } //class Styler

    /// <summary>
    /// Set in the size and position build model. Auto set the code's position
    /// </summary>
    enum CodePositinoMode : byte
    {
        CENTER,
        CENTER_LEFT,
        CENTER_RIGHT,
        CENTER_UP,
        CENTER_DOWN
    }
    /// <summary>
    /// <para> Set in the margin build model.</para>
    /// <para> The margin parameters are measured by pixel when MarginMode is PIXEL, by cell number when MarginMode is CELL. </para>
    /// <para> This parameter is [Requisite] in margin build model. Or Styler will be build in size and position model or build error. </para>
    /// </summary>
    enum MarginMode : byte
    {
        CELL,
        PIXEL
    }

    /// <summary>
    /// Represent the positon of the code eyes
    /// </summary>
    enum EyePosition : byte
    {
        LEFT_UP,
        RIGHT_UP,
        LEFT_DOWN
    }

    [Serializable]
    public class CodeSizeOutOfCanvasException : Exception
    {
        public CodeSizeOutOfCanvasException() { }
        public CodeSizeOutOfCanvasException(string message) : base(message) { }
        public CodeSizeOutOfCanvasException(string message, Exception inner) : base(message, inner) { }
        protected CodeSizeOutOfCanvasException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context)
        { }
    }


    [Serializable]
    public class CodePositionIncorrectException : Exception
    {
        public CodePositionIncorrectException() { }
        public CodePositionIncorrectException(string message) : base(message) { }
        public CodePositionIncorrectException(string message, Exception inner) : base(message, inner) { }
        protected CodePositionIncorrectException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context)
        { }
    }


    [Serializable]
    public class CodeSizeEmptyException : Exception
    {
        public CodeSizeEmptyException() { }
        public CodeSizeEmptyException(string message) : base(message) { }
        public CodeSizeEmptyException(string message, Exception inner) : base(message, inner) { }
        protected CodeSizeEmptyException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context)
        { }
    }


    [Serializable]
    public class MatrixEmptyException : Exception
    {
        public MatrixEmptyException() { }
        public MatrixEmptyException(string message) : base(message) { }
        public MatrixEmptyException(string message, Exception inner) : base(message, inner) { }
        protected MatrixEmptyException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context)
        { }
    }


    [Serializable]
    public class BrushInvalidException : Exception
    {
        public BrushInvalidException() { }
        public BrushInvalidException(string message) : base(message) { }
        public BrushInvalidException(string message, Exception inner) : base(message, inner) { }
        protected BrushInvalidException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context)
        { }
    }


    [Serializable]
    public class EyeRectangleCreationException : Exception
    {
        public EyeRectangleCreationException() { }
        public EyeRectangleCreationException(string message) : base(message) { }
        public EyeRectangleCreationException(string message, Exception inner) : base(message, inner) { }
        protected EyeRectangleCreationException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context)
        { }
    }
}
