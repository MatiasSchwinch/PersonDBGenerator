using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace GeneradorBaseDatos.Model
{
    // Modelo de respuesta de la API (https://randomuser.me).
    public class Coordinates
    {
        [Key, Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CoordinatesID { get; set; }

        [JsonPropertyName("latitude")]
        public string tempLatitude
        {
            set
            {
                Latitude = (decimal.TryParse(value, out _)) ? decimal.Parse(value) : 0m;
            }
        }
        public decimal Latitude { get; set; }

        [JsonPropertyName("longitude")]
        public string tempLongitude
        {
            set
            {
                Longitude = (decimal.TryParse(value, out _)) ? decimal.Parse(value) : 0m;
            }
        }
        public decimal Longitude { get; set; }
    }

    public class Timezone
    {
        [Key, Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int TimezoneID { get; set; }

        [MaxLength(10)]
        [JsonPropertyName("offset")]
        public string Offset { get; set; }

        [MaxLength(100)]
        [JsonPropertyName("description")]
        public string Description { get; set; }
    }

    public class Street
    {
        [Key, Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int StreetID { get; set; }

        [JsonPropertyName("number")]
        public int Number { get; set; }

        [MaxLength(100)]
        [JsonPropertyName("name")]
        public string Name { get; set; }
    }

    public class Location
    {
        [Key, Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int LocationID { get; set; }

        [ForeignKey("StreetID")]
        [JsonPropertyName("street")]
        public virtual Street Street { get; set; }

        [MaxLength(100)]
        [JsonPropertyName("city")]
        public string City { get; set; }

        [MaxLength(100)]
        [JsonPropertyName("state")]
        public string State { get; set; }

        [MaxLength(100)]
        [JsonPropertyName("country")]
        public string Country { get; set; }

        [NotMapped]
        [JsonPropertyName("postcode")]
        public object tempPostCode
        {
            set
            {
                Postcode = value.ToString();
            }
        }

        [MaxLength(20)]
        public string Postcode { get; set; }

        [ForeignKey("CoordinatesID")]
        [JsonPropertyName("coordinates")]
        public virtual Coordinates Coordinates { get; set; }

        [ForeignKey("TimezoneID")]
        [JsonPropertyName("timezone")]
        public virtual Timezone Timezone { get; set; }
    }

    public class Login
    {
        [Key, Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int LoginID { get; set; }

        [MaxLength(100)]
        [JsonPropertyName("uuid")]
        public string Uuid { get; set; }

        [MaxLength(50)]
        [JsonPropertyName("username")]
        public string Username { get; set; }

        [MaxLength(30)]
        [JsonPropertyName("password")]
        public string Password { get; set; }

        [MaxLength(100)]
        [JsonPropertyName("salt")]
        public string Salt { get; set; }

        [MaxLength(100)]
        [JsonPropertyName("md5")]
        public string Md5 { get; set; }

        [MaxLength(100)]
        [JsonPropertyName("sha1")]
        public string Sha1 { get; set; }

        [MaxLength(100)]
        [JsonPropertyName("sha256")]
        public string Sha256 { get; set; }
    }

    public class Dob
    {
        [Key, Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int DobID { get; set; }

        [Required]
        [JsonPropertyName("date")]
        public DateTime Date { get; set; }

        [JsonPropertyName("age")]
        public int Age { get; set; }
    }

    public class Registered
    {
        [Key, Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int RegisteredID { get; set; }

        [JsonPropertyName("date")]
        public DateTime Date { get; set; }

        [JsonPropertyName("age")]
        public int Age { get; set; }
    }

    public class Picture
    {
        [Key, Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int PictureID { get; set; }

        [MaxLength(100)]
        [JsonPropertyName("large")]
        public string Large { get; set; }

        [MaxLength(100)]
        [JsonPropertyName("medium")]
        public string Medium { get; set; }

        [MaxLength(100)]
        [JsonPropertyName("thumbnail")]
        public string Thumbnail { get; set; }
    }

    public class Name
    {
        [Key, Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int NameID { get; set; }

        [MaxLength(15)]
        [JsonPropertyName("title")]
        public string Title { get; set; }

        [Required]
        [MaxLength(30)]
        [JsonPropertyName("first")]
        public string First { get; set; }

        [Required]
        [MaxLength(30)]
        [JsonPropertyName("last")]
        public string Last { get; set; }
    }

    public class Person
    {
        [Key, Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int PersonID { get; set; }

        [NotMapped]
        [JsonPropertyName("gender")]
        public string tempGender
        {
            set
            {
                switch (value)
                {
                    case "male":
                        Gender = Genders.Male;
                        break;
                    case "female":
                        Gender = Genders.Female;
                        break;
                    default:
                        Gender = Genders.NotSelected;
                        break;
                }
            }
        }

        public Genders Gender { get; set; }

        [ForeignKey("NameID")]
        [JsonPropertyName("name")]
        public virtual Name Name { get; set; }

        [ForeignKey("LocationID")]
        [JsonPropertyName("location")]
        public virtual Location Location { get; set; }

        [MaxLength(100)]
        [JsonPropertyName("email")]
        public string Email { get; set; }

        [ForeignKey("LoginID")]
        [JsonPropertyName("login")]
        public virtual Login Login { get; set; }

        [ForeignKey("DobID")]
        [JsonPropertyName("dob")]
        public virtual Dob Dob { get; set; }

        [ForeignKey("RegisteredID")]
        [JsonPropertyName("registered")]
        public virtual Registered Registered { get; set; }

        [MaxLength(30)]
        [JsonPropertyName("phone")]
        public string Phone { get; set; }

        [MaxLength(30)]
        [JsonPropertyName("cell")]
        public string Cell { get; set; }

        [ForeignKey("PictureID")]
        [JsonPropertyName("picture")]
        public virtual Picture Picture { get; set; }

        [MaxLength(4)]
        [JsonPropertyName("nat")]
        public string Nationality { get; set; }

        public override string ToString()
        {
            return string.Format("{0}. {1} {2} (Edad: {3})", Name.Title, Name.First, Name.Last, Dob.Age);
        }
    }

    [NotMapped]
    public class RandomUserAPIModel
    {
        [JsonPropertyName("results")]
        public List<Person> Results { get; set; }
    }

    public enum Genders
    {
        Male,
        Female,
        NotSelected
    }
}
