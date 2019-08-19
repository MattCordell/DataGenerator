using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hl7.Fhir.Model;
using Hl7.Fhir.Rest;

namespace HealthConditionGenerator
{
    public class HealthConditionGenerator
    {
        List<List<Int64>> ECLs = new List<List<Int64>>();
        Dictionary<int, List<double>> FemaleFrequencies = new Dictionary<int, List<double>>();
        Dictionary<int, List<double>> MaleFrequencies = new Dictionary<int, List<double>>();
        List<Int64> availableCodes = new List<long>();
        Random rng = new Random(42);
        const string Endpoint = "https://ontoserver.csiro.au/stu3-latest";
        List<Int64> femaleExclusiveCodes;
        List<Int64> MaleExclusiveCodes;
        List<Int64> fertilityCodes;


        public HealthConditionGenerator()
        {
            FemaleFrequencies = InitialiseFrequencies(Properties.Resources.HealthConditions_Female);
            MaleFrequencies = InitialiseFrequencies(Properties.Resources.HealthConditions_Male);

            availableCodes = ECLexpandToList("^32570071000036102");

            femaleExclusiveCodes = ImportIdList(Properties.Resources.FemaleExclusiveConcepts);
            MaleExclusiveCodes = ImportIdList(Properties.Resources.MaleExclusiveConcepts);
            fertilityCodes = ImportIdList(Properties.Resources.FertilityConditions);

            ECLs = InitialiseHealthConditions(Properties.Resources.OrderedHealthConditions);
        }

        private List<long> ImportIdList(string resourceFile)
        {
            var l = new List<long>();

            var ids = resourceFile.Split(new string[] { Environment.NewLine }, StringSplitOptions.None);
            foreach (var id in ids)
            {
                l.Add(long.Parse(id));
            }
            return l;
        }

        public List<long> ECLexpandToList(string ecl)
        {
            const string TerminologyEndpoint = "https://ontoserver.csiro.au/stu3-latest";
            const string eclURL = "http://snomed.info/sct?fhir_vs=ecl/";
            var ValueSetURL = string.Concat(eclURL, ecl);
            var client = new FhirClient(TerminologyEndpoint);
            int pageSize = 25000;
            var codes = new List<long>();

            var parameters = new Parameters
            {
                Parameter = new List<Parameters.ParameterComponent>
                {
                new Parameters.ParameterComponent {Name = "url", Value = new FhirUri(ValueSetURL)},
                new Parameters.ParameterComponent {Name = "count", Value = new Integer(pageSize)},
                new Parameters.ParameterComponent {Name = "offset", Value = new Integer(0)}
                }
            };

            var result = (ValueSet)client.TypeOperation<ValueSet>("expand", parameters);

            // get each of the codes in the first page of results
            var q = result.Expansion.Contains.Select(x => x.Code);
            foreach (var item in q)
            {
                codes.Add(long.Parse(item));
            }

            //if the value set is more than 1 page, iterate through and get them all!
            for (int i = pageSize; i < result.Expansion.Total; i = i + pageSize)
            {
                parameters["offset"].Value = new Integer(i);
                result = (ValueSet)client.TypeOperation<ValueSet>("expand", parameters);
                q = result.Expansion.Contains.Select(x => x.Code);

                foreach (var item in q)
                {
                    codes.Add(long.Parse(item));
                }
            }

            Console.WriteLine("Fetched " + ecl + " : " + result.Expansion.Total);
            return codes;
        }

