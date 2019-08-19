using System;
using System.Collections.Generic;
using System.Linq;
using Hl7.Fhir.Model;
using Hl7.Fhir.Rest;

namespace HealthConditionGenerator
{
    class Program
    {
        static void Main(string[] args)
        {
            var HG = new HealthConditionGenerator();

            var rnd = new Random();

            for (int i = 0; i < 200; i++)
            {
                Console.WriteLine(HG.GetHealthCondition(rnd.Next(120), true));
                Console.WriteLine(HG.GetHealthCondition(rnd.Next(120), false));
            }
            

            Console.WriteLine("Done");

            
            

            Console.ReadKey();
        }
    }





}