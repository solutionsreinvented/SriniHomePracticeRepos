using System.IO;
using System.Linq;

using Newtonsoft.Json;

namespace GenericsPractice
{
    class Program
    {
        static void Main(string[] args)
        {
            string path = @"C:\Users\srini\source\files-store";

            string filename = "Datafile.json";

            SiteCoefficients siteCoefficients;

            JsonSerializer jsonSerializer = new JsonSerializer();

            TextReader sr = new StreamReader(Path.Combine(path, filename));

            using (JsonTextReader jtr = new JsonTextReader(sr))
            {
                siteCoefficients = jsonSerializer.Deserialize<SiteCoefficients>(jtr);
            }

            var genericClass = new ObjectCreator<Code>();

            var code = genericClass.Create();

            code.CodeClean();
            code.OrientToPrinciples();
            code.Ensure();

            //string directory = @"F:\02. Video Tutorials\05. New Downloads\00. Completed\03. Seggregate\Udemy(X) - Clean Code (2020)";

            //if (Directory.Exists(directory))
            //{
            //    var files = Directory.GetFiles(directory).Where(f => f.Contains(".mp4"));
            //}


        }
    }
}
