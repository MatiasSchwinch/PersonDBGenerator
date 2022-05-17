using System.Text.Json.Serialization;

namespace PersonDBGenerator.Model.ServiceAPIModel
{
    #region DataAnnotations

    public class Location
    {
        public int LocationID { get; set; }
        public int? CoordinatesID { get; set; }
        public int? TimezoneID { get; set; }

        public int StreetNumber { get; set; }

        public string StreetName { get; set; }

        [JsonPropertyName("city")]
        public string City { get; set; }

        [JsonPropertyName("state")]
        public string State { get; set; }

        [JsonPropertyName("country")]
        public string Country { get; set; }

        [JsonPropertyName("postcode")]
        public object tempPostCode
        {
            set
            {
                Postcode = value.ToString();
            }
        }
        public string Postcode { get; set; }

        [JsonPropertyName("street")]
        public virtual Street Street
        {
            set
            {
                StreetNumber = value.Number;
                StreetName = value.Name;
            }
        }

        [JsonPropertyName("coordinates")]
        public virtual Coordinates Coordinates { get; set; }

        [JsonPropertyName("timezone")]
        public virtual Timezone Timezone { get; set; }

        public Person Person { get; set; }
    }
    #endregion
}