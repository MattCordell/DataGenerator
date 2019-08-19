using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedGenerator
{
    public class MedGenerator
    {
        public List<(HashSet<long> conditions, List<long> medicines)> MedConditions = new List<(HashSet<long> conditions, List<long> medicines)>();
        private Random rng = new Random();

        public MedGenerator()
        {
            //initialise MedConditions
            var fhir = new FHIRhelper.FHIRhelper();

            //ConditionMedECLs
            // first column is an ECL for a set of conditions.
            // Second column is an ECL for associated medications (AMT ingredients) according to ATC
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
        }

        public string GetMedicationForCondition(string _condition)
        {
            long condition = long.Parse(_condition);

            //shuffle the list to encourage variety, as condition hashes are not disjoint.
            MedConditions = MedConditions.OrderBy(x => rng.Next()).ToList();

            foreach (var item in MedConditions)
            {
                if (item.conditions.Contains(condition))
                {
                    return item.medicines.OrderBy(x => rng.Next()).FirstOrDefault().ToString();
                }
            }

            return null;
        }

    }
}
