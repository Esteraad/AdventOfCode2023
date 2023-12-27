using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2023.challenges
{
    internal class Day14
    {
        public void Run()
        {
            string? filePath = @"C:\Users\KM\source\repos\AdventOfCode2023\inputs\day14Input.txt";

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

            var loads = new List<int>();

            for (int k = 0; k < 1000000000; k++)
            {
                TiltNorth(lines);
                TiltWest(lines);
                TiltSouth(lines);
                TiltEast(lines);

                var load = 0;

                for (int i = 0; i < lines.Count; i++)
                {
                    for (int j = 0; j < lines[i].Length; j++)
                    {
                        if (lines[i][j] == 'O')
                        {
                            load += lines[i].Length - i;
                        }
                    }
                }

                // loop: 113 - 125
                loads.Add(load);
                if (k == 125)
                    break;
            }

            var loadsLoop = loads.Skip(113).ToList();

            var loadIndex =  ((1000000000 - 113) % loadsLoop.Count) - 1;

            Console.WriteLine($"totalLoad: {loadsLoop[loadIndex]}");
        }


        private void TiltNorth(List<string> lines)
        {
            for (int i = 0; i < lines.Count; i++)
            {
                for (int j = 0; j < lines[i].Length; j++)
                {
                    if (lines[i][j] == 'O')
                    {
                        var pos = i;
                        while (true)
                        {
                            if (pos > 0 && lines[pos - 1][j] == '.')
                                pos--;
                            else
                            {
                                if (i != pos)
                                {
                                    lines[i] = ReplaceChar(lines[i], j, '.');
                                    lines[pos] = ReplaceChar(lines[pos], j, 'O');
                                }
                                break;
                            }

                        }
                    }
                }
            }
        }

        private void TiltSouth(List<string> lines)
        {
            for (int i = lines.Count - 1; i >= 0; i--)
            {
                for (int j = 0; j < lines[i].Length; j++)
                {
                    if (lines[i][j] == 'O')
                    {
                        var pos = i;
                        while (true)
                        {
                            if (pos < lines.Count - 1 && lines[pos + 1][j] == '.')
                                pos++;
                            else
                            {
                                if(i != pos)
                                {
                                    lines[i] = ReplaceChar(lines[i], j, '.');
                                    lines[pos] = ReplaceChar(lines[pos], j, 'O');
                                }
                                break;
                            }

                        }
                    }
                }
            }
        }

        private void TiltWest(List<string> lines)
        {
            for (int i = 0; i < lines.Count; i++)
            {
                for (int j = 0; j < lines[i].Length; j++)
                {
                    if (lines[i][j] == 'O')
                    {
                        var pos = j;
                        while (true)
                        {
                            if (pos > 0 && lines[i][pos - 1] == '.')
                                pos--;
                            else
                            {
                                if(j != pos)
                                {
                                    lines[i] = ReplaceChar(lines[i], j, '.');
                                    lines[i] = ReplaceChar(lines[i], pos, 'O');
                                }
                                
                                break;
                            }

                        }
                    }
                }
            }
        }

        private void TiltEast(List<string> lines)
        {
            for (int i = 0; i < lines.Count; i++)
            {
                for (int j = lines[i].Length - 1; j >= 0; j--)
                {
                    if (lines[i][j] == 'O')
                    {
                        var pos = j;
                        while (true)
                        {
                            if (pos < lines[i].Length - 1 && lines[i][pos + 1] == '.')
                                pos++;
                            else
                            {
                                if(j != pos)
                                {
                                    lines[i] = ReplaceChar(lines[i], j, '.');
                                    lines[i] = ReplaceChar(lines[i], pos, 'O');
                                }
                                
                                break;
                            }

                        }
                    }
                }
            }
        }

        private string ReplaceChar(string text, int index, char newChar)
        {
            StringBuilder sb = new StringBuilder(text);
            sb[index] = newChar;
            return sb.ToString();
        }
    }
}
