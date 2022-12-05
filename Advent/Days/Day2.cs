using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Advent.Days
{
    internal class Day2 : IDay
    {
        private static string[] _strategy1 = new string[]
        { "B X", "C Y", "A Z", "A X", "B Y", "C Z", "C X", "A Y", "B Z" };

        private static string[] _strategy2 = new string[]
        { "B X", "C X", "A X", "A Y", "B Y", "C Y", "C Z", "A Z", "B Z" };

        public string Part1(string[] rpsrounds)
        {
            return GetFinalScore(rpsrounds, _strategy1).ToString();
        }

        public string Part2(string[] rpsrounds)
        {
            return GetFinalScore(rpsrounds, _strategy2).ToString();
        }

        private int GetFinalScore(string[] rpsrounds, string[] _strategy)
        {
            int sum = 0;
            rpsrounds.ToList().ForEach(x => sum += Array.IndexOf(_strategy, x) + 1);
            return sum;
        }
    }
}
