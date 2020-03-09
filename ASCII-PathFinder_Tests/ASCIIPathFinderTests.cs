using ASCII_Pathfinder;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace ASCII_PathFinder_Tests
{
    [TestClass]
    public class ASCIIPathFinderTests
    {
        [TestMethod]
        public void Finds_Start_Of_Path_If_Exists()
        {

            var map = @"
0123456789
1     | 
2     |  
3     |
4     |
5     |
6     |  
7-----@
8
9
";
            var asciiPathFinder = new ASCIIPathFinder();
            asciiPathFinder.LoadASCIIMap(map);
            var canFindStart = asciiPathFinder.GoToStart();

            Assert.IsTrue(canFindStart);
            Assert.AreEqual(asciiPathFinder.CurrentChar, ConstantChars.START);
            Assert.AreEqual(asciiPathFinder.CurrentPosition.Item1, 7);
            Assert.AreEqual(asciiPathFinder.CurrentPosition.Item2, 6);
        }


        [TestMethod]
        public void Does_Not_Find_Start_Of_Path_If_Not_Exists()
        {

            var map = @"
0123456789
1     | 
2     |  
3     |
4     |
5     |
6     |  
7-----x
8
9
";
            var asciiPathFinder = new ASCIIPathFinder();
            asciiPathFinder.LoadASCIIMap(map);
            var canFindStart = asciiPathFinder.GoToStart();

            Assert.IsFalse(canFindStart);
            Assert.AreEqual(asciiPathFinder.CurrentChar, default);
            Assert.AreEqual(asciiPathFinder.CurrentPosition.Item1, default);
            Assert.AreEqual(asciiPathFinder.CurrentPosition.Item2, default);
        }

        [TestMethod]
        public void Look_Finds_Correct_Chars()
        {

            var map = @"
       C@A
        B
";
            var asciiPathFinder = new ASCIIPathFinder();
            asciiPathFinder.LoadASCIIMap(map);
            asciiPathFinder.GoToStart();

            // Up is out of bounds
            Assert.AreEqual(asciiPathFinder.Look(Direction.Up), null);
            Assert.AreEqual(asciiPathFinder.Look(Direction.Right), 'A');
            Assert.AreEqual(asciiPathFinder.Look(Direction.Down),'B');
            Assert.AreEqual(asciiPathFinder.Look(Direction.Left), 'C');
        }



        [TestMethod]
        public void Go_Changes_Position_And_Stores_The_Passed_Path()
        {

            var map = @"
0123456789
1      
2        
3         
4  A---+
5  |   |
6  @ x-B   
7  
8
9
";
            var asciiPathFinder = new ASCIIPathFinder();
            asciiPathFinder.LoadASCIIMap(map);
            asciiPathFinder.GoToStart();

            asciiPathFinder.Go(Direction.Up);
            asciiPathFinder.Go(Direction.Up);
            asciiPathFinder.Go(Direction.Right);
            asciiPathFinder.Go(Direction.Right);
            asciiPathFinder.Go(Direction.Right);
            asciiPathFinder.Go(Direction.Right);
            asciiPathFinder.Go(Direction.Down);
            asciiPathFinder.Go(Direction.Down);
            Assert.AreEqual(asciiPathFinder.CurrentPosition.Item1, 6);
            Assert.AreEqual(asciiPathFinder.CurrentPosition.Item2, 7);
            Assert.AreEqual(asciiPathFinder.CurrentChar, 'B');
            asciiPathFinder.Go(Direction.Left);
            asciiPathFinder.Go(Direction.Left);

            Assert.AreEqual(asciiPathFinder.CurrentPosition.Item1, 6);
            Assert.AreEqual(asciiPathFinder.CurrentPosition.Item2, 5);
            Assert.AreEqual(asciiPathFinder.PassedPath, "@|A---+|B-x");

        }

        [TestMethod]
        public void Go_Throws_Exception_If_Out_Pf_Bounds()
        {

            var map = @"
@-B
  |
--+
";
            var asciiPathFinder = new ASCIIPathFinder();
            asciiPathFinder.LoadASCIIMap(map);
            asciiPathFinder.GoToStart();
            asciiPathFinder.Go(Direction.Right);
            asciiPathFinder.Go(Direction.Right);
            asciiPathFinder.Go(Direction.Down);
            asciiPathFinder.Go(Direction.Down);
            asciiPathFinder.Go(Direction.Left);
            asciiPathFinder.Go(Direction.Left);
            Assert.ThrowsException<ArgumentException>(() => asciiPathFinder.Go(Direction.Left));
  

        }
    }
}


