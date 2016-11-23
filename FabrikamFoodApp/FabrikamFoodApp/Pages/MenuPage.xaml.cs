using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using FabrikamFoodApp.DataModels;
using Xamarin.Forms.Xaml;

namespace FabrikamFoodApp.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MenuPage : ContentPage
    {
        public MenuPage()
        {
            InitializeComponent();
            loadingIndicator.IsRunning = true;
            loadingIndicator.IsVisible = true;
            RefreshMenuItems();
        }

        private async Task RefreshMenuItems()
        {
            var menuTable = AzureManager.CurrentInstance.CurrentClient.GetTable<Menu>();
            var menuItems = await menuTable.ToListAsync();
            menu.ItemsSource = menuItems.OrderBy(x => x.Name);
            //var groupedItems = menuItems.GroupBy(x => x.Category).Select(grp => grp.ToList()).ToList();
            //menu.ItemsSource = groupedItems;
            loadingIndicator.IsRunning = false;
            await loadingIndicator.TranslateTo(0, -loadingIndicator.Height, 250, Easing.SinInOut);
            loadingIndicator.IsVisible = false;
        }

        private async void menu_Refreshing(object sender, EventArgs e)
        {
            await RefreshMenuItems();
            menu.EndRefresh();
        }
    }
}
