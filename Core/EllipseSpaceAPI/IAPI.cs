using System.Net.Http;
using System.Threading.Tasks;

namespace EllipseSpaceClient.Core.EllipseSpaceAPI
{
    public interface IAPI
    {
        Task<string> SendRequest(string address, HttpMethod reqMethod, bool authRequired, string? reqBody);
        string MakeAddress(string relative);
    }
}
