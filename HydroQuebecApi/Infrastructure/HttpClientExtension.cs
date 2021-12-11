using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Threading.Tasks;
using System.Web;

namespace HydroQuebecApi.Infrastructure
{
    public static class HttpClientExtension
    {
        private static string accessToken = null;

        public static void SetAccessToken(this HttpClient httpClient, string accessToken) => HttpClientExtension.accessToken = accessToken;

        public static async Task<T> HttpGetRequest<T>(this HttpClient httpClient, string url, Dictionary<string, string> queries, Dictionary<string, string> headers, HttpContent content = null, string acceptHeader = "application/json") where T : class
        {
            var queryString = HttpUtility.ParseQueryString(string.Empty);
            if (queries != null)
            {
                foreach (var query in queries)
                {
                    queryString[query.Key] = query.Value;
                }
            }

            var builder = new UriBuilder(url);
            builder.Port = -1;
            builder.Query = queryString.ToString();

            return await httpClient.HttpRequest<T>(HttpMethod.Get, builder.ToString(), headers, content, acceptHeader);
        }
        /// <summary>
        /// The returned value depends upon the type of T.
        /// - If T is HttpRequestMessage, then the request data is returned (the caller will therefore be able to determine if a redirection has happened)
        /// - If T is string, then the response content is returned as a string
        /// - Otherwise, the response is deserialized into an object of type T
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="httpClient"></param>
        /// <param name="method"></param>
        /// <param name="url"></param>
        /// <param name="headers"></param>
        /// <param name="content"></param>
        /// <param name="acceptHeader"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public static async Task<T> HttpRequest<T>(this HttpClient httpClient, HttpMethod method, string url, Dictionary<string, string> headers = null, HttpContent content = null, string acceptHeader = "application/json") where T : class
        {
            httpClient.DefaultRequestHeaders.Clear();
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(acceptHeader));//ACCEPT header

            if (headers != null)
            {
                foreach (var header in headers)
                {
                    httpClient.DefaultRequestHeaders.Add(header.Key, header.Value);
                }
            }

            if (!string.IsNullOrEmpty(accessToken))
            {
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            }

            var request = new HttpRequestMessage(method, url);
            request.Content = content;
            var response = await httpClient.SendAsync(request);

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception($"Http request at {url} failed (return status: {response.StatusCode}");
            }

            if (typeof(T) == typeof(HttpRequestMessage))
            {
                return request as T;
            }

            var responseStr = await response.Content.ReadAsStringAsync();

            if (typeof(T) == typeof(string))
            {
                return responseStr as T;
            }

            return JsonSerializer.Deserialize<T>(responseStr);
        }

    }
}
