using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2023.challenges
{
    internal class Day8
    {
        public void Run()
        {
            string? filePath = @"C:\Users\KM\source\repos\AdventOfCode2023\inputs\day8Input.txt";

            ProcessFile(filePath);
            Console.WriteLine("Part2");
            //ProcessFilePart2(filePath);

            Console.WriteLine("Naciśnij dowolny klawisz, aby zakończyć...");
            Console.ReadKey();
        }

        void ProcessFile(string filePath)
        {
            var lines = new List<string>();

            using (StreamReader reader = new StreamReader(filePath))
            {
                string? line;
                while ((line = reader.ReadLine()) != null)
                {
                    lines.Add(line);
                }
            }

            var lrInstructions = lines[0];
            var lrInstructionsP2 = lrInstructions.ToCharArray().Select(x => x =='L' ? true : false).ToArray();
            var nodes = new List<Node>();

            for (int i = 2; i < lines.Count; i++){
                nodes.Add(new Node
                {
                    Root = lines[i].Substring(0, 3),
                    LeftText = lines[i].Substring(7, 3),
                    RightText = lines[i].Substring(12, 3)
                });
            }

            foreach (var node in nodes)
            {
                node.Left = nodes.Where(x => x.Root == node.LeftText).First();
                node.Right = nodes.Where(x => x.Root == node.RightText).First();
                node.EndNode = node.Root.EndsWith("Z") ? true : false;
            }

            var endNodes = nodes.Where(x => x.EndNode).ToList();

            var reachedZZZ = false;
            //var currentNode = nodes.Where(x => x.Root == "AAA").First();

            var j = 0;
            long steps = 0;
            var currentNodes = nodes.Where(x => x.Root.EndsWith("A")).ToList();
            var lrInstructionsCount = lrInstructionsP2.Count();

            var nodeSteps = new List<long>();

            for (int i = 0; i < currentNodes.Count(); i++)
            {
                var currentNode = currentNodes[i];

                while (reachedZZZ == false)
                {
                    if (j == lrInstructionsCount)
                        j = 0;

                    var instruction = lrInstructionsP2[j];

                    if (instruction)
                        currentNode = currentNode.Left;
                    else
                        currentNode = currentNode.Right;

                    if (currentNode.EndNode)
                        reachedZZZ = true;

                    steps++;
                    j++;
                }

                nodeSteps.Add(steps);
                steps = 0;
                reachedZZZ = false;
            }
            

            Console.WriteLine($"Steps to reach zzz: {GetLCM(nodeSteps.ToArray())} ");
        }

        static long gcd(long n1, long n2)
        {
            if (n2 == 0)
            {
                return n1;
            }
            else
            {
                return gcd(n2, n1 % n2);
            }
        }

        public static long GetLCM(long[] numbers)
        {
            return numbers.Aggregate((S, val) => S * val / gcd(S, val));
        }

        private class Node
        {
            public string Root { get; set; }
            public string LeftText { get; set; }
            public Node Left { get; set; }
            public string RightText { get; set; }
            public Node Right { get; set; }
            public bool EndNode { get; set; }
        }
    }
}
