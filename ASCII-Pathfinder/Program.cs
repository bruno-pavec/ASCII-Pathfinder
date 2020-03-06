using System;
using System.Linq;

namespace ASCII_Pathfinder
{
    class Program
    {
        static void Main(string[] args)
        {
            var ASCIIMap = @"
@---A---+
        |
x-B-+   C
    |   |
    +---+";

            var asciiPathFinder = new ASCIIPathFinder();
            asciiPathFinder.LoadASCIIMap(ASCIIMap);
        }
    }
}
