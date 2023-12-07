using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2023.challenges
{
    public class Day7
    {
        public void Run()
        {
            string? filePath = @"C:\Users\KM\source\repos\AdventOfCode2023\inputs\day7Input.txt";

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

            var hands = new List<Hand>();

            foreach (var line in lines)
            {
                var s = line.Split(' ');
                var cards = s[0].ToArray().Select(MapCard).ToList();
                var bid = int.Parse(s[1]);
                var hand = new Hand { Cards = cards, Bid = bid };
                hands.Add(hand);
            }

            hands = hands
                .OrderBy(x => x.HandTypeStrength)
                .ThenBy(x => x.Cards[0])
                .ThenBy(x => x.Cards[1])
                .ThenBy(x => x.Cards[2])
                .ThenBy(x => x.Cards[3])
                .ThenBy(x => x.Cards[4])
                .ToList();

            var hands7 = hands.Where(x => x.HandTypeStrength == 7);
            var hands6 = hands.Where(x => x.HandTypeStrength == 6);
            var hands5 = hands.Where(x => x.HandTypeStrength == 5);
            var hands4 = hands.Where(x => x.HandTypeStrength == 4);
            var hands3 = hands.Where(x => x.HandTypeStrength == 3);
            var hands2 = hands.Where(x => x.HandTypeStrength == 2);
 

            long winnings = 0;

            for (int i = 0; i < hands.Count; i++)
            {
                winnings += hands[i].Bid * (i+1);
            }

            Console.WriteLine($"winnings: {winnings}");
        }

        private int MapCard(char card) =>
            card switch
            {
                'T' => 10,
                'J' => 1,
                'Q' => 12,
                'K' => 13,
                'A' => 14,
                _ => int.Parse(card.ToString())
            };


        private class Hand
        {
            public List<int> Cards { get; set; }
            public int Bid { get; set; }

            public int HandTypeStrength { get
                {
                    var groups = Cards.Where(x => x != 1).GroupBy(x => x);
                    var jokersCount = Cards.Where(x => x == 1).Count();

                    if (jokersCount == 5 || groups.Any(x => x.Count() + jokersCount == 5))
                        return 7;
                    if (groups.Any(x => x.Count() + jokersCount == 4))
                        return 6;
                    if ((groups.Any(x => x.Count() == 3) && groups.Any(x => x.Count() == 2)) || 
                        (groups.Where(x => x.Count() == 2).Count() == 2 && jokersCount == 1))
                        return 5;
                    if (groups.Any(x => x.Count() + jokersCount == 3))
                        return 4;
                    if (groups.Where(x => x.Count() == 2).Count() == 2)
                        return 3;
                    if (groups.Any(x => x.Count() + jokersCount == 2))
                        return 2;
                    return 1;
                }
            }
        }
    }
}
