using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FabrikamFoodApp.DataModels
{
    public class Stores
    {
        [JsonProperty(PropertyName = "id")]
        public string ID { get; set; }
        
        [JsonProperty(PropertyName = "lat")]
        public double lat { get; set; }

        [JsonProperty(PropertyName = "lon")]
        public double lon { get; set; }

        [JsonProperty(PropertyName = "address")]
        public string Address { get; set; }

        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }
    }
}
