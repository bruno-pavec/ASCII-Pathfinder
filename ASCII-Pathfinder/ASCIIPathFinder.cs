using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ASCII_Pathfinder
{
    public class ASCIIPathFinder
    {
        public char[,] ASCIIMapArray { get; private set; }

        public void LoadASCIIMap(string ASCIIMap)
        {
            var rows = ASCIIMap.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries)
                .ToList()
                .ConvertAll(s => s.ToCharArray());

            this.ASCIIMapArray = CreateRectangularArray(rows);
        }


        public static T[,] CreateRectangularArray<T>(IList<T[]> arrays)
        {
     
            int minorLength = arrays[0].Length;
            T[,] ret = new T[arrays.Count, minorLength];
            for (int i = 0; i < arrays.Count; i++)
            {
                var array = arrays[i];
                if (array.Length != minorLength)
                {
                    throw new ArgumentException
                        ("All arrays must be the same length");
                }
                for (int j = 0; j < minorLength; j++)
                {
                    ret[i, j] = array[j];
                }
            }
            return ret;
        }

    }
}
