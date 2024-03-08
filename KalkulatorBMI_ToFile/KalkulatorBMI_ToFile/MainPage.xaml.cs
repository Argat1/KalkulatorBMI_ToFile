using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace KalkulatorBMI_ToFile
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
            if (!File.Exists(App.DbPath))
            {
                string serializedResultList = JsonConvert.SerializeObject(new List<BMIResult>());
                File.WriteAllText(App.DbPath, serializedResultList);
            }
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
            string path = App.DbPath;
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
            btn_saveResult.IsEnabled = false;

            await DisplayAlert("Informacja", "Pomyślnie dodano nowy zapis.", "OK");
        }

        private void GoToList(object sender, EventArgs e)
        {
            Navigation.PushAsync(new ListViewPage());
        }
    }
}
