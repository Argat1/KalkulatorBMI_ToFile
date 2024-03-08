using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace KalkulatorBMI_ToFile
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ListViewPage : ContentPage
    {
        private ObservableCollection<BMIResult> resultList = new ObservableCollection<BMIResult>();

        public ListViewPage()
        {
            InitializeComponent();

            Load();
        }

        public void Load()
        {
            string path = App.DbPath;

            if (File.Exists(path))
            {
                string text = File.ReadAllText(path);

                resultList = JsonConvert.DeserializeObject<ObservableCollection<BMIResult>>(text);
                listViewBMI.ItemsSource = resultList;
            }
        }

        private void Delete_btn(object sender, EventArgs e)
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
        }
    }
}