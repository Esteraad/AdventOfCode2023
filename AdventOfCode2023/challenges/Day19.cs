using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2023.challenges
{
    internal class Day19
    {
        public void Run()
        {
            string? filePath = @"C:\Users\KM\source\repos\AdventOfCode2023\inputs\day19Input.txt";

            ProcessFile(filePath);

            Console.WriteLine("Naciśnij dowolny klawisz, aby zakończyć...");
            Console.ReadKey();
        }

        void ProcessFile(string filePath)
        {
            var lines = new List<string>();

            using (StreamReader reader = new StreamReader(filePath))
            {
                string? line;
                var partsLines = false;

                var workflows = new List<Workflow>();
                var parts = new List<Part>();

                while ((line = reader.ReadLine()) != null)
                {
                    if (string.IsNullOrEmpty(line))
                        partsLines = true;

                    if (string.IsNullOrEmpty(line) == false)
                    {
                        if (partsLines == false)
                            workflows.Add(new Workflow(line.Split('{')[0], line.Split('{')[1].Replace("}", "")));
                        else
                            parts.Add(new Part(int.Parse(line.Split(',')[0].Split('=')[1]),
                                int.Parse(line.Split(',')[1].Split('=')[1]),
                                int.Parse(line.Split(',')[2].Split('=')[1]),
                                int.Parse(line.Split(',')[3].Split('=')[1])));
                    }
                        lines.Add(line);

                }


                var firstWorkflow = workflows.Where(x => x.Code == "in").First();
                foreach(var part in parts)
                {
                    var currentWorkflow = firstWorkflow;
                    while (true)
                    {
                        var instructions = currentWorkflow.Instruction.Split(',');

                    }
                }
            }

            

            Console.WriteLine($"energized tiles: ");
        }


        private class Workflow
        {
            public Workflow(string code, string instruction)
            {
                Code = code;
                Instruction = instruction;
            }

            public string Code { get; set; }
            public string Instruction { get; set; }
        }

        private class Part
        {
            public Part(int x, int m, int a, int s)
            {
                X = x;
                M = m;
                A = a;
                S = s;
            }

            public int X { get; set; }
            public int M { get; set; }
            public int A { get; set; }
            public int S { get; set; }
        }

    }
}
