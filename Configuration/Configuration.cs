using EllipseSpaceClient.Models.Sessions;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace EllipseSpaceClient.Configuration
{
    internal class Configuration
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("jwt")]
        public string JwtToken { get; set; }

        internal string Marshal()
        {
            return JsonSerializer.Serialize(this);
        }

        internal void Save()
        {
            using (StreamWriter sw = new StreamWriter($"{Directory.GetCurrentDirectory()}/conf/conf.json"))
            {
                sw.Write(Marshal());
            }
        }

        internal static Configuration? Unmarshal(string json)
        {
            return JsonSerializer.Deserialize<Configuration>(json);
        }
    }
}
