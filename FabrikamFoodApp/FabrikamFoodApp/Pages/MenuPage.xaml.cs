using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Microsoft.WindowsAzure.MobileServices;
using FabrikamFoodApp.DataModels;
using Xamarin.Forms.Xaml;

namespace FabrikamFoodApp.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MenuPage : ContentPage
    {
        private static MobileServiceClient mobileClient;

        public MenuPage()
        {
            InitializeComponent();
            mobileClient = new MobileServiceClient("https://fabrikamfood.azurewebsites.net");
            SetMenuItems();
        }

        private async void SetMenuItems()
        {
            var menuTable = mobileClient.GetTable<Menu>();
            var menuItems = await menuTable.ToListAsync();
            menu.ItemsSource = menuItems.OrderBy(x => x.Name);
            //var groupedItems = menuItems.GroupBy(x => x.Category).Select(grp => grp.ToList()).ToList();
            //menu.ItemsSource = groupedItems;
        }

        private async void RefreshMenuItems(object sender, EventArgs e)
        {
            var menuTable = mobileClient.GetTable<Menu>();
            var menuItems = await menuTable.ToListAsync();
            menu.ItemsSource = menuItems.OrderBy(x => x.Name);
            //var groupedItems = menuItems.GroupBy(x => x.Category).Select(grp => grp.ToList()).ToList();
            //menu.ItemsSource = groupedItems;
            menu.EndRefresh();
        }
    }
}
