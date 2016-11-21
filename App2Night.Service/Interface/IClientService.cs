using System.Threading.Tasks;
using App2Night.Model.Model;
using App2Night.Service.Service;

namespace App2Night.Service.Interface
{
    public interface IClientService
    {
        /// <summary>
        /// Sends a requests, deserializes the content and handles exceptions.
        /// </summary>
        /// <typeparam name="TExpectedType">The expected object type.</typeparam> 
        /// <param name="uri">Uri of the REST endpoint as string.</param>
        /// <param name="restType">The <see cref="RestType"/> of the request.</param>
        /// <param name="cacheData">Optional: Should the data get cached.</param>
        /// <param name="urlQuery">Optional: A uri urlQuery that will be send with the request.</param>
        /// <param name="bodyParameter">Optional: A body parameter that will be send with the request.</param>
        /// <param name="wwwFormData"></param>
        /// <param name="token">Add a token.</param>
        /// <param name="endpoint">Request endpoint.</param>
        /// <param name="enableHttps"></param>
        /// <returns></returns>
        Task<Result<TExpectedType>> SendRequest<TExpectedType>(string uri, RestType restType, bool cacheData = false,
            string urlQuery = "", object bodyParameter = null, object wwwFormData = null, string token = null,
            Endpoint endpoint = Endpoint.Api, bool enableHttps = true);

        /// <summary>
        /// Sends a request without expecting a return value, handles exceptions.
        /// </summary> 
        /// <param name="uri">Uri of the REST endpoint as string.</param>
        /// <param name="restType">The <see cref="RestType"/> of the request.</param>
        /// <param name="cacheData">Optional: Should the data get cached.</param>
        /// <param name="urlQuery">Optional: A uri urlQuery that will be send with the request.</param>
        /// <param name="bodyParameter">Optional: A body parameter that will be send with the request.</param>
        /// <param name="wwwFormData"></param>
        /// <param name="token">Add a token.</param>
        /// <param name="endpoint"></param>
        /// <param name="enableHttps"></param>
        /// <returns></returns>
        Task<Result> SendRequest(string uri, RestType restType, bool cacheData = false, string urlQuery = "",
            object bodyParameter = null, object wwwFormData = null, string token = null,
            Endpoint endpoint = Endpoint.Api, bool enableHttps = true);
    }
}