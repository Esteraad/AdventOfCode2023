

using System.Collections.Generic;

namespace AdventOfCode2023.challenges
{
    public class Day4
    {
        public void Run()
        {
            string? filePath = @"C:\Users\KM\source\repos\AdventOfCode2023\inputs\day4Input.txt";

            ProcessFile(filePath);

            Console.WriteLine("Part 2:");

            ProcessFilePart2(filePath);

            Console.WriteLine("Naciśnij dowolny klawisz, aby zakończyć...");
            Console.ReadKey();
        }

        void ProcessFile(string filePath)
        {
            var lines = new List<string>();

            var cardPoints = 0;

            using (StreamReader reader = new StreamReader(filePath))
            {
                string? line;
                while ((line = reader.ReadLine()) != null)
                {
                    var winningNumbers = line.Split(':')[1].Split('|')[0].Split(' ').ToList();
                    winningNumbers.RemoveAll(x => string.IsNullOrEmpty(x));
                    var myNumbers = line.Split(':')[1].Split('|')[1].Split(' ').ToList();
                    myNumbers.RemoveAll(x => string.IsNullOrEmpty(x));

                    var matchingNumbersCount = myNumbers.Where(x => winningNumbers.Contains(x)).Count();

                    var points = 0;

                    if (matchingNumbersCount == 0)
                        points = 0;
                    else
                        points = (int)Math.Pow(2, matchingNumbersCount - 1);

                    cardPoints += points;
                }
            }


            Console.WriteLine($"Card points: {cardPoints}");
        }

        void ProcessFilePart2(string filePath)
        {
            var lines = new List<string>();
            var originalLines = new List<string>();

            using (StreamReader reader = new StreamReader(filePath))
            {
                string? line;
                while ((line = reader.ReadLine()) != null)
                {
                    lines.Add(line);
                }
            }

            originalLines = lines.ToList();

            var cardsCount = lines.Count;

            while (lines.Count > 0)
            {
                var linesCopies = new List<string>();

                for (var x = 0; x < lines.Count; x++)
                {
                    var cardNo = GetCardNumber(lines[x]);
                    var origCardPos = cardNo - 1;
                    var matchingNumbers = GetCardMatchingNumbers(lines[x]);
                    if (origCardPos == originalLines.Count - 1 || matchingNumbers == 0)
                        continue;

                    var firstCardToCopy = origCardPos + 1;
                    var lastCardToCopy = origCardPos + matchingNumbers > originalLines.Count - 1 ? originalLines.Count - 1 : origCardPos + matchingNumbers;

                    linesCopies.AddRange(originalLines.GetRange(firstCardToCopy, lastCardToCopy - firstCardToCopy + 1));
                }

                cardsCount += linesCopies.Count;
                lines = linesCopies;
            }

            Console.WriteLine($"Cards count: {cardsCount}");
        }

        private int GetCardNumber(string line)
        {
            var cardNo = line.Split(':')[0].Replace("Card", "").Replace(" ", "");
            return int.Parse(cardNo);
        }

        private int GetCardMatchingNumbers(string line)
        {
            var winningNumbers = line.Split(':')[1].Split('|')[0].Split(' ').ToList();
            winningNumbers.RemoveAll(x => string.IsNullOrEmpty(x));
            var myNumbers = line.Split(':')[1].Split('|')[1].Split(' ').ToList();
            myNumbers.RemoveAll(x => string.IsNullOrEmpty(x));

            var matchingNumbersCount = myNumbers.Where(x => winningNumbers.Contains(x)).Count();

            return matchingNumbersCount;
        }
    }
}
