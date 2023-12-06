using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2023.challenges
{
    public class Day6
    {
        public void Run()
        {
            string? filePath = @"C:\Users\KM\source\repos\AdventOfCode2023\inputs\day6Input.txt";

            ProcessFile(filePath);
            Console.WriteLine("Part2");
            ProcessFilePart2(filePath);

            Console.WriteLine("Naciśnij dowolny klawisz, aby zakończyć...");
            Console.ReadKey();
        }

        void ProcessFile(string filePath)
        {
            var noWaysToWin = new List<int>();

            var races = new List<RaceData>()
            {
                new RaceData(57, 291),
                new RaceData(72, 1172),
                new RaceData(69, 1176),
                new RaceData(92, 2026),
            };

            foreach (var race in races)
            {
                var waysToWin = 0;
                for (var i = 1; i <= race.Time; i++)
                {
                    if ((race.Time - i) * i > race.Distance)
                        waysToWin++;
                }
                noWaysToWin.Add(waysToWin);
            }

            var waysToWinMultiply = noWaysToWin[0];
            for (var i = 1; i < noWaysToWin.Count; i++)
            {
                waysToWinMultiply *= noWaysToWin[i];
            }

            Console.WriteLine($"ways to win: {waysToWinMultiply}");
        }

        void ProcessFilePart2(string filePath)
        {
            var noWaysToWin = new List<int>();

            var race = new RaceData(57726992, 291117211762026);

            var waysToWin = 0;
            for (var i = 1; i <= race.Time; i++)
            {
                if ((race.Time - i) * i > race.Distance)
                    waysToWin++;
            }

            Console.WriteLine($"ways to win: {waysToWin}");
        }

        private class RaceData
        {
            public RaceData(long time, long distance)
            {
                Time = time;
                Distance = distance;
            }

            public long Time { get; set; }
            public long Distance { get; set; }
        }

    }
}
