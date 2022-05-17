using System.Text.Json.Serialization;

namespace PersonDBGenerator.Model.ServiceAPIModel
{
    #region DataAnnotations

    public class Login
    {
        public int LoginID { get; set; }

        [JsonPropertyName("uuid")]
        public string Uuid { get; set; }

        [JsonPropertyName("username")]
        public string Username { get; set; }

        [JsonPropertyName("password")]
        public string Password { get; set; }

        [JsonPropertyName("salt")]
        public string Salt { get; set; }

        [JsonPropertyName("md5")]
        public string Md5 { get; set; }

        [JsonPropertyName("sha1")]
        public string Sha1 { get; set; }

        [JsonPropertyName("sha256")]
        public string Sha256 { get; set; }

        public Person Person { get; set; }
    }
    #endregion
}