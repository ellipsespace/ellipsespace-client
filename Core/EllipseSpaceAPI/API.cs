#pragma warning disable SYSLIB0014
using System.Net.Http;

namespace EllipseSpaceClient.Core.EllipseSpaceAPI
{
    public class API 
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

        public HttpResponseMessage SendRequest(string address, HttpMethod reqMethod, bool authRequired = false, string? reqBody = null)
        {
            using (HttpClient client = new HttpClient())
            {
                using (var req = new HttpRequestMessage(reqMethod, address))
                {
                    if (reqBody != null)
                        req.Content = new StringContent(reqBody);

                    if (authRequired)
                        req.Headers.Add("Authorization", $"Bearer {ApiKey}");

                    using (var resp = client.SendAsync(req))
                    {
                        return resp.Result;
                    }
                }
            }
        }

        public static string MakeAddress(string relative)
        {
            return "https://ellipsespace.onrender.com/api" + relative;
        }
    }
}
#pragma warning restore SYSLIB0014