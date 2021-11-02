using GeneradorBaseDatos.Model;
using GeneradorBaseDatos.Service;
using System;
using System.Collections;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Figgle;

namespace GeneradorBaseDatos
{
    class Program
    {
        static async Task Main(string[] args)
        {
            Console.Title = "Generador de bases de datos: SQLite";

            int QuantityToAdd;
            string ReadQuantity;
            bool ExitLoop = false;
            int i = 0;

            Console.WriteLine(FiggleFonts.Small.Render("Generador de bases de datos"));//Italic
            Console.WriteLine("Autor: Matias Schwinch");
            Console.WriteLine("Repositorio: https://github.com/MatiasSchwinch");
            Console.WriteLine("API utilizada: https://randomuser.me/\n");

            Console.WriteLine("Ingrese la cantidad de registros (máximo soportado por la API: 5000) que desea añadir a la base de datos:");

            do
            {
                ReadQuantity = Console.ReadLine();

                if (!int.TryParse(ReadQuantity, out QuantityToAdd)) { Console.WriteLine($"\"{ReadQuantity}\" no es un número, ingrese un numero valido:"); continue; }
                if (QuantityToAdd > 5000) { Console.WriteLine("El máximo que se pueden añadir son 5000 registros"); continue; }

                ExitLoop = true;

            } while (!ExitLoop);

            //  Conexión con la API.
            Connection<API> connection = new("https://randomuser.me/api/");
            API dataReceived = await connection.GetDeserializeData(QuantityToAdd);
            if (dataReceived is null) { Environment.Exit(1); }

            using DBConnector dB = new();

            foreach (var item in dataReceived.Results)
            {
                Person person = new();
                person = item;
                person.FirstName = item.Name.FirstName;
                person.LastName = item.Name.LastName;
                person.Date = item.Dob.Date;
                person.Age = item.Dob.Age;

                dB.Person.Add(person);
                Console.WriteLine($"Registro listado no.:{++i} - {person}");
            }

            //  Guardado de los registros en la Base de datos.
            try
            {
                dB.SaveChanges();
                Console.WriteLine("Los registros se añadieron a la base de datos correctamente.");
            }
            catch (Exception e)
            {
                Console.WriteLine($"No se pudieron añadir los registros debido a un error: {e.Message}");
            }
            
        }
    }

    class Connection<T> where T : class
    {
        private static HttpClient _client = new();
        private readonly string _apiURL;

        public Connection(string pURL)
        {
            _apiURL = pURL;
        }

        public async Task<T> GetDeserializeData(int pQuantityToGenerate = 10)
        {
            try
            {
                string _responseBody = await _client.GetStringAsync($"{_apiURL}?results={pQuantityToGenerate}");
                T _deserialize = JsonSerializer.Deserialize<T>(_responseBody);

                return _deserialize;
            }
            catch (Exception e)
            {
                Console.WriteLine($"Se ha producido un error: {e.Message}");
                return default;
            }
        }
    }
}
