using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using PersonGenerator;
using HealthConditionGenerator;
using MedGenerator;




namespace TabularDataSetGenerator
{
    class Program
    {
        static void Main(string[] args)
        {
            var datasetSize = 1000000;

            //10K = 439KB
            //200K = 8.7MB
            var pGenerator = new PersonGenerator.PersonGenerator(2019);
            var patient = new Person();
            string condition;
            string medication = null;
            var cGenerator = new HealthConditionGenerator.HealthConditionGenerator();
            var medGenerator = new MedGenerator.MedGenerator();
            StringBuilder dataRow = new StringBuilder();

            Console.WriteLine("Start");

            using (TextWriter wrtr = new StreamWriter("TabularDataSetGenerator.txt"))
            {
                for (int i = 0; i < datasetSize; i++)
                {
                    patient = pGenerator.NewGeneralPopulationMember();
                    condition = cGenerator.GetHealthCondition(patient.Age(), patient.isFemale);
                    medication = medGenerator.GetMedicationForCondition(condition);

                    dataRow.Append(patient.ToString());
                    dataRow.Append("\t");
                    dataRow.Append(condition);
                    dataRow.Append("\t");                    
                    dataRow.Append(medication);
                    Console.WriteLine(dataRow);
                    wrtr.WriteLine(dataRow);
                    dataRow.Clear();
                }
            }


            Console.WriteLine("Done");
            Console.ReadKey();


        }
    }
}
