using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HashCode2018
{
    public static class TupleExtensions
    {
        public static int Manhattan(this Tuple<int, int> a, Tuple<int, int> b)
        {
            return Math.Abs(a.Item1 - b.Item1) + Math.Abs(a.Item2 - b.Item2);
        }
    }

    public class Ride
    {
        public Tuple<int, int> start;
        public Tuple<int, int> finish;
        public int earliest;
        public int latest;

        public static Ride Parse(string line)
        {
            var nums = line.Split(' ').Select(int.Parse).ToArray();
            return new Ride
            {
                start = Tuple.Create(nums[0], nums[1]),
                finish = Tuple.Create(nums[2], nums[3]),
                earliest = nums[4],
                latest = nums[5]
            };
        }
    }

    class Program
    {
        static void ParseFirst(string line, out int rows, out int cols, out int vehicles, out int rides, out int bonus, out int steps)
        {
            var coords = line.Split(' ').Select(int.Parse).ToArray();
            rows = coords[0];
            cols = coords[1];
            vehicles = coords[2];
            rides = coords[3];
            bonus = coords[4];
            steps = coords[5];
        }

        static void Main(string[] args)
        {
            var lines = File.ReadAllLines(args[0]);

            ParseFirst(lines[0], out var rows, out var cols, out var vehicles, out var rideCount, out var bonus, out var steps);

            var rides = lines.Skip(1).Select(Ride.Parse).ToList();
        }
    }
}
