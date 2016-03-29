using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Drawing;
using System.Drawing.Imaging;
using QR.Drawing.Util;
using QR.Drawing.Data;

namespace QR.Drawing.Graphic
{
    class TetrisStyler : Styler
    {
        //protected member
        protected int TopGridRowNumber { get; set; }
        protected int BottomGridRowNumber { get; set; }
        protected PointF TopPosintion { get; set; }
        protected PointF BottomPosition { get; set; }
        protected SizeF TopSize { get; set; }
        protected SizeF BottomSize { get; set; }
        protected int TetrisStep { get; set; }  //Step in integer
        protected Bitmap TopCanvas { get; set; }
        protected Bitmap BottomCanvas { get; set; }
        protected CellColor[,] TopMatrix { get; set; }
        protected CellColor[,] BottomMatrix { get; set; }
        protected string point_path;
        protected string top_sample_path;
        protected string bottom_sample_path;
        protected Color canvas_color;

        //construction
        public TetrisStyler(int canvas_length, float margin, MarginMode margin_mode, string json_path) : base()
        {
            InitJson(json_path);
            TopGridRowNumber = (int)(JsonInstance.Size * 0.408);
            BottomGridRowNumber = (int)(JsonInstance.Size * 0.216);
            int canvas_width = canvas_length;
            int canvas_height;
            float code_length;
            float margin_length;
            float step_length;
            //calculate canvas_height
            if (margin_mode == MarginMode.PIXEL)
            {
                margin_length = margin;
                code_length = (float)(canvas_width - margin * 2);
                step_length = code_length / (float)JsonInstance.Size;
                canvas_height = (int)((TopGridRowNumber + BottomGridRowNumber + JsonInstance.Size) * step_length + margin * 2);
            }
            else
            {
                step_length = (float)canvas_width / (float)(JsonInstance.Size + margin * 2);
                margin_length = margin * step_length;
                code_length = step_length * JsonInstance.Size;
                canvas_height = (int)((TopGridRowNumber + BottomGridRowNumber + JsonInstance.Size + margin * 2) * step_length);
            }
            //top margin & bottom margin
            float margin_top = margin_length + step_length * TopGridRowNumber;
            float margin_bottom = margin_length + step_length * BottomGridRowNumber;

            //Initialation
            __InitStyleWithMarginMode(canvas_width, canvas_height, margin_length, margin_top, margin_length, margin_bottom, MarginMode.PIXEL, JsonInstance.Matrix, JsonInstance.Size);
            //set ToPosition and BottomPosition
            float step_height = (float)CodeSize.Height / (float)Matrix.MatrixOrder;
            float top_x = CodePosition.X;
            float top_y = CodePosition.Y - (step_height * TopGridRowNumber);
            TopPosintion = new PointF(top_x, top_y);

            float step_width = (float)CodeSize.Width / (float)Matrix.MatrixOrder;
            float bottom_x = CodePosition.X;
            float bottom_y = CodePosition.Y + CodeSize.Height;
            BottomPosition = new PointF(bottom_x, bottom_y);

            //set TopSize and BottomSize
            float top_width = CodeSize.Width;
            float top_height = step_height * TopGridRowNumber;
            TopSize = new SizeF(top_width, top_height);

            float bottom_width = CodeSize.Width;
            float bottom_height = step_height * BottomGridRowNumber;
            BottomSize = new SizeF(bottom_width, bottom_height);

            //set TetrisStep
            TetrisStep = IntStep.Width;

            //set TopCanvas and BottomCanvas
            TopCanvas = new Bitmap(TetrisStep * Matrix.MatrixOrder, TetrisStep * TopGridRowNumber);
            BottomCanvas = new Bitmap(TetrisStep * Matrix.MatrixOrder, TetrisStep * BottomGridRowNumber);

            //init top and bottom matrixs
            InitTopMatrix(TopGridRowNumber, Matrix.MatrixOrder);
            InitBottomMatrix(BottomGridRowNumber, Matrix.MatrixOrder);
        }

