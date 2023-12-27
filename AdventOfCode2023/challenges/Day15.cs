using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2023.challenges
{
    internal class Day15
    {
        public void Run()
        {
            string? filePath = @"C:\Users\KM\source\repos\AdventOfCode2023\inputs\day15Input.txt";

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
                while ((line = reader.ReadLine()) != null)
                {
                    if (string.IsNullOrEmpty(line) == false)
                        lines.Add(line);
                }
            }

            var steps = lines[0].Split(',').ToList();


            var sum = 0;


            var boxes = new List<Lens>[256];

            for (var i = 0; i < boxes.Length; i++)
            {
                boxes[i] = new List<Lens>();
            }


            foreach (var step in steps)
            {
                //p1
                var hash = GetHash(step);
                sum += hash;

                //p2
                if (step.Contains("="))
                {
                    var lens = new Lens(step.Split('=')[0], int.Parse(step.Split('=')[1]));
                    var hashP2 = GetHash(lens.Label);

                    var existingLens = boxes[hashP2].FirstOrDefault(x => x.Label == lens.Label);
                    
                    if (existingLens != null)
                        existingLens.FocalLen = lens.FocalLen;
                    else
                        boxes[hashP2].Add(lens);
                }

                if (step.Contains("-"))
                {
                    var lensLabel = step.Split('-')[0];
                    var hashP2 = GetHash(lensLabel);

                    var existingLens = boxes[hashP2].FirstOrDefault(x => x.Label == lensLabel);

                    if (existingLens != null)
                        boxes[hashP2].Remove(existingLens);
                }
            }

            //P2 
            var sumP2 = 0;
            for (var i = 0; i < boxes.Length; i++)
            {
                for (var j = 0; j < boxes[i].Count; j++)
                {
                    sumP2 += (i + 1) * (j + 1) * boxes[i][j].FocalLen;
                }
            }

            Console.WriteLine($"sum: {sum}");
            Console.WriteLine($"sumP2: {sumP2}");
        }

        private class Lens
        {
            public Lens(string label, int focalLen)
            {
                Label = label;
                FocalLen = focalLen;
            }

            public string Label { get; set; }
            public int FocalLen { get; set; }
        }


        private int GetHash(string step)
        {
            var asciiBytes = Encoding.ASCII.GetBytes(step);
            var currentValue = 0;

            foreach (var asciiByte in asciiBytes)
            {
                currentValue += asciiByte;
                currentValue *= 17;
                currentValue = currentValue % 256;
            }

            return currentValue;
        }
    }
}
