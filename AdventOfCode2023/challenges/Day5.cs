

using System.Collections.Generic;

namespace AdventOfCode2023.challenges
{
    public class Day5
    {
        public void Run()
        {
            string? filePath = @"C:\Users\KM\source\repos\AdventOfCode2023\inputs\day5Input.txt";

            ProcessFile(filePath);



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
                    lines.Add(line);
                }
            }

            var seedRangesList = lines[0].Replace("seeds: ", "").Split(' ').Select(x => long.Parse(x)).ToList();

            var seedRanges = new List<Range>();

            for (int i = 0; i < seedRangesList.Count; i += 2)
            {
                seedRanges.Add(new Range(seedRangesList[i], seedRangesList[i] + seedRangesList[i + 1]));
            }

            seedRanges.OrderBy(x => x.Start).ToList();

            var map1 = createMap(lines, 3, 49);
            var map2 = createMap(lines, 52, 83);
            var map3 = createMap(lines, 86, 95);
            var map4 = createMap(lines, 98, 123);
            var map5 = createMap(lines, 126, 151);
            var map6 = createMap(lines, 154, 193);
            var map7 = createMap(lines, 196, 236);



            long minLocation = 999999999999999;


            var result = MapPart2(map1, seedRanges);
            result = MapPart2(map2, result);
            result = MapPart2(map3, result);
            result = MapPart2(map4, result);
            result = MapPart2(map5, result);
            result = MapPart2(map6, result);
            result = MapPart2(map7, result);

            var resultMin = result.Min(x => x.Start);


            Console.WriteLine($"min location: {resultMin}");
        }

        private long Map(List<MapLine> mapLines, long src)
        {
            var mapLine = mapLines.Where(x => src >= x.SrcStart && src <= x.SrcStart + x.Range).FirstOrDefault();
            if (mapLine != null)
                return src + mapLine.SrcShift;

            return src;
        }


        private List<Range> MapPart2(List<MapLine> map, List<Range> srcRanges)
        {
            srcRanges = srcRanges.OrderBy(x => x.Start).ToList();

            var destRanges = new List<Range>();
            
            foreach(var srcRange in srcRanges)
            {
                destRanges.AddRange(MapPart2(map, srcRange));
            }

            return destRanges;
        }

        private List<Range> MapPart2(List<MapLine> map, Range srcRange)
        {
            var mapLines = map.Where(x => (srcRange.End >= x.SrcStart && srcRange.End <= x.SrcStart + x.Range) ||
                                            (srcRange.Start >= x.SrcStart && srcRange.Start <= x.SrcStart + x.Range)).OrderBy(x => x.SrcStart).ToList();
            if (mapLines.Count == 0)
                return new List<Range> { srcRange };


            var ranges = new List<Range>();

            for(var i = 0; i < mapLines.Count; i++)
            {
                if (i == 0)
                {
                    if (srcRange.Start < mapLines[i].SrcStart)
                        ranges.Add(new Range(srcRange.Start, mapLines[i].SrcStart - 1));
                }

                ranges.Add(new Range(srcRange.Start <= mapLines[i].SrcStart ? mapLines[i].SrcStart + mapLines[i].SrcShift : srcRange.Start + mapLines[i].SrcShift,
                                     srcRange.End >= mapLines[i].SrcStart + mapLines[i].Range ? mapLines[i].SrcStart + mapLines[i].Range + mapLines[i].SrcShift : srcRange.End + mapLines[i].SrcShift));

                if (i < mapLines.Count - 1)
                {
                    if (srcRange.End > mapLines[i].SrcStart + mapLines[i].Range && mapLines[i + 1].SrcStart > mapLines[i].SrcStart + mapLines[i].Range + 1)
                        ranges.Add(new Range(mapLines[i].SrcStart + mapLines[i].Range + 1,
                            srcRange.End >= mapLines[i + 1].SrcStart ? mapLines[i + 1].SrcStart - 1 : srcRange.End));
                }

                if (i == mapLines.Count - 1)
                {
                    if (srcRange.End > mapLines[i].SrcStart + mapLines[i].Range)
                        ranges.Add(new Range(mapLines[i].SrcStart + mapLines[i].Range + 1, srcRange.End));
                }
            }

            return ranges.OrderBy(x => x.Start).ToList();
        }

        private List<MapLine> createMap(List<string> lines, int firstLine, int lastLine)
        {
            var map = new List<MapLine>();
            for (int i = firstLine; i <= lastLine; i++)
            {
                var lineSplit = lines[i].Split(' ');
                map.Add(new MapLine { DestStart = long.Parse(lineSplit[0]), SrcStart = long.Parse(lineSplit[1]), Range = long.Parse(lineSplit[2]) });
            }

            map = map.OrderBy(x => x.SrcStart).ToList();

            return map;
        }

        private class MapLine
        {
            public long DestStart { get; set; }
            public long SrcStart { get; set; }
            public long Range { get; set; }
            public long SrcShift { get { return DestStart - SrcStart; }  }
        }

        private class Range
        {
            public Range(long start, long end)
            {
                Start = start;
                End = end;
            }

            public long Start { get; set; }
            public long End { get; set; }
        }
    }
}
