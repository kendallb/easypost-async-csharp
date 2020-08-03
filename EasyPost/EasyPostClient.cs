/*
 * Licensed under The MIT License (MIT)
 *
 * Copyright (c) 2014 EasyPost
 * Copyright (C) 2017 AMain.com, Inc.
 * All Rights Reserved
 */

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Reflection;
using System.Threading.Tasks;
using RestSharp;
using RestSharp.Serialization.Json;

namespace EasyPost
{
    public partial class EasyPostClient : IEasyPostClient
    {
        internal readonly RestClient RestClient;
        internal readonly ClientConfiguration Configuration;
        internal readonly string Version;

        /// <summary>
        /// Create a new EasyPost client
        /// </summary>
        /// <param name="apiKey">API key to use</param>
        public EasyPostClient(
            string apiKey)
            : this(new ClientConfiguration(apiKey))
        {
        }

        /// <summary>
        /// Create a new EasyPost client
        /// </summary>
        /// <param name="apiKey">API key to use</param>
        /// <param name="timeout">The timeout to use for client operations. 0 for the default.</param>
        public EasyPostClient(
            string apiKey,
            int timeout)
            : this(new ClientConfiguration(apiKey, timeout))
        {
        }

        /// <summary>
        /// True if the requests should be executed using non-async code for backwards compatibility
        /// </summary>
        public bool ExecuteNonAsync { get; set; }

        /// <summary>
        /// Create a new EasyPost client
        /// </summary>
        /// <param name="clientConfiguration">Client configuration to use</param>
        public EasyPostClient(
            ClientConfiguration clientConfiguration)
        {
            if (clientConfiguration == null) {
                throw new ArgumentNullException(nameof(clientConfiguration));
            }
            Configuration = clientConfiguration;
            RestClient = new RestClient(clientConfiguration.ApiBase);
            if (clientConfiguration.Timeout > 0) {
                RestClient.Timeout = clientConfiguration.Timeout;
            }

            var assembly = Assembly.GetExecutingAssembly();
            var info = FileVersionInfo.GetVersionInfo(assembly.Location);
            Version = info.FileVersion;
        }

        /// <summary>
        /// Internal function to execute a request
        /// </summary>
        /// <param name="request">EasyPost request to execute</param>
        private Task<IRestResponse> Execute(
            EasyPostRequest request)
        {
            return RestClient.ExecuteAsync(PrepareRequest(request));
        }

        /// <summary>
        /// Internal function to execute a typed request
        /// </summary>
        /// <typeparam name="TResponse">Type of the JSON response we are expecting</typeparam>
        /// <param name="request">EasyPost request to execute</param>
        /// <returns>Response for the request</returns>
        private async Task<TResponse> Execute<TResponse>(
            EasyPostRequest request) where TResponse : new()
        {
            IRestResponse<TResponse> response;
            if (ExecuteNonAsync) {
                response = RestClient.Execute<TResponse>(PrepareRequest(request));
            } else {
                response = await RestClient.ExecuteAsync<TResponse>(PrepareRequest(request));
            }
            var statusCode = response.StatusCode;
            var data = response.Data;

            if (data == null || statusCode >= HttpStatusCode.BadRequest) {
                // Bail early if this is not an EasyPost object
                var result = data as EasyPostObject;
                RequestError requestError;
                if (result == null) {
                    // Return the RestSharp error message if we can
                    data = new TResponse();
                    result = data as EasyPostObject;
                    if (response.ErrorMessage == null || result == null) {
                        return default(TResponse);
                    }
                    requestError = new RequestError {
                        Code = "RESPONSE.ERROR",
                        Message = response.ErrorMessage,
                        Errors = new List<Error>(),
                    };
                } else {
                    // Try to parse any generic EasyPost request errors first
                    var deserializer = new JsonDeserializer {
                        RootElement = "error",
                    };
                    requestError = deserializer.Deserialize<RequestError>(response);
                    if (requestError.Code == null) {
                        // Can't make sense of the error so return a general one
                        requestError = new RequestError {
                            Code = "RESPONSE.PARSE_ERROR",
                            Message = "Unknown request error or unable to parse response",
                            Errors = new List<Error>(),
                        };
                    }
                }
                requestError.StatusCode = statusCode;
                requestError.Content = response.Content;
                result.RequestError = requestError;
            }

            return data;
        }

        /// <summary>
        /// Internal function to prepate the request to be executed
        /// </summary>
        /// <param name="request">EasyPost request to be executed</param>
        /// <returns>RestSharp request to execute</returns>
        internal RestRequest PrepareRequest(
            EasyPostRequest request)
        {
            var restRequest = request.RestRequest;

            restRequest.AddHeader("user_agent", string.Concat("EasyPost/CSharpASync/", Version));
            restRequest.AddHeader("authorization", "Bearer " + Configuration.ApiKey);
            restRequest.AddHeader("content_type", "application/x-www-form-urlencoded");

            return restRequest;
        }
    }
}