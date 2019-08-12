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

            Person p = new Person();


            int num = 100;

            for (int i = 0; i < num; i++)
            {
                p = Generator.NewGeneralPopulationMember();
                Console.WriteLine(p.ToString());
            }

            //using (TextWriter wrtr = new StreamWriter("ListOfPeople.txt"))
            //{
            //    for (int i = 0; i < num; i++)
            //    {
            //        p.isFemale = Generator.GenerateSex();
            //        p.firstName = Generator.GetaFirstName(p.isFemale);
            //        p.lastName = Generator.GetaLastName();
            //        p.dob = Generator.GetaDOB(rnd.Next(100));

            //        wrtr.WriteLine(p.ToString());
            //    }

            //}

            Console.WriteLine("Done");
            Console.ReadKey();


        }
    }

}
