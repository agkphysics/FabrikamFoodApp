using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace FabrikamFoodApp.DataModels
{
    public class Menu
    {
        [JsonProperty(PropertyName = "id")]
        public string ID { get; set; }

        [JsonProperty(PropertyName = "createdAt")]
        public DateTime createdAt { get; set; }

        [JsonProperty(PropertyName = "updatedAt")]
        public DateTime updatedAt { get; set; }

        [JsonProperty(PropertyName = "version")]
        public string version { get; set; }

        [JsonProperty(PropertyName = "deleted")]
        public bool IsDeleted { get; set; }

        [JsonProperty(PropertyName = "category")]
        public string Category { get; set; }

        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }

        [JsonProperty(PropertyName = "price")]
        public double Price { get; set; }

        [JsonProperty(PropertyName = "vegetarian")]
        public bool IsVegetarian { get; set; }

        [JsonProperty(PropertyName = "vegan")]
        public bool IsVegan { get; set; }

        [JsonProperty(PropertyName = "glutenfree")]
        public bool IsGlutenFree { get; set; }

        [JsonProperty(PropertyName = "description")]
        public string Description { get; set; }
    }
}
