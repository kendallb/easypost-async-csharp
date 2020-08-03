/*
 * Licensed under The MIT License (MIT)
 *
 * Copyright (c) 2014 EasyPost
 * Copyright (C) 2017 AMain.com, Inc.
 * All Rights Reserved
 */

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using RestSharp;

namespace EasyPost
{
    public class EasyPostRequest
    {
        /// <summary>
        /// Create a new EasyPost request
        /// </summary>
        /// <param name="resource">Resource to communicate with</param>
        /// <param name="method">Request method</param>
        public EasyPostRequest(
            string resource,
            Method method = Method.GET)
        {
            RestRequest = new RestRequest(resource, method);
            RestRequest.AddHeader("Accept", "application/json");
        }

        /// <summary>
        /// The underlying RestRequest
        /// </summary>
        public RestRequest RestRequest;

        /// <summary>
        /// Adds a Url segment parameter to the request
        /// </summary>
        /// <param name="name">Name of the parameter</param>
        /// <param name="value">Value of the parameter</param>
        public void AddUrlSegment(
            string name,
            string value)
        {
            RestRequest.AddUrlSegment(name, value);
        }

        /// <summary>
        /// Adds a parameter to the request
        /// </summary>
        /// <param name="name">Name of the parameter</param>
        /// <param name="value">Value of the parameter</param>
        /// <param name="type">Type of the parameter</param>
        public void AddParameter(
            string name,
            string value,
            ParameterType type)
        {
            RestRequest.AddParameter(name, value, type);
        }

        /// <summary>
        /// Adds query strings to the request if the values in the dictionary are not null
        /// </summary>
        /// <param name="parameters">Parameters to add to the query string</param>
        public void AddQueryString(
            IDictionary<string, object> parameters)
        {
            foreach (var pair in parameters) {
                if (pair.Value != null) {
                    AddParameter(pair.Key, Convert.ToString(pair.Value), ParameterType.QueryString);
                }
            }
        }

        /// <summary>
        /// Adds the body to the request as a dictionary of flattened parameters
        /// </summary>
        /// <param name="parameters">Non-flattened dictionary of parameters</param>
        /// <param name="parent">Parent object</param>
        public void AddBody(
            IDictionary<string, object> parameters,
            string parent)
        {
            var encoded = EncodeParameters(FlattenParameters(parameters, parent));
            AddParameter("application/x-www-form-urlencoded", encoded, ParameterType.RequestBody);
        }

        /// <summary>
        /// Adds a list of dictionaries as parameters by combining them all together
        /// </summary>
        /// <param name="parameters">List of parameter dictionaries</param>
        /// <param name="parent">Parent object</param>
        public void AddBody(
            List<Dictionary<string, object>> parameters,
            string parent)
        {
            var result = new List<KeyValuePair<string, string>>();
            for (var i = 0; i < parameters.Count; i++) {
                result.AddRange(FlattenParameters(parameters.ToList()[i], string.Concat(parent, "[", i, "]")));
            }
            AddParameter("application/x-www-form-urlencoded", EncodeParameters(result), ParameterType.RequestBody);
        }

        /// <summary>
        /// Adds a list of key value pairs to the request body
        /// </summary>
        /// <param name="parameters"></param>
        public void AddBody(
            List<KeyValuePair<string, string>> parameters)
        {
            AddParameter("application/x-www-form-urlencoded", EncodeParameters(parameters), ParameterType.RequestBody);
        }

        /// <summary>
        /// URL encodes the list of key/value pairs for the request
        /// </summary>
        /// <param name="parameters">Parameters to encode</param>
        /// <returns>Encoded parameters</returns>
        internal string EncodeParameters(
            List<KeyValuePair<string, string>> parameters)
        {
            return string.Join("&", parameters.Select(EncodeParameter).ToArray());
        }

        /// <summary>
        /// URL encodes a single parameter
        /// </summary>
        /// <param name="parameter">Parameter to encode</param>
        /// <returns>Encoded parameter</returns>
        internal string EncodeParameter(
            KeyValuePair<string, string> parameter)
        {
            return string.Concat(Uri.EscapeDataString(parameter.Key), "=", Uri.EscapeDataString(parameter.Value));
        }

        /// <summary>
        /// Flattens the parameters for the request
        /// </summary>
        /// <param name="parameters">Dictionary of parameters to flattened</param>
        /// <param name="parent">Parent object</param>
        /// <returns>List of flattened parameters as a key value pair set</returns>
        internal List<KeyValuePair<string, string>> FlattenParameters(
            IDictionary<string, object> parameters,
            string parent)
        {
            var result = new List<KeyValuePair<string, string>>();
            foreach (var pair in parameters) {
                if (pair.Value is Dictionary<string, object>) {
                    result.AddRange(FlattenParameters((Dictionary<string, object>)pair.Value, string.Concat(parent, "[", pair.Key, "]")));
                } else if (pair.Value is IResource) {
                    var value = (IResource)pair.Value;
                    result.AddRange(FlattenParameters(value.AsDictionary(), string.Concat(parent, "[", pair.Key, "]")));
                } else if (pair.Value is List<IResource>) {
                    FlattenList(parent, result, pair);
                } else if (pair.Value is IList && pair.Value.GetType().GetGenericArguments().Single().GetInterfaces().Contains(typeof(IResource))) {
                    FlattenList(parent, result, pair);
                } else if (pair.Value is List<string>) {
                    var list = (List<string>)pair.Value;
                    for (var i = 0; i < list.Count; i++) {
                        result.Add(new KeyValuePair<string, string>(string.Concat(parent, "[", pair.Key, "][", i, "]"), list[i]));
                    }
                } else if (pair.Value is List<Dictionary<string, object>>) {
                    var list = (List<Dictionary<string, object>>)pair.Value;
                    for (var i = 0; i < list.Count; i++) {
                        result.AddRange(FlattenParameters(list[i], string.Concat(parent, "[", pair.Key, "][", i, "]")));
                    }
                } else if (pair.Value is DateTime time) {
                    // Force the date time to be UTC over the wire. Even though the docs say it should handle time
                    // zone offsets, it does not appear to do that.
                    var dateTime = time.ToUniversalTime();
                    result.Add(new KeyValuePair<string, string>(string.Concat(parent, "[", pair.Key, "]"),
                        Convert.ToString(dateTime.ToString("yyyy-MM-ddTHH:mm:ssZ"))));
                } else if (pair.Value != null) {
                    result.Add(new KeyValuePair<string, string>(string.Concat(parent, "[", pair.Key, "]"), pair.Value.ToString()));
                }
            }
            return result;
        }

        /// <summary>
        /// Flattens a list of values
        /// </summary>
        /// <param name="parent">Parent value</param>
        /// <param name="result">Place to store the result</param>
        /// <param name="pair">Pair we are flattening</param>
        private void FlattenList(
            string parent,
            List<KeyValuePair<string, string>> result,
            KeyValuePair<string, object> pair)
        {
            var index = 0;
            foreach (IResource resource in pair.Value as IEnumerable) {
                result.AddRange(FlattenParameters(resource.AsDictionary(), string.Concat(parent, "[", pair.Key, "][", index, "]")));
                index++;
            }
        }
    }
}
