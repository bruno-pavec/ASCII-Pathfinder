using System;
using System.Collections.Generic;
using System.Linq;

namespace ASCII_Pathfinder
{
    /// <summary>
    /// A class for navigating an ASCII Map.
    /// </summary>
    public class ASCIIPathFinder
    {
        private static readonly Direction[] AllDirections = (Enum.GetValues(typeof(Direction)) as Direction[]);

        /// <summary>
        /// The two-dimensional array of <see cref="char"/> representing the loaded ASCIIMap.
        /// </summary>
        public char[,] ASCIIMapArray { get; private set; }

        /// <summary>
        /// Specifies the current y,x position.
        /// </summary>
        public Tuple<int, int> CurrentPosition { get; private set; } = new Tuple<int, int>(0, 0);

        /// <summary>
        /// Specifies the <see cref="Direction"/> in which the last move was done.
        /// </summary>
        public Direction? LastMovedInDirection { get; private set; }

        /// <summary>
        /// All characters passed on the map so far.
        /// </summary>
        public string PassedPath { get; private set; }

        /// <summary>
        /// Char in the current position.
        /// </summary>
        public char CurrentChar => PassedPath.LastOrDefault();


        private List<FoundLetter> _foundLetters = new List<FoundLetter>();

        /// <summary>
        /// All letters found along the passed path.
        /// </summary>
        public string FoundLetters => new string(this._foundLetters.Select(fC => fC.Char).ToArray());

        /// <summary>
        /// Loads the string with ASCIIMap.
        /// </summary>
        /// <param name="ASCIIMap">The map.</param>
        public void LoadASCIIMap(string ASCIIMap)
        {
            if (string.IsNullOrWhiteSpace(ASCIIMap)) throw new ArgumentException("The provided ASCIIMap is empty!");

            var lines = ASCIIMap.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries);
            lines = this.SanitizeLines(lines);
            var rows = lines.ToList()
                .ConvertAll(s => s.ToCharArray());

            this.ASCIIMapArray = CreateRectangularArray(rows);

            this.PassedPath = string.Empty;
            this.CurrentPosition = new Tuple<int, int>(0, 0);
            this.LastMovedInDirection = null;
            this._foundLetters.Clear();
        }

        private string[] SanitizeLines(string[] lines)
        {
            var maxLength = lines.Max(l => l.Length);
            return lines.ToList().ConvertAll(l => l.Length == maxLength ? l : l.PadRight(maxLength, ' ')).ToArray();
        }


        private Tuple<int, int> GetCoordinatesFor(Direction direction)
        {
            return direction switch
            {
                Direction.Up => new Tuple<int, int>(this.CurrentPosition.Item1 - 1, this.CurrentPosition.Item2),
                Direction.Right => new Tuple<int, int>(this.CurrentPosition.Item1, this.CurrentPosition.Item2 + 1),
                Direction.Down => new Tuple<int, int>(this.CurrentPosition.Item1 + 1, this.CurrentPosition.Item2),
                Direction.Left => new Tuple<int, int>(this.CurrentPosition.Item1, this.CurrentPosition.Item2 - 1),
                _ => throw new ArgumentException($"Impossible to find coordinates for {direction.ToString("g")}!", nameof(direction)),
            };
        }

        private bool IsOutOfBounds(Direction direction)
        {
            return direction switch
            {
                Direction.Up => this.CurrentPosition.Item1 <= 0,
                Direction.Right => this.CurrentPosition.Item2 >= (ASCIIMapArray.GetLength(1) - 1),
                Direction.Down => this.CurrentPosition.Item1 >= (ASCIIMapArray.GetLength(0) - 1),
                Direction.Left => this.CurrentPosition.Item2 <= 0,
                _ => true,
            };
        }

        /// <summary>
        /// Finds character in the specified direction. Null if the field in that direction is out of bounds of the map array.
        /// </summary>
        /// <param name="direction">Direction to look in.</param>
        /// <returns>Character in the specified direction.</returns>
        public char? Look(Direction direction)
        {
            if (this.IsOutOfBounds(direction))
                return null;

            var coordinatesToLookAt = this.GetCoordinatesFor(direction);
            return this.ASCIIMapArray[coordinatesToLookAt.Item1, coordinatesToLookAt.Item2];
        }

        private bool IsValidPathChar(char? c)
        {
            return c.HasValue && !char.IsWhiteSpace(c.Value) &&
                (c.Value == ConstantChars.START || c.Value == ConstantChars.LEFT_OR_RIGHT || c.Value == ConstantChars.UP_OR_DOWN
                || c.Value == ConstantChars.TURNING_POINT || c.Value == ConstantChars.END || IsLetter(c));
        }

