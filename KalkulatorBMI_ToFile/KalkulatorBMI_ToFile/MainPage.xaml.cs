using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Newtonsoft.Json;
using Xamarin.Forms;

namespace KalkulatorBMI_ToFile
{
    public partial class MainPage : ContentPage
    {

        List<BMIResult> bmiResults = new List<BMIResult>();
       

        public static List<BMIResult> LoadTxt()
        {
            string path = App.DbPath;


            string text = File.ReadAllText(path);

            if (File.Exists(text))
            {
                List<string> results = File.ReadAllLines(path).ToList();
                List<BMIResult> bmiResults = new List<BMIResult>();

                foreach (var line in results)
                {
                    string[] entries = line.Split(';');

                    if (entries.Length >= 1)
                    {
                        BMIResult newBMIResults = new BMIResult();
                        newBMIResults.Title = entries[0];
                        newBMIResults.Date = DateTime.Parse(entries[1]);
                        newBMIResults.Gender = entries[2];
                        newBMIResults.Height = int.Parse(entries[3]);
                        newBMIResults.Weight = int.Parse(entries[4]);
                        newBMIResults.Result = entries[5];
                        newBMIResults.Score = int.Parse(entries[6]);

                        bmiResults.Add(newBMIResults);
                    }
                }

                return bmiResults;
            }
            else
            {
                return null;
            }


        }
        public MainPage()
        {
            InitializeComponent();
           
        }
        private async void CalculateBMI(object sender, EventArgs e)
        {
            if (!RadioButton_female.IsChecked && !RadioButton_male.IsChecked)
            {
                await DisplayAlert("Błąd", "Wybierz płeć.", "OK");
                return;
            }

            if (!int.TryParse(entry_weight.Text, out int weight) || !int.TryParse(entry_height.Text, out int height) || weight < 20 || height < 100)
            {
                await DisplayAlert("Błąd", "Podano błęny wzrost lub błędną masę ciała.", "OK");
                return;
            }


            string gender = "";
            if (RadioButton_female.IsChecked)
            {
                gender = "kobieta";
            }
            if (RadioButton_male.IsChecked)
            {
                gender = "mężczyzna";
            }

            float score = float.Parse(weight.ToString()) / ((float.Parse(height.ToString()) / 100) * (float.Parse(height.ToString()) / 100));

            string result = "";
            if (score < 16)
            {
                result = "wygłodzenie";
            }
            if (score >= 16 && score < 19)
            {
                result = "niedowaga";
            }
            else if (score >= 19 && score < 24)
            {
                result = "waga prawidłowa";
            }
            else if ((score >= 24 && score <= 30 && gender == "kobieta") || (score >= 25 && score <= 30 && gender == "mężczyzna"))
            {
                result = "nadwaga";
            }
            else if (score >= 30 && score <= 40)
            {
                result = "otyłość";
            }
            else if (score >= 40)
            {
                result = "skrajna otyłość";
            }

            label_score.Text = score.ToString("0.00") + " BMI";
            label_result.Text = "Wynik: " + result + ".";

            btn_saveResult.IsVisible = true;
            btn_saveResult.IsEnabled = true;

            label_score_invisible.Text = score.ToString("0.00");
            label_result_invisible.Text = result;
            label_gender_invisible.Text = gender;
        }

        private async void SaveResult(object sender, EventArgs e)
        {
            string title = await DisplayPromptAsync("Tytuł", "Nadaj tytuł", "OK", "ANULUJ", "tytuł");
            if (string.IsNullOrWhiteSpace(title))
            {
                await DisplayAlert("Błąd", "Podaj tytuł zapisu.", "OK");
                return;
            }
            
            
            
            BMIResult bmi = new BMIResult(title, DateTime.Now, int.Parse(entry_weight.Text), int.Parse(entry_height.Text), label_gender_invisible.Text, float.Parse(label_score_invisible.Text), label_result_invisible.Text);
            bmiResults.Add(bmi);
            List<string> outputFile = new List<string>();
            foreach(var result in bmiResults)
            {
                string line = $"{result.Title};{result.Date};{result.Gender};{result.Height};{result.Weight};{result.Result};{result.Score};";
                outputFile.Add(line);
            }
            File.WriteAllLines(App.DbPath, outputFile);
           
            /*string path = App.DbPath;
            string file = File.ReadAllText(path);
            List<BMIResult> resultList = JsonConvert.DeserializeObject<List<BMIResult>>(file);

            if (resultList.Count > 0)
            {
                resultList[resultList.Count - 1].SetLastId();
            }

            resultList.Add(new BMIResult(title, DateTime.Now, int.Parse(entry_weight.Text), int.Parse(entry_height.Text), label_gender_invisible.Text, float.Parse(label_score_invisible.Text), label_result_invisible.Text));

            string serializedResultList = JsonConvert.SerializeObject(resultList);
            File.WriteAllText(path, serializedResultList);

            btn_saveResult.IsVisible = false;
            btn_saveResult.IsEnabled = false;*/

            await DisplayAlert("Informacja", "Pomyślnie dodano nowy zapis.", "OK");
        }

        private void GoToList(object sender, EventArgs e)
        {
            Navigation.PushAsync(new ListViewPage());
        }
    }
}
