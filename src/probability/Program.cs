// See https://aka.ms/new-console-template for more information
using MathNet.Numerics.Distributions;
using System.Linq;
using System.IO;

var rng = new Random(1);
List<D> active = new List<D>();

Dictionary<int, double> probs = new Dictionary<int, double>(365);

var categorical = new Categorical(new double[] {0.5,  // 1 month
                                                0.2,  // 2 months
                                                0.1,  // 3 months
                                                0.1,  // 4 months
                                                0.02, // 5 months
                                                0.02, // 6 months
                                                0.02, // 7 months
                                                0.02, // 8 months
                                                0.02, // 9 months
                                                0.02, // 10 months
                                                0.02, // 11 months
                                                0.01  // 12 months
                                                });

categorical.RandomSource = rng;

var creation = new Categorical(new double[] { 1, // 0
                                              70, // 1 - 5
                                              10,  // 6-10
                                              5   // 11-50
                                              });

creation.RandomSource = rng;

var curId = 0;

using (var fs = new FileStream("simulation_run.csv", FileMode.Create))
using (var file = new StreamWriter(fs))
{
    for (int curDay = 1; curDay <= 365; curDay++)
    {
        // Get number we are creating
        var numToCreateCategory = creation.Sample();
        var numToCreate = 0;
        switch (numToCreateCategory)
        {
            case 0:
                numToCreate = 0;
                break;
            case 1:
                numToCreate = rng.Next(1, 5);
                break;
            case 2:
                numToCreate = rng.Next(6, 10);
                break;
            case 3:
                numToCreate = rng.Next(11, 50);
                break;
            default:
                numToCreate = 0;
                break;
        }

        for (int id = 1; id <= numToCreate; id++)
        {
            var newD = new D() { Id = (curId + id) };
            var numMonths = categorical.Sample() + 1;
            var day = (numMonths - 1) * 30 + rng.Next(30);
            newD.LifeExpectancy = curDay + day;

            active.Add(newD);
        }

        // Mark dead those that have exceeded their life expectancy
        var numDying = active.Where(d => d.LifeExpectancy < curDay).Count();
        foreach (var deadD in active.Where(d => d.LifeExpectancy < curDay).ToList())
        {
            deadD.IsDead = true;
            active.Remove(deadD);
        }

        Console.WriteLine($"Day {curDay}: {numToCreate} created, {numDying} died, {active.Where(d => d.IsDead == false).Count()} active");
        //Console.WriteLine($"{curDay},{active.Where(d => d.IsDead == false).Count()}");
        file.WriteLine($"{curDay},{active.Where(d => d.IsDead == false).Count()}");
    }
}

// for (double i = 0.0; i <= 365.0; i++)
// {
//     var normal = Normal.CDF(14.0, 365.0, i);
//     var lognormal = LogNormal.CDF(0.0, 0.1, i);
//     //var probmass = new double[] {0.9, 0.5, 0.2, 0.01 };


//     probs.Add((int)i, normal);
//     Console.WriteLine($"Day {i}: {lognormal} - {normal} - {categorical.Sample()}");
// }

// for (int d = 0; d < 400; d++)
// {
//     // bool dead = false;
//     // int day = 0;
//     // while(dead == false && day <= 365)
//     // {
//     //     dead = rng.NextDouble() < probs[day];
//     //     day++;
//     // }

//     // Get months it's alive
//     var numMonths = categorical.Sample() + 1;

//     // Use rng to pick a random day in the range of months assuming 30 days a month
//     var day = (numMonths-1) * 30 + rng.NextInt64(30);

//     Console.WriteLine($"D-{d} is dead after {day} day(s)");
// }
