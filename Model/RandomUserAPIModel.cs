using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace GeneradorBaseDatos.Model
{
    // Modelo de respuesta de la API (https://randomuser.me).

    #region DataAnnotations
    public class Coordinates
    {
        [Key, Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CoordinatesID { get; set; }
        public int LocationID { get; set; }

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

        [ForeignKey("LocationID")]
        public Location Location { get; set; }
    }

    public class Timezone
    {
        [Key, Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int TimezoneID { get; set; }
        public int LocationID { get; set; }

        [MaxLength(10)]
        [JsonPropertyName("offset")]
        public string Offset { get; set; }

        [MaxLength(100)]
        [JsonPropertyName("description")]
        public string Description { get; set; }

        [ForeignKey("LocationID")]
        public Location Location { get; set; }
    }

    public class Street
    {
        [Key, Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int StreetID { get; set; }
        public int LocationID { get; set; }

        [JsonPropertyName("number")]
        public int Number { get; set; }

        [MaxLength(100)]
        [JsonPropertyName("name")]
        public string Name { get; set; }

        [ForeignKey("LocationID")]
        public Location Location { get; set; }
    }

    public class Location
    {
        [Key, Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int LocationID { get; set; }
        public int PersonID { get; set; }

        [MaxLength(100)]
        [JsonPropertyName("city")]
        public string City { get; set; }

        [MaxLength(100)]
        [JsonPropertyName("state")]
        public string State { get; set; }

        [MaxLength(100)]
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

        [MaxLength(20)]
        public string Postcode { get; set; }

        [JsonPropertyName("street")]
        public virtual Street Street { get; set; }

        [JsonPropertyName("coordinates")]
        public virtual Coordinates Coordinates { get; set; }

        [JsonPropertyName("timezone")]
        public virtual Timezone Timezone { get; set; }

        [ForeignKey("PersonID")]
        public Person Person { get; set; }
    }

    public class Login
    {
        [Key, Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int LoginID { get; set; }
        public int PersonID { get; set; }

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

        [ForeignKey("PersonID")]
        public Person Person { get; set; }
    }

    public class Dob
    {
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
        public int PersonID { get; set; }

        [JsonPropertyName("date")]
        public DateTime Date { get; set; }

        [JsonPropertyName("age")]
        public int Age { get; set; }

        [ForeignKey("PersonID")]
        public Person Person { get; set; }
    }

    public class Picture
    {
        [Key, Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int PictureID { get; set; }

        public int PersonID { get; set; }

        [MaxLength(100)]
        [JsonPropertyName("large")]
        public string Large { get; set; }

        [MaxLength(100)]
        [JsonPropertyName("medium")]
        public string Medium { get; set; }

        [MaxLength(100)]
        [JsonPropertyName("thumbnail")]
        public string Thumbnail { get; set; }

        [ForeignKey("PersonID")]
        public Person Person { get; set; }
    }

    public class Name
    {
        [JsonPropertyName("title")]
        public string Title { get; set; }

        [JsonPropertyName("first")]
        public string First { get; set; }

        [JsonPropertyName("last")]
        public string Last { get; set; }
    }

    public class Person
    {
        [Key, Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int PersonID { get; set; }

        #region Nombre
        [JsonPropertyName("name")]
        public Name Name
        {
            set
            {
                Title = value.Title;
                FirstName = value.First;
                LastName = value.Last;
            }
        }
        [MaxLength(20)]
        public string Title { get; set; }
        [Required]
        [MaxLength(30)]
        public string FirstName { get; set; }
        [Required]
        [MaxLength(30)]
        public string LastName { get; set; }
        #endregion

        #region Género
        [JsonPropertyName("gender")]
        public string tempGender
        {
            set
            {
                Gender = value switch
                {
                    "male" => Genders.Male,
                    "female" => Genders.Female,
                    _ => Genders.NotSelected,
                };
            }
        }
        public Genders Gender { get; set; }
        #endregion

        #region Edad
        [JsonPropertyName("dob")]
        public Dob Dob
        {
            set
            {
                Date = value.Date;
                Age = value.Age;
            }
        }
        [Required]
        public DateTime Date { get; set; }
        public int Age { get; set; }
        #endregion

        #region Datos de contacto
        [Required]
        [MaxLength(100)]
        [JsonPropertyName("email")]
        public string Email { get; set; }

        [MaxLength(30)]
        [JsonPropertyName("phone")]
        public string Phone { get; set; }

        [MaxLength(30)]
        [JsonPropertyName("cell")]
        public string Cell { get; set; }
        #endregion

        #region Otros datos
        [MaxLength(4)]
        [JsonPropertyName("nat")]
        public string Nationality { get; set; }

        [JsonPropertyName("location")]
        public virtual Location? Location { get; set; }

        [JsonPropertyName("login")]
        public virtual Login? Login { get; set; }

        [JsonPropertyName("registered")]
        public virtual Registered? Registered { get; set; }

        [JsonPropertyName("picture")]
        public virtual Picture? Picture { get; set; }
        #endregion

        public override string ToString()
        {
            return string.Format("{0}. {1} {2} (Edad: {3})", Title, FirstName, LastName, Age);
        }
    }

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
    #endregion
}
