using System.Text.Json.Serialization;

namespace PersonDBGenerator.Model.ServiceAPIModel
{
    // Modelo de respuesta de la API (https://randomuser.me).

    #region DataAnnotations
    public class Coordinates
    {
        public int CoordinatesID { get; set; }

        [JsonPropertyName("latitude")]
        public string tempLatitude
        {
            set
            {
                Latitude = decimal.TryParse(value, out _) ? decimal.Parse(value) : 0m;
            }
        }
        public decimal Latitude { get; set; }

        [JsonPropertyName("longitude")]
        public string tempLongitude
        {
            set
            {
                Longitude = decimal.TryParse(value, out _) ? decimal.Parse(value) : 0m;
            }
        }
        public decimal Longitude { get; set; }

        public Location Location { get; set; }
    }
    #endregion
}