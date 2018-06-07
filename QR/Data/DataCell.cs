using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using QR.Drawing.Util;

namespace QR.Drawing.Data
{
    public class DataCell
    {
        //Private members
        private DataMatrix root_matrix;
        //public properties
        public Grid Position { get; set; }
        public CellColor Color { get; set; }
        public DataMatrix RootMatrix
        {
            get { return root_matrix; }
        }
        public CellExtraData ExData { get; set; }
        public Dictionary<string, string> Marks { get; set; }

        //Construction
        public DataCell(Grid posi, CellColor color, DataMatrix matrix) : this(posi, posi.MaxIndex, color, matrix, null) { }
        public DataCell(Grid posi, int max_index, CellColor color, DataMatrix matrix) : this(posi, max_index, color, matrix, null) { }
        public DataCell(Grid posi, int max_index, CellColor color, DataMatrix matrix, CellExtraData ex_data) : this(posi.Row, posi.Column, max_index, color, matrix, ex_data) { }
        public DataCell(int row_index, int col_index, int max_index, CellColor color, DataMatrix matrix) : this(row_index, col_index, max_index, color, matrix, null) { }
        public DataCell(int row_index, int col_index, int max_index, CellColor color, DataMatrix matrix, CellExtraData ex_data)
        {
            Position = new Grid(row_index, col_index, max_index);
            Color = color;
            root_matrix = matrix;
            ExData = ex_data;
            Marks = new Dictionary<string, string>();
        }

