using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace AdventOfCode2023.challenges
{
    internal class Day9
    {
        public void Run()
        {
            string? filePath = @"C:\Users\KM\source\repos\AdventOfCode2023\inputs\day9Input.txt";

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

            long sum = 0;
            long sumP2 = 0;
            foreach (var line in lines)
            {
                var values = line.Split(' ').Select(x => long.Parse(x)).ToList();

                var differences = new List<List<long>>();
                differences.Add(values);

                var currentValues = values;
                while (1 == 1)
                {
                    var nextValues = new List<long>();
                    for(int i = 1; i < currentValues.Count; i++)
                    {
                        nextValues.Add(currentValues[i] - currentValues[i - 1]);
                    }
                    currentValues = nextValues;
                    differences.Add(nextValues);
                    if (nextValues.All(x => x == 0)) break;
                }

                long currentValue = 0;
                for(int i = differences.Count - 1; i > 0; i--)
                {
                    currentValue = differences[i - 1][differences[i - 1].Count - 1] + currentValue;
                }

                sum += currentValue;

                long currentValueP2 = 0;
                for (int i = differences.Count - 1; i > 0; i--)
                {
                    currentValueP2 = differences[i - 1][0] - currentValueP2;
                }

                sumP2 += currentValueP2;
            }


            Console.WriteLine($"sum: {sum}");
            Console.WriteLine($"sumP2: {sumP2}");
        }
    }
}
