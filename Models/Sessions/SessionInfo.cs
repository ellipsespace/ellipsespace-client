using System.Text.Json;
using System.Text.Json.Serialization;

namespace EllipseSpaceClient.Models.Sessions
{
    internal class SessionInfo
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("sname")]
        public string SessionName { get; set; }

        [JsonPropertyName("access-level")]
        public int AccessLevel { get; set; }

        public SessionInfo()
        {

        }

        internal string Marshal()
        {
            return JsonSerializer.Serialize(this);
        }

        internal static SessionInfo? Unmarshal(string json)
        {
            return JsonSerializer.Deserialize<SessionInfo>(json);
        }
    }
}
