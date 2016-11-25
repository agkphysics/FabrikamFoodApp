using FabrikamFoodApp.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using Plugin.Settings;
using Microsoft.WindowsAzure.MobileServices;

namespace FabrikamFoodApp.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class HomePage : ContentPage
    {
        public HomePage()
        {
            InitializeComponent();
            imageSlider.ItemsSource = new string[]
            {
                "hot_chocolate.jpg",
                "chocolate_cake.jpg",
                "ice_cream.jpg",
                "soda_cans.jpg",
                "wine.jpg"
            };
            Device.StartTimer(new TimeSpan(0, 0, 5), CycleImage);
        }

        protected override void OnSizeAllocated(double width, double height)
        {
            base.OnSizeAllocated(width, height);

            mainGrid.RowDefinitions[0].Height = new GridLength(Math.Min(0.6666 * width, 150), GridUnitType.Absolute);
        }

        // Track whether the user has authenticated.
        bool authenticated = false;

        private async void loginButton_Clicked(object sender, EventArgs e)
        {
            loginButton.IsEnabled = false;

            if (App.Authenticator != null) authenticated = await App.Authenticator.Authenticate();
            if (authenticated)
            {
                CrossSettings.Current.AddOrUpdateValue("userid", AzureManager.CurrentInstance.CurrentClient.CurrentUser.UserId);
                CrossSettings.Current.AddOrUpdateValue("token", AzureManager.CurrentInstance.CurrentClient.CurrentUser.MobileServiceAuthenticationToken);

                loginButton.IsVisible = false;
                logoutButton.IsVisible = true;
                logoutButton.IsEnabled = true;

                var userInfo = await AzureManager.CurrentInstance.GetUserData();
                ((RootPage)Parent).Children.Insert(2, new UserPage(userInfo.Message.First_Name));

                await DisplayAlert("Logged In", "Successfully logged in.", "OK");
            }
            else loginButton.IsEnabled = true;
        }

        private async void logoutButton_Clicked(object sender, EventArgs e)
        {
            await AzureManager.CurrentInstance.DeleteCurrentUser();
            await AzureManager.CurrentInstance.CurrentClient.LogoutAsync();
            authenticated = false;
            CrossSettings.Current.Remove("userid");
            CrossSettings.Current.Remove("token");
            loginButton.IsEnabled = true;
            loginButton.IsVisible = true;
            logoutButton.IsVisible = false;
            logoutButton.IsEnabled = false;
            ((RootPage)Parent).Children.RemoveAt(2);
            await DisplayAlert("Logged Out", "Successfully logged out.", "OK");
        }

        private async void aboutButton_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new AboutPage());
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            if (!authenticated) // Only if were not authenticated already
            {
                string userId = CrossSettings.Current.GetValueOrDefault("userid", "");
                string token = CrossSettings.Current.GetValueOrDefault("token", "");

                if (!token.Equals("") && !userId.Equals(""))
                {
                    MobileServiceUser user = new MobileServiceUser(userId);
                    user.MobileServiceAuthenticationToken = token;

                    AzureManager.CurrentInstance.CurrentClient.CurrentUser = user;

                    authenticated = true;
                }

                if (authenticated)
                {
                    loginButton.IsVisible = false;
                    loginButton.IsEnabled = false;
                    logoutButton.IsVisible = true;
                    logoutButton.IsEnabled = true;

                    var userInfo = await AzureManager.CurrentInstance.GetUserData();
                    ((RootPage)Parent).Children.Insert(2, new UserPage(userInfo.Message.First_Name));
                }
            }
        }

        private bool CycleImage()
        {
            imageSlider.Position = (imageSlider.Position + 1) % 5;
            return true;
        }
    }
}
