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
using System.Collections;

namespace KalkulatorBMI_ToFile
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ListViewPage : ContentPage
    {
        public ListViewPage()
        {
            InitializeComponent();
            //Load();
            List<BMIResult> results = DataFile.LoadTxt();
            BMIList.ItemsSource = results;
        }
        public void Load()
        {
            string path = App.DbPath;

            if (File.Exists(path))
            {
                string serializedResults = File.ReadAllText(path);

                List<BMIResult> results = JsonConvert.DeserializeObject<List<BMIResult>>(serializedResults);

                BMIList.ItemsSource = results;
            }
        }

        private void DeleteBMI(object sender, EventArgs e)
        {

        }

        private void Back(object sender, EventArgs e)
        {
            
        }
    }
}