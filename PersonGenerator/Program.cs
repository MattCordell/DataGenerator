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


namespace PersonGenerator
{
    class MyClass
    {
        static void Main(string[] args)
        {
            var Generator = new PersonGenerator(2018);
            Random rnd = new Random();

            Console.WriteLine("Start");

            Person p = new Person();

            
            int num = 1000;

            using (TextWriter wrtr = new StreamWriter("ListOfPeople.txt"))
            {
                for (int i = 0; i < num; i++)
                {
                    p.isFemale = Generator.GenerateSex();
                    p.firstName = Generator.GetaFirstName(p.isFemale);
                    p.lastName = Generator.GetaLastName();
                    p.dob = Generator.GetaDOB(rnd.Next(100));

                    wrtr.WriteLine(p.ToString());
                }

            }

            Console.WriteLine("Done");
            Console.ReadKey();


        }
    }

    class Person
    {
        public string firstName;
        public string lastName;
        public bool isFemale;
        public DateTime dob;

        public override string ToString()
        {
            return firstName + " " + lastName + " " + dob.ToString("d");
        }


    }


    class PersonGenerator
    {
        public bool GenerateSex()
        {
            int x = rnd.Next(2);

            if (x==0)
            {
                return true;
            }
            else
            {
                return false;
            }

        }

        #region Name Generation

        struct GenderedName { public string name ; public bool isFemale ;}

        private ArrayList firstNames = new ArrayList();
        private StringCollection lastNames = new StringCollection();
        private Random rnd = new Random();
        private int currentYear;

        public PersonGenerator(int currYear)
        {
            InitialiseFirstNames();
            InitialiseLastNames();
            currentYear = currYear;

        }

        private void InitialiseLastNames()
        {
            using (TextReader rdr = new StreamReader(@"Data\LastNames.txt"))
            {
                string name;

                while ((name = rdr.ReadLine()) != null)
                {
                    lastNames.Add(name);
                }
            }
        }

        private void InitialiseFirstNames()
        {
            GenderedName first = new GenderedName();
            
            // Read Female Names
            using (TextReader rdr = new StreamReader(@"Data\FirstFemaleNames.txt"))
            {
                first.isFemale = true;
                while ((first.name = rdr.ReadLine()) != null)
                {
                    firstNames.Add(first);
                }
            }

            // Read Male Names
            using (TextReader rdr = new StreamReader(@"Data\FirstMaleNames.txt"))
            {
                first.isFemale = false;
                while ((first.name = rdr.ReadLine()) != null)
                {
                    firstNames.Add(first);
                }
            }

        }

        public string GetaLastName()
        {
            int i = rnd.Next(lastNames.Count);
            return lastNames[i];
        }

        public string GetaFirstName(bool isfem)
        {
            var query = from GenderedName f in firstNames
                        where f.isFemale == isfem
                        select f;

            int i = rnd.Next(query.Count());
            return query.ToArray()[i].name;
        }

        #endregion

        #region birthdate generation     

        public DateTime GetaDOB(int age)
        {
            int birthYear = currentYear - age;
            int m = rnd.Next(1, 13);
            int d = rnd.Next(1, DateTime.DaysInMonth(birthYear, m));

            return new DateTime(birthYear, m, d);
        }
        
        #endregion


    }


}
