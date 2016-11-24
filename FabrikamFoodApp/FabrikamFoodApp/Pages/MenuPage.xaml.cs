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
            var x = RefreshMenuItems();
        }

        private async Task RefreshMenuItems()
        {
            var menuTable = AzureManager.CurrentInstance.CurrentClient.GetTable<Menu>();
            var groupedItems = (await menuTable.ToListAsync()).GroupBy(x => x.Category);

            List<string> userFavItems = new List<string>();

            if (AzureManager.CurrentInstance.CurrentClient.CurrentUser != null)
            {
                var userFavTable = AzureManager.CurrentInstance.CurrentClient.GetTable<UserFavs>();
                userFavItems = await userFavTable.Where(x => x.UserID == AzureManager.CurrentInstance.CurrentClient.CurrentUser.UserId)
                    .Select(x => x.MenuID)
                    .ToListAsync();
            }

            List<MenuCategory> cats = new List<MenuCategory>();
            foreach (var c in groupedItems)
            {
                var cat = new MenuCategory(c.Key);
                var catItems = c.OrderBy(x => x.Name).ToList(); // Order items alphabetically within categories

                foreach (var i in catItems)
                {
                    FavMenuItem favItem = new FavMenuItem
                    {
                        Category = i.Category,
                        createdAt = i.createdAt,
                        Description = i.Description,
                        ID = i.ID,
                        IsDeleted = i.IsDeleted,
                        IsGlutenFree = i.IsGlutenFree,
                        IsVegan = i.IsVegan,
                        IsVegetarian = i.IsVegetarian,
                        Name = i.Name,
                        Price = i.Price,
                        updatedAt = i.updatedAt,
                        version = i.version,
                        IsFavourite = userFavItems.Count > 0 ? userFavItems.Contains(i.ID) : false
                    };
                    cat.Add(favItem);
                }

                cats.Add(cat);
            }
            menu.ItemsSource = cats;

            loadingIndicator.IsRunning = false;
            await loadingIndicator.TranslateTo(0, -loadingIndicator.Height, 250, Easing.SinInOut);
            loadingIndicator.IsVisible = false;
        }

        private async void menu_Refreshing(object sender, EventArgs e)
        {
            await RefreshMenuItems();
            menu.EndRefresh();
        }

        private void OnSelect(object sender, SelectedItemChangedEventArgs e)
        {
            // Disable selection
            ((ListView)sender).SelectedItem = null;
        }

        private void OnTap(object sender, ItemTappedEventArgs e)
        {
            var item = (FavMenuItem)e.Item;
            Device.OpenUri(new Uri($"https://fabrikamfood.azurewebsites.net/menu/{item.ID}"));
        }

        private async void FavTapped(object sender, TappedEventArgs e)
        {
            var img = (Image)sender;
            var favItem = (FavMenuItem)img.BindingContext;
            var id = favItem.ID;

            var favTable = AzureManager.CurrentInstance.CurrentClient.GetTable<UserFavs>();
            if (favItem.IsFavourite)
            {
                var items = await favTable.Where(x => x.MenuID == favItem.ID && x.UserID == AzureManager.CurrentInstance.CurrentClient.CurrentUser.UserId).ToListAsync();
                await favTable.DeleteAsync(items[0]);
                favItem.IsFavourite = false;
                img.Source = ImageSource.FromFile("star_border_24px.png");
            }
            else
            {
                await favTable.InsertAsync(new UserFavs
                {
                    MenuID = favItem.ID,
                    UserID = AzureManager.CurrentInstance.CurrentClient.CurrentUser.UserId
                });
                favItem.IsFavourite = true;
                img.Source = ImageSource.FromFile("star_24px.png");
            }
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            await RefreshMenuItems();
        }
    }

    // List of menu items
    class MenuCategory : List<FavMenuItem>
    {
        public string Title { get; set; }

        public MenuCategory(string title)
        {
            Title = title;
        }
    }

    // Menu item with favourite status
    class FavMenuItem : Menu
    {
        public bool IsFavourite { get; set; }

        public string Img
        {
            get
            {
                return IsFavourite ? "star_24px.png" : "star_border_24px.png"; // Only display star for auth'd users
            }
        }
    }

    public class AuthTemplateSelector : DataTemplateSelector
    {
        public DataTemplate AuthTemplate { get; set; }
        public DataTemplate NoAuthTemplate { get; set; }

        protected override DataTemplate OnSelectTemplate(object item, BindableObject container)
        {
            return AzureManager.CurrentInstance.CurrentClient.CurrentUser != null ? AuthTemplate : NoAuthTemplate;
        }
    }
}
