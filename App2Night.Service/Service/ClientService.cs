using System;
using System.Diagnostics;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using App2Night.Model.Model;
using Newtonsoft.Json;
using PartyUp.Model.Model;
using PartyUp.Service.Interface;  

namespace PartyUp.Service.Service
{

    //TODO CLIENT Add the correct nuget package
    
    public class ClientService : IClientService
    { 

        public async Task<Result<TExpectedType>> SendRequest<TExpectedType>(string uri,RestType restType, bool cacheData = false, string query = "", object bodyParameter = null)
        {
            Result<TExpectedType> result = new Result<TExpectedType>();
            
            //Add the query to the uri if one is stated.
            if (!string.IsNullOrEmpty(query))
                uri += query;

            try
            {
                using (HttpClient client = GetClient())
                {
                    HttpResponseMessage response;

                    //Execute the request with the proper request type.
                    Stopwatch timer = new Stopwatch();
                    timer.Start();
                    uri = "api/" + uri;
                    switch (restType)
                    {
                        case RestType.Post: 
                            response = await client.PostAsync(uri, new StringContent(
                                JsonConvert.SerializeObject(bodyParameter), Encoding.UTF8, "application/json"));
                            break;
                        case RestType.Get:
                            response = await client.GetAsync(uri);
                            break;
                        case RestType.Put:
                            response = await client.PutAsync(uri, new StringContent(
                                JsonConvert.SerializeObject(bodyParameter), Encoding.UTF8, "application/json"));
                            break;
                        case RestType.Delete:
                            response = await client.DeleteAsync(uri);
                            break;
                        default:
                            throw new Exception("Unexpected RestType " + restType);
                    }
                    result.StatusCode = (int) response.StatusCode;
                    Debug.WriteLine("Request excequted in: " + timer.Elapsed.ToString("c"));
                    //Check wheter or not the request was successfull.
                    if (response.IsSuccessStatusCode)
                    { 
                        string resultAsString = await response.Content.ReadAsStringAsync();
                        //Deserialize the json if one exists. 
                        if (!string.IsNullOrEmpty(resultAsString))
                        {
                            result.Data = JsonConvert.DeserializeObject<TExpectedType>(resultAsString); 
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

        public async Task<Result<Token>> GetToken(string username, string password)
        {
            Result<Token> result = new Result<Token>();
            try
            {
                using (var client = GetClient())
                {
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

                    if (requestResult.IsSuccessStatusCode)
                    {
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

        private HttpClient GetClient()
        {
            HttpClient client = new HttpClient {BaseAddress = new Uri("https://app2nightapi.azurewebsites.net/")};
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            client.DefaultRequestHeaders.Host = "app2nightapi.azurewebsites.net";
            return client;
        }
    }
}