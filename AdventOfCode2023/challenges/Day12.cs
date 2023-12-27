using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AdventOfCode2023.challenges
{
    internal class Day12
    {
        public void Run()
        {
            string? filePath = @"C:\Users\KM\source\repos\AdventOfCode2023\inputs\day12Input.txt";

            ProcessFile(filePath);

            Console.WriteLine("Naciśnij dowolny klawisz, aby zakończyć...");
            Console.ReadKey();
        }

        void ProcessFile(string filePath)
        {
            var origLines = new List<string>();

            using (StreamReader reader = new StreamReader(filePath))
            {
                string? line;
                while ((line = reader.ReadLine()) != null)
                {
                    origLines.Add(line);
                }
            }

            //lines = lines.Take(300).ToList();

            long combinationsSum = 0;
            int processed = 0;

            object lockObject = new object();

            // new ParallelOptions { MaxDegreeOfParallelism = 12 },

            var start = DateTime.Now;

            foreach(var line in origLines) 
            {
                var currentStart = DateTime.Now;
                var record = line.Split(' ')[0];
                //record = record + '?' + record + '?' + record;
                record = record + '?' + record + '?' + record + '?' + record + '?' + record;
                var groups = line.Split(' ')[1].Split(',').Select(int.Parse).ToArray();

                var groupsList = groups.ToList();
                groupsList.AddRange(groups);
                groupsList.AddRange(groups);
                groupsList.AddRange(groups);
                groupsList.AddRange(groups);

                //var memory = new Dictionary<string, int>();

                var recordNums = new List<int>();

                for (var i = 0; i < record.Length; i++)
                {
                    recordNums.Add(record.Substring(i).Where(x => x == '#' || x == '?').Count());
                }

                var checkRecords = GetCheckRecord(record, true, groupsList.Select(x => (int)x).ToArray(), groupsList.Select(x => (int)x).ToArray().Sum(), recordNums.Select(x => (int)x).ToList());

                int result = 0;


                Parallel.ForEach(checkRecords, checkRecord2 =>
                {
                    var memoize = new Dictionary<(bool, int, int, int), int>();
                    var record2 = checkRecord2.Record;
                    var partialResult = checkRecord(ref record2, checkRecord2.IsHash, memoize, groupsList.ToArray(), groupsList.ToArray().Sum(x => x), recordNums, checkRecord2.CurrentI, checkRecord2.CurrentSize, checkRecord2.CurrentGroups, checkRecord2.CurrentGroupSum);

                    lock (lockObject)
                    {
                        result += partialResult;
                    }
                });

                combinationsSum += result;
                processed++;
                Console.WriteLine($"Processed: {processed} | overall: {(DateTime.Now - start)} | current: {(DateTime.Now - currentStart)}");
            }



            Console.WriteLine($"Combinations sum: {combinationsSum} ");
        }

        int checkRecord(ref string record, bool isHash, Dictionary<(bool, int, int, int), int> memoize, int[] groups, int groupsSum, List<int> recordNums, int currentI = 0, int currentSize = 0, List<int> currentGroups = null, int currentGroupsSum = 0)
        {
            //if (currentGroups == null)
            //    currentGroups = new List<int>();

            if (memoize.ContainsKey((isHash, currentI, currentGroupsSum, currentSize)))
            {
                return memoize[(isHash, currentI, currentGroupsSum, currentSize)];
            }


            var memCurrentGroupsSum = currentGroupsSum;
            var memCurrentSize = currentSize;

            if (isHash)
                currentSize++;
            else if (currentSize != 0)
            {
                currentGroups.Add(currentSize);
                currentGroupsSum += currentSize;
                currentSize = 0;

                if (currentGroups.Count > groups.Length || CompareGroups(currentGroups, groups) == false)
                {
                    return 0;
                }
            }


            for (int i = currentI + 1; i < record.Length; i++) 
            {
                if (record[i] == '.' && currentSize != 0)
                {
                    currentGroups.Add(currentSize);
                    currentGroupsSum += currentSize;
                    currentSize = 0;

                    if (currentGroups.Count > groups.Count() || CompareGroups(currentGroups, groups) == false)
                    {
                        return 0;
                    }
                }
                else if (record[i] == '#')
                    currentSize++;
                else if (record[i] == '?')
                {


                    var currentSum = currentGroupsSum + currentSize;
                    if (currentSum + recordNums[i] < groupsSum || currentSum > groupsSum)
                        return 0;

                    int answer = 0;
                    if (currentSum < groupsSum)
                        answer += checkRecord(ref record, true, memoize, groups, groupsSum, recordNums, i, currentSize, currentGroups.ToList(), currentGroupsSum);

                    answer += checkRecord(ref record, false, memoize, groups, groupsSum, recordNums, i, currentSize, currentGroups.ToList(), currentGroupsSum);

                    memoize.Add((isHash, currentI, memCurrentGroupsSum, memCurrentSize), answer);

                    return answer;
                }
            }

            if (currentSize != 0)
                currentGroups.Add(currentSize);

            var result = currentGroups.Count == groups.Length && CompareGroups(currentGroups, groups) ? 1 : 0;
            return result;
        }

        private class CheckRecordDto
        {
            public string Record { get; set; }
            public int CurrentI { get; set; }
            public int CurrentSize  { get; set; }
            public List<int> CurrentGroups { get; set; }
            public bool IsHash { get; set; }
            public int CurrentGroupSum { get; set; }
        }


        List<CheckRecordDto> GetCheckRecord(string record, bool isHash, int[] groups, int groupsSum, List<int> recordNums, int currentI = 0, int currentSize = 0, List<int> currentGroups = null, int currentGroupsSum = 0, int level = 0)
        {
            if (currentGroups == null)
                currentGroups = new List<int>();

            var dtos = new List<CheckRecordDto>();

            if (level == 5)
            {
                var dto = new CheckRecordDto { Record = record, CurrentI = currentI, CurrentSize = currentSize, CurrentGroups = currentGroups, IsHash = isHash, CurrentGroupSum = currentGroupsSum };
                dtos.Add(dto);

                return dtos;
            }


            for (int i = currentI; i < record.Length; i++)
            {
                if (record[i] == '.' && currentSize != 0)
                {
                    currentGroups.Add(currentSize);
                    currentGroupsSum += currentSize;
                    currentSize = 0;

                    if (currentGroups.Count() > groups.Count() || CompareGroups(currentGroups, groups) == false)
                    {
                        return dtos;
                    }
                }
                else if (record[i] == '#')
                    currentSize++;
                else if (record[i] == '?')
                {


                    var currentSum = currentGroups.Sum() + currentSize;
                    if (currentSum + recordNums[i] < groupsSum || currentSum > groupsSum)
                        return dtos;

                    var nextLevel = level + 1;

                    if (currentSum < groupsSum)
                        dtos.AddRange(GetCheckRecord(ReplaceChar(record, i, '#'), true, groups, groupsSum, recordNums, i, currentSize, currentGroups.ToList(), currentGroupsSum, nextLevel));

                    dtos.AddRange(GetCheckRecord(ReplaceChar(record, i, '.'), false, groups, groupsSum, recordNums, i, currentSize, currentGroups.ToList(), currentGroupsSum, nextLevel));

                    return dtos;
                }
            }
            return dtos;
        }



        private bool CompareGroups(List<int> currentGroups, int[] groups)
        {
            for(int i = 0; i < currentGroups.Count; i++)
            {
                if (currentGroups[i] != groups[i])
                    return false;
            }

            return true;
        }

        private string GetKey(int currentI, List<int> groups)
        {
            return currentI.ToString() + string.Join(',', groups);
        }

        private string ReplaceChar(string text, int index, char newChar) 
        {
            StringBuilder sb = new StringBuilder(text);
            sb[index] = newChar;
            return sb.ToString();
        }
    }
}