        private void InitTopMatrix(int row, int col)
        {
            TopMatrix = new CellColor[row, col];
            for (int i = 0; i < row; i++)
            {
                for (int j = 0; j < col; j++)
                {
                    TopMatrix[i, j] = CellColor.WHITE;
                }
            }
        }
        private void InitBottomMatrix(int row, int col)
        {
            BottomMatrix = new CellColor[row, col];
            for (int i = 0; i < row; i++)
            {
                for (int j = 0; j < col; j++)
                {
                    BottomMatrix[i, j] = CellColor.BLACK;
                }
            }
        }
        /// <summary>
        /// Get the position with specific row and column index in the TopCanvas.
        /// </summary>
        /// <param name="row"></param>
        /// <param name="col"></param>
        /// <returns></returns>
        private Point GetCellCanvasPosition(int row, int col)
        {
            int x = TetrisStep * col;
            int y = TetrisStep * row;
            return new Point(x, y);
        }
        /// <summary>
        /// Get the center position with specific row and column index in the TopCanvas.
        /// </summary>
        /// <param name="row"></param>
        /// <param name="col"></param>
        /// <returns></returns>
        private Point GetCellCanvasCenterPosition(int row, int col)
        {
            int x = TetrisStep * col + TetrisStep / 2;
            int y = TetrisStep * row + TetrisStep / 2;
            return new Point(x, y);
        }
        private void UpdateTopCellColors()
        {
            Bitmap sample = new Bitmap(top_sample_path);
            Bitmap sample_back = new Bitmap(TopCanvas.Width, TopCanvas.Height);
            Graphics sample_paint = Graphics.FromImage(sample_back);
            sample_paint.DrawImage(sample, new Rectangle(0, 0, sample_back.Width, sample_back.Height), new Rectangle(0, 0, sample.Width, sample.Height), GraphicsUnit.Pixel);
            Point center_point;
            for (int i = 0; i < TopMatrix.GetLength(0); i++)
            {
                for (int j = 0; j < TopMatrix.GetLength(1); j++)
                {
                    center_point = GetCellCanvasCenterPosition(i, j);
                    if (sample_back.GetPixel(center_point.X, center_point.Y).A > 128)
                    {
                        TopMatrix[i, j] = CellColor.BLACK;
                    }
                }
            }
        }
        private void UpdateBottomCellColors()
        {
            Bitmap sample = new Bitmap(bottom_sample_path);
            Bitmap sample_back = new Bitmap(BottomCanvas.Width, BottomCanvas.Height);
            Graphics sample_paint = Graphics.FromImage(sample_back);
            sample_paint.DrawImage(sample, new Rectangle(0, 0, sample_back.Width, sample_back.Height), new Rectangle(0, 0, sample.Width, sample.Height), GraphicsUnit.Pixel);
            Point center_point;
            for (int i = 0; i < BottomMatrix.GetLength(0); i++)
            {
                for (int j = 0; j < BottomMatrix.GetLength(1); j++)
                {
                    center_point = GetCellCanvasCenterPosition(i, j);
                    if (sample_back.GetPixel(center_point.X, center_point.Y).A < 15)
                    {
                        BottomMatrix[i, j] = CellColor.WHITE;
                    }
                }
            }
        }
        private void UpdateCellColors()
        {
            UpdateTopCellColors();
            UpdateBottomCellColors();
        }

        //public methods
        public void InitStyle(string i_folder, string i_point_path, string i_top_sample_path, string i_bottom_sample_path, Color i_canvas_color)
        {
            point_path = i_folder + @"/" + i_point_path;
            top_sample_path = i_folder + @"/" + i_top_sample_path;
            bottom_sample_path = i_folder + @"/" + i_bottom_sample_path;
            canvas_color = i_canvas_color;
        }

        public override void Draw()
        {
            UpdateCellColors();
            Bitmap point_img = new Bitmap(point_path);
            Bitmap top_layer = NewLayer();
            Bitmap bottom_layer = NewLayer();
            Bitmap black_layer = NewLayer();
            Bitmap black = NewIntegerPixelLayer();
            Bitmap canvas_layer = NewLayer();
            Graphics paint = Graphics.FromImage(TopCanvas);

            //draw top canvas
            for(int i=0; i<TopMatrix.GetLength(0); i++)
            {
                for(int j = 0; j<TopMatrix.GetLength(1); j++)
                {
                    if(TopMatrix[i,j] == CellColor.BLACK)
                    {
                        Point position = GetCellCanvasPosition(i, j);
                        paint.DrawImage(point_img, new Rectangle(position.X, position.Y, TetrisStep, TetrisStep), new Rectangle(0, 0, point_img.Width, point_img.Height), GraphicsUnit.Pixel);
                    }
                }
            }
            paint = Graphics.FromImage(top_layer);
            paint.DrawImage(TopCanvas, new RectangleF(TopPosintion.X, TopPosintion.Y, TopSize.Width, TopSize.Height), new Rectangle(0, 0, TopCanvas.Width, TopCanvas.Height), GraphicsUnit.Pixel);

            //draw bottom canvas
            paint = Graphics.FromImage(BottomCanvas);
            for(int i=0; i<BottomMatrix.GetLength(0); i++)
            {
                for(int j=0; j<BottomMatrix.GetLength(1); j++)
                {
                    if(BottomMatrix[i,j] == CellColor.BLACK)
                    {
                        Point position = GetCellCanvasPosition(i, j);
                        paint.DrawImage(point_img, new Rectangle(position.X, position.Y, TetrisStep, TetrisStep), new Rectangle(0, 0, point_img.Width, point_img.Height), GraphicsUnit.Pixel);
                    }
                }
            }
            paint = Graphics.FromImage(bottom_layer);
            paint.DrawImage(BottomCanvas, new RectangleF(BottomPosition.X, BottomPosition.Y, BottomSize.Width, BottomSize.Height), new Rectangle(0, 0, BottomCanvas.Width, BottomCanvas.Height), GraphicsUnit.Pixel);

            //draw black layer
            paint = Graphics.FromImage(black);
            var bc = from b in Matrix.CellMatrix.Cast<DataCell>() where b.Color == CellColor.BLACK select b;
            foreach(var b in bc)
            {
                paint.DrawImage(point_img, GetCellRectangle(b.Position.Row, b.Position.Column), new Rectangle(0, 0, point_img.Width, point_img.Height), GraphicsUnit.Pixel);
            }
            paint = Graphics.FromImage(black_layer);
            paint.DrawImage(black, new RectangleF(CodePosition.X, CodePosition.Y, CodeSize.Width, CodeSize.Height), new Rectangle(0, 0, black.Width, black.Height), GraphicsUnit.Pixel);

            //draw canvas
            paint = Graphics.FromImage(canvas_layer);
            paint.FillRectangle(new SolidBrush(canvas_color), new Rectangle(0, 0, Canvas.Width, Canvas.Height));

            MergeLayers(canvas_layer, top_layer, bottom_layer, black_layer);
        }

    }//class Tetris
}
