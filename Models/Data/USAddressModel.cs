using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Models.Data
{
    public class USAddressModel
    {
        [JsonPropertyName("zip")]
        public string Zip { get; set; }
        [JsonPropertyName("type")]
        public string Type { get; set; }
        [JsonPropertyName("decomissioned")]
        public string Decomissioned { get; set; }
        [JsonPropertyName("acceptable_cities")]
        public string AcceptableCities { get; set; }
        [JsonPropertyName("unacceptable_cities")]
        public string UnacceptableCities { get; set; }
        [JsonPropertyName("state")]
        public string State { get; set; }
        [JsonPropertyName("county")]
        public string County { get; set; }
        [JsonPropertyName("timezone")]
        public string Timezone { get; set; }
        [JsonPropertyName("area_codes")]
        public string AreaCodes { get; set; }
        [JsonPropertyName("world_region")]
        public string WorldRegion { get; set; }
        [JsonPropertyName("Country")]
        public string Country { get; set; }
        [JsonPropertyName("latitude")]
        public string Latitude { get; set; }
        [JsonPropertyName("longitude")]
        public string Longitude { get; set; }
        [JsonPropertyName("irs_estimated_poppulation")]
        public string IrsEstimatedPopulation { get; set; }
    }
}
