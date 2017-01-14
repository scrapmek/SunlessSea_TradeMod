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

            double saleMultiplier = 1.75;
            string result = duplicateSellingValues(json, saleMultiplier);
            result = capSellingValues(result);

            Directory.CreateDirectory(getAddonExchangesFolder());
            string path = getAddonExchangesFile();

            using (FileStream stream = File.Create(path))
            using (StreamWriter writer = new StreamWriter(stream))
            {
                writer.Write(result);
            }

            Console.WriteLine(String.Format("{0}Modding complete. Press any key to continue...", Environment.NewLine));
            Console.ReadLine();

        }

        private static string duplicateSellingValues(string s, double salePriceMultiplier)
        {
            int progress = 0;
            int lastProgress = -1;
            int number = 50000;

            for (double i = number; i > 0; i--)
            {
                string oldSubString = String.Format("\"SellPrice\":{0},", i);
                if (s.Contains(oldSubString))
                {
                    string newSubString = String.Format("\"SellPrice\":{0},", i * salePriceMultiplier);
                    s = s.Replace(oldSubString, newSubString);
                }

                progress = Convert.ToInt32((((double)(number - i) / number) * 100));
                if (progress > lastProgress)
                {
                    printProgress("Increasing cargo sale prices", progress);
                    lastProgress = progress;
                }

            }

            return s;
        }

        private static string capSellingValues(string s)
        {
            int progress = 0;
            int lastProgress = -1;

            int number = 100000;

            for (int i = number; i > 0; i--)
            {
                if (s.Contains(String.Format("\"SellPrice\":{0},", i)))
                {
                    for (int j = 1; j < i; j++)
                    {

                        string oldSubString = String.Format("\"Cost\":{0},\"SellPrice\":{1},", j, i);
                        string newSubString = String.Format("\"Cost\":{0},\"SellPrice\":{0},", j);

                        if (s.Contains(oldSubString))
                        {
                            s = s.Replace(oldSubString, newSubString);
                        }
                    }
                }

                progress = Convert.ToInt32((((double)(number - i) / number) * 100));
                if (progress > lastProgress)
                {
                    printProgress("Rebalancing sale prices", progress);
                    lastProgress = progress;
                }
            }

            return s;

        }

        private static void printProgress(string message, int percentage)
        {
            Console.Clear();
            string progressMessage = String.Format("{0}: {1}% complete", message, percentage);
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
