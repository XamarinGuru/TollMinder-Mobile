using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using MvvmCross.Platform;
using MvvmCross.Plugins.Network.Reachability;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Tollminder.Core.Models;

namespace Tollminder.Core.Services.Implementation
{
    public class ServerApiServiceBase
    {
        const string BaseApiUrl = "http://192.168.13.168:7000/";

        protected HttpClient client = new HttpClient();

        public void SetClientToken(string token)
        {
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }

        public void CleanClientToken()
        {
            client.DefaultRequestHeaders.Authorization = null;
        }

        #region GET

        protected Task<JObject> GetWithResultAsync(string relativePath, Dictionary<string, object> parameters = null)
        {
            return RequestWithResultAsync(GetInternalAsync(relativePath, parameters));
        }

        protected Task<TResponse> GetWithResultAsync<TResponse>(Expression<Func<JObject, JToken>> jtokenExpr, string relativePath, Dictionary<string, object> parameters = null)
        {
            return RequestWithResultAsync<TResponse>(GetWithResultAsync(relativePath, parameters), jtokenExpr);
        }

        protected async Task GetAsync(string relativePath, Dictionary<string, object> parameters = null)
        {
            await GetInternalAsync(relativePath, parameters).ConfigureAwait(false);
        }

        Task<HttpResponseMessage> GetInternalAsync(string relativePath, Dictionary<string, object> parameters = null)
        {
            return RequestInternalAsync(client.GetAsync(CreateUri(relativePath, parameters)));
        }

        protected Task<JObject> GetWithResultAsync(string relativePath, CancellationToken token, Dictionary<string, object> parameters = null)
        {
            return RequestWithResultAsync(GetInternalAsync(relativePath, token, parameters));
        }

        protected Task<TResponse> GetWithResultAsync<TResponse>(Expression<Func<JObject, JToken>> jtokenExpr, string relativePath, CancellationToken token, Dictionary<string, object> parameters = null)
        {
            return RequestWithResultAsync<TResponse>(GetWithResultAsync(relativePath, token, parameters), jtokenExpr);
        }

        protected async Task GetAsync(string relativePath, CancellationToken token, Dictionary<string, object> parameters = null)
        {
            await GetInternalAsync(relativePath, token, parameters).ConfigureAwait(false);
        }

        Task<HttpResponseMessage> GetInternalAsync(string relativePath, CancellationToken token, Dictionary<string, object> parameters = null)
        {
            return RequestInternalAsync(client.GetAsync(CreateUri(relativePath, parameters), token));
        }


        #endregion // GET

        #region POST

        protected Task<JObject> PostWithResultAsync(string relativePath, Dictionary<string, object> parameters = null)
        {
            return PostWithResultAsync<Dictionary<string, object>>(relativePath, parameters);
        }

        protected Task<JObject> PostWithResultAsync<TRequest>(string relativePath, TRequest request)
        {
            return RequestWithResultAsync(PostInternalAsync(relativePath, request));
        }

        protected Task<TResponse> PostWithResultAsync<TResponse>(Expression<Func<JObject, JToken>> jtokenExpr, string relativePath, Dictionary<string, object> parameters = null)
        {
            return PostWithResultAsync<Dictionary<string, object>, TResponse>(jtokenExpr, relativePath, parameters);
        }

        protected Task<TResponse> PostWithResultAsync<TRequest, TResponse>(Expression<Func<JObject, JToken>> jtokenExpr, string relativePath, TRequest request)
        {
            return RequestWithResultAsync<TResponse>(PostWithResultAsync(relativePath, request), jtokenExpr);
        }

        protected async Task PostAsync(string relativePath, Dictionary<string, object> parameters = null)
        {
            await PostInternalAsync(relativePath, parameters);
        }

        protected async Task PostAsync<TRequest>(string relativePath, TRequest request)
        {
            await PostInternalAsync(relativePath, request);
        }

        Task<HttpResponseMessage> PostInternalAsync<TRequest>(string relativePath, TRequest request)
        {
            return RequestInternalAsync(client.PostAsync(CreateUri(relativePath), СreateContent(request, relativePath)));
        }

        #endregion // POST

        #region Delete

        protected async Task DeleteAsync(string relativePath, Dictionary<string, object> parameters = null)
        {
            await DeleteInternalAsync(relativePath, parameters);
        }

        Task<HttpResponseMessage> DeleteInternalAsync(string relativePath, Dictionary<string, object> parameters = null)
        {
            return RequestInternalAsync(client.DeleteAsync(CreateUri(relativePath, parameters)));
        }

        #endregion

        #region PUT

        protected Task<JObject> PutWithResultAsync(string relativePath, Dictionary<string, object> parameters = null)
        {
            return PutWithResultAsync<Dictionary<string, object>>(relativePath, parameters);
        }

        protected Task<JObject> PutWithResultAsync<TRequest>(string relativePath, TRequest request)
        {
            return RequestWithResultAsync(PutInternalAsync(relativePath, request));
        }

