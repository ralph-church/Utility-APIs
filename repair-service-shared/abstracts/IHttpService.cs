using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace repair.service.shared.abstracts
{
    public interface IHttpService
    {
        ValueTask<HttpResponseMessage> DeleteAsync(string baseUrl, string path, string authToken, CancellationToken cancellationToken, Dictionary<string, string> headers = null);
        ValueTask<HttpResponseMessage> GetAsync(string baseUrl, string relativePath, string authToken, CancellationToken cancellationToken, Dictionary<string, string> headers = null);
        ValueTask<HttpResponseMessage> PostAsync(string baseUrl, string relativePath, string authToken, string contentType, object bodyContent, CancellationToken cancellationToken, Dictionary<string, string> headers = null);
        ValueTask<HttpResponseMessage> PutAsync(string baseUrl, string path, string authToken, string contentType, object bodyContent, CancellationToken cancellationToken, Dictionary<string, string> headers = null);
    }
}