using Figgle;
using PersonDBGenerator.Helper;
using PersonDBGenerator.Model.DatabaseEngineOptions;
using PersonDBGenerator.Model.ServiceAPIModel;
using PersonDBGenerator.Repository;
using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace PersonDBGenerator
{
    class Program
    {
        static async Task Main(string[] args)
        {
            // Carga el titulo y alguna información extra.
            Header();

            #region Seleccionar motor de base de datos.
            //  Elegir que gestor de bases de datos utilizar.
            ConsoleEx.WriteLineColor("En que gestor de bases de datos desea guardar la información recibida?:", ConsoleColor.Green);
            ConsoleEx.WriteLineColor("Debe tener configurada previamente la cadena de conexión en el archivo \"config.json\".", ConsoleColor.DarkGreen);
            ConsoleEx.WriteLineColor("[1]: SQL Server \n[2]: PostgreSQL\n[3]: SQLite", ConsoleColor.DarkGreen);
            ConsoleEx.WriteColor("Ingrese el motor elegido: ", ConsoleColor.Cyan);
            var selectDatabaseEngine = ConsoleEx.ReadLineAndValidate<int>(input => !Validate.ValidateRange(input, 1, 3), "Ingrese un valor correcto: ");
            #endregion

            #region Leer la cantidad de cantidad de registros a insertar.
            //  Ingresar la cantidad de registros a generar.
            ConsoleEx.WriteLineColor("Ingrese la cantidad de registros que desea añadir a la base de datos:", ConsoleColor.Yellow);
            ConsoleEx.WriteLineColor("Máximo soportado por la API: 5000 por solicitud.\nPuede solicitar hasta 20.000 registros en 4 solicitudes por separado.", ConsoleColor.DarkYellow);
            ConsoleEx.WriteColor("Ingrese la cantidad de registros a solicitar: ", ConsoleColor.Cyan);
            var QuantityToAdd = ConsoleEx.ReadLineAndValidate<int>(input => !Validate.ValidateRange(input, 1, 20000), "Ingrese un valor correcto: ");
            #endregion

            #region Realizar solicitud al servicio y las guarda en la base de datos.
            await ProcessAndSaveInformationReceived(QuantityToAdd, (DatabaseEngine)selectDatabaseEngine);
            #endregion

            //  Salida del programa.
            Console.Write("\nPresione una tecla para salir...");
            Console.ReadKey();
        }

        static void Header()
        {
            Console.Title = "PersonDBGenerator: SQL Server, PostgreSQL & SQLite";
            Console.OutputEncoding = System.Text.Encoding.Unicode;
            ConsoleEx.WriteLineColor(FiggleFonts.Stop.Render("PersonDBGenerator"), ConsoleColor.Red);
            ConsoleEx.WriteLineColor("Autor: Matias Schwinch\nRepositorio: https://github.com/MatiasSchwinch\nAPI utilizada: https://randomuser.me\n", ConsoleColor.Blue);
        }

        static async Task ProcessAndSaveInformationReceived(int quantityToAdd, DatabaseEngine engine)
        {
            ConsoleEx.WriteLineInfo("Conectando con la API...");

            Connection<ServiceAPIModel> connection = new($"https://randomuser.me/api/");

            //  Conexión con la API.
            if (quantityToAdd <= 5000)
            {
                var response = await connection.GetDeserializeData($"?results={quantityToAdd}");

                if (response is null)
                {
                    ConsoleEx.WriteLineColor("No se han recibido datos de la API.");
                    Environment.Exit(1);
                }

                ConsoleEx.WriteLineInfo("Registros recibidos...");

                await SaveInformationInDatabase(response, engine, true);
            }
            else
            {
                await foreach (var currentResponse in connection.GetDeserializeBulkData("?results={0}", quantityToAdd))
                {
                    if (currentResponse is null)
                    {
                        ConsoleEx.WriteLineColor("No se han recibido datos de la API.");
                        Environment.Exit(1);
                    }

                    ConsoleEx.WriteLineInfo("Registros recibidos...");

                    await SaveInformationInDatabase(currentResponse, engine, false);
                }
            }
        }

        static async Task SaveInformationInDatabase(ServiceAPIModel dataReceived, DatabaseEngine engine, bool showRecords = true)
        {
            var stopwatch = new Stopwatch();

            try
            {
                using DatabaseConnectorContext dB = new(engine);

                stopwatch.Start();

                if (showRecords)
                {
                    var i = 0;
                    foreach (var item in dataReceived.Results)
                    {
                        dB.Persons.Add(item);
                        Console.WriteLine($"Registro listado no. {++i}: {item}");
                    }
                }
                else
                {
                    dB.Persons.AddRange(dataReceived.Results);
                }

                ConsoleEx.WriteLineInfo("Guardando registros...");

                //  Guardado de los registros en la Base de datos.
                await dB.SaveChangesAsync();

                stopwatch.Stop();

                ConsoleEx.WriteLineColor(string.Format("► Los registros se añadieron en la base de datos de {0} correctamente en {1} ms.", engine, stopwatch.Elapsed.TotalMilliseconds), ConsoleColor.Green);
            }
            catch (Exception e)
            {
                ConsoleEx.WriteLineColor($"No se pudieron añadir los registros debido a un error: {e.Message}", ConsoleColor.Red);
            }
        }
    }
}