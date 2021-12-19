using SemestralniProjektModel;
using System;
using System.IO;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SemestralniProjekt
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            //Standardni cesty pro jednotliva zarizeni lze ziskat ze tridy Environment a metody GetFolderPath.
            //vysvetlivky k jednotlivym SpecialFolder na nasledujicich odkazech:
            //https://docs.microsoft.com/en-us/xamarin/android/platform/files/
            //https://docs.microsoft.com/en-us/xamarin/ios/app-fundamentals/file-system
            //https://stackoverflow.com/questions/47237414/what-is-the-best-environment-specialfolder-for-store-application-data-in-xamarin
            Databaze databaze = new Databaze(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "databaze.sqlite3"));
            databaze.VytvorDatabazi();
            ////Tato druha varianta vyuziva Xamarin.Essentials, ale mela by udelat to same, co vyse
            //Databaze databaze = new Databaze(Path.Combine(Xamarin.Essentials.FileSystem.AppDataDirectory, "databaze.sqlite3"));

            MainPage = new MainMasterDetailPage(databaze);
            //MainPage = new AdministratorMainTabbedPage(databaze);
        }

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}
