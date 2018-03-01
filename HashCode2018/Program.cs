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
        public int index;
        public Tuple<int, int> start;
        public Tuple<int, int> finish;
        public int earliest;
        public int latest;
        public bool busy = false;
        public bool done = false;

        public static Ride Parse(string line, int i)
        {
            var nums = line.Split(' ').Select(int.Parse).ToArray();

            return new Ride
            {
                index = i,
                start = Tuple.Create(nums[0], nums[1]),
                finish = Tuple.Create(nums[2], nums[3]),
                earliest = nums[4],
                latest = nums[5]
            };
        }
    }

    public class Car
    {
        public int index;
        public Ride ride = null;
        public Tuple<int, int> location = Tuple.Create(0, 0);
        public int distanceToStart;
        public int distanceToFinish;
        public List<int> ridesDone = new List<int>();
        public int score;
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

            var rides = lines
                .Skip(1)
                .Select((line, i) => Ride.Parse(line, i))
                .ToList();

            var cars = Enumerable
                .Range(0, vehicles)
                .Select(i => new Car { index = i })
                .ToList();

            RunSimulation(ClosestStrategy, rides, cars, bonus, steps);
        }

        static Ride FirstStrategy(List<Ride> rides, Car car, int time)
        {
            return rides.FirstOrDefault(r => !r.busy && !r.done);
        }

        static Ride ClosestStrategy(List<Ride> rides, Car car, int time)
        {
            // zo vaak mogelijk bonus : 
            // vind er een waar de earliest zo dicht mogelijk bij huidige time + afstand van car location naar ride start
            return rides
                .Where(r => !r.busy && !r.done)
                .Select(r => Tuple.Create(r.earliest - time - car.location.Manhattan(r.start), r))
                .OrderByDescending(t => t.Item1)
                .Select(t => t.Item2)
                .FirstOrDefault();
        }

        static void RunSimulation(Func<List<Ride>, Car, int, Ride> strategy, List<Ride> rides, List<Car> cars, int bonus, int steps)
        {
            for (int t = 0; t < steps; t++)
            {
                foreach (var car in cars)
                {
                    if (car.ride == null)
                    {
                        var ride = strategy(rides, car, t);
                        if (ride == null)
                        {
                            continue;
                        }
                        ride.busy = true;
                        car.ride = ride;
                        car.distanceToStart = car.location.Manhattan(ride.start);
                        car.distanceToFinish = ride.start.Manhattan(ride.finish);
                    }

                    if (car.distanceToStart > 0)
                    {
                        car.distanceToStart -= 1;
                    }
                    else if (t == car.ride.earliest)
                    {
                        car.score += bonus;
                        car.distanceToFinish -= 1;
                    }
                    else if (t > car.ride.earliest)
                    {
                        car.distanceToFinish -= 1;
                    }

                    if (car.distanceToFinish == 0)
                    {
                        car.location = car.ride.finish;

                        car.ridesDone.Add(car.ride.index);

                        car.score += t < car.ride.latest 
                            ? car.ride.start.Manhattan(car.ride.finish) 
                            : 0;

                        car.ride.done = true;
                        car.ride.busy = false;
                        car.ride = null;
                    }
                }
            }

            foreach (var car in cars)
            {
                Console.WriteLine($"{car.ridesDone.Count} {string.Join(" ", car.ridesDone)}");
            }
            Console.Error.WriteLine("SCORE: {0}", cars.Sum(c => c.score));
        }
    }
}
