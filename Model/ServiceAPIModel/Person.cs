using System;
using System.Text.Json.Serialization;

namespace PersonDBGenerator.Model.ServiceAPIModel
{
    #region DataAnnotations

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
    #endregion
}