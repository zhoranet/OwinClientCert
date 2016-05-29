using System.Net.Http;
using System.Threading.Tasks;

namespace OwinServer.Test
{
    public interface IHttpClientProvider
    {
        Task<HttpClient> GetHttpClientAsync(string url);
    }
}