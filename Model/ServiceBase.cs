using PersonDBGenerator.Interface;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace PersonDBGenerator.Model
{
    public abstract class ServiceBase<T> : IService where T : class
    {
        [JsonPropertyName("results")]
        public IList<T> Results { get; set; }
    }
}
