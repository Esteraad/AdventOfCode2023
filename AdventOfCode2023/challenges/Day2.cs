
using AdventOfCode2023.enums;
using System.Numerics;

namespace AdventOfCode2023.challenges
{
    public class Day2
    {
        private readonly PartEnum part;

        public Day2 (PartEnum part)
        {
            this.part = part;
        }

        public void Run()
        {
            string? filePath = @"C:\Users\KM\source\repos\AdventOfCode2023\inputs\day2Input.txt";

            ProcessFile(filePath);

            Console.WriteLine("Naciśnij dowolny klawisz, aby zakończyć...");
            Console.ReadKey();
        }

        void ProcessFile(string filePath)
        {

            using (StreamReader reader = new StreamReader(filePath))
            {
                    
                var games = new List<Game>();
                string? line;
                while ((line = reader.ReadLine()) != null)
                {
                    if (line == "")
                        continue;

                    line = line.Replace(" ", "");
                    var gameId = int.Parse(line.Split(':')[0].Replace("Game", ""));
                    var sets = line.Split(':')[1].Split(';');

                    var setsOfCubes = new List<SetOfCubes>();

                    foreach ( var set in sets) 
                    { 
                        var setOfCubes = new SetOfCubes();
                        var colors = set.Split(',');
                        foreach (var color in colors)
                        {
                            if (color.Contains("blue"))
                                setOfCubes.BlueCount = int.Parse(color.Replace("blue", ""));
                            if (color.Contains("red"))
                                setOfCubes.RedCount = int.Parse(color.Replace("red", ""));
                            if (color.Contains("green"))
                                setOfCubes.GreenCount = int.Parse(color.Replace("green", ""));
                        }
                        setsOfCubes.Add(setOfCubes);
                    }

                    var game = new Game() { GameId = gameId, SetsOfCubes = setsOfCubes };
                    games.Add(game);
                }

                var possibleGameIdsSum = 0;

                // Part1
                foreach(var game in games)
                {
                    if (game.SetsOfCubes.Any(x => x.RedCount > 12) == false &&
                        game.SetsOfCubes.Any(x => x.GreenCount > 13) == false &&
                        game.SetsOfCubes.Any(x => x.BlueCount > 14) == false)
                        possibleGameIdsSum += game.GameId;
                }
                Console.WriteLine($"Possible game Ids sum: {possibleGameIdsSum}");

                // Part2
                var cubesPower = 0;

                foreach (var game in games)
                {
                    cubesPower += game.SetsOfCubes.Max(x => x.GreenCount) *
                        game.SetsOfCubes.Max(x => x.RedCount) *
                        game.SetsOfCubes.Max(x => x.BlueCount);
                }

                Console.WriteLine($"Cubes power sum: {cubesPower}");
            }

        }

        private class SetOfCubes
        {
            public int BlueCount { get; set; } = 0;
            public int RedCount { get; set; } = 0;
            public int GreenCount { get; set; } = 0;
        }

        private class Game
        {
            public int GameId { get; set; }
            public List<SetOfCubes> SetsOfCubes { get; set; }
        }
    }
}
