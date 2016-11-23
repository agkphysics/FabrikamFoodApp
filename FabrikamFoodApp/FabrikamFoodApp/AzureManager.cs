using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.WindowsAzure.MobileServices;
using Newtonsoft.Json.Linq;
using FabrikamFoodApp.DataModels;
using System.Net.Http;

namespace FabrikamFoodApp
{
    public class SocialLoginResult
    {
        public Message Message { get; set; }
    }

    public class Message
    {
        public string SocialId
        {
            get { return string.IsNullOrEmpty(Sub) ? Id : Sub; }
        }

        public string Email { get; set; }
        public string Sub { get; set; }
        public string First_Name { get; set; }
        public string Id { get; set; }
    }

    public class AzureManager
    {
        private static MobileServiceClient client;
        private static AzureManager currentInstance = new AzureManager();

        public MobileServiceClient CurrentClient
        {
            get { return client; }
        }

        public AzureManager()
        {
            client = new MobileServiceClient("https://fabrikamfood.azurewebsites.net");
        }

        public static AzureManager CurrentInstance
        {
            get { return currentInstance; }
        }

        public string GetUserProperties()
        {
            client.GetTable<Users>();
            return "";
        }

        public async Task<SocialLoginResult> GetUserData()
        {
            return await client.InvokeApiAsync<SocialLoginResult>("getuserinfo", HttpMethod.Get, null);
        }
    }
}
