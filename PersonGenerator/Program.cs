using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Specialized;
using System.IO;

namespace PersonGenerator
{
    class MyClass
    {
        static void Main(string[] args)
        {
            PersonGenerator pGen = new PersonGenerator();

            for (int i = 0; i < 10; i++)
            {                
                Console.WriteLine(pGen.GetaFirstName('F'));
            }

            Console.WriteLine("Done");
            Console.ReadKey();
        }
    }


    class PersonGenerator
    {

        struct FirstName { public string name ; public char sex ;}

        private ArrayList firstNames = new ArrayList();
        private StringCollection lastNames = new StringCollection();
        private Random rnd = new Random();

        public PersonGenerator()
        {
            InitialiseFirstNames();
            InitialiseLastNames();
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
            FirstName first = new FirstName();
            
            // Read Female Names
            using (TextReader rdr = new StreamReader(@"Data\FirstFemaleNames.txt"))
            {
                first.sex = 'F';
                while ((first.name = rdr.ReadLine()) != null)
                {
                    firstNames.Add(first);
                }
            }

            // Read Male Names
            using (TextReader rdr = new StreamReader(@"Data\FirstMaleNames.txt"))
            {
                first.sex = 'M';
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

        public string GetaFirstName(char sex)
        {
            var query = from FirstName f in firstNames
                        where f.sex == sex
                        select f;

            int i = rnd.Next(query.Count());
            return query.ToArray()[i].name;
        }
    }


}
