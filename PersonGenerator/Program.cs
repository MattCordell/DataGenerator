using System;
using System.Collections.Generic;
using System.Linq;



namespace PersonGenerator
{
    class MyClass
    {
        static void Main(string[] args)
        {
            var Generator = new PersonGenerator(2019);
            

            Console.WriteLine("Start");


            var p = new Person();

            int num = 100;

            for (int i = 0; i < num; i++)
            {                
                p = Generator.NewGeneralPopulationMember();
                Console.WriteLine(p.ToString());
                
            }
            
            Console.WriteLine("Done");
            Console.ReadKey();


        }
    }

}
