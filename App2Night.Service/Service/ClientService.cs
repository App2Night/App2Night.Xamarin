using System;
using System.Collections.Generic;
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

        public async Task<Result<TExpectedType>> SendRequest<TExpectedType>(string uri,
            RestType restType, 
            bool cacheData = false, 
            string urlQuery = "", 
            object bodyParameter = null, 
            object wwwFormData = null, 
            string token = null, 
            Endpoint endpoint = Endpoint.Api,
            bool httpsEnabled = true)
        {
            Result<TExpectedType> result = new Result<TExpectedType>();
            
            //Add the urlQuery to the uri if one is stated.
            if (!string.IsNullOrEmpty(urlQuery))
                uri += urlQuery;

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
                    uri = uri.ToLower();

                    //Serialize data 
                    StringContent content = null;
                    if (restType == RestType.Post || restType == RestType.Put)
                    {
                        
                        if(bodyParameter != null && wwwFormData != null) throw new Exception("Cant send a x-www-form-urlencoed and body parameter at the same time.");
                        if (bodyParameter != null)
                        { 
                            content = new StringContent(
                                JsonConvert.SerializeObject(bodyParameter), Encoding.UTF8, "application/json");
                        }
                        if (wwwFormData != null)
                        {  
                            content = new StringContent(ObjectToWwwForm(wwwFormData), Encoding.UTF8, "application/x-www-form-urlencoded"); 
                        }
                    }

                    //Execute the request with the proper request type. 
                    switch (restType)
                    {
                        case RestType.Post: 
                            requestResult = await client.PostAsync(uri, content);
                            break;
                        case RestType.Get:
                            requestResult = await client.GetAsync(uri);
                            break;
                        case RestType.Put:
                            requestResult = await client.PutAsync(uri, content);
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

        public async Task<Result> SendRequest(string uri, RestType restType, bool cacheData = false, string urlQuery = "", object bodyParameter = null, object wwwFormData = null,
            string token = null, Endpoint endpoint = Endpoint.Api, bool httpsEnabled = true)
        {
            return await SendRequest<Type>(uri, restType, cacheData, urlQuery, bodyParameter, wwwFormData, token, endpoint, httpsEnabled);
        } 

        private HttpClient GetClient(Endpoint endpoint)
        {
            var domain = endpoint == Endpoint.Api ? "app2nightapi" : "app2nightuser";
            HttpClient client = new HttpClient {BaseAddress = new Uri($"https://{domain}.azurewebsites.net")};
            client.Timeout = TimeSpan.FromSeconds(30);
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            client.DefaultRequestHeaders.Host = $"{domain}.azurewebsites.net";
            return client;
        }

        //Helper
        /// <summary>
        /// Converts an object to a formatted x-www-urlformencoded string.
        /// </summary>
        /// <param name="data">Oobject.</param>
        /// <returns>Formatted x-www-urlformencoded string.</returns>
        string ObjectToWwwForm(object data)
        {
            var json = JsonConvert.SerializeObject(data);
            var dic = JsonConvert.DeserializeObject<Dictionary<string, string>>(json);

            StringBuilder sb = new StringBuilder();
            foreach (KeyValuePair<string, string> valuePair in dic)
            {
                if (!string.IsNullOrEmpty(valuePair.Key) && !string.IsNullOrEmpty(valuePair.Value))
                {
                    if (sb.Length > 0) sb.Append("&");
                    sb.Append(valuePair.Key);
                    sb.Append("=");
                    sb.Append(valuePair.Value);
                }
            }
            return sb.ToString();
        }
    } 
}