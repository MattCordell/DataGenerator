using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Troschuetz.Random;

namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {
            var poiss = new Troschuetz.Random.Distributions.Discrete.PoissonDistribution(28d);

            for (int i = 0; i < 10000; i++)
            {
                Console.WriteLine(poiss.Next().ToString());
            }

            Console.ReadKey();
        }
    }
}
