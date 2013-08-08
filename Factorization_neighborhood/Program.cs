using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Factorization_neighborhood
{
    class Program
    {
        static void Main(string[] args)
        {
            
            string folderPath = @"C:\Users\sbiesan\Desktop\Projects\input";
            //string folderPath = @"C:\Users\sbiesan\Desktop\download\training_set";
            IEnumerable<string> files = from file in Directory.EnumerateFiles(folderPath) select file;
            //foreach (string entry in files)1
            //{
            //    Console.WriteLine(entry);
           // }
            RecommenderSystem rec = new RecommenderSystem(files);
            Console.WriteLine(rec.PredictValue(2059652-1, 1-1));
        }
    }
}
