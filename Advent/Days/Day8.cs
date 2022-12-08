using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace Advent.Days
{
    internal class Day8 : IDay
    {
        public string Part1(string[] items)
        {
            string returnValue = null;
            Forest forest = new Forest(items);
            returnValue = forest.VisibleTreeCount().ToString();
            return returnValue;
        }

        public string Part2(string[] items)
        {
            string returnValue = null;
            Forest forest = new Forest(items);
            returnValue = forest.HighestScenicScore().ToString();
            return returnValue;
        }
    }

    class Forest
    {
        enum Direction {
            NORTH,
            SOUTH,
            EAST,
            WEST
        }
        public Forest(string[] inputGrid)
        {
            GetForestMap(inputGrid);
        }
        public char[,] forestGrid { get; set; }
        public int xLength {
            get {
                return forestGrid.GetLength(0);
            }
        }
        public int yLength
        {
            get
            {
                return forestGrid.GetLength(1);
            }
        }

        private void GetForestMap(string[] items)
        {
            int xAxis = items[0].Length;
            int yAxis = items.Length;
            forestGrid = new char[xAxis, yAxis];

            for (int y = 0; y < items.Length; y++)
            {
                string currentItem = items[y];
                for (int x = 0; x < currentItem.Length; x++)
                {
                    forestGrid[x, y] = currentItem[x];
                }
            }
        }

        public int VisibleTreeCount()
        {
            int visibleTrees = (xLength * 2) + ((yLength-2)*2);
            for (int y = 1; y < yLength-1; y++)
            {
                for( int x = 1; x < xLength-1; x++)
                {
                    bool visible = false;
                    visible |= VisibleFromDirection(Direction.NORTH, x, y);
                    visible |= VisibleFromDirection(Direction.SOUTH, x, y);
                    visible |= VisibleFromDirection(Direction.EAST, x, y);
                    visible |= VisibleFromDirection(Direction.WEST, x, y);
                    if (visible)
                    {
                        visibleTrees++;
                    }
                }
            }
            return visibleTrees;
        }

        public int HighestScenicScore()
        {
            int scenicScore = 0;
            for (int y = 0; y < yLength; y++)
            {
                for (int x = 0; x < xLength; x++)
                {
                    scenicScore = Math.Max(scenicScore, CalculateScenicScore(x, y));
                }
            }
            return scenicScore;
        }

        private bool VisibleFromDirection(Direction direction, int startX, int startY)
        {
            int targetTreeHeight = forestGrid[startX, startY];
            if (direction == Direction.NORTH)
            {
                for (int y = startY - 1; y >= 0; y--)
                {
                    if (forestGrid[startX, y] >= targetTreeHeight)
                    {
                        return false;
                    }
                }
            }
            if (direction == Direction.SOUTH)
            {
                for (int y = startY + 1; y < yLength; y++)
                {
                    if (forestGrid[startX, y] >= targetTreeHeight)
                    {
                        return false;
                    }
                }
            }
            if (direction == Direction.EAST)
            {
                for (int x = startX + 1; x < xLength; x++)
                {
                    if (forestGrid[x, startY] >= targetTreeHeight)
                    {
                        return false;
                    }
                }
            }
            if (direction == Direction.WEST)
            {
                for (int x = startX - 1; x >= 0; x--)
                {
                    if (forestGrid[x, startY] >= targetTreeHeight)
                    {
                        return false;
                    }
                }
            }
            return true;
        }
        private int CalculateScenicScore(int startX, int startY)
        {
            int scenicScore = 1;

            int targetTreeHeight = forestGrid[startX, startY];
            int numberOfVisibleTrees = 0;
            // NORTH
            for (int y = startY - 1; y >= 0; y--)
            {
                int nextTree = forestGrid[startX, y];
                if (nextTree < targetTreeHeight)
                {
                    numberOfVisibleTrees++;
                }
                else if (nextTree >= targetTreeHeight)
                {
                    numberOfVisibleTrees++;
                    break;
                }
            }
            scenicScore *= numberOfVisibleTrees;

            // SOUTH
            numberOfVisibleTrees = 0;
            for (int y = startY + 1; y < yLength; y++)
            {
                int nextTree = forestGrid[startX, y];
                if (nextTree < targetTreeHeight)
                {
                    numberOfVisibleTrees++;
                }
                else if (nextTree >= targetTreeHeight)
                {
                    numberOfVisibleTrees++;
                    break;
                }
            }
            scenicScore *= numberOfVisibleTrees;

            // EAST
            numberOfVisibleTrees = 0;
            for (int x = startX + 1; x < xLength; x++)
            {
                int nextTree = forestGrid[x, startY];
                if (nextTree < targetTreeHeight)
                {
                    numberOfVisibleTrees++;
                }
                else if (nextTree >= targetTreeHeight)
                {
                    numberOfVisibleTrees++;
                    break;
                }
            }
            scenicScore *= numberOfVisibleTrees;

            // WEST
            numberOfVisibleTrees = 0;
            for (int x = startX - 1; x >= 0; x--)
            {
                int nextTree = forestGrid[x, startY];
                if (nextTree < targetTreeHeight)
                {
                    numberOfVisibleTrees++;
                }
                else if (nextTree >= targetTreeHeight)
                {
                    numberOfVisibleTrees++;
                    break;
                }
            }
            scenicScore *= numberOfVisibleTrees;

            return scenicScore;
        }
    }
}
