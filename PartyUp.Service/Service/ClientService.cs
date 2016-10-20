using System;
using System.Diagnostics;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using PartyUp.Service.Interface;  

namespace PartyUp.Service.Service
{

    //TODO CLIENT Add the correct nuget package
    
    public class ClientService : IClientService
    { 

        public async Task<TExpectedType> SendRequest<TExpectedType>(string uri,RestType restType, bool cacheData = false, string query = "", object bodyParameter = null)
        { 
            TExpectedType result = default(TExpectedType);
            
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
                    switch (restType)
                    {
                        case RestType.Post:
                            //TODO CLIENT Add the body
                            response = await client.PostAsync(uri, null);
                            break;
                        case RestType.Get:
                            response = await client.GetAsync(uri);
                            break;
                        case RestType.Put:
                            response = await client.PutAsync(uri, null);
                            break;
                        case RestType.Delete:
                            response = await client.DeleteAsync(uri);
                            break;
                        default:
                            throw new Exception("Unexpected RestType " + restType);
                    }
                    Debug.WriteLine("Request excequted in: " + timer.Elapsed.ToString("c"));
                    //Check wheter or not the request was successfull.
                    if (response.IsSuccessStatusCode)
                    {

                        string resultAsString = await response.Content.ReadAsStringAsync();
                        //Deserialize the json if one exists. 
                        if (!string.IsNullOrEmpty(resultAsString))
                        {
                            result = JsonConvert.DeserializeObject<TExpectedType>(resultAsString); 
                        }  
                    }
                    else
                    {
                        
                    } 
                }
            }
            catch(Exception e)
            {
               
            } 
            return result;
        } 

        private HttpClient GetClient()
        {
            HttpClient client = new HttpClient {BaseAddress = new Uri("https://app2nightapi.azurewebsites.net/api/")};
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json")); 
            return client;
        }
    }
}