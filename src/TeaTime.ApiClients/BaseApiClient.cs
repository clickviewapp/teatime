namespace TeaTime.ApiClients
{
    using System;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using Models;
    using Newtonsoft.Json;

    public abstract class BaseApiClient
    {
        private readonly HttpClient _httpClient;

        protected BaseApiClient(string connString)
        {
            _httpClient = new HttpClient { BaseAddress = new Uri(connString) };

            //Set our default headers
            _httpClient.DefaultRequestHeaders.Accept.Clear();
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        protected ApiResponse Send(HttpRequestMessage request)
        {
            var response = _httpClient.SendAsync(request).Result;

            var returnValue = new ApiResponse
            {
                StatusCode = response.StatusCode,
                Content = response.Content.ReadAsStringAsync().Result
            };

            return returnValue;
        }

        protected ApiResponse<T> Send<T>(HttpRequestMessage request)
        {
            var response = Send(request);
            var returnValue = new ApiResponse<T> { StatusCode = response.StatusCode, Content = response.Content };

            try
            {
                returnValue.Data = JsonConvert.DeserializeObject<T>(response.Content);
            }
            catch (Exception ex)
            {
                returnValue.Exception = ex;
            }

            return returnValue;
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposing)
                return;

            _httpClient?.Dispose();

        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
