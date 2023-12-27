using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2023.challenges
{
    internal class Day13
    {
        public void Run()
        {
            string? filePath = @"C:\Users\KM\source\repos\AdventOfCode2023\inputs\day13Input.txt";

            ProcessFile(filePath);

            Console.WriteLine("Naciśnij dowolny klawisz, aby zakończyć...");
            Console.ReadKey();
        }

        void ProcessFile(string filePath)
        {
            var lines = new List<string>();

            var currentBlock = new List<string>();
            var blocks = new List<List<string>>();

            using (StreamReader reader = new StreamReader(filePath))
            {
                string? line;
                while ((line = reader.ReadLine()) != null)
                {
                    
                    if (string.IsNullOrEmpty(line))
                    {
                        blocks.Add(currentBlock);
                        currentBlock = new List<string>();
                    }
                    else
                    {
                           currentBlock.Add(line);
                    }
                }
            }

            blocks.Add(currentBlock);
            currentBlock = new List<string>();

            var sum = 0;

            foreach (var block in blocks) 
            {
                var origHorizontalReflPos = -1;
                var origVerticalReflPos = -1;
                var newHorizontalReflPos = -1;
                var newVerticalReflPos = -1;

                for (var i = 0; i < block.Count - 1; i++) 
                {
                    if (CheckHorizontalRefl(block, i))
                        origHorizontalReflPos = i;
                }

                for (var i = 0; i < block[0].Length - 1; i++)
                {
                    if (CheckVerticalRefl(block, i))
                        origVerticalReflPos = i;
                }

                for (var i = 0; i < block.Count; i++)
                {
                    for (var j = 0; j < block[i].Length; j++)
                    {
                        var newBlock = block.ToList();
                        var oldChar = newBlock[i][j];
                        var newChar = oldChar == '#' ? '.' : '#';

                        newBlock[i] = ReplaceChar(newBlock[i], j, newChar);

                        for (var k = 0; k < newBlock.Count - 1; k++)
                        {
                            if (k == origHorizontalReflPos)
                                continue;

                            if (CheckHorizontalRefl(newBlock, k))
                                newHorizontalReflPos = k;
                        }

                        for (var k = 0; k < newBlock[0].Length - 1; k++)
                        {
                            if (k == origVerticalReflPos)
                                continue;

                            if (CheckVerticalRefl(newBlock, k))
                                newVerticalReflPos = k;
                        }

                        if (newHorizontalReflPos != -1 || newVerticalReflPos != -1)
                            break;
                    }
                    if (newHorizontalReflPos != -1 || newVerticalReflPos != -1)
                        break;
                }


                if (newHorizontalReflPos != -1)
                {
                    sum += (newHorizontalReflPos + 1) * 100;
                }

                if (newVerticalReflPos != -1)
                {
                    sum += newVerticalReflPos + 1;
                }
            }

            Console.WriteLine($"sum: {sum}");
        }

        private bool CheckHorizontalRefl(List<string> block, int pos)
        {
            var reflection = true;

            int offset = 0;
            while (true)
            {
                if (pos + 1 + offset >= block.Count)
                    break;

                if (pos - offset < 0)
                    break;

                if (block[pos - offset] != block[pos + 1 + offset])
                {
                    return false;
                }

                offset++;
            }

            return reflection;
        }

        private bool CheckVerticalRefl(List<string> block, int pos)
        {
            var reflection = true;

            int offset = 0;
            while (true)
            {
                if (pos + 1 + offset >= block[0].Length)
                    break;

                if (pos - offset < 0)
                    break;

                var line1 = new string(block.Select(x => x[pos - offset]).ToArray());
                var line2 = new string(block.Select(x => x[pos + 1 + offset]).ToArray());

                if (line1 != line2)
                {
                    return false;
                }

                offset++;
            }

            return reflection;
        }

        private string ReplaceChar(string text, int index, char newChar)
        {
            StringBuilder sb = new StringBuilder(text);
            sb[index] = newChar;
            return sb.ToString();
        }
    }
}
