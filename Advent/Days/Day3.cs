using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Advent.Days
{
    internal class Day3 : IDay
    {
        public string Part1(string[] sackContents)
        {
            int sum = 0;
            foreach (var content in sackContents)
            {
                var compartment1 = content.Substring(0, content.Length / 2);
                var compartment2 = content.Substring(content.Length / 2, content.Length - compartment1.Length);
                var output = (int)compartment1.Intersect(compartment2).Single();
                sum += OutputLogic(output);
            }
            return sum.ToString();
        }

        public string Part2(string[] sackContents)
        {
            int sum = 0;
            for (int i = 0; i < sackContents.Length; i += 3)
            {
                var common = sackContents[i].Intersect(sackContents[i + 1]).Intersect(sackContents[i + 2]).Single();
                sum += OutputLogic(common);
            }
            return sum.ToString();
        }

        private int OutputLogic(int compartmentValue)
        {
            return compartmentValue >= 97 ? compartmentValue - 96 : compartmentValue - 38;
        }
    }
}
