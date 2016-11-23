using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Newtonsoft.Json;

namespace FabrikamFoodApp.DataModels
{
    class Users
    {
        [JsonProperty(PropertyName = "id")]
        public string ID { get; set; }

        [JsonProperty(PropertyName = "fbid")]
        public string FacebookID { get; set; }

        [JsonProperty(PropertyName = "home")]
        public string Home { get; set; }
    }
}
