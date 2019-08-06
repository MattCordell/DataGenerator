﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hl7.Fhir.Model;
using Hl7.Fhir.Rest;

namespace HealthConditionGenerator
{
    class Program
    {
        static void Main(string[] args)
        {
            var HG = new HealthConditionGenerator();

            Console.WriteLine("Done");

            
            

            Console.ReadKey();
        }
    }


    class HealthConditionGenerator
    {
        List<List<Int64>> ECLs = new List<List<Int64>>();
        Dictionary<int, List<double>> FemaleFrequencies = new Dictionary<int, List<double>>();
        Dictionary<int, List<double>> MaleFrequencies = new Dictionary<int, List<double>>();
        List<Int64> availableCodes = new List<long>();
        Random rng = new Random(42);
        const string Endpoint = "https://ontoserver.csiro.au/stu3-latest";


        public HealthConditionGenerator()
        {
            FemaleFrequencies = InitialiseFrequencies(Properties.Resources.HealthConditions_Female);
            MaleFrequencies = InitialiseFrequencies(Properties.Resources.HealthConditions_Male);

            //availableCodes = ECLexpandToList("^32570071000036102");

            //femaleExclusiveCodes = ECLexpandToList(string ecl);
            //maleExclusiveCodes = ECLexpandToList(string ecl);
            //fertilityCodes = ECLexpandToList(string ecl);

            ECLs = InitialiseHealthConditions(Properties.Resources.OrderedHealthConditions);


        }

        public List<long> ECLexpandToList(string v)
        {      
            //need to paginate :(
            const string ValueSetURL = "http://snomed.info/sct?fhir_vs=ecl/";
            var exp = string.Concat(ValueSetURL,v);


            var client = new FhirClient(Endpoint);
            var url = new FhirUri(exp);
            var result = client.ExpandValueSet(url);

            var q = result.Expansion.Contains.Select(x => x.Code);

            var codes = new List<long>();

            foreach (var item in q)
            {
                codes.Add(long.Parse(item));
            }

            return codes;
        }

        public string GetHealthCondition(int age, bool isfemale)
        {
            //Add some fertility conditions
            //Add sex exclusive filters

            if (isfemale)
            {
                var ageFrequencies = FemaleFrequencies.Where(i => i.Key >= age).FirstOrDefault().Value;
                // Below could be out of If, test performance for linq execution
                var freq = ageFrequencies.Where(x => x >= rng.NextDouble()).FirstOrDefault();
                var healthIndex = Array.IndexOf(ageFrequencies.ToArray(), freq);
                return ECLs[healthIndex][rng.Next(ECLs[healthIndex].Count)].ToString();

            }
            else
            {
                var ageFrequencies = MaleFrequencies.Where(i => i.Key >= age).FirstOrDefault().Value;
                // Below could be out of If, test performance for linq execution
                var freq = ageFrequencies.Where(x => x >= rng.NextDouble()).FirstOrDefault();
                var healthIndex = Array.IndexOf(ageFrequencies.ToArray(), freq);
                return ECLs[healthIndex][rng.Next(ECLs[healthIndex].Count)].ToString();
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
            var ECL = new List<List<Int64>>();
            var uber = new List<List<Int64>>();

            //interate and expand
            foreach (var line in input)
            {
                Console.WriteLine("Expanding " + line);

                uber.Add(ECLexpandToList(line));
                Console.WriteLine("Done Expanind");
                //Execute valueSet Expand on the ECL (standalone method)
                //Add each code to List<>
                //Exclude codes that have already been used (not available)
                //Remove remainder from "available codes"

            }

            return ECL;
        }

    }


}