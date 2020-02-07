using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace ReqHub
{
    public interface IApiClient
    {
        Task<TResult> GetAsync<TResult>(string path, CancellationToken cancellationToken = default);

        Task<TResult> PostAsync<TResult>(string path, HttpContent content, CancellationToken cancellationToken = default);

        Task<TResult> PutAsync<TResult>(string path, HttpContent content, CancellationToken cancellationToken = default);

        Task<TResult> PatchAsync<TResult>(string path, HttpContent content, CancellationToken cancellationToken = default);

        Task<TResult> DeleteAsync<TResult>(string path, CancellationToken cancellationToken = default);

        Task<HttpResponseMessage> SendAsync(string path, HttpMethod method, HttpContent content = null, IDictionary<string, string> headers = null, CancellationToken cancellationToken = default);

        Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken = default);
    }
}
