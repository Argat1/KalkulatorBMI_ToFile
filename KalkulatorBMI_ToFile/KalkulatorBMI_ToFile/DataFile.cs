using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace KalkulatorBMI_ToFile
{
    public class DataFile
    {
        public static void WriteToFile(List<BMIResult> bMIResults)
        {
            string path = App.DbPath;

            List<string> output = new List<string>();
            foreach (BMIResult result in bMIResults)
            {
                string line = $"{result.Title}; {result.Date}; {result.Height}; {result.Weight}; {result.Gender}; {result.Score}; {result.Result};";
                output.Add(line);
            }
            File.WriteAllLines(path, output);
        }

        public static List<BMIResult> LoadTxt()
        {
            string path = App.DbPath;

            if (File.Exists(path))
            {
                List<string> results = File.ReadAllLines(path).ToList();
                List<BMIResult> bMIResults = new List<BMIResult>();

                foreach (var line in results)
                {
                    string[] entries = line.Split(';');

                    if (entries.Length >= 1)
                    {
                        BMIResult newBMIResult = new BMIResult();

                        newBMIResult.Title = entries[0];
                        newBMIResult.Date = DateTime.Parse(entries[1]);
                        newBMIResult.Weight = int.Parse(entries[2]);
                        newBMIResult.Height = int.Parse(entries[3]);
                        newBMIResult.Gender = entries[4];
                        newBMIResult.Score = float.Parse(entries[5]);
                        newBMIResult.Result = entries[6];

                        bMIResults.Add(newBMIResult);
                    }
                }

                return bMIResults;
            }
            else
            {
                return null;
            }
        }
        public static void DeleteLine(BMIResult bMIResult)
        {
            string path = App.DbPath;

            if (File.Exists(path))
            {
                List<string> results = File.ReadAllLines(path).ToList();
                List<BMIResult> bMIResultsList = new List<BMIResult>();

                foreach (var line in results)
                {
                    string[] entries = line.Split(';');

                    if (entries.Length >= 1)
                    {
                        BMIResult newBMIResult = new BMIResult();

                        newBMIResult.Title = entries[0];
                        newBMIResult.Date = DateTime.Parse(entries[1]);
                        newBMIResult.Weight = int.Parse(entries[2]);
                        newBMIResult.Height = int.Parse(entries[3]);
                        newBMIResult.Gender = entries[4];
                        newBMIResult.Score = float.Parse(entries[5]);
                        newBMIResult.Result = entries[6];

                        bMIResultsList.Add(newBMIResult);
                    }
                }


                bMIResultsList.Remove(bMIResult);

                WriteToFile(bMIResultsList);
            }
        }
    }
}
