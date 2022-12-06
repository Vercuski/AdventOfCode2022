using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Threading.Tasks;

namespace Advent.Days
{
    internal class Day6 : IDay
    {
        public string Part1(string[] items)
        {
            string message = items[0];
            return UniqueMessagePosition(message, 4).ToString();
        }

        public string Part2(string[] items)
        {
            string message = items[0];
            return UniqueMessagePosition(message, 14).ToString();
        }

        private int UniqueMessagePosition(string message, int messageSize)
        {
            int returnValue = 0; ;
            for (int i = messageSize; i <= message.Length; i++)
            {
                var candidate = message[(i - messageSize)..(i)].Distinct();
                if (candidate.Count() == messageSize)
                {
                    returnValue = i;
                    break;
                }
            }
            return returnValue;
        }
    }
}
