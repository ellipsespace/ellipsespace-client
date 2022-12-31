﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace EllipseSpaceClient.Models.Sessions
{
    internal class Session
    {
        [JsonPropertyName("sname")]
        public string SessionName { get; set; }
        [JsonPropertyName("password")]
        public string Password { get; set; }

        public Session()
        {

        }

        public Session(string sessionName, string password)
        {
            SessionName = sessionName;
            Password = password;
        }

        internal string Marshal()
        {
            return JsonSerializer.Serialize(this);
        }

        internal static Session? Unmarshal(string json)
        {
            return JsonSerializer.Deserialize<Session>(json);
        }
    }
}
