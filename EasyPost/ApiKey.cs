/*
 * Licensed under The MIT License (MIT)
 *
 * Copyright (c) 2014 EasyPost
 * Copyright (C) 2017 AMain.com, Inc.
 * All Rights Reserved
 */

using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EasyPost
{
    public class ApiKey
    {
        /// <summary>
        /// The actual key value to use for authentication
        /// </summary>
        public string Key { get; set; }

        /// <summary>
        /// Set based on which api-key you used, either "test" or "production".
        /// </summary>
        public string Mode { get; set; }

        /// <summary>
        /// Date the key was created
        /// </summary>
        public DateTime? CreatedAt { get; set; }
    }

    /// <summary>
    /// Address API implementation
    /// </summary>
    public partial class EasyPostClient
    {
        public class ApiKeysWrapper : EasyPostObject
        {
            public List<ApiKey> Keys { get; set; }
        }

        /// <summary>
        /// Retrieve a list of API Keys
        /// </summary>
        /// <returns>List of API keys</returns>
        public async Task<List<ApiKey>> GetApiKeys()
        {
            var request = new EasyPostRequest("api_keys");
            var keys = await Execute<ApiKeysWrapper>(request);
            return keys?.Keys ?? new List<ApiKey>();
        }
    }
}
