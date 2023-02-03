using System.Text.Json;
using System.Text.Json.Serialization;

namespace EllipseSpaceClient.Models.ServerStatus
{
    public class ServerStatus
    {
        [JsonPropertyName("msg")]
        public string Message { get; set; }

        internal static ServerStatus? Unmarshal(string json)
        {
            return JsonSerializer.Deserialize<ServerStatus>(json);
        }
    }
}