        //Public Methods ***********************************************************************************************
        //Structures
        //Eye
        /// <summary>
        /// If the Cell's position is in the 7*7 Eye position.
        /// </summary>
        /// <returns></returns>
        public bool IsEye()
        {
            if (Position.Column < 7 && Position.Row < 7)
            {
                return true;
            }
            else if (Position.Column >= RootMatrix.MatrixOrder - 7 && Position.Row < 7)
            {
                return true;
            }
            else if (Position.Column < 7 && Position.Row >= RootMatrix.MatrixOrder - 7)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        // .
        public bool IsSinglePoint()
        {
            if (SideAdjoinCount() == 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        // --
        public bool IsEndPoint()
        {
            if (SideAdjoinCount() == 1)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        // |__
        public bool IsElbowNode()
        {
            if (SideAdjoinCount() == 2 && !IsPathNode())
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        // |
        public bool IsPathNode()
        {
            if (SideAdjoinCount() == 2)
            {
                if (HasLeftCell() && HasRightCell() && LeftCell().Color == Color && RightCell().Color == Color)
                {
                    return true;
                }
                else if (HasUpCell() && HasDownCell() && UpCell().Color == Color && DownCell().Color == Color)
                {
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
        // |-
        public bool IsTCenter()
        {
            if (SideAdjoinCount() == 3)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        // -|-
        public bool IsCrossCenter()
        {
            if (SideAdjoinCount() == 4)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Judge if left_up right_up left_down right_down is the same color of this color. Returns the same color count.
        /// </summary>
        /// <returns>The number of the same color (with this.Color) in left_up right_up left_down right_down cells.</returns>
        public int CornerAdjoinCount()
        {
            int count = 0;
            if (SameLeftUpColor()) { ++count; }
            if (SameRightUpColor()) { ++count; }
            if (SameLeftDownColor()) { ++count; }
            if (SameRightDownColor()) { ++count; }
            return count;
        }
        /// <summary>
        /// Judge if left right up down cells is the same color of this color. Returns the same color count.
        /// </summary>
        /// <returns>The number of the same color (with this.Color) in left right up down cells.</returns>
        public int SideAdjoinCount()
        {
            int count = 0;
            if (SameLeftColor()) { ++count; }
            if (SameRightColor()) { ++count; }
            if (SameUpColor()) { ++count; }
            if (SameDownColor()) { ++count; }
            return count;
        }

        public delegate bool DeleFeatureFlag(DataCell cell);
        /// <summary>
        /// Features the 3*3 grid whose left top cell is current cell. Mark based flag.
        /// </summary>
        /// <returns></returns>
        public int Feature33(DeleFeatureFlag flag)
        {
            int feature = 0;
            if (HasRightCell() && RightCell().HasRightCell() && HasDownCell() && DownCell().HasDownCell())
            {
                if (flag(this)) { feature += 1; }
                if (flag(RightCell())) { feature += 2; }
                if (flag(RightCell().RightCell())) { feature += 4; }

                if (flag(DownCell())) { feature += 8; }
                if (flag(DownCell().RightCell())) { feature += 16; }
                if (flag(DownCell().RightCell().RightCell())) { feature += 32; }

                if (flag(DownCell().DownCell())) { feature += 64; }
                if (flag(DownCell().DownCell().RightCell())) { feature += 128; }
                if (flag(DownCell().DownCell().RightCell().RightCell())) { feature += 256; }
            }
            else
            {
                feature = -1;
            }
            return feature;
        }
        /// <summary>
        /// Features the 3 rows 2 cols grid. The left top cell is current cell. Mark based flag.
        /// </summary>
        /// <param name="flag"></param>
        /// <returns>1,2  4,8  16,32</returns>
        public int Feature32(DeleFeatureFlag flag)
        {
            int feature = 0;
            if (HasRightCell() && HasDownCell() && DownCell().HasDownCell())
            {
                if (flag(this)) { feature += 1; }
                if (flag(RightCell())) { feature += 2; }

                if (flag(DownCell())) { feature += 4; }
                if (flag(DownCell().RightCell())) { feature += 8; }

                if (flag(DownCell().DownCell())) { feature += 16; }
                if (flag(DownCell().DownCell().RightCell())) { feature += 16; }
            }
            else
            {
                feature = -1;
            }

            return feature;
        }
        /// <summary>
        /// Features the 'row' rows 'col' cols grid. The left top cell is current cell. Mark based flag.
        /// </summary>
        /// </summary>
        /// <returns></returns>
        public int Feature(int row, int col, DeleFeatureFlag flag)
        {
            int feature = 0;

            DataCell cell = this;
            //test if has enough rows
            for (int i = 0; i < row - 1; i++)
            {
                if (cell.HasDownCell())
                {
                    cell = cell.DownCell();
                }
                else
                {
                    return -1;
                }
            }
            cell = this;
            //test if has enough columns
            for (int i = 0; i < col - 1; i++)
            {
                if (cell.HasRightCell())
                {
                    cell = cell.RightCell();
                }
                else
                {
                    return -1;
                }
            }
            cell = this;
            DataCell cell_j = cell;
            //feature
            int bit_flag = 1;
            for (int i = 0; i < row; i++)
            {
                for (int j = 0; j < col; j++)
                {
                    if (flag(cell_j))
                    {
                        feature += bit_flag;
                    }
                    bit_flag <<= 1;
                    if (j < col - 1)
                    {
                        cell_j = cell_j.RightCell();
                    }
                }
                if (i < row - 1)
                {
                    cell = cell.DownCell();
                    cell_j = cell;
                }
            }
            return feature;
        }
        /// <summary>
        /// Returns a int number representes which cells around this cell has the same color with this cell's color.
        /// </summary>
        /// <returns>
        /// 1,2,4,8 => left,up,right,down.  16,32,64,128 => left up, right up, right down, left down. 0 is none.
        /// </returns>
        public int AroundColorInfo()
        {
            int info = 0;
            if (HasLeftCell() && LeftCell().Color == Color) { info += 1; }
            if (HasUpCell() && UpCell().Color == Color) { info += 2; }
            if (HasRightCell() && RightCell().Color == Color) { info += 4; }
            if (HasDownCell() && DownCell().Color == Color) { info += 8; }
            if (HasLeftUpCell() && LeftUpeCell().Color == Color) { info += 16; }
            if (HasRightUpCell() && RightUpCell().Color == Color) { info += 32; }
            if (HasRightDownCell() && RightDownCell().Color == Color) { info += 64; }
            if (HasLeftDownCell() && LeftDownCell().Color == Color) { info += 128; }
            return info;
        }
        public bool SameRightDownColor()
        {
            if (HasRightDownCell() && RightDownCell().Color == Color)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public bool SameLeftDownColor()
        {
            if (HasLeftDownCell() && LeftDownCell().Color == Color)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public bool SameRightUpColor()
        {
            if (HasRightUpCell() && RightUpCell().Color == Color)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public bool SameLeftUpColor()
        {
            if (HasLeftUpCell() && LeftUpeCell().Color == Color)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public bool SameDownColor()
        {
            if (HasDownCell() && DownCell().Color == Color)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public bool SameUpColor()
        {
            if (HasUpCell() && UpCell().Color == Color)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public bool SameRightColor()
        {
            if (HasRightCell() && RightCell().Color == Color)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public bool SameLeftColor()
        {
            if (HasLeftCell() && LeftCell().Color == Color)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        //Neighbours
        public DataCell RightDownCell()
        {
            if (HasRightDownCell())
            {
                return RootMatrix.CellMatrix[Position.Row + 1, Position.Column + 1];
            }
            else
            {
                return null;
            }
        }
        public DataCell LeftDownCell()
        {
            if (HasLeftDownCell())
            {
                return RootMatrix.CellMatrix[Position.Row + 1, Position.Column - 1];
            }
            else
            {
                return null;
            }
        }
        public DataCell RightUpCell()
        {
            if (HasRightUpCell())
            {
                return RootMatrix.CellMatrix[Position.Row - 1, Position.Column + 1];
            }
            else
            {
                return null;
            }
        }
        public DataCell LeftUpeCell()
        {
            if (HasLeftUpCell())
            {
                return RootMatrix.CellMatrix[Position.Row - 1, Position.Column - 1];
            }
            else
            {
                return null;
            }
        }
        public DataCell DownCell()
        {
            if (HasDownCell())
            {
                return RootMatrix.CellMatrix[Position.Row + 1, Position.Column];
            }
            else
            {
                return null;
            }
        }
        public DataCell UpCell()
        {
            if (HasUpCell())
            {
                return RootMatrix.CellMatrix[Position.Row - 1, Position.Column];
            }
            else
            {
                return null;
            }
        }
        public DataCell RightCell()
        {
            if (HasRightCell())
            {
                return RootMatrix.CellMatrix[Position.Row, Position.Column + 1];
            }
            else
            {
                return null;
            }
        }
        public DataCell LeftCell()
        {
            if (HasLeftCell())
            {
                return RootMatrix.CellMatrix[Position.Row, Position.Column - 1];
            }
            else
            {
                return null;
            }
        }
        public bool HasRightDownCell()
        {
            return !IsRightSide() && !IsDownSide();
        }
        public bool HasLeftDownCell()
        {
            return !IsLeftSide() && !IsDownSide();
        }
        public bool HasRightUpCell()
        {
            return !IsRightSide() && !IsUpSide();
        }
        public bool HasLeftUpCell()
        {
            return !IsLeftSide() && !IsUpSide();
        }
        public bool HasDownCell()
        {
            return !IsDownSide();
        }
        public bool HasUpCell()
        {
            return !IsUpSide();
        }
        public bool HasRightCell()
        {
            return !IsRightSide();
        }
        public bool HasLeftCell()
        {
            return !IsLeftSide();
        }

        //Positions based on RootMatirnx
        public bool IsCenter()
        {
            return !IsSide();
        }
        public bool IsCorner()
        {
            if (IsLeftUpCorner() || IsRightUpCorner() || IsLeftDownCorner() || IsRightDownCorner()) { return true; } else { return false; }
        }
        public bool IsRightDownCorner()
        {
            if (IsRightSide() && IsDownSide()) { return true; } else { return false; }
        }
        public bool IsLeftDownCorner()
        {
            if (IsLeftSide() && IsDownSide()) { return true; } else { return false; }
        }
        public bool IsRightUpCorner()
        {
            if (IsRightSide() && IsUpSide()) { return true; } else { return false; }
        }
        public bool IsLeftUpCorner()
        {
            if (IsLeftSide() && IsUpSide()) { return true; } else { return false; }
        }
        public bool IsSide()
        {
            if (IsLeftSide() || IsRightSide() || IsUpSide() || IsDownSide()) { return true; } else { return false; }
        }
        public bool IsDownSide()
        {
            if (Position.Row == Position.MaxIndex) { return true; } else { return false; }
        }
        public bool IsUpSide()
        {
            if (Position.Row == 0) { return true; } else { return false; }
        }
        public bool IsRightSide()
        {
            if (Position.Column == Position.MaxIndex) { return true; } else { return false; }
        }
        public bool IsLeftSide()
        {
            if (Position.Column == 0) { return true; } else { return false; }
        }
    }

    public enum CellColor : byte { BLACK, WHITE }
}
