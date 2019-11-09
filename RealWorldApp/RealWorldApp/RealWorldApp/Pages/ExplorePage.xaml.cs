using RealWorldApp.Models;
using RealWorldApp.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace RealWorldApp.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ExplorePage : ContentPage
    {
        public ObservableCollection<HotAndNewAd> HotVehicleCollection;
        public ExplorePage()
        {
            InitializeComponent();
            HotVehicleCollection = new ObservableCollection<HotAndNewAd>();
            GetHotAndNewVehicles();
        }

        private async void GetHotAndNewVehicles()
        {
            var vehicles = await ApiService.GetHotAndNewAds();
            foreach (var vehicle in vehicles)
            {
                HotVehicleCollection.Add(vehicle);
            }
            CvVehicles.ItemsSource = HotVehicleCollection;
        }

        private void CvVehicles_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var currentSelection = e.CurrentSelection.FirstOrDefault() as HotAndNewAd;
            if (currentSelection == null) return;
            Navigation.PushModalAsync(new ItemDetailPage(currentSelection.id));
            ((CollectionView)sender).SelectedItem = null;
        }

        private void TapBike_Tapped(object sender, EventArgs e)
        {
            Navigation.PushModalAsync(new ItemsListPage(1));
        }

        private void TapCar_Tapped(object sender, EventArgs e)
        {
            Navigation.PushModalAsync(new ItemsListPage(2));
        }

        private void TapTruck_Tapped(object sender, EventArgs e)
        {
            Navigation.PushModalAsync(new ItemsListPage(3));
        }

        private void TapSearch_Tapped(object sender, EventArgs e)
        {
            Navigation.PushModalAsync(new SearchPage());
        }
    }
}