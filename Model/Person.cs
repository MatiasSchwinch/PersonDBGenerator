using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace GeneradorBaseDatos.Model
{
    [NotMapped]
    public class Name
    {
        [JsonPropertyName("first")]
        public string FirstName { get; set; }

        [JsonPropertyName("last")]
        public string LastName { get; set; }
    }

    public class Street
    {
        [Key, Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int StreetID { get; set; }

        [JsonPropertyName("number")]
        public int Number { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }
    }

    public class Location
    {
        [Key, Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int LocationID { get; set; }

        [JsonPropertyName("street")]
        public Street Street { get; set; }

        [JsonPropertyName("city")]
        public string City { get; set; }

        [JsonPropertyName("state")]
        public string State { get; set; }

        [JsonPropertyName("country")]
        public string Country { get; set; }

    }

    [NotMapped]
    public class Dob
    {
        [JsonPropertyName("date")]
        public DateTime Date { get; set; }

        [JsonPropertyName("age")]
        public int Age { get; set; }
    }

    public class Person
    {
        [JsonPropertyName("gender")]
        public string Gender { get; set; }

        [NotMapped]
        [JsonPropertyName("name")]
        public Name Name { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public DateTime Date { get; set; }

        public int Age { get; set; }

        [JsonPropertyName("location")]
        public Location Location { get; set; }

        [JsonPropertyName("email")]
        public string Email { get; set; }

        [JsonPropertyName("dob")]
        public Dob Dob { get; set; }

        [JsonPropertyName("phone")]
        public string Phone { get; set; }

        [Key, Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int PersonId { get; set; }

        [JsonPropertyName("nat")]
        public string Nationality { get; set; }

        public override string ToString()
        {
            return $"{FirstName} {LastName} (Edad: {Age})";
        }
    }

    public class API
    {
        [JsonPropertyName("results")]
        public List<Person> Results { get; set; }
    }

}
