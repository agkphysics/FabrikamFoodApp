using FabrikamFoodApp.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Auth;
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
        }

        // Track whether the user has authenticated.
        bool authenticated = false;

        private async void loginButton_Clicked(object sender, EventArgs eventArgs)
        {
            loginButton.IsEnabled = false;

            if (App.Authenticator != null) authenticated = await App.Authenticator.Authenticate();
            if (authenticated)
            {
                CrossSettings.Current.AddOrUpdateValue("userid", AzureManager.CurrentInstance.CurrentClient.CurrentUser.UserId);
                CrossSettings.Current.AddOrUpdateValue("token", AzureManager.CurrentInstance.CurrentClient.CurrentUser.MobileServiceAuthenticationToken);

                loginButton.IsVisible = false;

                var userInfo = await AzureManager.CurrentInstance.GetUserData();
                ((RootPage)this.Parent).Children.Insert(2, new UserPage(userInfo.Message.First_Name));
            }
            else loginButton.IsEnabled = true;
        }

        private async void aboutButton_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new AboutPage());
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            string userId = CrossSettings.Current.GetValueOrDefault("user", "");
            string token = CrossSettings.Current.GetValueOrDefault("token", "");

            if (!token.Equals("") && !userId.Equals(""))
            {
                MobileServiceUser user = new MobileServiceUser(userId);
                user.MobileServiceAuthenticationToken = token;

                AzureManager.CurrentInstance.CurrentClient.CurrentUser = user;

                authenticated = true;
            }

            if (authenticated == true)
            {
                loginButton.IsVisible = false;
            }
        }
    }
}
