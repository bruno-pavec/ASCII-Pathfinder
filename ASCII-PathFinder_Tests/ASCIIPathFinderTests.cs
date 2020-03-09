using ASCII_Pathfinder;
using Microsoft.VisualStudio.TestTools.UnitTesting;

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
    }
}

