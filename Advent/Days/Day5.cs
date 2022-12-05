using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Advent.Days
{
    internal class Day5 : IDay
    {
        public string Part1(string[] items)
        {
            string message = String.Empty;
            Stack<string>[] stack = ReadCrates(items);
            stack = MoveCrates(items, stack, 1);
            message = GetMessage(stack);
            return message;
        }

        public string Part2(string[] items)
        {
            string message = String.Empty;
            Stack<string>[] stack = ReadCrates(items);
            stack = MoveCrates(items, stack, 2);
            message = GetMessage(stack);
            return message;
        }

        private Stack<string>[] ReadCrates(string[] items)
        {
            Stack<string>[] crates = null;
            List<string> listOfCrates = new List<string>();
            string crateStackCount = String.Empty;
            foreach(var item in items)
            {
                if (String.IsNullOrEmpty(item))
                {
                    break;
                }
                if (item.StartsWith(" 1 "))
                {
                    crateStackCount = item.Trim();
                } else
                {
                    listOfCrates.Add(item);
                }
            }

            var numberOfStacks = crateStackCount.Split("   ").Length;
            crates = new Stack<string>[numberOfStacks];
            for(int i=0; i<crates.Length; i++)
            {
                crates[i] = new Stack<string>();
            }

            listOfCrates.Reverse();
            foreach (var crateList in listOfCrates)
            {
                int x = 0;
                for (int i=0; i<crateList.Length; i += 3)
                {
                    var crate = crateList[i..(i + 3)].Trim();
                    if (!String.IsNullOrEmpty(crate))
                    {
                        crates[x].Push(crate);
                    }
                    x++;
                    i++;
                }
            }
            return crates;
        }

        private Stack<string>[] MoveCrates(string[] items, Stack<string>[] stack, int moveMethod)
        {
            var moveListTop = items.ToList().IndexOf(String.Empty);
            Regex moveRegex = new Regex("move [0-9]*");
            Regex fromRegex = new Regex("from [0-9]*");
            Regex toRegex = new Regex("to [0-9]*");
            for (int i = moveListTop+1; i < items.Length; i++)
            {
                string command = items[i];
                int numMoves = Convert.ToInt32(moveRegex.Match(command).Value.Replace("move ", ""));
                int from = Convert.ToInt32(fromRegex.Match(command).Value.Replace("from", ""));
                int to = Convert.ToInt32(toRegex.Match(command).Value.Replace("to ", ""));

                if (moveMethod == 1)
                {
                    for (int x = 0; x < numMoves; x++)
                    {
                        stack[to - 1].Push(stack[from - 1].Pop());
                    }
                } else if (moveMethod == 2)
                {
                    Stack<string> temp = new Stack<string>();

                    for (int x = 0; x < numMoves; x++)
                    {
                        temp.Push(stack[from - 1].Pop());
                    }
                    do
                    {
                        stack[to - 1].Push(temp.Pop());
                    } while (temp.Count > 0);
                }
            }
            return stack;
        }

        private string GetMessage(Stack<string>[] stack)
        {
            StringBuilder message = new StringBuilder();
            foreach(var nextStack in stack.ToList())
            {
                var popped = message.Append(nextStack.Pop()[1]);
            }
            return message.ToString();
        }
    }
}
