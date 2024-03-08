using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace KalkulatorBMI_ToFile
{
    public partial class App : Application
    {
        public static string DbPath
        {
            get
            {
                return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "BMIResults.json");
            }
        }
        public App()
        {
            InitializeComponent();

            MainPage = new NavigationPage(new MainPage());
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
