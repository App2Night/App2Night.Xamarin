using System;
using System.Diagnostics;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using App2Night.Model.Model;
using App2Night.Service.Interface;
using Newtonsoft.Json; 

namespace App2Night.Service.Service
{
    public enum Endpoint
    {
        Api, User
    }

    public class ClientService : IClientService
    { 

        public async Task<Result<TExpectedType>> SendRequest<TExpectedType>(string uri,RestType restType, bool cacheData = false, string query = "", object bodyParameter = null, string token = null, Endpoint endpoint = Endpoint.Api)
        {
            Result<TExpectedType> result = new Result<TExpectedType>();
            
            //Add the query to the uri if one is stated.
            if (!string.IsNullOrEmpty(query))
                uri += query;

            try
            {
                using (var client = GetClient(endpoint))
                {
                    if (!string.IsNullOrEmpty(token))
                    {
                        client.DefaultRequestHeaders.Add("Authorization", "Baerer " + token);
                    }
                    HttpResponseMessage requestResult;

                    Stopwatch timer = new Stopwatch();
                    timer.Start();
                    uri = "api/" + uri;
                    uri = uri.ToLower();
                    //Execute the request with the proper request type. 
                    switch (restType)
                    {
                        case RestType.Post: 
                            requestResult = await client.PostAsync(uri, new StringContent(
                                JsonConvert.SerializeObject(bodyParameter), Encoding.UTF8, "application/json"));
                            break;
                        case RestType.Get:
                            requestResult = await client.GetAsync(uri);
                            break;
                        case RestType.Put:
                            requestResult = await client.PutAsync(uri, new StringContent(
                                JsonConvert.SerializeObject(bodyParameter), Encoding.UTF8, "application/json"));
                            break;
                        case RestType.Delete:
                            requestResult = await client.DeleteAsync(uri);
                            break;
                        default:
                            throw new Exception("Unexpected RestType " + restType);
                    }
                    result.StatusCode = (int)requestResult.StatusCode;
                    Debug.WriteLine("Request excequted in: " + timer.Elapsed.ToString("c"));
                    //Check wheter or not the request was successfull.
                    if (requestResult.IsSuccessStatusCode)
                    {
                        result.Success = true;
                        if (requestResult.Content != null && typeof (TExpectedType) != typeof (Type))
                        {
                             string resultAsString = await requestResult.Content.ReadAsStringAsync();
                            //Deserialize the json if one exists. 
                            if (!string.IsNullOrEmpty(resultAsString))
                            {
                                result.Data = JsonConvert.DeserializeObject<TExpectedType>(resultAsString);
                            } 
                        }
                    } 
                }
            }
            catch(Exception e)
            {
                result.RequestFailedToException = true;
            } 
            return result;
        } 

        public async Task<Result> SendRequest(string uri, RestType restType, bool cacheData = false, string query = "", object bodyParameter = null,
            string token = null, Endpoint endpoint = Endpoint.Api)
        {
            return await SendRequest<Type>(uri, restType, cacheData, query, bodyParameter, token, endpoint);
        }

        public async Task<Result<Token>> GetToken(string username, string password)
        {
            Result<Token> result = new Result<Token>();
            try
            {
                using (var client = GetClient(Endpoint.User))
                {
                    client.BaseAddress = new Uri("http://app2nightuser.azurewebsites.net");
                    client.DefaultRequestHeaders.Accept.Clear(); 
                    var query = "client_id=nativeApp&" +
                                "client_secret=secret&" +
                                "grant_type=password&" +
                                $"username={username}&" +
                                $"password={password}&" +
                                "scope=App2NightAPI offline_access&" +
                                "offline_access=true";
                    var content = new StringContent(
                        query, Encoding.UTF8, "application/x-www-form-urlencoded");
                    var requestResult = await client.PostAsync("connect/token", content);
                    result.StatusCode = (int)requestResult.StatusCode;
                    if (requestResult.IsSuccessStatusCode)
                    {
                        result.Success = true;
                        var response = await requestResult.Content.ReadAsStringAsync(); 
                        result.Data = JsonConvert.DeserializeObject<Token>(response);
                    }
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
                result.RequestFailedToException = true;
            }
            return result;
        }

        private HttpClient GetClient(Endpoint endpoint)
        {
            var domain = endpoint == Endpoint.Api ? "app2nightapi" : "app2nightuser";
            HttpClient client = new HttpClient {BaseAddress = new Uri($"https://{domain}.azurewebsites.net")};
            client.Timeout = TimeSpan.FromSeconds(10);
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            client.DefaultRequestHeaders.Host = $"{domain}.azurewebsites.net";
            return client;
        }
    }
}