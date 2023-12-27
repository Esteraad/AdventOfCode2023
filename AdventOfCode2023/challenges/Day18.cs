using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2023.challenges
{
    internal class Day18
    {
        public void Run()
        {
            string? filePath = @"C:\Users\KM\source\repos\AdventOfCode2023\inputs\day18Input.txt";

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

            var steps = new List<Step>();

            foreach (var line in lines)
            {
                var lineSplit = line.Split(' ');

                var direction = lineSplit[0][0] switch
                {
                    'R' => Direction.Right,
                    'L' => Direction.Left,
                    'U' => Direction.Up,
                    'D' => Direction.Down,
                    _ => throw new NotSupportedException()
                };

                steps.Add(new Step(int.Parse(lineSplit[1]), direction));
            }

            var DigLoop = new List<(int x, int y)>();
            var currentPos = (x: 0, y: 0);

            DigLoop.Add(currentPos);

            foreach (var step in steps)
            {
                if (step.Direction == Direction.Up)
                {
                    for(var i = currentPos.y - 1; i > currentPos.y - step.Distance - 1; i--)
                        DigLoop.Add((currentPos.x, i));

                    currentPos.y = currentPos.y - step.Distance;
                }

                if (step.Direction == Direction.Down)
                {
                    for (var i = currentPos.y + 1; i < currentPos.y + step.Distance + 1; i++)
                        DigLoop.Add((currentPos.x, i));

                    currentPos.y = currentPos.y + step.Distance;
                }

                if (step.Direction == Direction.Left)
                {
                    for (var i = currentPos.x - 1; i > currentPos.x - step.Distance - 1; i--)
                        DigLoop.Add((i, currentPos.y));

                    currentPos.x = currentPos.x - step.Distance;
                }

                if (step.Direction == Direction.Right)
                {
                    for (var i = currentPos.x + 1; i < currentPos.x + step.Distance + 1; i++)
                        DigLoop.Add((i, currentPos.y));

                    currentPos.x = currentPos.x + step.Distance;
                }
            }

            var interior = new List<(int x, int y)>();

            var minX = DigLoop.Min(x => x.x);
            var maxX = DigLoop.Max(x => x.x);
            var minY = DigLoop.Min(x => x.y);
            var maxY = DigLoop.Max(x => x.y);


            for (var y = minY; y <= maxY + 1; y++)
            {
                var addInterior = false;
                for (var x = minX; x <= maxX + 1; x++)
                {
                    if (DigLoop.Contains((x, y)) && DigLoop.Contains((x, y-1)))
                        addInterior = !addInterior;

                    if (addInterior && DigLoop.Contains((x, y)) == false)
                        interior.Add((x, y));
                }
            }


            Console.WriteLine($" {DigLoop.Count} {DigLoop.Distinct().ToList().Count} {interior.Count} {interior.Distinct().ToList().Count}");


            //for (var x = minX; x <= maxX; x++)
            //{
            //    var cLine = "";
            //    for (var y = minY; y <= maxY; y++)
            //    {
            //        if (DigLoop.Contains((x, y)) || interior.Contains((x, y)))
            //            cLine += "#";
            //        else
            //            cLine += ".";
            //    }

            //    Console.WriteLine(cLine);
            //}


            Console.WriteLine($"sum: {DigLoop.Distinct().ToList().Count + interior.Distinct().ToList().Count}");
        }


        private enum Direction
        {
            Up,
            Down,
            Right,
            Left
        }

        private class Step
        {
            public Step(int distance, Direction direction)
            {
                Distance = distance;
                Direction = direction;
            }

            public int Distance { get; set; }
            public Direction Direction { get; set; }
        }
    }
}
