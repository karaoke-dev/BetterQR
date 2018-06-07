using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QR.Drawing.Util
{
    public class Traverse
    {
        //Delegates
        public delegate void DeleFuncMatrix<T, S>(ref T[,] target_matrix, S[,] source_matrix, int i, int j);

        //Delegate Methods
        /// <summary>
        /// Traverse target_matrix and do something with source_matrix or nobody use func method at index of [i,j].
        /// </summary>
        /// <typeparam name="T"></typeparam> Type of target_matrix
        /// <typeparam name="S"></typeparam> Type of source_matrix
        /// <param name="target_matrix"></param>
        /// <param name="source_matrix"></param>
        /// <param name="func"></param> The function do something about the target_matrix[i,j] and source_matirx[i,j]
        static public void TravMatrix<T, S>(T[,] target_matrix, S[,] source_matrix, DeleFuncMatrix<T, S> func)
        {
            for (int i = 0; i < target_matrix.GetLength(0); ++i)
            {
                for (int j = 0; j < target_matrix.GetLength(1); ++j)
                {
                    func(ref target_matrix, source_matrix, i, j);
                }
            }
        }

        static public void PrintMatirx<T>(T[,] target_matrix)
        {
            TravMatrix<T, T>(target_matrix, null, Print);
        }

        //Delegate Functions
        static private void Print<T, S>(ref T[,] matrix, S[,] null_matrix, int i, int j)
        {
            if (j == matrix.GetLength(1) - 1)
            {
                Console.WriteLine();
            }
            else
            {
                Console.Write("{0}, ", matrix[i, j]);
            }
        }
    }
}