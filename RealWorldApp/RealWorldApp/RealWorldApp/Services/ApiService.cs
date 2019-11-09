using Newtonsoft.Json;
using RealWorldApp.Models;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using System.Net.Http.Headers;
using UnixTimeStamp;

namespace RealWorldApp.Services
{
    public static class ApiService
    {
        public static async Task<bool> RegisterUser(string name, string email, string password)
        {
            var registerModel = new RegisterModel()
            {
                Name = name,
                Email = email,
                Password = password
            };

            var httpClient = new HttpClient();
            var json = JsonConvert.SerializeObject(registerModel);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await httpClient.PostAsync("https://cvehicles.azurewebsites.net/api/accounts/register", content);
            if (!response.IsSuccessStatusCode) return false;
            return true;
        }

        public static async Task<bool> Login(string email, string password)
        {
            var loginModel = new LoginModel()
            {
                Email = email,
                Password = password
            };

            var httpClient = new HttpClient();
            var json = JsonConvert.SerializeObject(loginModel);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await httpClient.PostAsync("https://cvehicles.azurewebsites.net/api/accounts/login", content);
            if (!response.IsSuccessStatusCode) return false;
            var jsonResult = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<Token>(jsonResult);
            Preferences.Set("accessToken", result.access_token);
            Preferences.Set("userId", result.user_Id);
            Preferences.Set("tokenExpirationTime", result.expiration_Time);
            Preferences.Set("currentTime", (int)UnixTime.GetCurrentTime());
            return true;
        }

        public static async Task<bool> ChangePassword(string oldPassword, string newPassword, string confirmPassword)
        {
            var changePasswordModel = new ChangePasswordModel()
            {
                OldPassword = oldPassword,
                NewPassword = newPassword,
                ConfirmPassword = confirmPassword
            };
            await TokenValidator.CheckTokenValidity();
            var httpClient = new HttpClient();
            var json = JsonConvert.SerializeObject(changePasswordModel);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", Preferences.Get("accessToken", string.Empty));
            var response = await httpClient.PostAsync("https://cvehicles.azurewebsites.net/api/accounts/ChangePassword", content);
            if (!response.IsSuccessStatusCode) return false;
            return true;
        }

        public static async Task<bool> EditPhoneNumber(string phoneNumber)
        {
            await TokenValidator.CheckTokenValidity();
            var httpClient = new HttpClient();
            var content = new StringContent($"Number={phoneNumber}", Encoding.UTF8, "application/x-www-form-urlencoded");
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", Preferences.Get("accessToken", string.Empty));
            var response = await httpClient.PostAsync("https://cvehicles.azurewebsites.net/api/accounts/EditPhoneNumber", content);
            if (!response.IsSuccessStatusCode) return false;
            return true;
        }

        public static async Task<bool> EditUserProfile(byte[] imageArray)
        {
            await TokenValidator.CheckTokenValidity();
            var httpClient = new HttpClient();
            var json = JsonConvert.SerializeObject(imageArray);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", Preferences.Get("accessToken", string.Empty));
            var response = await httpClient.PostAsync("https://cvehicles.azurewebsites.net/api/accounts/EditUserProfile", content);
            if (!response.IsSuccessStatusCode) return false;
            return true;
        }

        public static async Task<UserImageModel> GetUserProfileImage()
        {
            await TokenValidator.CheckTokenValidity();
            var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", Preferences.Get("accessToken", string.Empty));
            var response = await httpClient.GetStringAsync("https://cvehicles.azurewebsites.net/api/accounts/UserProfileImage");
            return JsonConvert.DeserializeObject<UserImageModel>(response);
        }

        public static async Task<List<Category>> GetCategories()
        {
            await TokenValidator.CheckTokenValidity();
            var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", Preferences.Get("accessToken", string.Empty));
            var response = await httpClient.GetStringAsync("https://cvehicles.azurewebsites.net/api/Categories");
            return JsonConvert.DeserializeObject<List<Category>>(response);
        }

        public static async Task<bool> AddImage(int vehicleId, byte[] imageArray)
        {
            var vehicleImage = new VehicleImage()
            {
                VehicleId = vehicleId,
                ImageArray = imageArray
            };
            await TokenValidator.CheckTokenValidity();
            var httpClient = new HttpClient();
            var json = JsonConvert.SerializeObject(vehicleImage);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", Preferences.Get("accessToken", string.Empty));
            var response = await httpClient.PostAsync("https://cvehicles.azurewebsites.net/api/Images", content);
            if (!response.IsSuccessStatusCode) return false;
            return true;
        }

        public static async Task<VehicleDetail> GetVehicleDetail(int id)
        {
            await TokenValidator.CheckTokenValidity();
            var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", Preferences.Get("accessToken", string.Empty));
            var response = await httpClient.GetStringAsync($"https://cvehicles.azurewebsites.net/api/Vehicles/VehicleDetails?id={id}");
            return JsonConvert.DeserializeObject<VehicleDetail>(response);
        }

        public static async Task<List<VehicleByCategory>> GetVehicleByCategory(int categoryId)
        {
            await TokenValidator.CheckTokenValidity();
            var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", Preferences.Get("accessToken", string.Empty));
            var response = await httpClient.GetStringAsync($"https://cvehicles.azurewebsites.net/api/Vehicles?categoryId={categoryId}");
            return JsonConvert.DeserializeObject<List<VehicleByCategory>>(response);
        }

        public static async Task<List<SearchVehicle>> SearchVehicle(string search)
        {
            await TokenValidator.CheckTokenValidity();
            var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", Preferences.Get("accessToken", string.Empty));
            var response = await httpClient.GetStringAsync($"https://cvehicles.azurewebsites.net/api/Vehicles/SearchVehicles?search={search}");
            return JsonConvert.DeserializeObject<List<SearchVehicle>>(response);
        }

        public static async Task<VehicleResponse> AddVehicle(Vehicle vehicle)
        {
            await TokenValidator.CheckTokenValidity();
            var httpClient = new HttpClient();
            var json = JsonConvert.SerializeObject(vehicle);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", Preferences.Get("accessToken", string.Empty));
            var response = await httpClient.PostAsync("https://cvehicles.azurewebsites.net/api/Vehicles", content);
            var jsonResult = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<VehicleResponse>(jsonResult);
        }

        public static async Task<List<HotAndNewAd>> GetHotAndNewAds()
        {
            await TokenValidator.CheckTokenValidity();
            var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", Preferences.Get("accessToken", string.Empty));
            var response = await httpClient.GetStringAsync("https://cvehicles.azurewebsites.net/api/Vehicles/HotAndNewAds");
            return JsonConvert.DeserializeObject<List<HotAndNewAd>>(response);
        }

        public static async Task<List<MyAd>> GetMyAds()
        {
            await TokenValidator.CheckTokenValidity();
            var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", Preferences.Get("accessToken", string.Empty));
            var response = await httpClient.GetStringAsync("https://cvehicles.azurewebsites.net/api/Vehicles/MyAds");
            return JsonConvert.DeserializeObject<List<MyAd>>(response);
        }
    }

    public static class TokenValidator
    {
        public static async Task CheckTokenValidity()
        {
            var expirationTime = Preferences.Get("tokenExpirationTime", 0);
            Preferences.Set("currentTime", (int)UnixTime.GetCurrentTime());
            var currentTime = Preferences.Get("currentTime", 0);
            if (expirationTime < currentTime)
            {
                var email = Preferences.Get("email", string.Empty);
                var password = Preferences.Get("password", string.Empty);
                await ApiService.Login(email, password);
            }

        }
    }
}
