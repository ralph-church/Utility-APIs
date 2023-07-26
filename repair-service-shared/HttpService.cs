using repair.service.shared.abstracts;
using repair.service.shared.constants;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace telematics.shared
{
    public class HttpService : IHttpService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly HttpClient _httpClient;

        //public HttpService(IHttpClientFactory httpClientFactory)
        //{
        //    _httpClientFactory = httpClientFactory;
        //}

        public HttpService(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async ValueTask<HttpResponseMessage> GetAsync(string baseUrl, string relativePath, string authToken,
                                                CancellationToken cancellationToken, Dictionary<string, string> headers = null)
        {
            var client = CreateClient(baseUrl, authToken, headers);
            var response = await client.GetAsync(relativePath, cancellationToken);
            return response;
        }

        public async ValueTask<HttpResponseMessage> PostAsync(string baseUrl, string relativePath, string authToken, string contentType, object content,
                                                CancellationToken cancellationToken, Dictionary<string, string> headers = null)
        {
            var client = CreateClient(baseUrl, authToken, headers);

            HttpContent bodyContent = null;

            if (contentType == AppConstants.HttpContentTypeJson)
            {
                var json = content is string ? content.ToString() : Newtonsoft.Json.JsonConvert.SerializeObject(content);
                bodyContent = new StringContent(json, Encoding.Default, AppConstants.HttpContentTypeJson);
            }
            else
                bodyContent = (content as HttpContent);

            var response = await client.PostAsync(relativePath, bodyContent, cancellationToken);
            return response;
        }

        public async ValueTask<HttpResponseMessage> PutAsync(string baseUrl, string path, string authToken, string contentType, object content,
                                                CancellationToken cancellationToken, Dictionary<string, string> headers = null)
        {
            var client = CreateClient(baseUrl, authToken, headers);

            HttpContent bodyContent = null;

            if (contentType == AppConstants.HttpContentTypeJson)
            {
                var json = Newtonsoft.Json.JsonConvert.SerializeObject(content);
                bodyContent = new StringContent(json, Encoding.Default, AppConstants.HttpContentTypeJson);
            }
            else
                bodyContent = (content as HttpContent);

            return await client.PutAsync(path, bodyContent, cancellationToken);
        }

        public async ValueTask<HttpResponseMessage> DeleteAsync(string baseUrl, string path, string authToken,
                                CancellationToken cancellationToken, Dictionary<string, string> headers = null)
        {
            var client = CreateClient(baseUrl, authToken, headers);
            return await client.DeleteAsync(path, cancellationToken);
        }

        private HttpClient CreateClient(string baseUrl, string authToken = "", Dictionary<string, string> headers = null)
        {
            var client = _httpClientFactory.CreateClient(baseUrl);

            if (!Uri.TryCreate(baseUrl, UriKind.Absolute, out var baseUri))
            {
                throw new Exception("Not a valid Uri " + baseUrl);
            }

            client.BaseAddress = baseUri;
            if (headers != null)
            {
                foreach (var (key, val) in headers)
                {
                    client.DefaultRequestHeaders.Add(key, val);
                }
            }

            if (!client.DefaultRequestHeaders.Contains("Accept"))
            {
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(AppConstants.HttpContentTypeJson));
            }

            if (!string.IsNullOrEmpty(authToken))
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authToken);
            }

            return client;
        }

    }
}
