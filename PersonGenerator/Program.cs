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

            using (TextWriter wrtr = new StreamWriter("ListOfPeople.txt"))
            {
                for (int i = 0; i < num; i++)
                {
                    p.isFemale = Generator.GenerateSex();
                    p.firstName = Generator.GetFirstName(p.isFemale);
                    p.lastName = Generator.GetLastName();
                    p.dob = Generator.GetaDOB(rnd.Next(100));

                    wrtr.WriteLine(Generator.GetRandomAge());
                }

            }


            Console.WriteLine(Generator.GetRandomAge().ToString());

            Console.WriteLine("Done");
            Console.ReadKey();


        }
    }

    }
