using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using App2Night.Model.Model;
using App2Night.Service.Helper;
using App2Night.Service.Interface;
using FreshMvvm;
using Newtonsoft.Json;
using System.Net.Http;

namespace App2Night.Service.Service
{
    /// <summary>
    /// Possible endpoints for a request.
    /// </summary>
    public enum Endpoint
    {
        Api, //app2nightapi.azu...
        User //app2nightuser.azu...
    }

    public class ClientService : IClientService
    {
        public async Task<Result<TExpectedType>> SendRequest<TExpectedType>(string uri, RestType restType,
            bool cacheData = false, string urlQuery = "", object bodyParameter = null,
            Dictionary<string, string> wwwFormData = null, string token = null, Endpoint endpoint = Endpoint.Api,
            bool httpsEnabled = true)
        {
            //Object that will contain the resul
            Result<TExpectedType> result = new Result<TExpectedType>();


            using (var client = GetClient(endpoint))
            {
                try
                {
                    uri = AddQuery(uri, urlQuery);

                    //Timer to check the duration of the call.
                    Stopwatch timer = new Stopwatch();
                    timer.Start();

                    AddTokenToClient(token, client);
                    HttpResponseMessage requestResult;

                    StringContent content = SerializeToStringContent(restType, bodyParameter, wwwFormData);

                    //Execute the request with the proper request type. 
                    requestResult = await MakeRequest(uri, restType, client, content);
                    result.StatusCode = (int) requestResult.StatusCode;

                    //The token is not valid anymore!
                    await CheckIfTokenIsValid(requestResult.StatusCode, token);


                    //Check wheter or not the request was successfull.
                    if (requestResult.IsSuccessStatusCode ||
                        requestResult.StatusCode == HttpStatusCode.NotAcceptable)
                        //Basicly a workaround for a not so well made backend implementation that sends a 406 if a location is not correct while still sending data that are needed for the app.
                        await HandleSuccess(uri, result, timer, requestResult);
                    else
                        await HandleFailure(uri, timer, requestResult);
                }
                catch (Exception e)
                {
                    result.RequestFailedToException = true;
                    DebugHelper.PrintDebug(DebugType.Error, $"Request to {uri} failed due to an exception: \n" + e);
                }
            }

            return result;
        }

        /// <summary>
        /// Handles a failed request. Prints the given error message from the request result if one is available.
        /// </summary> 
        private static async Task HandleFailure(string uri, Stopwatch timer, HttpResponseMessage requestResult)
        {
            string errorMessage = string.Empty;
            if (requestResult.Content != null)
            {
                errorMessage = await requestResult.Content.ReadAsStringAsync();
            }
            timer.PrintTime(
                $"Request to {uri} failed with {requestResult.StatusCode} {(string.IsNullOrEmpty(errorMessage) ? "." : $"to:\n{errorMessage}")}");
        }

        /// <summary>
        /// Handles a successfull request.
        /// </summary> 
        private async Task HandleSuccess<TExpectedType>(string uri, Result<TExpectedType> result, Stopwatch timer,
            HttpResponseMessage requestResult)
        {
            result.Success = true;
            result.Data = (TExpectedType) await DeserilizeContent<TExpectedType>(requestResult);
            timer.PrintTime($"Request to {uri} succeeded");
        }

        /// <summary>
        /// Deserializes the content of the request.
        /// </summary>
        /// <typeparam name="TExpectedType"></typeparam>
        /// <param name="result"></param>
        /// <param name="requestResult"></param>
        /// <returns></returns>
        private async Task<object> DeserilizeContent<TExpectedType>(HttpResponseMessage requestResult)
        {
            if (requestResult.Content != null && typeof(TExpectedType) != typeof(Type))
            {
                string resultAsString = await requestResult.Content.ReadAsStringAsync();
                //Deserialize the json if one exists. 
                if (!string.IsNullOrEmpty(resultAsString))
                {
                    return JsonConvert.DeserializeObject<TExpectedType>(resultAsString);
                }
            }
            return null;
        }

        /// <summary>
        /// Executes a request with given <see cref="RestType"/>.
        /// </summary>
        /// <param name="uri">Endpoint</param>
        /// <param name="restType">RestType for the request.</param>
        /// <param name="client"><see cref="HttpClient"/> that should execute the request.</param>
        /// <param name="content">Content of the request.</param>
        /// <returns></returns>
        private static async Task<HttpResponseMessage> MakeRequest(string uri, RestType restType, HttpClient client,
            StringContent content)
        {
            switch (restType)
            {
                case RestType.Post:
                    return await client.PostAsync(uri, content);
                case RestType.Get:
                    return await client.GetAsync(uri);
                case RestType.Put:
                    return await client.PutAsync(uri, content);
                case RestType.Delete:
                    return await client.DeleteAsync(uri);
                default:
                    throw new Exception("Unexpected RestType " + restType);
            }
        }

