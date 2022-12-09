using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Diagnostics.SymbolStore;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Advent.Days
{
    internal class Day9 : IDay
    {
        public string Part1(string[] items)
        {
            string returnValue = null;
            var commands = ParseCommands(items);
            var grid = new Grid();
            foreach (var command in commands)
            {
                grid.MoveHead(command);
            }
            returnValue = grid.VisitedCoordinatesCount().ToString();
            return returnValue;
        }

        public string Part2(string[] items)
        {
            string returnValue = null;
            var commands = ParseCommands(items);
            var grid = new Grid(10);
            foreach (var command in commands)
            {
                grid.MoveHead(command);
            }
            returnValue = grid.VisitedCoordinatesCount().ToString();
            return returnValue;
        }

        private List<Command> ParseCommands(string[] items)
        {
            List<Command> returnValue = new List<Command>();
            foreach(var command in items)
            {
                returnValue.Add(new Command(command));
            }
            return returnValue;
        }
    }

    class Grid
    {
        private List<GridPoint> coordinatesVisitedByTail { get; set; }
        private GridPoint[] locations;
        private GridPoint previousHeadLocation;

        public Grid(int numberOfKnots = 2)
        {
            int startX = 0, startY = 0;
            coordinatesVisitedByTail = new List<GridPoint>();
            previousHeadLocation = new GridPoint(startX, startY);
            locations = new GridPoint[numberOfKnots];
            for(int i = 0; i < locations.Length; i++)
            {
                locations[i] = new GridPoint(startX, startY);
            }
            coordinatesVisitedByTail.Add(new GridPoint(startX, startY));
        }

        public void PrintGrid()
        {
            //int xMax = 26, yMax = 21;
            int xMax = 6, yMax = 5;

            for (int y = yMax; y >0 ; y--)
            {
                for (int x = 0; x < xMax; x++)
                {
                    Console.SetCursorPosition(x, y);
                    var cPoint = new GridPoint(x, yMax-y);
                    if (locations.Contains(cPoint))
                    {
                        bool writtenToPoint = false;
                        for (int z = locations.Length-1; z >= 0; z--) {
                            if (locations[z] == cPoint)
                            {
                                Console.SetCursorPosition(x, y);
                                if (z == 0)
                                { Console.Write("H"); }
                                else
                                { Console.Write(z);  }
                            }                           
                        }
                    }
                    else
                    {
                        Console.Write(".");
                    }
                }
            }
            Console.SetCursorPosition(0, yMax + 2);
            for(int x=0; x < locations.Length; x++)
            {
                string position;
                if (x == 0)
                {
                    position = "H";
                } else
                {
                    position = x.ToString();
                }
                bool equal = false; ;
                if (x != locations.Length - 1)
                {
                    equal = locations[x].Equals(locations[x + 1]);
                }
                Console.WriteLine($"{position} - ({locations[x].x}, {locations[x].y}) - {equal}");
            }
        }

        public int VisitedCoordinatesCount()
        {
            return coordinatesVisitedByTail.Count();
        }

        public void MoveHead(Command move)
        {
            for(int i = 0; i < move.magnitude; i++)
            {
                previousHeadLocation = new GridPoint(locations[0]);
                switch (move.direction)
                {
                    case "U":
                        locations[0].MoveUp();
                        break;
                    case "D":
                        locations[0].MoveDown();
                        break;
                    case "R":
                        locations[0].MoveRight();
                        break;
                    case "L":
                        locations[0].MoveLeft();
                        break;
                }

                for (int x = 0; x < locations.Length-1; x++)
                {
                    var head = locations[x];
                    var tail = locations[x+1];
                    if (!head.isAdjacentTo(tail))
                    {                        
                        MoveSegment(head, tail);
                        if ((x+1) == (locations.Length - 1) && !coordinatesVisitedByTail.Any(coord => coord.x == tail.x && coord.y == tail.y))
                        {
                            coordinatesVisitedByTail.Add(new GridPoint(tail));
                        }
                    }
                }
                //Console.Clear();
                //PrintGrid();
                //Console.WriteLine(move.direction + " " + move.magnitude);
                //Console.ReadLine();
            }
        }

        private void MoveSegment(GridPoint leader, GridPoint follower)
        {
            int xDiff = leader.x - follower.x;
            int yDiff = leader.y - follower.y;
            if(yDiff == 2 && xDiff == 0)
            {
                follower.y++;
            }
            if (xDiff == 2 && yDiff == 0)
            {
                follower.x++;
            }
            if (yDiff == -2 && xDiff == 0)
            {
                follower.y--;
            }
            if (xDiff == -2 && yDiff == 0)
            {
                follower.x--;
            }
            if ((yDiff == 2 || yDiff == 1) && (xDiff == 2 || xDiff == 1))
            {
                follower.y++;
                follower.x++;
            }
            if ((yDiff == 2 || yDiff == 1) && (xDiff == -2 || xDiff == -1))
            {
                follower.y++;
                follower.x--;
            }
            if ((yDiff == -2 || yDiff == -1) && (xDiff == 2 || xDiff == 1))
            {
                follower.y--;
                follower.x++;
            }
            if ((yDiff == -2 || yDiff == -1) && (xDiff == -2 || xDiff == -1))
            {
                follower.y--;
                follower.x--;
            }
        }
    }

    class Command
    {
        public string direction { get; set; }
        public int magnitude { get; set; }

        public Command(string command)
        {
            var splitCommand = command.Split(' ');
            direction = splitCommand[0];
            magnitude = Convert.ToInt32(splitCommand[1]);
        }
    }

    class GridPoint
    {
        public int x { get; set; }
        public int y { get; set; }

        public GridPoint(int startX, int startY)
        {
            x = startX; 
            y = startY;
        }

        public GridPoint(GridPoint otherPoint)
        {
            x = otherPoint.x;
            y = otherPoint.y;
        }

        public bool isAdjacentTo(GridPoint otherPoint)
        {
            if (this == otherPoint) { return true; }
            if (Math.Abs(x - otherPoint.x) <= 1
                && Math.Abs(y - otherPoint.y) <= 1)
            { return true; }
            return false;
        }

        public override bool Equals(object? obj) => this.Equals(obj as GridPoint);

        public bool Equals(GridPoint p)
        {
            if (p is null)
            {
                return false;
            }

            if (Object.ReferenceEquals(this, p))
            {
                return true;
            }

            if (this.GetType() != p.GetType())
            {
                return false;
            }

            return (x == p.x) && (y == p.y);
        }

        public static bool operator ==(GridPoint lhs, GridPoint rhs)
        {
            if (lhs is null)
            {
                if (rhs is null)
                {
                    return true;
                }

                // Only the left side is null.
                return false;
            }
            // Equals handles case of null on right side.
            return (lhs.x == rhs.x) && (lhs.y == rhs.y);
        }

        public static bool operator !=(GridPoint lhs, GridPoint rhs) => !(lhs == rhs);

        public void MoveUp()
        {
            y++;
        }

        public void MoveDown()
        {
            y--;
        }

        public void MoveLeft()
        {
            x--;
        }

        public void MoveRight()
        {
            x++;
        }
    }
}
