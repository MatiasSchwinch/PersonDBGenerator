using System.Text.Json.Serialization;

namespace PersonDBGenerator.Model.ServiceAPIModel
{
    #region DataAnnotations

    public class Timezone
    {
        public int TimezoneID { get; set; }

        [JsonPropertyName("offset")]
        public string Offset { get; set; }

        [JsonPropertyName("description")]
        public string Description { get; set; }

        public Location Location { get; set; }
    }
    #endregion
}