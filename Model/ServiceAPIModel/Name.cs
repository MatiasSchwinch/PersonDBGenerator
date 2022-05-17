using System.Text.Json.Serialization;

namespace PersonDBGenerator.Model.ServiceAPIModel
{
    #region DataAnnotations

    public class Name
    {
        [JsonPropertyName("title")]
        public string Title { get; set; }

        [JsonPropertyName("first")]
        public string First { get; set; }

        [JsonPropertyName("last")]
        public string Last { get; set; }
    }
    #endregion
}