        /// <summary>
        /// Chooses the next direction to go in to follow the path.
        /// </summary>
        /// <returns>The <see cref="Direction"/> to go in or null if no possible direction is found.</returns>
        public Direction? WhereToNext()
        {
            switch (this.CurrentChar)
            {
                case ConstantChars.START:
                    {
                        var allPossibleDirections = AllDirections.Where(d => this.IsValidPathChar(this.Look(d)));
                        return allPossibleDirections.Any() ? allPossibleDirections.First() : (Direction?)null;
                    }
                case ConstantChars.END:
                    return null;
                default:
                    {
                        //The below .OrderBy orders false first, and we want the last direction we moved in to take precedence over others 
                        var allPossibleDirections = AllDirections.OrderBy(d => d != this.LastMovedInDirection).Where(d => d != OppositeOf(this.LastMovedInDirection) &&
                                     this.IsValidPathChar(this.Look(d)));
                        return allPossibleDirections.Any() ? allPossibleDirections.First() : (Direction?)null;
                    }
            }
        }

        /// <summary>
        /// Moves in the specified direction, collecting the character found.
        /// </summary>
        /// <param name="direction">The <see cref="Direction"/> to move to.</param>
        public void Go(Direction direction)
        {
            var characterInDirection = this.Look(direction);
            if (!characterInDirection.HasValue)
                throw new ArgumentException("Can't go there.", nameof(direction));

            this.PassedPath += characterInDirection;
            this.CurrentPosition = this.GetCoordinatesFor(direction);
            this.LastMovedInDirection = direction;


            if (this.IsNewFoundLetter(characterInDirection, this.CurrentPosition.Item1, this.CurrentPosition.Item2))
                this._foundLetters.Add(new FoundLetter { Char = characterInDirection.Value, Coordinates = new Tuple<int, int>(this.CurrentPosition.Item1, this.CurrentPosition.Item2) });
        }

        private static bool IsLetter(char? c) => c.HasValue && char.IsLetter(c.Value) && c.Value != ConstantChars.END;


        private bool IsNewFoundLetter(char? c, int y, int x)
        {
            return IsLetter(c) && !this._foundLetters.Any(c => c.Coordinates.Item1 == y
                  && c.Coordinates.Item2 == x);
        }

        /// <summary>
        /// Finds the start of path <see cref="ConstantChars.START"/> and goes to that position.
        /// </summary>
        /// <returns>True if start is found, else false.</returns>
        public bool GoToStart()
        {
            var height = this.ASCIIMapArray.GetLength(0);
            var width = this.ASCIIMapArray.GetLength(1);
            for (var i = 0; i < height; i++)
            {
                for (var j = 0; j < width; j++)
                {
                    if (this.ASCIIMapArray[i, j] == ConstantChars.START)
                    {
                        this.PassedPath = ConstantChars.START.ToString();
                        this.CurrentPosition = new Tuple<int, int>(i, j);
                        this.LastMovedInDirection = null;
                        this._foundLetters.Clear();
                        return true;
                    }

                }
            }
            return false;
        }

        /// <summary>
        /// Finds the start position and follows the path to the end.
        /// </summary>
        public void WalkThePath()
        {
            if (!this.GoToStart())
                throw new InvalidOperationException("Can't find the start of path!");

            while (this.CurrentChar != ConstantChars.END)
            {
                var nextDirection = this.WhereToNext();
                if (!nextDirection.HasValue)
                    throw new InvalidOperationException("Couldn't find next direction to go to! Invalid map?");

                this.Go(nextDirection.Value);
            }
        }



        /// <summary>
        /// Creates a two-dimensional array from a list of arrays of <typeparam name="T">.
        /// </summary>
        /// <typeparam name="T">The underlying type.</typeparam>
        /// <param name="arrays">List of arrays.</param>
        /// <returns>Two-dimensional array of <typeparam name="T".></returns>
        public static T[,] CreateRectangularArray<T>(IList<T[]> arrays)
        {
            if (!arrays.Any())
                return new T[0, 0];

            int minorLength = arrays.First().Length;
            if (arrays.Any(arr => arr.Length != minorLength))
                throw new ArgumentException
                        ("All arrays must be of the same length.", nameof(arrays));

            T[,] ret = new T[arrays.Count, minorLength];

            for (var i = 0; i < arrays.Count; i++)
            {
                var array = arrays[i];

                for (var j = 0; j < minorLength; j++)
                {
                    ret[i, j] = array[j];
                }
            }
            return ret;
        }


        /// <summary>
        /// Finds the oppostite <see cref="Direction"/>.
        /// </summary>
        /// <param name="direction">The <see cref="Direction"/> to find the opposite of.</param>
        /// <returns>The opposite <see cref="Direction"/></returns>
        public static Direction OppositeOf(Direction? direction)
        {
            if (!direction.HasValue) throw new ArgumentNullException(nameof(direction));
            return direction switch
            {
                Direction.Up => Direction.Down,
                Direction.Right => Direction.Left,
                Direction.Down => Direction.Up,
                Direction.Left => Direction.Right,
                _ => throw new ArgumentOutOfRangeException(nameof(direction))
            };
        }

        /// <summary>
        /// Utility to avoid magic strings.
        /// </summary>
        public struct ConstantChars
        {
            public const char START = '@';
            public const char TURNING_POINT = '+';
            public const char END = 'x';
            public const char UP_OR_DOWN = '|';
            public const char LEFT_OR_RIGHT = '-';
        }


        protected struct FoundLetter
        {
            public char Char;
            public Tuple<int, int> Coordinates;
        }
    }
}
