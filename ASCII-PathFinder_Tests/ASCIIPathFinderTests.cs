using ASCII_Pathfinder;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using static ASCII_Pathfinder.ASCIIPathFinder;

namespace ASCII_PathFinder_Tests
{
    [TestClass]
    public class ASCIIPathFinderTests
    {
        [TestMethod]
        public void Finds_Start_Of_Path_If_Exists()
        {

            var map = @"
                            | 
                            |  
                       -----|
                            |
                            |
                            |  
                       -----@


                        ";
            var asciiPathFinder = new ASCIIPathFinder();
            asciiPathFinder.LoadASCIIMap(map);
            var canFindStart = asciiPathFinder.GoToStart();

            Assert.IsTrue(canFindStart);
            Assert.AreEqual(asciiPathFinder.CurrentChar, ConstantChars.START);
            Assert.AreEqual(asciiPathFinder.CurrentPosition.Item1, 6);
            Assert.AreEqual(asciiPathFinder.CurrentPosition.Item2, 28);
        }


        [TestMethod]
        public void Does_Not_Find_Start_Of_Path_If_Not_Exists()
        {

            var map = @"
                     
                             |  
                             |
                             |
                             | 
                             |
                             |  
                        -----x
                        
                        
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
                              
                                
                                 
                          A---+
                          |   |
                          @ x-B   
                          
                        
                        
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
            Assert.AreEqual(asciiPathFinder.CurrentPosition.Item1, 5);
            Assert.AreEqual(asciiPathFinder.CurrentPosition.Item2, 30);

            asciiPathFinder.Go(Direction.Left);
            asciiPathFinder.Go(Direction.Left);

            Assert.AreEqual(asciiPathFinder.CurrentPosition.Item1, 5);
            Assert.AreEqual(asciiPathFinder.CurrentPosition.Item2, 28);
            Assert.AreEqual(asciiPathFinder.PassedPath, "@|A---+|B-x");

        }

        [TestMethod]
        public void Go_Changes_Current_Char_Correctly()
        {

            var map = @"
                          B-+
                          x |
                        @-A-+
    
                        ";
            var asciiPathFinder = new ASCIIPathFinder();
            asciiPathFinder.LoadASCIIMap(map);
            asciiPathFinder.GoToStart();

            asciiPathFinder.Go(Direction.Right);
            asciiPathFinder.Go(Direction.Right);
            Assert.AreEqual(asciiPathFinder.CurrentChar, 'A');

            asciiPathFinder.Go(Direction.Right);
            asciiPathFinder.Go(Direction.Right);
            asciiPathFinder.Go(Direction.Up);
            asciiPathFinder.Go(Direction.Up);
            Assert.AreEqual(asciiPathFinder.CurrentChar, '+');

            asciiPathFinder.Go(Direction.Left);
            asciiPathFinder.Go(Direction.Left);
            Assert.AreEqual(asciiPathFinder.CurrentChar, 'B');

            asciiPathFinder.Go(Direction.Down);
            Assert.AreEqual(asciiPathFinder.CurrentChar, 'x');

        }


        [TestMethod]
        public void Go_Throws_Exception_If_Out_Of_Bounds()
        {

            var map = @"
                        @-B
                          |
                          +";

            var asciiPathFinder = new ASCIIPathFinder();
            asciiPathFinder.LoadASCIIMap(map);
            asciiPathFinder.GoToStart();
            asciiPathFinder.Go(Direction.Right);
            asciiPathFinder.Go(Direction.Right);
            asciiPathFinder.Go(Direction.Down);
            asciiPathFinder.Go(Direction.Down);
            Assert.ThrowsException<ArgumentException>(() => asciiPathFinder.Go(Direction.Down));
        }


