using PersonDBGenerator.Interface;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace PersonDBGenerator.Helper
{
    public class Connection<T> where T : IService
    {
        private static HttpClient _client;
        private readonly string _baseUrl;

        /// <summary>
        ///     Capaz de conectarse con servicios a través de solicitudes HTTP, esta enfocado principalmente a servicios de generación de perfiles ficticios.
        /// </summary>
        public Connection(string baseUrl)
        {
            _baseUrl = baseUrl;
            _client = new();
        }

        /// <summary>
        ///     Conecta con la API, y deserializa la información obtenida por la misma.
        /// </summary>
        /// <returns></returns>
        public async Task<T> GetDeserializeData(string endpoint)
        {
            try
            {
                string _responseBody = await _client.GetStringAsync(string.Concat(_baseUrl, endpoint));
                T _deserialize = JsonSerializer.Deserialize<T>(_responseBody);

                return _deserialize;
            }
            catch (Exception e)
            {
                ConsoleEx.WriteLineColor($"Se ha producido un error: {e.Message}");
                return default;
            }
        }

        public async IAsyncEnumerable<T> GetDeserializeBulkData(string formateableEndpoint, int quantityToAdd)
        {
            var NumberRequestsPerShift = new Queue<int>();

            // Separa en numero de solicitudes en partes.
            while (quantityToAdd >= 0)
            {
                if (quantityToAdd > 5000)
                {
                    NumberRequestsPerShift.Enqueue(5000);
                    quantityToAdd -= 5000;
                }
                else
                {
                    NumberRequestsPerShift.Enqueue(quantityToAdd);
                    break;
                }
            }

            var repeats = NumberRequestsPerShift.Count;

            for (int i = 0; i < repeats; i++)
            {
                ConsoleEx.WriteLineInfo(string.Format("Procesando solicitud: {0} de {1}.", i + 1, repeats));

                var _responseBody = await GetDeserializeData(string.Format(formateableEndpoint, NumberRequestsPerShift.Dequeue()));

                yield return _responseBody;
            }
        }
    }
}
