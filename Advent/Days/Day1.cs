using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Advent.Days
{
    internal class Day1 : IDay
    {
        public string Part1(string[] items)
        {
            List<int> food = new List<int>();
            int sum = 0;
            foreach (var elf in items)
            {
                if (string.IsNullOrEmpty(elf))
                {
                    food.Add(sum);
                    sum = 0;
                }
                else
                {
                    sum += Convert.ToInt32(elf);
                }
            }
            food.Add(sum);
            return food.Max().ToString();
        }

        public string Part2(string[] items)
        {
            List<int> food = new List<int>();
            int sum = 0;
            foreach (var elf in items)
            {
                if (string.IsNullOrEmpty(elf))
                {
                    food.Add(sum);
                    sum = 0;
                }
                else
                {
                    sum += Convert.ToInt32(elf);
                }
            }
            food.Add(sum);
            return food.OrderByDescending(x => x).Take(3).Sum().ToString();
        }
    }
}
