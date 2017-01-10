using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

using System.Threading.Tasks;

namespace TradeMod
{
    class Program
    {
        static void Main(string[] args)
        {
            string file = getExchangesFile();
            string json;
            using (StreamReader reader = new StreamReader(file))
            {
                 json = reader.ReadToEnd();

            }

            string result = duplicateSellingValues(json);
            Console.WriteLine("");
            Console.ReadLine();
        }

        private static string duplicateSellingValues(string s)
        {
            string salePricePattern = "\"SellPrice:\"\\s\\d+";


            s.
        }

        private static string getExchangesFile()
        {
            return @"C: \Users\ScrapMek\AppData\LocalLow\Failbetter Games\Sunless Sea\entities\exchanges.json";
            //return Path.GetFullPath("~/entities/exchanges.json");
        }
    }
}
