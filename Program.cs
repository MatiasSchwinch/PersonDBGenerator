using GeneradorBaseDatos.Model;
using GeneradorBaseDatos.Service;
using System;
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
            Console.Title = "PersonDBGenerator: SQL Server & SQLite";
            Console.OutputEncoding = System.Text.Encoding.Unicode;

            int QuantityToAdd = 0;
            int i = 0;
            
            Console.WriteLine(FiggleFonts.Stop.Render("PersonDBGenerator"));
            Console.WriteLine("Autor: Matias Schwinch");
            Console.WriteLine("Repositorio: https://github.com/MatiasSchwinch");
            Console.WriteLine("API utilizada: https://randomuser.me/\n");

            //  Ingresar la cantidad de registros a generar.
            Console.WriteLine("Ingrese la cantidad de registros (máximo soportado por la API: 5000) que desea añadir a la base de datos:");
            RequestUserNumber(1, 5000, ref QuantityToAdd, "El mínimo de registros que se pueden generar es {0}, y el máximo es {1}.");

            Console.WriteLine("Conectando con la API...");

            //  Conexión con la API.
            Connection<RandomUserAPIModel> connection = new("https://randomuser.me/api/", "?results={0}");
            RandomUserAPIModel dataReceived = await connection.GetDeserializeData(QuantityToAdd);
            if (dataReceived is null) { Console.WriteLine("No se han recibido datos de la API."); Environment.Exit(1); }

            Console.WriteLine("Registros recibidos...");

            //  Elegir que gestor de bases de datos utilizar.
            Console.WriteLine("\nEn que gestor de bases de datos desea guardar la información recibida?:\n" +
                "1: SQL Server (debe tener configurada previamente la cadena de conexión en el archivo \"config.json\")\n" +
                "2: SQLite (Genera una base de datos local junto a este ejecutable)");
            RequestUserNumber(1, 2, ref QuantityToAdd, "Solo puede elegir entre 2 opciones.");

            try
            {
                using DBConnector dB = new((DBConnector.DatabaseEngine)QuantityToAdd);

                foreach (var item in dataReceived.Results)
                {
                    dB.Persons.Add(item);
                    Console.WriteLine($"Registro listado no. {++i}: {item}");
                }

                Console.WriteLine("Guardando registros...");

                //  Guardado de los registros en la Base de datos.
                dB.SaveChanges();

                Console.WriteLine("Los registros se añadieron a la base de datos en {0} correctamente.", (DBConnector.DatabaseEngine)QuantityToAdd);
            }
            catch (Exception e)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"No se pudieron añadir los registros debido a un error: {e.Message}");
                Console.ResetColor();
            }

            //  Salida del programa.
            Console.WriteLine("\nPresione una tecla para salir...");
            Console.ReadKey();
        }

        /// <summary>
        ///     Pide al usuario que ingrese un numero entre un rango preestablecido, valida y transforma lo que el usuario ingresa y devuelve el resultado en una variable que debe ser pasada como referencia.
        /// </summary>
        /// <param name="pMin">El valor mínimo que puede ser aceptado.</param>
        /// <param name="pMax">El valor máximo que puede ser aceptado.</param>
        /// <param name="pStoreNum">Una variable que debe ser pasada como referencia, aquí es donde se va a almacenar el numero en el caso que este sea valido.</param>
        /// <param name="pValidationExceedMaxMSG">Mensaje que el usuario va a ver como feedback si excede el máximo, puede usar {0} y {1} para que se reemplacen por el numero mínimo y máximo aceptados respectivamente.</param>
        static void RequestUserNumber(int pMin, int pMax, ref int pStoreNum, string pValidationExceedMaxMSG)
        {
            bool endLoop = false;
            do
            {
                var ReadQuantity = Console.ReadLine();

                if (!int.TryParse(ReadQuantity, out pStoreNum)) { Console.WriteLine("\"{0}\" no es un número, ingrese un numero valido.", ReadQuantity); continue; }
                if (pStoreNum < pMin || pStoreNum > pMax) { Console.WriteLine(pValidationExceedMaxMSG, pMin, pMax); continue; }

                endLoop = true;

            } while (!endLoop);
        }
    }

    class Connection<T> where T : class
    {
        private static HttpClient _client = new();
        private readonly string _apiURL;
        private readonly string _apiEndpoint;

        /// <summary>
        ///     Capaz de conectarse con servicios a través de solicitudes HTTP, esta enfocado principalmente a servicios de generación de perfiles ficticios.
        /// </summary>
        /// <param name="pURL">La URL del servicio, esta debe terminar en "/"</param>
        /// <param name="pEndpoint">El endpoint que nos va a proveer la información, este debe contener un parámetro numérico, que debe ser reemplazo por "{0}". EJ: ?results=5 debe quedar ?results={0}</param>
        public Connection(string pURL, string pEndpoint)
        {
            _apiURL = pURL;
            _apiEndpoint = pEndpoint;
        }

        /// <summary>
        ///     Conecta con la API, y deserializa la información obtenida por la misma.
        /// </summary>
        /// <param name="pQuantityToGenerate">La cantidad de registros que se quieran obtener, este parámetro reemplaza el "{0}" del endpoint</param>
        /// <returns></returns>
        public async Task<T> GetDeserializeData(int pQuantityToGenerate = 10)
        {
            try
            {
                string _responseBody = await _client.GetStringAsync($"{_apiURL}{string.Format(_apiEndpoint, pQuantityToGenerate)}");
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
