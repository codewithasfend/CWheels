using RealWorldApp.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Image = RealWorldApp.Models.Image;
namespace RealWorldApp.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ItemDetailPage : ContentPage
    {
        public ObservableCollection<Image> VehicleImage;
        private int totalImages;
        private string number;
        private string email;
        public ItemDetailPage(int id)
        {
            InitializeComponent();
            VehicleImage = new ObservableCollection<Image>();
            GetVehicleDetails(id);
        }

        private async void GetVehicleDetails(int id)
        {
            var vehicle = await ApiService.GetVehicleDetail(id);
            LblTitle.Text = vehicle.title;
            LblLocation.Text = vehicle.location;
            LblCondition.Text = vehicle.condition;
            LblPrice.Text = vehicle.price.ToString();
            LblCompany.Text = vehicle.company;
            LblDescription.Text = vehicle.description;
            LblColor.Text = vehicle.color;
            LblModelNo.Text = vehicle.model;
            LblEngine.Text = vehicle.engine;
            ImgUser.Source = vehicle.FullImageUrl;
            var images = vehicle.images;
            totalImages = vehicle.images.Count;
            number = vehicle.phone;
            email = vehicle.email;
            foreach (var image in images)
            {
                VehicleImage.Add(image);
            }
            CrvImages.ItemsSource = VehicleImage;
        }

        private void CrvImages_Scrolled(object sender, ItemsViewScrolledEventArgs e)
        {
            if (e.FirstVisibleItemIndex <= totalImages)
            {
                var count = e.FirstVisibleItemIndex + 1;
                LblCount.Text = count + "of" + totalImages;
            }
        }

        private void BtnEmail_Clicked(object sender, EventArgs e)
        {
            var emailMessage = new EmailMessage("Query about vehicle", "I want to know more about this vehicle", email);
            Email.ComposeAsync(emailMessage);
        }

        private void BtnSms_Clicked(object sender, EventArgs e)
        {
            var smsMessage = new SmsMessage("Hi! I want to know about this vehicle", number);
            Sms.ComposeAsync(smsMessage);
        }

        private void BtnCall_Clicked(object sender, EventArgs e)
        {
            PhoneDialer.Open(number);
        }

        private void BtnBack_Clicked(object sender, EventArgs e)
        {
            Navigation.PopModalAsync();
        }
    }
}