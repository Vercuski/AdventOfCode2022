using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Threading.Tasks;

namespace Advent.Days
{
    internal class Day4 : IDay
    {
        public string Part1(string[] items)
        {
            int count = 0;
            foreach(var item in items)
            {
                var itemSplit = item.Split(',');
                Section sec1 = new Section(itemSplit[0]);
                Section sec2 = new Section(itemSplit[1]);
                if (sec1.ContainedIn(sec2) || sec2.ContainedIn(sec1))
                {
                    count++;
                }
            }
            return count.ToString();
        }

        public string Part2(string[] items)
        {
            int count = 0;
            foreach (var item in items)
            {
                var itemSplit = item.Split(',');
                Section sec1 = new Section(itemSplit[0]);
                Section sec2 = new Section(itemSplit[1]);
                if (sec1.Overlap(sec2) || sec2.Overlap(sec1))
                {
                    count++;
                }
            }
            return count.ToString();
        }
    }

    public class Section
    {
        public int start { get; private set; }
        public int end { get; private set; }
        public Section(string sectionDefinition)
        {
            var split = sectionDefinition.Split('-');
            start = Convert.ToInt32(split[0]);
            end = Convert.ToInt32(split[1]);
        }

        public bool ContainedIn(Section section)
        {
            return start >= section.start && end <= section.end;
        }
        public bool Overlap(Section section)
        {
            return ContainedIn(section)
                || start <= section.end && section.start <= end;
        }
    }
}
