using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Numerics;
using System.Reflection.PortableExecutable;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Advent.Days
{
    internal class Day11 : IDay
    {
        public string Part1(string[] items)
        {
            //List<Monkey> monkeys = new List<Monkey>();
            //var parsedInput = ParseInput(items);
            //foreach (var input in parsedInput)
            //{
            //    monkeys.Add(new Monkey(input, 3));
            //}
            //for (int round = 1; round <= 20; round++)
            //{
            //    foreach (var monkey in monkeys)
            //    {
            //        while (monkey.items.Count != 0)
            //        {
            //            var item = monkey.items.Dequeue();
            //            int newWorryLevel = item.CalculateWorryLevel(monkey);
            //            //Console.WriteLine($"Round #{round} - Monkey #{monkey.monkeyId} - Test: {monkey.TestItem(item)} - Pass To: {monkey.ThrowToMonkey(item)}");
            //            //Console.ReadLine();
            //            var targetMonkeyId = monkey.ThrowToMonkey(item);
            //            var targetMonkey = monkeys.Where(x => x.monkeyId == targetMonkeyId).Single();
            //            item.itemValue = newWorryLevel;
            //            targetMonkey.items.Enqueue(item);
            //        }
            //    }
            //    if ((round == 1 || round == 20 || round % 1000 == 0) && round != 0)
            //    {
            //        Console.WriteLine($"== After round {round} ==");
            //        foreach (var m in monkeys)
            //        {
            //            Console.WriteLine($"Monkey {m.monkeyId} inspected items {m.itemsInspected} times and has {m.items.Count()} items.");
            //        }
            //        Console.WriteLine();
            //    }
            //}
            //var topInspectors = monkeys.OrderByDescending(x => x.itemsInspected).Take(2).ToList();
            //return (topInspectors[0].itemsInspected * topInspectors[1].itemsInspected).ToString();
            return "0";
        }

        public string Part2(string[] items)
        {
            List<Monkey> monkeys = new List<Monkey>();
            var parsedInput = ParseInput(items);
            foreach (var input in parsedInput)
            {
                monkeys.Add(new Monkey(input));
            }
            ulong worryDivider = 1;
            foreach(var monkey in monkeys)
            {
                worryDivider *= monkey.test;
            }
            foreach(var monkey in monkeys)
            {
                monkey.operation.worryDivider = worryDivider;
            }
            for (int round = 1; round <= 10000; round++)
            {
                foreach (var monkey in monkeys)
                {
                    while (monkey.items.Count != 0)
                    {
                        var item = monkey.items.Dequeue();
                        ulong newWorryLevel = item.CalculateWorryLevel(monkey);
                        var targetMonkeyId = monkey.ThrowToMonkey(item);
                        //Console.WriteLine($"Round #{round} - Monkey #{monkey.monkeyId} - Pass To: {targetMonkeyId}");
                        var targetMonkey = monkeys.Where(x => x.monkeyId == targetMonkeyId).Single();
                        item.itemValue = newWorryLevel;
                        targetMonkey.items.Enqueue(item);
                        //Console.ReadLine();
                    }
                }
                if ((round == 1 || round == 20 || round % 1000 == 0) && round != 0)
                {
                    Console.WriteLine($"== After round {round} ==");
                    foreach(var m in monkeys)
                    {
                        Console.WriteLine($"Monkey {m.monkeyId} inspected items {m.itemsInspected} times and has {m.items.Count()} items.");
                    }
                    Console.WriteLine();
                }
            }
            var topInspectors = monkeys.OrderByDescending(x => x.itemsInspected).Take(2).ToList();
            return (topInspectors[0].itemsInspected * topInspectors[1].itemsInspected).ToString();
        }

        List<string[]> ParseInput(string[] items)
        {
            List<string[]> returnValue = new();
            string[] inputBlock = new string[6];
            int counter = 0;
            for(int i = 0; i<items.Length; i++)
            {
                if (counter == 6)
                {
                    returnValue.Add(inputBlock);
                    inputBlock = new string[6];
                    counter = 0;
                    continue;
                }
                inputBlock[counter++] = items[i].Trim();
            }
            returnValue.Add(inputBlock);
            return returnValue;
        }
    }

    class Monkey
    {
        public int monkeyId { get; private set; }
        public ulong itemsInspected { get; private set; }
        public Queue<Item> items { get; private set; }
        public MonkeyOp operation { get; private set; }
        public ulong test { get; private set; }
        public int trueCondition { get; private set; }
        public int falseCondition { get; private set; }
        
        public Monkey(string[] monkeyConfig)
        {
            itemsInspected = 0;
            items = new();
            monkeyId = Convert.ToInt32(new string(monkeyConfig[0].Where(c => char.IsDigit(c)).ToArray()));

            var startingItems = monkeyConfig[1].Replace("Starting items: ", "").Split(',').ToList();
            startingItems.ForEach(x => items.Enqueue(new Item(Convert.ToUInt64(x))));

            operation = new MonkeyOp(monkeyConfig[2]);
            test = Convert.ToUInt64(new string(monkeyConfig[3].Where(c => char.IsDigit(c)).ToArray()));

            trueCondition = Convert.ToInt32(new string(monkeyConfig[4].Where(c => char.IsDigit(c)).ToArray()));
            falseCondition = Convert.ToInt32(new string(monkeyConfig[5].Where(c => char.IsDigit(c)).ToArray()));
        }

        public bool TestItem(Item item)
        {
            itemsInspected++;
            ulong wl = item.CalculateWorryLevel(this);
            return (wl % test) == 0;
        }

        public int ThrowToMonkey(Item item)
        {
            bool itemTestResult = TestItem(item);
            if(itemTestResult) { item.itemValue = test; }
            return itemTestResult ? trueCondition : falseCondition;
        }
    }

    class MonkeyOp
    {
        public string lhs { get; private set; }
        public string operation { get; private set; }
        public string rhs { get; private set; }
        public ulong worryDivider { get; set; } = 1;

        public MonkeyOp(string opText)
        {
            if (opText.StartsWith("Operation"))
            {
                var formulaSplit = opText.Replace("Operation: new = ", "").Split(' ');
                lhs = formulaSplit[0];
                operation = formulaSplit[1];
                rhs = formulaSplit[2];
            }
        }

        public ulong CalculatedWorryLevel(Item item)
        {
            ulong worryLevel = 0;
            switch(operation)
            {
                case "*":
                    if(rhs == "old")
                    {
                        worryLevel = item.itemValue * item.itemValue;
                    } else
                    {
                        worryLevel = item.itemValue * Convert.ToUInt64(rhs);
                    }
                    break;

                case "+":
                    if (rhs == "old")
                    {
                        worryLevel = item.itemValue + item.itemValue;
                    }
                    else
                    {
                        worryLevel = item.itemValue + Convert.ToUInt64(rhs);
                    }
                    break;
            }
            //Console.WriteLine($"WL: {worryLevel}");
            //Console.WriteLine($"WD: {_worryDivider}");
            //Console.WriteLine($"WL/WD: {worryLevel / _worryDivider}");
            return worryLevel %= worryDivider;
        }
    }

    class Item
    {
        public ulong itemValue { get; set; }
        public Item(ulong itemValue)
        {
            this.itemValue = itemValue;
        }

        public ulong CalculateWorryLevel(Monkey monkey)
        {
            return monkey.operation.CalculatedWorryLevel(this);
        }
    }
}
