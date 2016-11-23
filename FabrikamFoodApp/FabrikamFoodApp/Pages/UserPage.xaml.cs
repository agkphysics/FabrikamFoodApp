using FabrikamFoodApp.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Maps;

namespace FabrikamFoodApp.Pages
{
    public partial class UserPage : ContentPage
    {
        private string Username { get; set; }

        public UserPage(string username)
        {
            Title = username;
            InitializeComponent();
        }
    }
}