        public string GetHealthCondition(int age, bool isfemale)
        {
            var candidates = new List<long>();
            //roll dice first to try for a baby!
            if (FertilityCandidate(age,isfemale))
            {
                if (isfemale)
                {
                    candidates = fertilityCodes.Except(MaleExclusiveCodes).ToList();
                }
                else
                {
                    candidates = fertilityCodes.Except(femaleExclusiveCodes).ToList();
                }

                return candidates[rng.Next(candidates.Count)].ToString();

            }

            //otherwise...

            if (isfemale)
            {
                //get age specific frequencies
                var ageFrequencies = FemaleFrequencies.Where(i => i.Key >= age).FirstOrDefault().Value;
                // choose a random frequency
                var freq = ageFrequencies.Where(x => x >= rng.NextDouble()).FirstOrDefault();
                var healthIndex = Array.IndexOf(ageFrequencies.ToArray(), freq);

                candidates = ECLs[healthIndex].Except(MaleExclusiveCodes).ToList();

            }
            else
            {
                var ageFrequencies = MaleFrequencies.Where(i => i.Key >= age).FirstOrDefault().Value;
                // choose a random frequency
                var freq = ageFrequencies.Where(x => x >= rng.NextDouble()).FirstOrDefault();
                var healthIndex = Array.IndexOf(ageFrequencies.ToArray(), freq);

                candidates = ECLs[healthIndex].Except(MaleExclusiveCodes).ToList();
            }

            //age based exclusions
            if (age < 15 || age > 50)
            {
                candidates = candidates.Except(fertilityCodes).ToList();
            }

            return candidates[rng.Next(candidates.Count)].ToString();
        }

        //very basic/primitive fertility dice.
        //needs age basis
        private bool FertilityCandidate(int age, bool isfemale)
        {
            var fertilityRate = 0.15;

            if (isfemale && age > 14 && age < 50 && rng.NextDouble() < fertilityRate)
            {
                return true;
            }
            else if (!isfemale && age > 18 && age < 60 && rng.NextDouble() < fertilityRate)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        internal Dictionary<int, List<double>> InitialiseFrequencies(string frequencyFileResource)
        {
            var input = frequencyFileResource.Split(new string[] { Environment.NewLine }, StringSplitOptions.None);

            //temp List to hold frequencies. First value = age, remainder values.
            var frequencies = new List<List<string>>();

            //parse out HEADER
            var line = input[0];
            var header = line.Split('\t');
            input = input.Skip(1).ToArray();

            //Use Header to initialse the List Of Lists
            foreach (var item in header)
            {
                var f = new List<string>();
                f.Add(item);
                frequencies.Add(f);
            }


            //Subsequent lines
            //Add Frequency to appropriate age lists
            foreach (var row in input)
            {
                var values = row.Split('\t');
                for (int i = 0; i < values.Length; i++)
                {
                    frequencies[i].Add(values[i]);
                }
            }

            //now convert the frequency table into a dictionary
            //with Key=MaxAge, Values=OrderedList of cummulative Frequency
            var FrequencyDictionary = new Dictionary<int, List<double>>();

            foreach (var list in frequencies)
            {
                var k = int.Parse(list[0]);
                FrequencyDictionary.Add(k, new List<double>());

                foreach (var item2 in list.Skip(1))
                {
                    FrequencyDictionary[k].Add(double.Parse(item2));
                }
            }

            return FrequencyDictionary;
        }

        internal List<List<Int64>> InitialiseHealthConditions(string ECLfile)
        {
            var input = ECLfile.Split(new string[] { Environment.NewLine }, StringSplitOptions.None);
            var ecls = new List<List<Int64>>();

            //interate and expand
            foreach (var line in input)
            {
                Console.WriteLine("Expanding " + line);
                //Execute valueSet Expand on the ECL
                var temp = ECLexpandToList(line);
                var og = temp.Count;
                //Restrict to codes that haven't already been used
                temp = temp.Intersect(availableCodes).ToList();
                //Remove from "available codes"
                availableCodes = availableCodes.Except(temp).ToList();

                Console.WriteLine("ECL {0} size {1} / {2}", line, temp.Count, og);

                //just incase there's no codes left...
                if (temp.Count == 0) { temp.Add(213257006); } // Generally unwell (finding)

                ecls.Add(temp);
            }

            return ecls;
        }

    }
}
