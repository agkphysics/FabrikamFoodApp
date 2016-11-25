using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.WindowsAzure.MobileServices;
using Newtonsoft.Json.Linq;
using FabrikamFoodApp.DataModels;
using System.Net.Http;
using Xamarin.Forms.Maps;

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
        private SocialLoginResult currSocialData;

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

        public async Task<Users> GetCurrentUserFromDB()
        {
            var user = await client.GetTable<Users>().LookupAsync(client.CurrentUser.UserId);
            return user;
        }

        public async Task UpdateCurrentUserInDB(string fbid = null, string address = null, string email = null)
        {
            Users user;
            var userTable = client.GetTable<Users>();
            var u = await userTable.Where(x => x.ID == client.CurrentUser.UserId).ToListAsync();
            string currAddress = "";

            if (u.Count > 0)
            {
                currAddress = u[0].Address;
            }

            if (address != null)
            {
                var coords = await new Geocoder().GetPositionsForAddressAsync(address);
                var pos = coords.First();
                user = new Users
                {
                    ID = client.CurrentUser.UserId,
                    Address = address,
                    Homelat = pos.Latitude,
                    Homelon = pos.Longitude,
                    FacebookID = fbid == null ? currSocialData.Message.SocialId : fbid,
                    Email = email == null ? currSocialData.Message.Email : email
                };
            }
            else
            {
                if (currAddress != null && !currAddress.Equals(""))
                {
                    user = new Users
                    {
                        ID = client.CurrentUser.UserId,
                        Address = currAddress,
                        FacebookID = fbid == null ? currSocialData.Message.SocialId : fbid,
                        Email = email == null ? currSocialData.Message.Email : email
                    };
                }
                else
                {
                    user = new Users
                    {
                        ID = client.CurrentUser.UserId,
                        Address = address,
                        FacebookID = fbid == null ? currSocialData.Message.SocialId : fbid,
                        Email = email == null ? currSocialData.Message.Email : email
                    };
                }
            }
            
            
            if (u.Count > 0) await userTable.UpdateAsync(user);
            else await userTable.InsertAsync(user);
        }

        public async Task<SocialLoginResult> GetUserData()
        {
            var socialData = await client.InvokeApiAsync<SocialLoginResult>("getuserinfo", HttpMethod.Get, null);
            currSocialData = socialData;
            await UpdateCurrentUserInDB(socialData.Message.SocialId, null, socialData.Message.Email);
            return socialData;
        }

        public async Task DeleteCurrentUser()
        {
            var userTable = client.GetTable<Users>();
            var user = await userTable.LookupAsync(client.CurrentUser.UserId);
            await userTable.DeleteAsync(user);
        }
    }
}
