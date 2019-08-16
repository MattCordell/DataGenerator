using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hl7.Fhir.Rest;
using Hl7.Fhir.Model;

namespace FHIRhelper
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Helper class");

            Console.ReadKey();

        }
    }

    public class FHIRhelper
    {
        public HashSet<long> ECLexpandToHash(string ecl)
        {
            var hash = new HashSet<long>();
            var list = ECLexpandToList(ecl);
            foreach (var item in list)
            {
                hash.Add(item);
            }

            return hash;
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
    }
}
