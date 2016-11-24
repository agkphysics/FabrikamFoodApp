using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Newtonsoft.Json;

namespace FabrikamFoodApp.DataModels
{
    public class Users
    {
        [JsonProperty(PropertyName = "id")]
        public string ID { get; set; }

        [JsonProperty(PropertyName = "fbid")]
        public string FacebookID { get; set; }

        [JsonProperty(PropertyName = "homelat")]
        public double Homelat { get; set; }

        [JsonProperty(PropertyName = "homelon")]
        public double Homelon { get; set; }

        [JsonProperty(PropertyName = "address")]
        public string Address { get; set; }

        [JsonProperty(PropertyName = "email")]
        public string Email { get; set; }
    }
}
