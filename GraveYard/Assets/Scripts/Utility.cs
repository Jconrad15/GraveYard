using System;
using System.Collections.Generic;

namespace GraveYard
{
    public static class Utility
    {
        /// <summary>
        /// Returns a random Enum of given type.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T GetRandomEnum<T>()
        {
            Array enumArray = Enum.GetValues(typeof(T));
            T selectedEnum = (T)enumArray.GetValue(UnityEngine.Random.Range(0, enumArray.Length));
            return selectedEnum;
        }

        /// <summary>
        /// Converts a 2D array to a Jagged Array
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="twoDimensionalArray"></param>
        /// <returns></returns>
        public static T[][] ToJaggedArray<T>(this T[,] twoDimensionalArray)
        {
            int rowsFirstIndex = twoDimensionalArray.GetLowerBound(0);
            int rowsLastIndex = twoDimensionalArray.GetUpperBound(0);
            int numberOfRows = rowsLastIndex + 1;

            int columnsFirstIndex = twoDimensionalArray.GetLowerBound(1);
            int columnsLastIndex = twoDimensionalArray.GetUpperBound(1);
            int numberOfColumns = columnsLastIndex + 1;

            T[][] jaggedArray = new T[numberOfRows][];
            for (int i = rowsFirstIndex; i <= rowsLastIndex; i++)
            {
                jaggedArray[i] = new T[numberOfColumns];

                for (int j = columnsFirstIndex; j <= columnsLastIndex; j++)
                {
                    jaggedArray[i][j] = twoDimensionalArray[i, j];
                }
            }
            return jaggedArray;
        }
    }
}