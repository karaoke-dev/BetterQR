using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QR.Drawing.Util
{
    /// <summary>
    /// Indexs begin from 0.
    /// No negative values.
    /// </summary>
    public class Grid
    {
        //private values
        private int max_index;
        private int row;
        private int column;

        //public properties
        public int Row
        {
            get { return row; }
            set
            {
                try
                {
                    if (value > max_index)
                    {
                        throw new IndexOutOfRangeException(
                            "Row index should not larger than " + max_index.ToString() + ". Your index is: " + value.ToString());
                    }
                    else if (value < 0)
                    {
                        throw new IndexOutOfRangeException("Row index should be from 0. Your index is: " + value.ToString());
                    }
                    else
                    {
                        row = value;
                    }
                }
                catch (IndexOutOfRangeException e)
                {
                    Console.WriteLine(e.Message);
                }

            }
        }
        public int Column
        {
            get { return column; }
            set
            {
                try
                {
                    if (value > max_index)
                    {
                        throw new IndexOutOfRangeException(
                           "Column index should not larger than " + max_index.ToString() + ". Your index is: " + value.ToString());
                    }
                    else if (value < 0)
                    {
                        throw new IndexOutOfRangeException("Column index Shold be from 0. Your index is: " + value.ToString());
                    }
                    else
                    {
                        column = value;
                    }
                }
                catch (IndexOutOfRangeException e)
                {
                    Console.WriteLine(e.Message);
                }

            }
        }
        public int MaxIndex
        {
            get
            {
                return max_index;
            }
        }

        public Grid() : this(0, 0) { }
        public Grid(int row, int col) : this(row, col, Int32.MaxValue) { }
        public Grid(int row, int col, int max)
        {
            max_index = max;

            Row = row;
            Column = col;
        }
    }


    [Serializable]
    public class IndexOutOfRangeException : Exception
    {
        public IndexOutOfRangeException() { }
        public IndexOutOfRangeException(string message) : base(message) { }
        public IndexOutOfRangeException(string message, Exception inner) : base(message, inner) { }
        protected IndexOutOfRangeException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context)
        { }
    }
}
