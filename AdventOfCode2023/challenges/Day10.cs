using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2023.challenges
{
    internal class Day10
    {
        public void Run()
        {
            string? filePath = @"C:\Users\KM\source\repos\AdventOfCode2023\inputs\day10Input.txt";

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

            Coord startingPos = null;

            Pipe currentPipe = null;

            for (int y = 0; y < lines.Count; y++)
            {
                for (int x = 0; x < lines[y].Length; x++)
                {
                    if (lines[y][x] == 'S')
                    {
                        startingPos = new Coord(x, y);
                        currentPipe = GetPipe(lines, x, y);
                        break;
                    }
                }
            }

            var loopSteps = 0;
            Pipe lastPipe = null;
            List<Pipe> pipes = new List<Pipe>();  
            pipes.Add(currentPipe);

            while (true)
            {
                Pipe nextPipe = null;

                if (currentPipe.Coord.X == startingPos.X && currentPipe.Coord.Y == startingPos.Y)
                {
                    var connectedPipes = new List<Pipe>();

                    for (int y = currentPipe.Coord.Y - 1; y <= currentPipe.Coord.Y + 1; y++)
                    {
                        for (int x = currentPipe.Coord.X - 1; x <= currentPipe.Coord.X + 1; x++)
                        {
                            if (x == currentPipe.Coord.X && y == currentPipe.Coord.Y)
                                continue;

                            if (x < 0 || x > lines[0].Length - 1 || y < 0 || y > lines.Count - 1)
                                continue;

                            if ((x == currentPipe.Coord.X - 1 && y == currentPipe.Coord.Y - 1) ||
                                (x == currentPipe.Coord.X - 1 && y == currentPipe.Coord.Y + 1) ||
                                (x == currentPipe.Coord.X + 1 && y == currentPipe.Coord.Y - 1) ||
                                (x == currentPipe.Coord.X + 1 && y == currentPipe.Coord.Y + 1))
                                continue;

                            var pipe = GetPipe(lines, x, y);

                            if (pipe != null && pipe.Connections.Any(x => x.X == currentPipe.Coord.X && x.Y == currentPipe.Coord.Y))
                                connectedPipes.Add(pipe);
                        }
                    }

                    nextPipe = connectedPipes.Where(x => x.Coord.X != lastPipe?.Coord.X || x.Coord.Y != lastPipe?.Coord.Y).First();
                }
                else
                {
                    var nextPipeCoord = currentPipe.Connections.Where(x => x.X != lastPipe?.Coord.X || x.Y != lastPipe?.Coord.Y).First();
                    nextPipe = GetPipe(lines, nextPipeCoord.X, nextPipeCoord.Y);
                }

                loopSteps++;
                lastPipe = currentPipe;
                currentPipe = nextPipe;
                pipes.Add(nextPipe);

                if (currentPipe.Coord.X == startingPos.X && currentPipe.Coord.Y == startingPos.Y)
                    break;
            }

            var insides = 0;
            for(var y = 0; y < lines.Count; y++)
            {
                for(var x = 0; x < lines[y].Length; x++)
                {
                    if (pipes.Any(p => p.Coord.X == x && p.Coord.Y == y))
                        continue;

                    var inside = false;

                    for (var x2 = x; x2 < lines[y].Length; x2++)
                    {
                        var loopPipe = pipes.Where(p => p.Coord.X == x2 && p.Coord.Y == y).FirstOrDefault();

                        if (loopPipe != null && loopPipe.HasUp)
                        {
                            inside = !inside;
                        }
                    }

                    if (inside)
                    {
                        insides++;
                    }
                }
            }

            Console.WriteLine($"loop steps: {loopSteps / 2}");
            Console.WriteLine($"inside elements count: {insides}");
        }

        private Pipe GetPipe(List<string> lines, int x, int y)
        {
            var symbol = lines[y][x];

            if (symbol == '.')
                return null;

            var coord = new Coord(x,y);
            var Connections = new List<Coord>();

            if (symbol == '|')
            {
                Connections.Add(new Coord(x, y-1));
                Connections.Add(new Coord(x, y+1));
            }

            if (symbol == '-')
            {
                Connections.Add(new Coord(x-1, y));
                Connections.Add(new Coord(x+1, y));
            }

            if (symbol == 'L')
            {
                Connections.Add(new Coord(x, y-1));
                Connections.Add(new Coord(x+1, y));
            }

            if (symbol == 'J')
            {
                Connections.Add(new Coord(x, y - 1));
                Connections.Add(new Coord(x-1, y));
            }

            if (symbol == '7')
            {
                Connections.Add(new Coord(x, y+1));
                Connections.Add(new Coord(x-1, y));
            }

            if (symbol == 'F')
            {
                Connections.Add(new Coord(x, y + 1));
                Connections.Add(new Coord(x + 1, y));
            }

            if (symbol == 'S')
            {
                Connections.Add(new Coord(x, y + 1));
                Connections.Add(new Coord(x, y - 1));
                Connections.Add(new Coord(x + 1, y));
                Connections.Add(new Coord(x - 1, y));
                Connections.Add(new Coord(x - 1, y -1));
                Connections.Add(new Coord(x - 1, y +1));
                Connections.Add(new Coord(x + 1, y -1));
                Connections.Add(new Coord(x + 1, y +1));
            }

            return new Pipe { Coord = coord, Connections = Connections, Symbol = symbol };
        }

        private class Pipe 
        {
            public char Symbol { get; set; }
            public Coord Coord { get; set; }
            public List<Coord> Connections { get; set; }
            public bool HasUp { get { return Symbol != 'S' && Connections != null && Connections.Any(x => x.Y < Coord.Y); } }
        }

        private class Coord
        {
            public Coord(int x, int y)
            {
                X = x;
                Y = y;
            }

            public int X { get; set; }
            public int Y { get; set; }
        }
    }
}
