using EllipseSpaceClient.Core.EllipseSpaceAPI;
using System;
using System.ComponentModel;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace EllipseSpaceClient.Models.Sessions
{
    public class Session : INotifyPropertyChanged
    {
        private const string REGISTATION_API = "/session/create";
        private const string AUTHORIZATION_API = "/session/auth";
        private string name;
        private string password;

        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }

        [JsonPropertyName("sname")]
        public string Name { 
            get => name;
            set
            {
                name = value;
                OnPropertyChanged(nameof(Name));
            }
        }
        [JsonPropertyName("password")]
        public string Password
        {
            get => password;
            set
            {
                password = value;
                OnPropertyChanged(nameof(Password));
            }
        }

        public Session()
        {

        }

        public Session(string sessionName, string password)
        {
            Name = sessionName;
            Password = password;
        }

        /// <summary>
        /// Sends a request to the server for a JWT authorization token
        /// </summary>
        /// <returns>
        /// Returns a tuple with two elements:
        /// Item1 - a string representing the server response
        /// Item2 - bool, indicating the success of getting the token
        /// </returns>
        public Tuple<string, bool> Authorize()
        {
            var API = new API();

            var authResp = new API().SendRequest(API.MakeAddress(AUTHORIZATION_API), HttpMethod.Get, false, Marshal());
            string jwt = authResp.Content.ReadAsStringAsync().Result.Replace("\"", "");

            if (!authResp.IsSuccessStatusCode)
                return new Tuple<string, bool>(jwt, true);
            else return new Tuple<string, bool>(ServerStatus.ServerStatus.Unmarshal(authResp.Content.ReadAsStringAsync().Result).Message, false);
        }

        /// <summary>
        /// Sends a registration request to the server
        /// </summary>
        /// <returns>
        /// Returns a tuple with two elements:
        /// Item1 - a string representing the server response
        /// Item2 - bool, indicating the success of registration
        /// </returns>
        public Tuple<string, bool> Register()
        {
            var idResp = new API().SendRequest(API.MakeAddress(REGISTATION_API), HttpMethod.Post, false, Marshal());
            string id = idResp.Content.ReadAsStringAsync().Result.Replace("\"", "");

            if (idResp.IsSuccessStatusCode)
                return new Tuple<string, bool>(id, true);
            else return new Tuple<string, bool>(ServerStatus.ServerStatus.Unmarshal(idResp.Content.ReadAsStringAsync().Result).Message, false);
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
