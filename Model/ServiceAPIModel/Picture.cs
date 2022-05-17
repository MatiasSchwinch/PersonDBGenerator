using System.Text.Json.Serialization;

namespace PersonDBGenerator.Model.ServiceAPIModel
{
    #region DataAnnotations

    public class Picture
    {
        public int PictureID { get; set; }

        [JsonPropertyName("large")]
        public string Large { get; set; }

        [JsonPropertyName("medium")]
        public string Medium { get; set; }

        [JsonPropertyName("thumbnail")]
        public string Thumbnail { get; set; }

        public Person Person { get; set; }
    }
    #endregion
}