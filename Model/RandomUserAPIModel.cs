using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace GeneradorBaseDatos.Model
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

        public Location Location { get; set; }
    }

    public class Timezone
    {
        public int TimezoneID { get; set; }

        [JsonPropertyName("offset")]
        public string Offset { get; set; }

        [JsonPropertyName("description")]
        public string Description { get; set; }

        public Location Location { get; set; }
    }

    public class Street
    {
        [JsonPropertyName("number")]
        public int Number { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }
    }

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

    public class Dob
    {
        [JsonPropertyName("date")]
        public DateTime Date { get; set; }

        [JsonPropertyName("age")]
        public int Age { get; set; }
    }

    public class Registered
    {
        public int RegisteredID { get; set; }

        [JsonPropertyName("date")]
        public DateTime Date { get; set; } = DateTime.Now;

        [JsonPropertyName("age")]
        public int Age { get; set; }

        public Person Person { get; set; }
    }

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
        public int PersonID { get; set; }
        public int? LocationID { get; set; }
        public int? LoginID { get; set; }
        public int? RegisteredID { get; set; }
        public int? PictureID { get; set; }

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
        public string Title { get; set; }
        public string FirstName { get; set; }
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
        public DateTime Date { get; set; }
        public int Age { get; set; }
        #endregion

        #region Datos de contacto
        [JsonPropertyName("email")]
        public string Email { get; set; }

        [JsonPropertyName("phone")]
        public string Phone { get; set; }

        [JsonPropertyName("cell")]
        public string Cell { get; set; }
        #endregion

        #region Otros datos
        [JsonPropertyName("nat")]
        public string Nationality { get; set; }

        [JsonPropertyName("location")]
        public virtual Location Location { get; set; }

        [JsonPropertyName("login")]
        public virtual Login Login { get; set; }

        [JsonPropertyName("registered")]
        public virtual Registered Registered { get; set; }

        [JsonPropertyName("picture")]
        public virtual Picture Picture { get; set; }
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