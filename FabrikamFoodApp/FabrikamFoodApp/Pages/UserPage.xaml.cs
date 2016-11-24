using FabrikamFoodApp.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Maps;
using Newtonsoft.Json;
using System.Net;
using System.Web;
using System.Net.Http;

namespace FabrikamFoodApp.Pages
{
    public partial class UserPage : ContentPage
    {
        private string Username { get; set; }

        public UserPage(string username)
        {
            Title = username;
            InitializeComponent();
            nameLabel.Text = username;
            RefreshThings();
        }

        private async void RefreshThings()
        {
            var user = await AzureManager.CurrentInstance.CurrentClient.GetTable<Users>().LookupAsync(AzureManager.CurrentInstance.CurrentClient.CurrentUser.UserId);
            if (user.Address != null && !user.Address.Equals(""))
            {
                addressInput.Text = user.Address;
            }

            var favTable = AzureManager.CurrentInstance.CurrentClient.GetTable<UserFavs>();
            var menuTable = AzureManager.CurrentInstance.CurrentClient.GetTable<Menu>();
            
            var favItems = await favTable.Where(x => x.UserID == user.ID).ToListAsync();
            var idfavItems = favItems.Select(x => x.MenuID);
            var menuItems = await menuTable.Where(x => idfavItems.Contains(x.ID)).ToListAsync();

            favList.ItemsSource = menuItems;
        }

        private void OnSelect(object sender, SelectedItemChangedEventArgs e)
        {
            // Disable selection
            ((ListView)sender).SelectedItem = null;
        }

        private void OnTap(object sender, ItemTappedEventArgs e)
        {
            var item = (Menu)e.Item;
            Device.OpenUri(new Uri($"https://fabrikamfood.azurewebsites.net/menu/{item.ID}"));
        }

        private async void address_Update(object sender, EventArgs e)
        {
            await AzureManager.CurrentInstance.UpdateCurrentUserInDB(null, addressInput.Text, null);
            await DisplayAlert("Updated address", "Updated address", "OK");
            GetDistanceData();
        }

        private async void GetDistanceData()
        {
            var stores = await AzureManager.CurrentInstance.CurrentClient.GetTable<Stores>().ToListAsync();
            var storeCoords = stores.Select(x => x.lat.ToString() + "," + x.lon.ToString());
            var destinationStr = string.Join("|", storeCoords);
            
            HttpClient client = new HttpClient();

            var resp = await client.GetAsync("https://maps.googleapis.com/maps/api/distancematrix/json?units=metric" +
                "&key=AIzaSyDsSpBk1d-DalrTv55B9APC7tIrL3Qoy7c" +
                "&origins=" + HttpUtility.UrlEncode(addressInput.Text) +
                "&destinations=" + HttpUtility.UrlEncode(destinationStr));
            if (resp.IsSuccessStatusCode)
            {
                var content = await resp.Content.ReadAsStringAsync();
                var distanceMat = JsonConvert.DeserializeObject<DistanceAPIModel.RootObject>(content);

                int idx = 0, minDuration = 99999;
                for (int i = 0; i < distanceMat.rows[0].elements.Count; i++)
                {
                    var row = distanceMat.rows[0].elements[i];
                    if (row.status == "OK" && row.duration.value < minDuration)
                    {
                        idx = i;
                        minDuration = row.duration.value;
                    }
                }

                await DisplayAlert("Distance", "Nearest store is " + stores[idx].Name + " at " + distanceMat.rows[0].elements[idx].duration.text + " away", "OK");
            }
        }
    }
}
