using System;
using System.Linq;

namespace ASCII_Pathfinder
{
    class Program
    {
        static void Main(string[] args)
        {
            var ASCIIMap1 = @"
                              @---A---+
                                      |
                              x-B-+   C
                                  |   |
                                  +---+";

            var ASCIIMap2 = @"  
                              @
                              | C----+
                              A |    |
                              +---B--+
                                |      x
                                |      |
                                +---D--+";


            var ASCIIMap3 = @"  
                              @---+
                                  B
                            K-----|--A
                            |     |  |
                            |  +--E  |
                            |  |     |
                            +--E--Ex C
                               |     |
                               +--F--+";


            var asciiPathFinder = new ASCIIPathFinder();

            //Map 1
            WalkTheMap(asciiPathFinder, ASCIIMap1, "Map 1");

            //Map 2
            WalkTheMap(asciiPathFinder, ASCIIMap2, "Map 2");

            //Map 3
            WalkTheMap(asciiPathFinder, ASCIIMap3, "Map 3");
        }

        private static void WalkTheMap(ASCIIPathFinder asciiPathFinder, string map, string mapName)
        {
            asciiPathFinder.LoadASCIIMap(map);
            asciiPathFinder.WalkThePath();

            Console.WriteLine(mapName);
            Console.WriteLine(map);
            Console.WriteLine();
            Console.WriteLine($"Path as characters {asciiPathFinder.FoundChars}");
            Console.WriteLine($"Letters {asciiPathFinder.PassedPath}");
            Console.WriteLine("=========================================================================");
        }
    }
}
