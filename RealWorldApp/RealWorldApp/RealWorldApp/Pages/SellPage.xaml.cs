using RealWorldApp.Models;
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

namespace RealWorldApp.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SellPage : ContentPage
    {
        public ObservableCollection<Category> CategoriesCollection;
        private int categoryId;
        private string condition;
        public SellPage()
        {
            InitializeComponent();
            CategoriesCollection = new ObservableCollection<Category>();
            GetVehicleCategories();
        }

        private async void GetVehicleCategories()
        {
            var categories = await ApiService.GetCategories();
            foreach (var category in categories)
            {
                CategoriesCollection.Add(category);
            }
            PickerCategory.ItemsSource = CategoriesCollection;
        }

        private async void BtnSell_Clicked(object sender, EventArgs e)
        {
            var vehicle = new Vehicle();
            vehicle.title = EntTitle.Text;
            vehicle.price = Convert.ToInt32(EntPrice.Text);
            vehicle.engine = EntEngine.Text;
            vehicle.model = EntModel.Text;
            vehicle.color = EntColor.Text;
            vehicle.company = EntCompany.Text;
            vehicle.location = EntLocation.Text;
            vehicle.description = EdiDescription.Text;
            vehicle.datePosted = DateTime.Now;
            vehicle.userid = Preferences.Get("userId", 0);
            vehicle.categoryid = categoryId;
            vehicle.condition = condition;
            var response = await ApiService.AddVehicle(vehicle);
            if (response == null) return;
            var vehicleId = response.vehicleId;
            await Navigation.PushAsync(new AddImagePage(vehicleId)); 
        }

        private void PickerCategory_SelectedIndexChanged(object sender, EventArgs e)
        {
            var picker = sender as Picker;
            var selectedCategory = (Category)picker.SelectedItem;
            categoryId = selectedCategory.id;
        }

        private void TapUsed_Tapped(object sender, EventArgs e)
        {
            condition = "used";
            FrameUsed.BackgroundColor = Color.FromHex("#303F9F");
            LblUsed.TextColor = Color.White;
            FrameNew.BackgroundColor = Color.White;
            LblNew.TextColor = Color.FromHex("#303F9F");
        }

        private void TapNew_Tapped(object sender, EventArgs e)
        {
            condition = "new";
            FrameNew.BackgroundColor = Color.FromHex("#303F9F");
            LblNew.TextColor = Color.White;
            FrameUsed.BackgroundColor = Color.White;
            LblUsed.TextColor = Color.FromHex("#303F9F");

        }
    }
}