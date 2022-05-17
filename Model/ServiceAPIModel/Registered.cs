using System;
using System.Text.Json.Serialization;

namespace PersonDBGenerator.Model.ServiceAPIModel
{
    #region DataAnnotations

    public class Registered
    {
        public int RegisteredID { get; set; }

        [JsonPropertyName("date")]
        public DateTime Date { get; set; } = DateTime.Now;

        [JsonPropertyName("age")]
        public int Age { get; set; }

        public Person Person { get; set; }
    }
    #endregion
}