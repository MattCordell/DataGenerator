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
            //initialise MedConditions
            var MedConditions = new List<(HashSet<long> conditions, List<long> medicines)>();

            var fhir = new FHIRhelper.FHIRhelper();

            string[] ConditionMedECLs = Properties.Resources.Condition_Substance_ECL_Map.Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
            foreach (var item in ConditionMedECLs)
            {
                var x = item.Split('\t');
                var ingredients = x[1];

                var TPUU = $"<<30425011000036101:ANY=({ingredients})";
                var TPUUswithMods = $"<<30425011000036101:ANY=((<30388011000036105:30394011000036104=({ingredients}))OR({ingredients}))";

                var bucket = fhir.ECLexpandToHash(x[0]);
                var TPUUs = fhir.ECLexpandToList(TPUU);

                MedConditions.Add((bucket, TPUUs));
            }

            Console.WriteLine("Done");
            Console.ReadKey();

        }
    }

}
