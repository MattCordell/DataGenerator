using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Specialized;
using System.IO;
using MathNet.Numerics;
using MathNet.Numerics.Distributions;
using MathNet.Numerics.Random;
using System.Resources;
using System.Diagnostics;

namespace PersonGenerator
{
    class MyClass
    {
        static void Main(string[] args)
        {
            var Generator = new PersonGenerator(2018);
            Random rnd = new Random();
            Person p = new Person();

            Console.WriteLine("Start");
            
            int num = 1000;
            var watch = new Stopwatch();
            watch.Start();
            using (TextWriter wrtr = new StreamWriter("ListOfPeople.txt"))
            {
                for (int i = 0; i < num; i++)
                {
                    wrtr.WriteLine(Generator.NewGeneralPopulationMember().ToString());
                }

            }
            watch.Stop();

            Console.WriteLine(watch.ElapsedMilliseconds);

            Console.WriteLine("Done");
            Console.ReadKey();


        }
    }

    }
