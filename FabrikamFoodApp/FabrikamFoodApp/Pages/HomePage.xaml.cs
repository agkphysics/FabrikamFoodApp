using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace FabrikamFoodApp.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class HomePage : ContentPage
    {
        public HomePage()
        {
            //var beachImage = new Image { WidthRequest = 500, HeightRequest = 500 };
            //beachImage.Source = ImageSource.FromFile("hot_chocolate.jpg");
            //beachImage.Aspect = Aspect.AspectFit;

            //Content = new StackLayout
            //{
            //    Children = {
            //        beachImage
            //    },
            //    Padding = new Thickness(0),
            //    VerticalOptions = LayoutOptions.CenterAndExpand,
            //    HorizontalOptions = LayoutOptions.CenterAndExpand
            //};
            InitializeComponent();
        }
    }
}
