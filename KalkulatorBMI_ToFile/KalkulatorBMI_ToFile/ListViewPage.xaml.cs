using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Newtonsoft.Json;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace KalkulatorBMI_ToFile
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ListViewPage : ContentPage
    {
        public List<BMIResult> resultList = new List<BMIResult>();

        public ListViewPage()
        {
            InitializeComponent();

            

            resultList = LoadTxt();
            listViewBMI.ItemsSource = resultList;

        }

        public static List<BMIResult> LoadTxt()
        {
            string path = App.DbPath;

            
                string text = File.ReadAllText(path);

                if(File.Exists(text)) 
                {
                    List<string> results = File.ReadAllLines(path).ToList();
                    List<BMIResult> bmiResults = new List<BMIResult>();

                    foreach(var line in results)
                    {
                        string[] entries = line.Split(';');

                        if(entries.Length >= 1) 
                        {
                            BMIResult newBMIResults  = new BMIResult();
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

        /*private void Delete_btn(object sender, EventArgs e)
        {
            string path = App.DbPath;

            if (!File.Exists(path))
            {
                DisplayAlert("Error", "JSON file not found.", "OK");
                return;
            }

            BMIResult selectedItem = (BMIResult)listViewBMI.SelectedItem;

            if (selectedItem == null)
            {
                DisplayAlert("Błąd", "Prosze wybrac dane.", "OK");
                return;
            }

            resultList.Remove(selectedItem);

            string updatedJson = JsonConvert.SerializeObject(resultList);

            File.WriteAllText(path, updatedJson);

            DisplayAlert("Sukces", "Usunieto.", "OK");
        }*/
    }
}