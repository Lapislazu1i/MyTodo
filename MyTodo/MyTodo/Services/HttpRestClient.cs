using MyTodo.SharedLib;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyTodo.Services
{
    public class HttpRestClient
    {
        private readonly string _apiUrl;
        protected RestClient _client;

        public HttpRestClient(string apiUrl)
        {
            _apiUrl = apiUrl;
            _client = new RestClient();

        }

        public async Task<ApiResponse> ExecuteAsync(BaseRequest baseRequest)
        {
            var request = new RestRequest(_apiUrl + baseRequest.Route, baseRequest.Method);
            request.AddHeader("Content-Type", "application/json");

            if (baseRequest.Parameter != null)
            {
                request.AddParameter("param", JsonConvert.SerializeObject(baseRequest.Parameter),ParameterType.RequestBody);
            }
            var response = await _client.ExecuteAsync(request);
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
                return JsonConvert.DeserializeObject<ApiResponse>(response.Content);
            else
                return new ApiResponse { Status = false, Result = null, Message = response.ErrorMessage };
        }

        public async Task<ApiResponse<T>> ExecuteAsync<T>(BaseRequest baseRequest)
        {
            var request = new RestRequest(_apiUrl + baseRequest.Route, baseRequest.Method);
            request.AddHeader("Content-Type", "application/json");
            request.AddHeader("Connection", "keep-alive");
            if (baseRequest.Parameter != null)
            {
                var jsonvalue = JsonConvert.SerializeObject(baseRequest.Parameter);

                request.AddStringBody(jsonvalue, DataFormat.Json);
            }
            var response = await _client.ExecuteAsync(request);
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
                return JsonConvert.DeserializeObject<ApiResponse<T>>(response.Content);
            else
                return new ApiResponse<T> { Status = false, Message = response.ErrorMessage };
        }
    }
}
