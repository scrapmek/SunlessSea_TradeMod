using System;
using System.IO;

namespace TradeMod
{
    class Program
    {

        static void Main(string[] args)
        {
            string file = getOriginalExchangesFile();
            string json;
            using (StreamReader reader = new StreamReader(file))
            {
                json = reader.ReadToEnd();

            }

            string result = duplicateSellingValues(json);

            Directory.CreateDirectory(getAddonExchangesFolder());
            string path = getAddonExchangesFile();
            
            using (FileStream stream = File.Create(path))
            using (StreamWriter writer = new StreamWriter(stream))
            {
                writer.Write(result);
            }

            printProgress(100);
            Console.WriteLine(String.Format("{0}Modding complete. Press any key to continue...", Environment.NewLine));
            Console.ReadLine();

        }

        private static string duplicateSellingValues(string s)
        {
            double progress = 0;
            double lastProgress = -1;


            for (double i = 50000; i > 0; i--)
            {
                string oldSubString = String.Format("\"SellPrice\":{0},", i);
                string newSubString = String.Format("\"SellPrice\":{0},", i * 2);
                s = s.Replace(oldSubString, newSubString);
                progress = (((50000 - i) / 50000) * 100);
                if (Math.Floor(progress) > lastProgress)
                {
                    printProgress(Math.Floor(progress));
                    lastProgress = Math.Floor(progress);
                }

            }

            return s;
        }

        private static void printProgress(double percentage)
        {
            Console.Clear();
            string progressMessage = String.Format("Doubling SalePrice values: {0}% complete", Math.Floor(percentage));
            Console.WriteLine(progressMessage);
        }

        private static string getAddonExchangesFile()
        {
            string addonDirectory = getAddonExchangesFolder();
            string path = Path.Combine(addonDirectory, @"exchanges.json");
            return path;
        }

        private static string getAddonExchangesFolder()
        {
            string sunlessSeaDirectory = getSunlessSeaDirectory();
            string path = Path.Combine(sunlessSeaDirectory, @"addon\Trade Mod\entities");
            return path;
        }

        private static string getOriginalExchangesFile()
        {
            string sunlessSeaDirectory = getSunlessSeaDirectory();
            string path = Path.Combine(sunlessSeaDirectory, @"entities\exchanges.json");
            return path;
        }

        private static string getSunlessSeaDirectory()
        {
            string directory = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            directory = Directory.GetParent(directory).FullName;
            string path = Path.Combine(directory, @"LocalLow\Failbetter Games\Sunless Sea");
            return path;
        }
    }
}
