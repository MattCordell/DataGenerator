using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FHIRhelper;

namespace MedGenerator
{
    class Program
    {
        static void Main(string[] args)
        {

            var meds = new MedGenerator();

            Console.WriteLine(meds.GetMedicationForCondition("103017008"));
            Console.WriteLine(meds.GetMedicationForCondition("103017008"));
            Console.WriteLine(meds.GetMedicationForCondition("103017008"));

            Console.WriteLine("Done");
            Console.ReadKey();
        }
    }

}