        [TestMethod]
        public void WhereToNext_Finds_Direction_From_Start()
        {
            var asciiPathFinder = new ASCIIPathFinder();

            var map1 = @"
                        @A
                        ";
            asciiPathFinder.LoadASCIIMap(map1);
            asciiPathFinder.GoToStart();
            Assert.AreEqual(asciiPathFinder.WhereToNext(), Direction.Right);

            var map2 = @"
                        @
                        |
                        ";
            asciiPathFinder.LoadASCIIMap(map2);
            asciiPathFinder.GoToStart();
            Assert.AreEqual(asciiPathFinder.WhereToNext(), Direction.Down);

            var map3 = @"
                        +@
                        ";
            asciiPathFinder.LoadASCIIMap(map3);
            asciiPathFinder.GoToStart();
            Assert.AreEqual(asciiPathFinder.WhereToNext(), Direction.Left);

        }

        [TestMethod]
        public void WhereToNext_Prefers_Maintaining_Direction()
        {
            var asciiPathFinder = new ASCIIPathFinder();

            var map1 = @"
                           @
                           |
                           B--+
                           |  |
                           x--+
                        ";
            asciiPathFinder.LoadASCIIMap(map1);
            asciiPathFinder.GoToStart();
            asciiPathFinder.Go(Direction.Down);
            asciiPathFinder.Go(Direction.Down);

            Assert.AreEqual(asciiPathFinder.CurrentChar, 'B');
            Assert.AreEqual(asciiPathFinder.LastMovedInDirection, Direction.Down);
            Assert.AreEqual(asciiPathFinder.WhereToNext(), Direction.Down);
        }

        [TestMethod]
        public void WhereToNext_Null_If_No_Path()
        {
            var asciiPathFinder = new ASCIIPathFinder();

            var map1 = @"
                           @
                           |
                           B   --+
                                 |
                              x--+
                        ";
            asciiPathFinder.LoadASCIIMap(map1);
            asciiPathFinder.GoToStart();
            asciiPathFinder.Go(Direction.Down);
            asciiPathFinder.Go(Direction.Down);

            Assert.AreEqual(asciiPathFinder.CurrentChar, 'B');
            Assert.AreEqual(asciiPathFinder.WhereToNext(), null);
        }

        [TestMethod]
        public void FoundLetters_Contains_Passed_Letters()
        {
            var asciiPathFinder = new ASCIIPathFinder();

            var map1 = @"
                           @
                           |
                           A--B
                              |
                           x--C
                        ";
            asciiPathFinder.LoadASCIIMap(map1);
            asciiPathFinder.GoToStart();

            asciiPathFinder.Go(Direction.Down);
            asciiPathFinder.Go(Direction.Down);
            Assert.AreEqual(asciiPathFinder.FoundLetters, "A");

            asciiPathFinder.Go(Direction.Right);
            asciiPathFinder.Go(Direction.Right);
            asciiPathFinder.Go(Direction.Right);
            Assert.AreEqual(asciiPathFinder.FoundLetters, "AB");

            asciiPathFinder.Go(Direction.Down);
            asciiPathFinder.Go(Direction.Down);
            asciiPathFinder.Go(Direction.Left);
            Assert.AreEqual(asciiPathFinder.FoundLetters, "ABC");
        }


        [TestMethod]
        public void FoundLetters_Ignores_Previously_Found_Letters()
        {
            var asciiPathFinder = new ASCIIPathFinder();

            var map1 = @"
                           x 
                        @--A--+
                           |  |
                           +--+
                        ";
            asciiPathFinder.LoadASCIIMap(map1);
            asciiPathFinder.GoToStart();


            asciiPathFinder.Go(Direction.Right);
            asciiPathFinder.Go(Direction.Right);
            asciiPathFinder.Go(Direction.Right);
            Assert.AreEqual(asciiPathFinder.FoundLetters, "A");

            asciiPathFinder.Go(Direction.Right);
            asciiPathFinder.Go(Direction.Right);
            asciiPathFinder.Go(Direction.Right);

            asciiPathFinder.Go(Direction.Down);
            asciiPathFinder.Go(Direction.Down);

            asciiPathFinder.Go(Direction.Left);
            asciiPathFinder.Go(Direction.Left);
            asciiPathFinder.Go(Direction.Left);

            asciiPathFinder.Go(Direction.Up);
            asciiPathFinder.Go(Direction.Up);
            asciiPathFinder.Go(Direction.Up);
            Assert.AreEqual(asciiPathFinder.FoundLetters, "A");

        }
    }
}


