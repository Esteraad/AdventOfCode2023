using AdventOfCode2023.enums;
using System.Text;

namespace AdventOfCode2023.challenges
{
    public class Day1
    {
        private readonly PartEnum part;

        public Day1(PartEnum part)
        {
            this.part = part;
        }

        public void Run()
        {
            string? filePath = @"C:\Users\KM\source\repos\AdventOfCode2023\inputs\day1Input.txt";

            ProcessFile(filePath);

            Console.WriteLine("Naciśnij dowolny klawisz, aby zakończyć...");
            Console.ReadKey();
        }

        void ProcessFile(string filePath)
        {
            int sumOfCalibrationValues = 0;
            using (StreamReader reader = new StreamReader(filePath))
            {
                string? line;
                    

                while ((line = reader.ReadLine()) != null)
                {
                    var firstDigit = "";
                    var lastDigit = "";
                    var pos = 1;

                    while(firstDigit == "")
                    {
                        var linePart = line.Substring(0, pos);
                        if (part == PartEnum.Day1Part2) linePart = ConvertTextNumbersToDigits(linePart);
                        linePart = ExtractDigits(linePart);
                        if (linePart != "") firstDigit = linePart;
                        pos++;
                    }

                    pos = 1;
                    while (lastDigit == "")
                    {
                        var linePart = line.Substring(line.Length - pos, pos);
                        if (part == PartEnum.Day1Part2) linePart = ConvertTextNumbersToDigits(linePart);
                        linePart = ExtractDigits(linePart);
                        if (linePart != "") lastDigit = linePart;
                        pos++;
                    }

                    var digitsOnly = ExtractDigits(line);
                    sumOfCalibrationValues += int.Parse($"{firstDigit}{lastDigit}");
                }
            }

            Console.WriteLine($"Calibration values sum: {sumOfCalibrationValues}");
        }

        string ExtractDigits(string input)
        {
            StringBuilder result = new StringBuilder();
            foreach (char character in input)
            {
                if (char.IsDigit(character))
                {
                    result.Append(character);
                }
            }

            return result.ToString();
        }

        static string ConvertTextNumbersToDigits(string input)
        {
            return input
                .Replace("one", "1")
                .Replace("two", "2")
                .Replace("four", "4")
                .Replace("three", "3")
                .Replace("five", "5")
                .Replace("six", "6")
                .Replace("seven", "7")
                .Replace("eight", "8")
                .Replace("nine", "9");
        }
    }
}
