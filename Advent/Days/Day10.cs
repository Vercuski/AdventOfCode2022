using Microsoft.VisualBasic.FileIO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection.Metadata.Ecma335;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Threading.Tasks;

namespace Advent.Days
{
    internal class Day10 : IDay
    {
        private static int[] cycleReads = { 20, 60, 100, 140, 180, 220 };
        private int maxCycle = cycleReads.Max()+1;

        public string Part1(string[] items)
        {
            int signalStrength = 0;
            int opCount = 0;
            var operations = ReadOperations(items);
            var os = new OS();
            for(int i = 1; i <= maxCycle; i++)
            {
                if (cycleReads.Contains(os.clockCycle))
                {
                    //Console.WriteLine($"Cycle #{os.clockCycle} = {(os.registerX * os.clockCycle)}");
                    signalStrength += (os.registerX * os.clockCycle);
                }
                if (os.remainingCyclesForCurrentOp == 0)
                {
                    os.ProcessOperation(operations[opCount++]);
                }
                //Console.WriteLine(os.ToString());
                os.FinishProcessCycle();
            }
            return signalStrength.ToString();
        }

        public string Part2(string[] items)
        {
            var operations = ReadOperations(items);
            var os = new OS();
            for (int opCount = 0; opCount < operations.Count;)
            {
                if (os.remainingCyclesForCurrentOp == 0)
                {
                    os.ProcessOperation(operations[opCount++]);
                }
                os.BuildImage();
                os.FinishProcessCycle();
            }
            Console.WriteLine("----------------------------------------");
            Console.WriteLine(os.Render());
            Console.WriteLine("----------------------------------------");
            return "";
        }

        private List<Operation> ReadOperations(string[] items)
        {
            List<Operation> returnValue = new();
            foreach(var item in items)
            {
                returnValue.Add(new Operation(item));
            }
            return returnValue;
        }
    }

    class OS
    {
        public int clockCycle { get; private set; } = 1;
        public int registerX { get; private set; } = 1;
        public int remainingCyclesForCurrentOp { get; private set; } = 0;
        private Operation currentOp { get; set; }
        private StringBuilder CRT { get; set; }
        private int drawPosition { get; set; } = 0;
        public OS()
        {
            CRT = new StringBuilder();
        }

        public void FinishProcessCycle()
        {
            clockCycle++;
            remainingCyclesForCurrentOp = Math.Max(0, --remainingCyclesForCurrentOp);
            if (remainingCyclesForCurrentOp == 0)
            {
                registerX += currentOp.commandValue;
            }
        }

        public void ProcessOperation(Operation operation)
        {
            remainingCyclesForCurrentOp = operation.cycleTime;
            currentOp = operation;
        }

        public void BuildImage()
        {
            string line = new string('.', 40);
            char[] line2 = line.ToCharArray();
            if ((registerX - 1) >= 0)
            {
                line2[registerX - 1] = '#';
            }
            if (registerX >= 0)
            {
                line2[registerX] = '#';
            }
            if ((registerX + 1) < 39)
            {
                line2[registerX + 1] = '#';
            }
            line = new string(line2);
            CRT.Append(line[drawPosition]);
            //Console.WriteLine($"Sprint position: {line}");
            //Console.WriteLine($"Start cycle {clockCycle}");
            //Console.WriteLine($"During cycle {clockCycle}: draw as position {drawPosition}");
            //Console.WriteLine(CRT.ToString());

            drawPosition++;
            if (drawPosition == 40)
            {
                CRT.AppendLine();
                drawPosition = 0;
            }
        }

        public string Render()
        {
            return CRT.ToString();
        }

        public override string ToString()
         {
            StringBuilder osState = new StringBuilder();
            osState.AppendLine($"ClockCycle: {clockCycle}");
            osState.AppendLine($"Register X: {registerX}");
            osState.AppendLine($"Current Op: {currentOp.ToString()}");
            osState.AppendLine($"Remaining Cycles: {remainingCyclesForCurrentOp}");
            return osState.ToString();
        }
    }

    class Operation
    {
        public string commandName { get; private set; }
        public int commandValue { get; private set; } = 0;
        public int cycleTime { get; private set; } = 1;

        public Operation(string command)
        {
            var split = command.Split(' ');
            commandName = split[0];
            if (split.Length > 1)
            {
                commandValue = Convert.ToInt16(split[1]);
                cycleTime++;
            }
        }

        public override string ToString()
        {
            return $"{commandName} {commandValue}";
        }
    }
}
