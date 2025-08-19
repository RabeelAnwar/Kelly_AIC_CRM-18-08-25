using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Serialization;
using Newtonsoft.Json;

namespace Utility.OutputData
{
    [JsonObject(NamingStrategyType = typeof(CamelCaseNamingStrategy))]
    public class OutputDTO<T>
    {
        public bool Succeeded { get; set; } = true;
        public string Message { get; set; } = null!;
        public int HttpStatusCode { get; set; } = 200;
        public int TotalCounts { get; set; } = 0;
        public T Data { get; set; }
    }
}
