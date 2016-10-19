﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using PartyUp.Model.Enum;
using PartyUp.Model.Model;
using PartyUp.Service.Service;

namespace PartyUp.Service.Interface
{
    public interface IClientService
    {
        //TODO CLIENT return a "response object" with additionel infos :)
        /// <summary>
        /// Sends a requests, deserializes the content and handles exceptions.
        /// </summary>
        /// <typeparam name="TExpectedType">The expected object type.</typeparam> 
        /// <param name="uri">Uri of the REST endpoint as string.</param>
        /// <param name="restType">The <see cref="RestType"/> of the request.</param>
        /// <param name="query">Optional: A uri query that will be send with the request.</param>
        /// <param name="bodyParameter">Optional: A body parameter that will be send with the request.</param> 
        /// <returns></returns>
        Task<TExpectedType> SendRequest<TExpectedType>(string uri, RestType restType, string query = "", object bodyParameter = null);
    }
}