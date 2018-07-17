using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PersonGenerator;
using Extensions;

namespace PersonGenerator
{
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
        private string[] maleNames = Properties.Resources.FirstMaleNames.Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
        private string[] femaleNames = Properties.Resources.FirstFemaleNames.Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
        private string[] lastNames = Properties.Resources.LastNames.Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
        private float[] ageDistribution = Array.ConvertAll(Properties.Resources.InitialAgeDistribution.Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries),float.Parse);
        internal static Random rnd = new Random();
        
        private int currentYear;

        public PersonGenerator(int currYear)
        {
            currentYear = currYear;
        }

        public string GetFirstName(bool isFemale)
        {
            if (isFemale)
            {
                return femaleNames.RandomElement(rnd);
            }
            else
            {
                return maleNames.RandomElement(rnd);
            }
        }

        public string GetLastName()
        {
            return lastNames.RandomElement(rnd);
        }
        
        public bool GenerateSex()
        {
            int x = rnd.Next(2);

            if (x == 0)
            {
                return true;
            }
            else
            {
                return false;
            }

        }

        public DateTime GetaDOB(int age)
        {
            int birthYear = currentYear - age;
            int m = rnd.Next(1, 13);
            int d = rnd.Next(1, DateTime.DaysInMonth(birthYear, m));

            return new DateTime(birthYear, m, d);
        }

        public int GetRandomAge()
        {
            var v = rnd.NextDouble();
            // select the min element greater than v
            //currently the index == age
            //cummulative age distribution allows normal randoms to select ages at correct rate.
            return Array.IndexOf(ageDistribution, ageDistribution.Where(g => g > v).Min());                       
        }

    }
}
