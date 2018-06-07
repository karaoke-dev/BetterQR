using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using QR.Drawing.Util;

namespace QR.Drawing.Data
{
    public class DataMatrix
    {
        //Private Values
        private int matrix_order = 0;

        //Public Properties
        /// <summary>
        /// Order of the Matrix, which represents the number of the cells in a row or a column.
        /// </summary>
        public int MatrixOrder
        {
            get
            {
                return matrix_order;
            }
            set
            {
                if (value <= 0)
                {
                    throw new MatrixOrderInvalidException(
                        "MatrixOrder should be larger than 0. Your value is: " + value.ToString());
                }
                else
                {
                    matrix_order = value;
                }
            }
        }
        /// <summary>
        /// The origin color information of the given data. These information will be used to set CellMatrix's cell color.
        /// </summary>
        public bool[,] MatrixColorInfo { get; set; }
        /// <summary>
        /// A matrix which is composed of cells. The row number equals to the column number.
        /// </summary>
        public DataCell[,] CellMatrix { get; set; }

        //Constructions **************************************************************************************
        public DataMatrix(int order)
        {
            if (order <= 0)
            {
                throw new MatrixOrderInvalidException(
                    "Matrix order should be larger than 0. The order offered is: " + order.ToString());
            }
            else
            {
                MatrixOrder = order;
                InitMatrixColorInfo();
                UpdateCellMatrix();
            }
        }
        public DataMatrix(bool[,] info) : this(info, info.GetLength(0)) { }
        public DataMatrix(bool[,] info, int order)
        {
            if (order <= 0)
            {
                throw new MatrixOrderInvalidException(
                    "Matrix order should be larger than 0. The order offered is: " + order.ToString());
            }
            if (info.GetLength(0) != order || info.GetLength(1) != order)
            {
                throw new MatrixColorInfoErrorException(
                    "Matrix color info should be row number equals to column number and equals to the parameter order.");
            }
            else
            {
                MatrixOrder = order;
                InitMatrixColorInfo(info);
                UpdateCellMatrix();
            }
        }

        //Private Methods **************************************************************************************
        private void MatrixSetFalse(ref bool[,] target_matrix, bool[,] source_matrix, int i, int j) //delegate function
        {
            target_matrix[i, j] = false;
        }
        private void MatrixAssignment<T>(ref T[,] target_marix, T[,] source_matrix, int i, int j) //delegate function
        {
            target_marix[i, j] = source_matrix[i, j];
        }
        /// <summary>
        /// Init DataCells in this Matrix. Very Important.
        /// </summary>
        /// <param name="target_matrix"></param>
        /// <param name="source_matrix"></param>
        /// <param name="i"></param>
        /// <param name="j"></param>
        private void NewCellWithColorInfo(ref DataCell[,] target_matrix, bool[,] source_matrix, int i, int j) //delegate function
        {
            target_matrix[i, j] = source_matrix[i, j] == true ?
                new DataCell(i, j, MatrixOrder - 1, CellColor.BLACK, this, null) : new DataCell(i, j, MatrixOrder - 1, CellColor.WHITE, this, null);
        }

        private void InitMatrixColorInfo()
        {
            if (MatrixOrder <= 0)
            {
                throw new MatrixOrderInvalidException(
                    "Matrix order is invalid. Please set MatrixOrder before invoke this method.");
            }
            else
            {
                MatrixColorInfo = new bool[MatrixOrder, MatrixOrder];
                Traverse.TravMatrix<bool, bool>(MatrixColorInfo, null, MatrixSetFalse);
            }
        }
        private void InitMatrixColorInfo(bool[,] info)
        {
            if (info.GetLength(0) != MatrixOrder || info.GetLength(1) != MatrixOrder)
            {
                throw new MatrixColorInfoErrorException(
                    "Matrix color info should be row number equals to column number and equals to the MatrixOrder.");
            }
            else
            {
                MatrixColorInfo = new bool[MatrixOrder, MatrixOrder];
                Traverse.TravMatrix<bool, bool>(MatrixColorInfo, info, MatrixAssignment);
                for (int i = 0; i < MatrixOrder; ++i)
                {
                    for (int j = 0; j < MatrixOrder; ++j)
                    {
                        MatrixColorInfo[i, j] = info[i, j];
                    }
                }
            }
        }

        //Public Methods **************************************************************************************
        /// <summary>
        /// Only update the MatrixColorInfo but not change the MatrixOrder.
        /// So the info array should be equals to the former info array in row number and column number.
        /// </summary>
        /// <param name="info"></param>
        public void UpdateMatrixColorInfo(bool[,] info)
        {
            if (info.GetLength(0) != MatrixOrder || info.GetLength(1) != MatrixOrder)
            {
                throw new MatrixOrderInvalidException(
                    "Matrix color info should be row number equals to column number, and they should be equals to MatixOrder.");
            }
            else
            {
                Traverse.TravMatrix<bool, bool>(MatrixColorInfo, info, MatrixAssignment);
            }
        }

        /// <summary>
        /// Update Cells in CellMatrix according to the current MatrixColorInfo property.
        /// </summary>
        public void UpdateCellMatrix()
        {
            if (MatrixColorInfo == null)
            {
                throw new MatrixColorInfoEmptyException(
                    "Matrix color information has not been initialized. Please set MatrixColorInfo property before invoke this method.");
            }
            else
            {
                CellMatrix = new DataCell[MatrixOrder, MatrixOrder];
                Traverse.TravMatrix<DataCell, bool>(CellMatrix, MatrixColorInfo, NewCellWithColorInfo);
            }
        }
    }// class DataMatrix


    [Serializable]
    public class MatrixOrderInvalidException : Exception
    {
        public MatrixOrderInvalidException() { }
        public MatrixOrderInvalidException(string message) : base(message) { }
        public MatrixOrderInvalidException(string message, Exception inner) : base(message, inner) { }
        protected MatrixOrderInvalidException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context)
        { }
    }


    [Serializable]
    public class MatrixColorInfoEmptyException : Exception
    {
        public MatrixColorInfoEmptyException() { }
        public MatrixColorInfoEmptyException(string message) : base(message) { }
        public MatrixColorInfoEmptyException(string message, Exception inner) : base(message, inner) { }
        protected MatrixColorInfoEmptyException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context)
        { }
    }


    [Serializable]
    public class MatrixColorInfoErrorException : Exception
    {
        public MatrixColorInfoErrorException() { }
        public MatrixColorInfoErrorException(string message) : base(message) { }
        public MatrixColorInfoErrorException(string message, Exception inner) : base(message, inner) { }
        protected MatrixColorInfoErrorException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context)
        { }
    }
}
