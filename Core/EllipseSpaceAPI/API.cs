#pragma warning disable SYSLIB0014
using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace EllipseSpaceClient.Core.EllipseSpaceAPI
{
    internal class API
    {
        /// <summary>
        /// Presents JWT authorization token
        /// </summary>
        internal string ApiKey;

        public API()
        {

        }

        public API(string apiKey)
        {
            ApiKey = apiKey;
        }

        internal async Task<string> SendRequest(string address, HttpMethod reqMethod, bool authRequired, string? reqBody)
        {
            using (HttpClient client = new HttpClient())
            {
                using (var req = new HttpRequestMessage(reqMethod, address))
                {
                    if (reqBody != null)
                        req.Content = new StringContent(reqBody);

                    if (authRequired)
                        req.Headers.Add("Authorization", $"Bearer {ApiKey}");

                    using (var resp = client.Send(req))
                    {
                        return await resp.Content.ReadAsStringAsync();
                    }
                }
            }
        }

        internal static string MakeAddress(string relative)
        {
            return "https://ellipsespace.onrender.com/api" + relative;
        }
    }
}
#pragma warning restore SYSLIB0014