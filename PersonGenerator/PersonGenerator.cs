using System;
using System.Collections.Generic;
using System.Linq;
using Extensions;

namespace PersonGenerator
{
    public class Person
    {
        public string firstName;
        public string lastName;
        public char Sex;        
        public DateTime dob;

        public bool isFemale
        {
            get
            {
                if (Sex == 'F')
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        //Calculate Basic Age in years at specific date
        public int Age(DateTime relativeTime)
        {
            var date = int.Parse(relativeTime.ToString("yyyyMMdd"));
            var birthDate = int.Parse(this.dob.ToString("yyyyMMdd"));

            return (date - birthDate) / 10000;
        }

        //Calculate Basic Age in years as of NOW!
        public int Age()
        {            
            var now = int.Parse(DateTime.UtcNow.ToString("yyyyMMdd"));
            var birthDate = int.Parse(this.dob.ToString("yyyyMMdd"));

            return (now - birthDate) / 10000;
        }

        public override string ToString()
        {
            return firstName + "\t" + lastName + "\t" + Sex + "\t" + dob.ToString("d");
        }
        
    }

    public class PersonGenerator
    {
        private string[] maleNames = Properties.Resources.FirstMaleNames.Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
        private string[] femaleNames = Properties.Resources.FirstFemaleNames.Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
        //private string[] lastNames = Properties.Resources.LastNames.Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
        private string[] lastNames = Properties.Resources.LastNames.Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
        private double[] maleAgeSpread = ImportFrequencyList(Properties.Resources.MaleAgeSpread);
        private double[] femaleAgeSpread = ImportFrequencyList(Properties.Resources.FemaleAgeSpread);
        
        private static double[] ImportFrequencyList(string frequencyList)
        {          
            var fList = new List<double>();

            foreach (var item in frequencyList.Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries))
            {
                fList.Add(double.Parse(item));
            }

            return fList.ToArray();
        }              

        internal static Random rng = new Random();
        private int currentYear;
        internal float maleRatio = 0.48f;


    //object for generating new humans :D
    Person human = new Person();

        public Person NewGeneralPopulationMember()
        {            
            human.Sex = GenerateSex();
            human.dob = GetaDOB(human.isFemale);            
            human.firstName = GetFirstName(human.isFemale);
            human.lastName = GetLastName();

            return human;
        }

        public PersonGenerator(int currYear)
        {
            currentYear = currYear;
        }

        public string GetFirstName(bool isFemale)
        {
            if (isFemale)
            {
                return femaleNames.RandomElement(rng);
            }
            else
            {
                return maleNames.RandomElement(rng);
            }
        }

        public string GetLastName()
        {
            return lastNames.RandomElement(rng);
        }

        public char GenerateSex()
        {
            if (rng.NextDouble() > maleRatio)
            {
                return 'F';
            }
            else
            {
                return 'M';
            }
        }

        public DateTime GetaDOB(bool isfemale)
        {
            int age;
            var rnd = rng.NextDouble();
            //get a age appropriate for sex
            if (isfemale)
            {
                
                age = Array.IndexOf(femaleAgeSpread, femaleAgeSpread.Where(x => x > rnd).First());
            }
            else
            {
                age = Array.IndexOf(maleAgeSpread, maleAgeSpread.Where(x => x > rnd).First());
            }
            //special randomising for 100+
            if (age==100)
            {
                age = rng.Next(100, 112);
            }
            

            int birthYear = currentYear - age;
            int m = rng.Next(1, 13);
            int d = rng.Next(1, DateTime.DaysInMonth(birthYear, m));

            return new DateTime(birthYear, m, d);
        }

    }
}
