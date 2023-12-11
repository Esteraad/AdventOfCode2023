using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace AdventOfCode2023.challenges
{
    internal class Day11
    {
        public void Run()
        {
            string? filePath = @"C:\Users\KM\source\repos\AdventOfCode2023\inputs\day11Input.txt";

            ProcessFile(filePath);
            Console.WriteLine("Part2");
            //ProcessFilePart2(filePath);

            Console.WriteLine("Naciśnij dowolny klawisz, aby zakończyć...");
            Console.ReadKey();
        }

        void ProcessFile(string filePath)
        {
            var lines = new List<string>();
            var expandedLines = new List<string>();

            using (StreamReader reader = new StreamReader(filePath))
            {
                string? line;
                while ((line = reader.ReadLine()) != null)
                {
                    lines.Add(line);
                }
            }

            var rowstoDuplicate = new List<int>();
            var columnsToDuplicate = new List<int>();

            for (int i = 0; i < lines.Count; i++)
            {
                if (lines[i].All(x => x == '.'))
                    rowstoDuplicate.Add(i);
            }

            for (int i = 0; i < lines[0].Length; i++)
            {
                if(lines.All(x => x[i] == '.'))
                    columnsToDuplicate.Add(i);
            }

            for (int i = 0; i < lines.Count; i++) 
            {
                string line = "";
                for (int j = 0; j < lines[i].Length; j++)
                {
                    line += lines[i][j];
                    if (columnsToDuplicate.Contains(j))
                        line += '.';
                }
                expandedLines.Add(line);
                if (rowstoDuplicate.Contains(i))
                    expandedLines.Add(line);
            }

            var galaxies = new List<Coord>();

            for (int i = 0; i < expandedLines.Count; i++)
            {
                for (int j = 0; j < expandedLines[i].Length; j++)
                {
                    if (expandedLines[i][j] == '#')
                        galaxies.Add(new Coord(j, i));
                }
            }

            var galaxiesP2 = new List<Coord>();

            for (int i = 0; i < lines.Count; i++)
            {
                for (int j = 0; j < lines[i].Length; j++)
                {
                    if (lines[i][j] == '#')
                        galaxiesP2.Add(new Coord(j, i));
                }
            }

            var sumOfShortestDis = 0;
            long sumOfShortestDisP2 = 0;

            for (int i = 0; i < galaxies.Count - 1; i++)
            {
                for (int j = i+1; j < galaxies.Count; j++)
                {
                    var shortestDis = Math.Abs(galaxies[i].Y - galaxies[j].Y) + Math.Abs(galaxies[i].X - galaxies[j].X);
                    sumOfShortestDis += shortestDis;

                    // P2
                    long shortestDisP2 = Math.Abs(galaxiesP2[i].Y - galaxiesP2[j].Y) + Math.Abs(galaxiesP2[i].X - galaxiesP2[j].X);

                    //var rowsToDup = rowstoDuplicate.Where(x => x > int.Min(galaxiesP2[i].Y, galaxiesP2[j].Y) && x < int.Max(galaxiesP2[i].Y, galaxiesP2[j].Y)).ToList();
                    //var colsToDup = columnsToDuplicate.Where(x => x > int.Min(galaxiesP2[i].X, galaxiesP2[j].X) && x < int.Max(galaxiesP2[i].X, galaxiesP2[j].X)).ToList();

                    //if (rowsToDup.Any())
                    //    shortestDisP2 += rowsToDup.Count * 999999;

                    //if (colsToDup.Any())
                    //    shortestDisP2 += colsToDup.Count * 999999;

                    sumOfShortestDisP2 += shortestDisP2 + ((shortestDis - shortestDisP2) * 999999);
                }
            }


            Console.WriteLine($"Sum of shortest dis: {sumOfShortestDis} ");
            Console.WriteLine($"Sum of shortest dis P2: {sumOfShortestDisP2} ");
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
