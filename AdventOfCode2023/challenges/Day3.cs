using System.Drawing;
using System.Runtime.ExceptionServices;

namespace AdventOfCode2023.challenges
{
    public class Day3
    {
        public void Run()
        {
            string? filePath = @"C:\Users\KM\source\repos\AdventOfCode2023\inputs\day3Input.txt";

            ProcessFile(filePath);
            Console.WriteLine("Part2:");
            ProcessFilePart2(filePath);

            Console.WriteLine("Naciśnij dowolny klawisz, aby zakończyć...");
            Console.ReadKey();
        }

        #region Part1
        void ProcessFile(string filePath)
        {
            var lines = new List<string>();
            var numbersWithSurroundings = new List<NumberWithSurroundings>();

            using (StreamReader reader = new StreamReader(filePath))
            {
                string? line;
                while ((line = reader.ReadLine()) != null)
                {
                    lines.Add(line);
                }
            }

            for (var j = 0; j < lines.Count; j++)
            {
                var numberStartPos = 0;
                var numberEndPos = 0;

                var lastPosWasDigit = false;
                for (var i = 0; i < lines[j].Length; i++)
                {
                    if (lastPosWasDigit && (char.IsDigit(lines[j][i]) == false || i == lines[j].Length - 1))
                    {
                        numberEndPos = i == lines[j].Length - 1 && char.IsDigit(lines[j][i]) ? i : i - 1;
                        var numberWithSurroundings = GetNumberWithSurroundings(j, numberStartPos, numberEndPos, lines);
                        numbersWithSurroundings.Add(numberWithSurroundings);
                        lastPosWasDigit = false;
                    }
                    else if (lastPosWasDigit == false && char.IsDigit(lines[j][i]))
                    {
                        numberStartPos = i;
                        lastPosWasDigit = true;
                    }
                }
            }

            var sumOfPartNumbers = numbersWithSurroundings.Where(x => x.Surroundings.Any(y => y != '.')).Sum(x => x.Number);

            Console.WriteLine($"Part numbers sum: {sumOfPartNumbers}");
        }

        private NumberWithSurroundings GetNumberWithSurroundings(int lineNo, int numberStartPos, int numberEndPos, List<string> lines)
        {
            var surrStartPos = numberStartPos == 0 ? numberStartPos : numberStartPos - 1;
            var surrEndPos = numberEndPos == lines[lineNo].Length - 1 ? numberEndPos : numberEndPos + 1;

            var number = lines[lineNo].Substring(numberStartPos, numberEndPos - numberStartPos + 1);

            List<char> surroundings = new List<char>();

            if (lineNo != 0)
            {
                surroundings.AddRange(lines[lineNo - 1].Substring(surrStartPos, surrEndPos - surrStartPos + 1).ToCharArray());
            }

            if (surrStartPos != numberStartPos)
            {
                surroundings.Add(lines[lineNo].Substring(surrStartPos, 1).ToCharArray()[0]);
            }

            if (surrEndPos != numberEndPos)
            {
                surroundings.Add(lines[lineNo].Substring(surrEndPos, 1).ToCharArray()[0]);
            }

            if (lineNo != lines.Count - 1)
            {
                surroundings.AddRange(lines[lineNo + 1].Substring(surrStartPos, surrEndPos - surrStartPos + 1).ToCharArray());
            }

            return new NumberWithSurroundings { Number = int.Parse(number), Surroundings = surroundings };
        }

        private class NumberWithSurroundings
        {
            public int Number { get; set; }
            public List<char> Surroundings { get; set; }
        }
        #endregion

        #region Part2
        void ProcessFilePart2(string filePath)
        {
            var lines = new List<string>();
            var numbers = new List<Number>();

            using (StreamReader reader = new StreamReader(filePath))
            {
                string? line;
                while ((line = reader.ReadLine()) != null)
                {
                    lines.Add(line);
                }
            }

            for (var j = 0; j < lines.Count; j++)
            {
                var numberStartPos = 0;
                var numberEndPos = 0;

                var lastPosWasDigit = false;
                for (var i = 0; i < lines[j].Length; i++)
                {
                    if (lastPosWasDigit && (char.IsDigit(lines[j][i]) == false || i == lines[j].Length - 1))
                    {
                        numberEndPos = i == lines[j].Length - 1 && char.IsDigit(lines[j][i]) ? i : i - 1;
                        var number = GetNumberWithCoords(j, numberStartPos, numberEndPos, lines);
                        numbers.Add(number);
                        lastPosWasDigit = false;
                    }
                    else if (lastPosWasDigit == false && char.IsDigit(lines[j][i]))
                    {
                        numberStartPos = i;
                        lastPosWasDigit = true;
                    }
                }
            }

            var gears = new List<Gear>();

            // find gears
            for (var j = 0; j < lines.Count; j++)
            {
                for (var i = 0; i < lines[j].Length; i++)
                {
                    if (lines[j][i] == '*')
                    {
                        var gear = new Gear
                        {
                            Numbers = numbers.Where(x => x.Coords
                            .Any(y => y.Row >= j - 1 && y.Row <= j + 1 &&
                            y.Column >= i - 1 && y.Column <= i + 1)).ToList()
                        };
                        gears.Add(gear);
                    }
                }
            }

            var gearsWithOnly2Numbers = gears.Where(x => x.Numbers.Count == 2);

            var resultSum = gearsWithOnly2Numbers.Sum(x => x.Numbers[0].Value * x.Numbers[1].Value);

            Console.WriteLine($"Part 2 sum: {resultSum}");
        }

        private Number GetNumberWithCoords(int lineNo, int numberStartPos, int numberEndPos, List<string> lines)
        {
            var number = lines[lineNo].Substring(numberStartPos, numberEndPos - numberStartPos + 1);

            var coords = new List<Coord>();
            for (var i = numberStartPos; i <= numberEndPos; i++)
            {
                var coord = new Coord() { Row = lineNo, Column = i };
                coords.Add(coord);
            }

            return new Number() { Value = int.Parse(number), Coords = coords };
        }

        private class Number
        {
            public int Value { get; set; }
            public List<Coord> Coords { get; set; }
        }

        private class Coord
        {
            public int Row { get; set; }
            public int Column { get; set; }
        }

        private class Gear
        {
            public List<Number> Numbers { get; set; }
        }
        #endregion
    }
}