        /// <summary>
        /// Serializes the given object to a json body or x-www-formurlencoded StringContent.
        /// StringContent can only contain a json or www-form data.
        /// </summary>
        /// <param name="restType">RestType to determine if a serilization is nesseccary.</param>
        /// <param name="bodyParameter"></param>
        /// <param name="wwwFormData"></param>
        /// <returns></returns>
        private StringContent SerializeToStringContent(RestType restType, object bodyParameter,
            Dictionary<string, string> wwwFormData)
        {
            StringContent content = null;
            //Check if the requested RestType can contain content.
            if (restType == RestType.Post || restType == RestType.Put)
            {
                //Check if a body and wwwFormData is given, a request can only contain a body or wwwForm content.
                if (bodyParameter != null && wwwFormData != null)
                    throw new Exception("Cant send a x-www-form-urlencoed and body parameter at the same time.");

                if (bodyParameter != null)
                {
                    //Serialize the body
                    var json = JsonConvert.SerializeObject(bodyParameter);
                    content = new StringContent(json
                        , Encoding.UTF8, "application/json");
                }

                if (wwwFormData != null)
                {
                    //Serialize the wwwFormData
                    content = new StringContent(DictionaryToWwwwFormData(wwwFormData), Encoding.UTF8,
                        "application/x-www-form-urlencoded");
                }
            }

            return content;
        }

        public async Task<Result> SendRequest(string uri, RestType restType, bool cacheData = false,
            string urlQuery = "", object bodyParameter = null, Dictionary<string, string> wwwFormData = null,
            string token = null, Endpoint endpoint = Endpoint.Api, bool httpsEnabled = true)
        {
            /*
             * Calls SendRequest<> with Type as expected deserialized data.
             * Type won't get deserialized by SendRequest<>
             */
            return await SendRequest<Type>(uri, restType, cacheData, urlQuery, bodyParameter, wwwFormData, token,
                endpoint, httpsEnabled);
        }

        /// <summary>
        /// Add a token to the client if the token is not null.
        /// </summary>
        /// <param name="token">An access_token, can be empty (will not get set then).</param>
        /// <param name="client">Client where the token should get set for.</param>
        private void AddTokenToClient(string token, HttpClient client)
        {
            //Only set the token if the token is not null.
            if (!string.IsNullOrEmpty(token))
            {
                client.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);
            }
        }

        /// <summary>
        /// Adds the query to a string that represents an uri.
        /// </summary>
        /// <param name="uri">Endpoint</param>
        /// <param name="urlQuery">Query</param>
        /// <returns>A combined uri of the original uri and query.</returns>
        private string AddQuery(string uri, string urlQuery)
        {
            if (!string.IsNullOrEmpty(urlQuery))
                uri = uri + urlQuery;
            return uri;
        }

        /// <summary>
        /// Handles what should happen if a used token is not valid.
        /// </summary>
        private async Task CheckIfTokenIsValid(HttpStatusCode responseCode, string token)
        {
            if (responseCode == HttpStatusCode.Unauthorized && !string.IsNullOrEmpty(token))
                //Do a user logout.
                await FreshIOC.Container.Resolve<IStorageService>().DeleteStorage();
        }

        private HttpClient GetClient(Endpoint endpoint)
        {
            var domain = endpoint == Endpoint.Api ? "app2nightapi" : "app2nightuser";

            var client = new HttpClient(new HttpClientHandler()) {BaseAddress = new Uri($"https://{domain}.azurewebsites.net")};

            client.Timeout = TimeSpan.FromSeconds(15);

            //Explicity set the requested domain to avoid a bug.
            client.DefaultRequestHeaders.Host = $"{domain}.azurewebsites.net";

            return client;
        }

        /// <summary>
        /// Converts an object to a formatted x-www-urlformencoded string.
        /// </summary>
        /// <param name="dictionary">Object.</param>
        /// <returns>Formatted x-www-urlformencoded string.</returns>
        string DictionaryToWwwwFormData(Dictionary<string, string> dictionary)
        {
            StringBuilder sb = new StringBuilder();

            //Format the key/value pairs from the dictionary to a x-wwww-formurlencoded string.
            foreach (KeyValuePair<string, string> valuePair in dictionary)
            {
                if (!string.IsNullOrEmpty(valuePair.Key) && !string.IsNullOrEmpty(valuePair.Value))
                {
                    //Dont append an '&' at the start of the string.
                    if (sb.Length > 0) sb.Append("&");

                    //Write the key/value pair to the string
                    sb.Append(valuePair.Key);
                    sb.Append("=");
                    sb.Append(valuePair.Value);
                }
            }
            return sb.ToString();
        }
    }
}