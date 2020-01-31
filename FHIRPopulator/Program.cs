using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hl7.Fhir.Model;
using Hl7.Fhir.Rest;

namespace FHIRPopulator
{
    class Program
    {
        static void Main(string[] args)
        {
            //Console.WriteLine("Starting");
            const string R3Server = "https://vonk.fire.ly/";

            var patient = new Patient();

            var name = new HumanName();
            name.Family = "Sanchez";
            var givens = new List<string>();
            givens.Add("Rick");                            
            name.Given = givens;

            patient.Name.Add(name);

            patient.BirthDate = (DateTime.Now.Year - 70).ToString();

            var id = new Identifier();
            //id.System = "http://snoyowie.com";
            id.Value = "470641001";
            patient.Identifier.Add(id);


            var x = new Hl7.Fhir.Serialization.FhirJsonSerializer();
            

            //Console.WriteLine(x.SerializeToString(patient));
            // https://fhir-drills.github.io/simple-patient.html
            // http://docs.simplifier.net/fhirnetapi/model/patient-example.html
            // http://www.interopsante.org/offres/doc_inline_src/412/W12.pdf

            var client = new FhirClient(R3Server);
            client.PreferredFormat = ResourceFormat.Json;

            var p = client.Create<Patient>(patient);
            

            //Console.WriteLine("patient written");
            //var pat_A = client.Read<Patient>("Patient/1212");
            //Bundle results = client.WholeSystemSearch(new string[] { "identifier=470641001" });
            //Console.WriteLine("patient read");


            //Console.WriteLine(x.SerializeToString(results));           



            //client.Create(patient);
            //
            //var pat_A = client.Read<Patient>("Patient/2");
            //Console.WriteLine(x.SerializeToString(pat_A));

            var Doc = new Composition();
            Doc.Subject = new ResourceReference();
            Doc.Subject = patient.r




            Console.WriteLine("Done");
            Console.ReadKey();


        }
    }
}
