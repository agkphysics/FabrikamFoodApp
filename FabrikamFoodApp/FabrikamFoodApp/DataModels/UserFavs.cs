using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FabrikamFoodApp.DataModels
{
    class UserFavs
    {
        [JsonProperty(PropertyName = "id")]
        public string ID { get; set; }

        [JsonProperty(PropertyName = "uid")]
        public string UserID { get; set; }

        [JsonProperty(PropertyName = "menuid")]
        public string MenuID { get; set; }
    }
}
