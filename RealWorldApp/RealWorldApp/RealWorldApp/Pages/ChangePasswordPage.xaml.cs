using RealWorldApp.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace RealWorldApp.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ChangePasswordPage : ContentPage
    {
        public ChangePasswordPage()
        {
            InitializeComponent();
        }

        private async void BtnChangePassword_Clicked(object sender, EventArgs e)
        {
            var response = await ApiService.ChangePassword(EntOldPassword.Text, EntNewPassword.Text, EntConfirmPassword.Text);
            if (!response)
            {
                await DisplayAlert("Oops", "Something went wrong", "Cancel");
            }
            else
            {
                await DisplayAlert("Password Changed", "Kindly login with the new password", "Alright");
                Preferences.Set("accessToken",string.Empty);
                Application.Current.MainPage = new NavigationPage(new SignupPage());
            }
        }
    }
}