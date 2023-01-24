using EllipseSpaceClient.Core.EllipseSpaceAPI;
using EllipseSpaceClient.Models.Sessions;
using System;
using System.IO;
using System.Net.Http;
using System.Text.Json;
using System.Text.Json.Serialization;
using Windows.Services.Maps;

namespace EllipseSpaceClient.Core.Configuration
{
    public class Configuration
    {
        private readonly string PATH = $"{Directory.GetCurrentDirectory()}/conf/conf.json";

        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("sname")]
        public string SessionName { get; set; }

        [JsonPropertyName("access-level")]
        public int AccessLevel { get; set; }

        [JsonPropertyName("jwt")]
        public string JwtToken { get; set; }

        [JsonPropertyName("lang")]
        public string DefaultLanguage { get; set; }
        [JsonPropertyName("tick")]
        public int Tick { get; set; }

        public Configuration()
        {

        }

        internal string Marshal()
        {
            return JsonSerializer.Serialize(this);
        }

        internal void Save()
        {
            using (StreamWriter sw = new StreamWriter(PATH))
            {
                sw.Write(Marshal());
            }
        }

        internal static Configuration? Create()
        {
            var current = Unmarshal(File.ReadAllText($"{Directory.GetCurrentDirectory()}/conf/conf.json"));

            if (current != null)
                return current;
            else return null;
        }

        internal static void UpdateSessionInfo(string jwt)
        {
            var API = new API();
            API.ApiKey = jwt;
            var configuration = Create();
            var infoResp = API.SendRequest(API.MakeAddress("/session/info"), HttpMethod.Get, true);
            var info = SessionInfo.Unmarshal(infoResp.Content.ReadAsStringAsync().Result);

            configuration.JwtToken = jwt;
            configuration.Id = info.Id;
            configuration.SessionName = info.SessionName;
            configuration.AccessLevel = info.AccessLevel;
            configuration.Save();
        }

        internal static void Logout()
        {
            var configuration = Create();

            configuration.JwtToken = string.Empty;
            configuration.Id = 0;
            configuration.SessionName = string.Empty;
            configuration.AccessLevel = 0;
            configuration.Save();
        }

        internal static Configuration? Unmarshal(string json)
        {
            return JsonSerializer.Deserialize<Configuration>(json);
        }
    }
}
