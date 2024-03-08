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
        List<BMIResult> BMIresults = new List<BMIResult>();
        public MainPage()
        {
            InitializeComponent();
            if (!File.Exists(App.DbPath))
            {
                string serializedBMI = JsonConvert.SerializeObject(new List<BMIResult>());
                File.WriteAllText(App.DbPath, serializedBMI);
            }
        }

        private async void CalculateBMI(object sender, EventArgs e)
        {
            double height = slider_height.Value;

            if (!female.IsChecked && !male.IsChecked)
            {
                await DisplayAlert("Błąd", "Wybierz płeć", "OK");
                return;
            }

            if (!int.TryParse(weight_Entry.Text, out int weight)  || weight < 20 )
            {
                await DisplayAlert("Błąd", "Podano błędny wzrost lub wagę", "OK");
                return;
            }
            string gender = "";
            if (male.IsChecked)
            {
                gender = "mężczyzna";
            }
            if (female.IsChecked)
            {
                gender = "kobieta";
            }
            float score = float.Parse(weight.ToString()) / ((float.Parse(height.ToString()) / 100) * (float.Parse(height.ToString()) / 100));

            string result = "";

            if (score < 16)
            {
                result = "Wygłodzenie";
            }
            if (score >= 16 && score < 19)
            {
                result = "Niedowaga";
            }
            if (score >= 19 && score < 24)
            {
                result = "Waga prawidłowa";
            }
            if (score >= 24 && score <= 30 && gender == "kobieta" || score >= 25 && score <= 30 && gender == "mężczyzna")
            {
                result = "Nadwaga";
            }
            if (score > 30 && score <= 40)
            {
                result = "Otyłość";
            }
            if (score > 40)
            {
                result = "Skrajna otyłość";
            }

            scoreLbl.Text = score.ToString("0.00");
            resultLbl.Text = result;
            genderInvis.Text = gender;

            saveBtn.IsEnabled = true;
            saveBtn.IsVisible = true;
        }

        private void GoToSavedBMI(object sender, EventArgs e)
        {
            Navigation.PushAsync(new ListViewPage());
        }

        private async void SaveBMI(object sender, EventArgs e)
        {
            string title = await DisplayPromptAsync("Tytuł", "Podaj tytuł", "OK", "Anuluj", "tytuł");
            if (string.IsNullOrWhiteSpace(title))
            {
                await DisplayAlert("Błąd", "Podaj poprawny tytuł", "OK");
                return;
            }
            BMIResult bmi = new BMIResult(title, DateTime.UtcNow, int.Parse(weight_Entry.Text), slider_height.Value, genderInvis.Text, float.Parse(scoreLbl.Text), resultLbl.Text);

            BMIresults.Add(bmi);

            DataFile.WriteToFile(BMIresults);

            saveBtn.IsEnabled = false;
            saveBtn.IsVisible = false;

            await DisplayAlert("Wydarzenie", "Pomyślnie dodano", "OK");
            /*
            string path = App.DbPath;
            string file = File.ReadAllText(path);
            List<BMIResult> results = JsonConvert.DeserializeObject<List<BMIResult>>(file);

            if (results.Count > 0)
            {
                results[results.Count - 1].SetLastId();
            }

            BMIResult bmi = new BMIResult(title, DateTime.UtcNow, int.Parse(weight_Entry.Text), int.Parse(height_Entry.Text), genderInvis.Text, float.Parse(scoreLbl.Text), resultLbl.Text);

            results.Add(bmi);

            string serializedBMI = JsonConvert.SerializeObject(results);
            File.WriteAllText(path, serializedBMI);

            
            */
        }

        private void slider_height_ValueChanged(object sender, ValueChangedEventArgs e)
        {
            slider_info.Text = slider_height.Value.ToString() + "cm";
        }
    }
}
