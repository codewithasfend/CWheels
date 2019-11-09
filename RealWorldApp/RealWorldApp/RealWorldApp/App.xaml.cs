using RealWorldApp.Pages;
using System;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace RealWorldApp
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
            var accesstoken = Preferences.Get("accessToken", string.Empty);
            if (string.IsNullOrEmpty(accesstoken))
            {
                MainPage = new NavigationPage(new SignupPage());
            }
            else
            {
                MainPage = new NavigationPage(new HomePage());
            }

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
