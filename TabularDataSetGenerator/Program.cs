using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using PersonGenerator;
using HealthConditionGenerator;




namespace TabularDataSetGenerator
{
    class Program
    {
        static void Main(string[] args)
        {
            var datasetSize = 50;
            var pGenerator = new PersonGenerator.PersonGenerator(2019);
            var patient = new Person();
            string condition;
            string medication = null;
            var cGenerator = new HealthConditionGenerator.HealthConditionGenerator();
            StringBuilder dataRow = new StringBuilder();

            Console.WriteLine("Start");

            for (int i = 0; i < datasetSize; i++)
            {
                patient = pGenerator.NewGeneralPopulationMember();
                condition = cGenerator.GetHealthCondition(patient.Age(), patient.isFemale);
                dataRow.Append(patient.ToString());
                dataRow.Append("    ");
                dataRow.Append(condition);
                dataRow.Append("    ");
                dataRow.Append(medication);
                Console.WriteLine(dataRow);
                dataRow.Clear();
            }




            //using (TextWriter wrtr = new StreamWriter("TabularDataSetGenerator.txt"))
            //{
            //    for (int i = 0; i < datasetSize; i++)
            //    {
            //        patient = pGenerator.NewGeneralPopulationMember();
            //        cGenerator.GetHealthCondition(patient.Age(DateTime.Now()), patient.isFemale);


            //        wrtr.WriteLine(patient.ToString());
            //    }

            //}


            Console.WriteLine("Done");
            Console.ReadKey();


        }
    }
}
