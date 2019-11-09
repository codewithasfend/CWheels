using ImageToArray;
using Plugin.Media;
using Plugin.Media.Abstractions;
using RealWorldApp.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace RealWorldApp.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AddImagePage : ContentPage
    {
        private MediaFile file;
        private int _vehicleId;
        public AddImagePage(int vehicleId)
        {
            InitializeComponent();
            _vehicleId = vehicleId;
        }

        private async void PickImageFromGallery(Image imageControl)
        {
            await CrossMedia.Current.Initialize();

            if (!CrossMedia.Current.IsTakePhotoSupported)
            {
                await DisplayAlert("Oops", "Your device does not support this feature", "OK");
                return;
            }

            file = await CrossMedia.Current.PickPhotoAsync(new PickMediaOptions()
            {
                CompressionQuality = 50,
                PhotoSize = PhotoSize.Large
            });

            if (file == null)
                return;

            imageControl.Source = ImageSource.FromStream(() =>
            {
                var stream = file.GetStream();
                AddImageToServer();
                return stream;
            });
        }

        private async void AddImageToServer()
        {
            var imageArray = FromFile.ToArray(file.GetStream());
            file.Dispose();
            var response = await ApiService.AddImage(_vehicleId, imageArray);
            if (response) return;
            await DisplayAlert("Something wrong", "Please upload the image again", "Alright");
        }



        private void TapImg1_Tapped(object sender, EventArgs e)
        {
            PickImageFromGallery(Img1);
        }

        private void TapImg2_Tapped(object sender, EventArgs e)
        {
            PickImageFromGallery(Img2);
        }

        private void TapImg3_Tapped(object sender, EventArgs e)
        {
            PickImageFromGallery(Img3);
        }

        private void TapImg4_Tapped(object sender, EventArgs e)
        {
            PickImageFromGallery(Img4);
        }

        private void TapImg5_Tapped(object sender, EventArgs e)
        {
            PickImageFromGallery(Img5);
        }

        private void TapImg6_Tapped(object sender, EventArgs e)
        {
            PickImageFromGallery(Img6);
        }

        private void BtnDone_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new MyAdsPage());
            Navigation.RemovePage(this);
        }
    }
}