        protected Task<TResponse> PutWithResultAsync<TResponse>(Expression<Func<JObject, JToken>> jtokenExpr, string relativePath, Dictionary<string, object> parameters = null)
        {
            return PutWithResultAsync<Dictionary<string, object>, TResponse>(jtokenExpr, relativePath, parameters);
        }

        protected Task<TResponse> PutWithResultAsync<TRequest, TResponse>(Expression<Func<JObject, JToken>> jtokenExpr, string relativePath, TRequest request)
        {
            return RequestWithResultAsync<TResponse>(PutWithResultAsync(relativePath, request), jtokenExpr);
        }

        protected async Task PutAsync(string relativePath, Dictionary<string, object> parameters = null)
        {
            await PutInternalAsync(relativePath, parameters).ConfigureAwait(false);
        }

        protected async Task PutAsync<TRequest>(string relativePath, TRequest request)
        {
            await PutInternalAsync(relativePath, request).ConfigureAwait(false);
        }

        Task<HttpResponseMessage> PutInternalAsync<TRequest>(string relativePath, TRequest request)
        {
            return RequestInternalAsync(client.PutAsync(CreateUri(relativePath), СreateContent(request, relativePath)));
        }

        #endregion // PUT

        #region // Internal Requests

        async Task<JObject> RequestWithResultAsync(Task<HttpResponseMessage> task)
        {
            CheckInternetConnection();

            var response = await task.ConfigureAwait(false);
            return await ReadResponseAsync(response);
        }

        async Task<TResponse> RequestWithResultAsync<TResponse>(Task<JObject> task, Expression<Func<JObject, JToken>> jtokenExpr)
        {
            CheckInternetConnection();

            var json = await task.ConfigureAwait(false);
            return GetObjectFromJson<TResponse>(json, jtokenExpr);
        }

        async Task<HttpResponseMessage> RequestInternalAsync(Task<HttpResponseMessage> task)
        {
            CheckInternetConnection();

            var response = await task.ConfigureAwait(false);
            await CheckHttpResponse(response);
            return response;
        }

        #endregion

        #region Helper methods

        void CheckInternetConnection()
        {
            if (!Mvx.Resolve<IMvxReachability>().IsHostReachable(BaseApiUrl))
                throw new Exceptions.UiApiException("Please, check your internet connection");
        }

        TResponse GetObjectFromJson<TResponse>(JObject json, Expression<Func<JObject, JToken>> jtokenExpr)
        {
            try
            {
                var jtoken = jtokenExpr.Compile()(json);
                var result = jtoken.ToObject<TResponse>();
                return result;
            }
            catch (Exception e)
            {
                throw new Exceptions.ApiException($"Response parse {jtokenExpr} {e.Message} in {json}");
            }
        }

        async Task<JObject> ReadResponseAsync(HttpResponseMessage response)
        {
            var responseString = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
            return HandleSuccessResponse(JObject.Parse(responseString));
        }

        StringContent СreateContent<TRequest>(TRequest request, string relativePath)
        {
            var paramsString = JsonConvert.SerializeObject(request);
            Mvx.Trace("{0} request content {1}", relativePath, paramsString);
            var content = new StringContent(paramsString, System.Text.Encoding.UTF8, "application/json");
            return content;
        }
        Uri CreateUri(string relativePath, Dictionary<string, object> parameters = null)
        {
            return new Uri(new Uri(BaseApiUrl), ToQueryString(relativePath, parameters));
        }

        string ToQueryString(string url, Dictionary<string, object> parameters)
        {
            if (parameters == null)
            {
                return url;
            }

            var array = parameters?.Keys.Select(k => string.Format("{0}={1}", WebUtility.UrlEncode(k), WebUtility.UrlEncode(String.Format(CultureInfo.InvariantCulture, "{0}", parameters[k])))).ToList();
            var str = url + "?" + string.Join("&", array);
            Mvx.Trace($"New request query {str}");
            return str;
        }

        #endregion

        #region Response Handling

        async Task CheckHttpResponse(HttpResponseMessage response)
        {
            Contract.Ensures(Contract.Result<Task>() != null);
            if (!response.IsSuccessStatusCode)
            {

                var json = await response.Content.ReadAsStringAsync();
                Mvx.Trace(string.Format("Bad HTTP response from server {0} {1}, json = {2}", response.RequestMessage.RequestUri.PathAndQuery, response.ReasonPhrase, json.Replace("{", "{{").Replace("}", "}}")));
                throw new Models.Exceptions.ApiException("ServerError");
            }
        }

        JObject HandleSuccessResponse(JObject json)
        {
            if (json.ToObject<BaseApiResponse>()?.Success ?? false)
                return json;
            else
                throw new Exception("Bad API response from server" + json);
        }

        #endregion

        #region IDisposable implementation

        public void Dispose()
        {
            client.Dispose();
        }

        #endregion

    }
}