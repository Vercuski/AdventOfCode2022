using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Threading.Tasks;

namespace Advent.Days
{
    internal class Day7 : IDay
    {
        private Directory root = new Directory();
        private Directory currentDir;
        private int totalSpace = 70000000;
        private int updateSpace = 30000000;
        private int unusedSpace;
        private int neededSpace;

        public Day7()
        {
            currentDir = root;
        }

        public string Part1(string[] items)
        {
            string returnValue = null;
            CommandParser(items);
            var sizeList = root.FindByMaxSize(100000);
            returnValue = sizeList.Sum(x => x.DirSize()).ToString();
            return returnValue;
        }

        public string Part2(string[] items)
        {
            string returnValue = null;
            unusedSpace = totalSpace - root.DirSize();
            neededSpace = updateSpace - unusedSpace;
            var sizeList = root.FindByMaxSize(Int32.MaxValue)
                .OrderBy(x => x.DirSize())
                .Where(x => x.DirSize() > neededSpace).First();
            returnValue = sizeList.DirSize().ToString();
            return returnValue;
        }

        private void CommandParser(string[] items)
        {
            for (int i = 0; i < items.Length; i++)
            {
                string command = items[i];
                if (command.StartsWith("$ cd "))
                {
                    var dirName = command.Replace("$ cd ", "");
                    currentDir = dirName switch
                    {
                        "/" => root,
                        ".." => currentDir = currentDir.parent,
                        _ => currentDir.subDirectories.Where(x => x.dirName == dirName).Single()
                    };
                }
                else if (command.StartsWith("$ ls"))
                {
                    do
                    {
                        command = items[++i];
                        if (command.StartsWith("dir "))
                        {
                            var dirName = command.Replace("dir ", "");
                            currentDir.NewDirectory(dirName);
                        }
                        else
                        {
                            currentDir.AddFile(command);
                        }
                    } while (i < items.Length-1 && !items[i+1].StartsWith("$"));
                }
            }
        }
    }

    public class Directory
    {
        public string dirName { get; set; }
        public List<(int, string)> files { get; set; }
        public List<Directory> subDirectories { get; set; }
        public Directory parent { get; set; }

        public Directory()
        {
            dirName = "/";
            files = new List<(int, string)>();
            subDirectories = new List<Directory>();
        }

        public Directory NewDirectory(string dirName)
        {
            var newdir = new Directory()
            {
                dirName = dirName,
                parent = this
            };
            subDirectories.Add(newdir);
            return newdir;
        }

        public void AddFile(string fileSizeAndName)
        {
            var split = fileSizeAndName.Split(' ');
            files.Add(new(Convert.ToInt32(split[0]), split[1]));
        }

        public int DirSize()
        {
            int sum = files.Sum(x => x.Item1);
            foreach(var subdir in subDirectories)
            {
                sum += subdir.DirSize();
            }
            return sum;
        }

        public List<Directory> FindByMaxSize(int maxSize)
        {
            var sizeList = new List<Directory>();
            foreach(var dir in subDirectories)
            {
                int dirSize = dir.DirSize();
                if(dirSize <= maxSize)
                {
                    sizeList.Add(dir);
                }
                sizeList.AddRange(dir.FindByMaxSize(maxSize).Where(x => !sizeList.Contains(x)));
            }
            var currentDirSize = this.DirSize();
            if (currentDirSize <= maxSize)
            {
                sizeList.Add(this);
            }
            return sizeList;
        }
    }
}
