using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2023.challenges
{
    internal class Day16
    {
        public void Run()
        {
            string? filePath = @"C:\Users\KM\source\repos\AdventOfCode2023\inputs\day16Input.txt";

            ProcessFile(filePath);

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
                    if (string.IsNullOrEmpty(line) == false)
                        lines.Add(line);
                }
            }

            var energizedTilesSum = 0;


            for (int i = 0; i < lines.Count; i++)
            {
                var beams = new List<Beam>
                {
                    new Beam(i, -1, Direction.Right)
                };

                var currentEnergizedTilesSum = GetEnergizedTilesSum(lines, beams);
                if (currentEnergizedTilesSum > energizedTilesSum) energizedTilesSum = currentEnergizedTilesSum;
            }

            for (int i = 0; i < lines.Count; i++)
            {
                var beams = new List<Beam>
                {
                    new Beam(i, lines[0].Length, Direction.Left)
                };

                var currentEnergizedTilesSum = GetEnergizedTilesSum(lines, beams);
                if (currentEnergizedTilesSum > energizedTilesSum) energizedTilesSum = currentEnergizedTilesSum;
            }

            for (int i = 0; i < lines[0].Length; i++)
            {
                var beams = new List<Beam>
                {
                    new Beam(-1, i, Direction.Down)
                };

                var currentEnergizedTilesSum = GetEnergizedTilesSum(lines, beams);
                if (currentEnergizedTilesSum > energizedTilesSum) energizedTilesSum = currentEnergizedTilesSum;
            }

            for (int i = 0; i < lines[0].Length; i++)
            {
                var beams = new List<Beam>
                {
                    new Beam(lines.Count, i, Direction.Up)
                };

                var currentEnergizedTilesSum = GetEnergizedTilesSum(lines, beams);
                if (currentEnergizedTilesSum > energizedTilesSum) energizedTilesSum = currentEnergizedTilesSum;
            }

            Console.WriteLine($"energized tiles: {energizedTilesSum}");
        }

        private int GetEnergizedTilesSum(List<string> lines, List<Beam> beams)
        {
            var energizedTiles = new bool[lines.Count, lines[0].Length];
            int energizedTilesSum;

            var bouncesCount = 0;
            while (true)
            {
                bouncesCount++;

                var beamsTemp = beams.ToList();
                foreach (var beam in beamsTemp)
                {

                    var nextPos = GetNextPos(beam);

                    if (nextPos.x < 0 || nextPos.y < 0 || nextPos.x > lines[0].Length - 1 || nextPos.y > lines.Count - 1)
                        continue;

                    var nextTile = lines[nextPos.y][nextPos.x];

                    beam.PosX = nextPos.x;
                    beam.PosY = nextPos.y;
                    energizedTiles[nextPos.y, nextPos.x] = true;

                    if (nextTile == '.')
                    {

                    }
                    else if (nextTile == '/')
                    {
                        if (beam.Direction == Direction.Up)
                            beam.Direction = Direction.Right;
                        else if (beam.Direction == Direction.Down)
                            beam.Direction = Direction.Left;
                        else if (beam.Direction == Direction.Left)
                            beam.Direction = Direction.Down;
                        else
                            beam.Direction = Direction.Up;
                    }
                    else if (nextTile == '\\')
                    {
                        if (beam.Direction == Direction.Up)
                            beam.Direction = Direction.Left;
                        else if (beam.Direction == Direction.Down)
                            beam.Direction = Direction.Right;
                        else if (beam.Direction == Direction.Left)
                            beam.Direction = Direction.Up;
                        else
                            beam.Direction = Direction.Down;
                    }
                    else if (nextTile == '-')
                    {
                        if (beam.Direction == Direction.Up || beam.Direction == Direction.Down)
                        {
                            beam.Direction = Direction.Left;

                            //if (beams.Any(x => x.PosY == nextPos.y && x.PosX == nextPos.x && x.Direction == Direction.Right) == false)
                            beams.Add(new Beam(nextPos.y, nextPos.x, Direction.Right));
                        }
                    }
                    else if (nextTile == '|')
                    {
                        if (beam.Direction == Direction.Left || beam.Direction == Direction.Right)
                        {
                            beam.Direction = Direction.Up;

                            //if (beams.Any(x => x.PosY == nextPos.y && x.PosX == nextPos.x && x.Direction == Direction.Down) == false)
                            beams.Add(new Beam(nextPos.y, nextPos.x, Direction.Down));
                        }
                    }
                    else
                        throw new NotSupportedException();

                }

                beams = beams.Distinct(new BeamComparer()).ToList();

                //count only 700 bounces
                if (bouncesCount > 700)
                    break;
            }

            energizedTilesSum = 0;
            foreach (var i in energizedTiles)
            {
                energizedTilesSum += i ? 1 : 0;
            }

            return energizedTilesSum;
        }

        private (int x, int y) GetNextPos(Beam beam)
        {
            if (beam.Direction == Direction.Up)
                return (beam.PosX, beam.PosY - 1);
            else if (beam.Direction == Direction.Down)
                return (beam.PosX, beam.PosY + 1);
            else if (beam.Direction == Direction.Left)
                return (beam.PosX - 1, beam.PosY);
            else
                return (beam.PosX + 1, beam.PosY);
        }

        private enum Direction
        {
            Up,
            Down,
            Right,
            Left
        }

        private class Beam
        {
            public Beam(int posY, int posX, Direction direction)
            {
                PosY = posY;
                PosX = posX;
                Direction = direction;
            }

            public int PosY { get; set; }
            public int PosX { get; set; }
            public Direction Direction { get; set; }
        }


        class BeamComparer : IEqualityComparer<Beam>
        {
            // Products are equal if their names and product numbers are equal.
            public bool Equals(Beam x, Beam y)
            {

                //Check whether the compared objects reference the same data.
                if (Object.ReferenceEquals(x, y)) return true;

                //Check whether any of the compared objects is null.
                if (Object.ReferenceEquals(x, null) || Object.ReferenceEquals(y, null))
                    return false;

                //Check whether the products' properties are equal.
                return x.PosY == y.PosY && x.PosX == y.PosX && x.Direction == y.Direction;
            }

            // If Equals() returns true for a pair of objects
            // then GetHashCode() must return the same value for these objects.

            public int GetHashCode(Beam beam)
            {
                //Check whether the object is null
                if (Object.ReferenceEquals(beam, null)) return 0;

                //Get hash code for the Name field if it is not null.
                int hashPosY = beam.PosY.GetHashCode();

                //Get hash code for the Code field.
                int hashPosX= beam.PosX.GetHashCode();
                int hashDirection = beam.Direction.GetHashCode();

                //Calculate the hash code for the product.
                return hashPosY ^ hashPosX ^ hashDirection;
            }
        }
    }
}
