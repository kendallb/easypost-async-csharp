/*
 * Licensed under The MIT License (MIT)
 *
 * Copyright (c) 2014 EasyPost
 * Copyright (C) 2017 AMain.com, Inc.
 * All Rights Reserved
 */

using System.Collections.Generic;
using System.Linq;
using EasyPost;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RestSharp;

namespace EasyPostTest
{
    [TestClass]
    public class RequestTest
    {
        private EasyPostClient _client;

        [TestInitialize]
        public void Initialize()
        {
            _client = new EasyPostClient("NvBX2hFF44SVvTPtYjF0zQ");
        }

        [TestMethod]
        public void TestRestRequest()
        {
            var request = new EasyPostRequest("resource");
            Assert.IsInstanceOfType(request.RestRequest, typeof(RestRequest));
        }

        [TestMethod]
        public void TestAddBody()
        {
            var request = new EasyPostRequest("resource");
            request.AddBody(new Dictionary<string, object> { { "foo", "bar" } }, "parent");

            var restRequest = request.RestRequest;
            CollectionAssert.Contains(restRequest.Parameters.Select(parameter => parameter.ToString()).ToList(),
                "application/x-www-form-urlencoded=parent%5Bfoo%5D=bar");
        }

        [TestMethod]
        public void TestEncodePayload()
        {
            var request = new EasyPostRequest("resource");
            var result = request.EncodeParameters(new List<KeyValuePair<string, string>> {
                new KeyValuePair<string, string>("parent[foo]", "bar"),
                new KeyValuePair<string, string>("parent[baz]", "qux")
            });
            Assert.AreEqual(result, "parent%5Bfoo%5D=bar&parent%5Bbaz%5D=qux");
        }

        [TestMethod]
        public void TestFlattenParameters()
        {
            var request = new EasyPostRequest("resource");
            var parameters = new Dictionary<string, object> { { "foo", "bar" }, { "baz", "qux" } };
            var result = request.FlattenParameters(parameters, "parent");
            CollectionAssert.Contains(result, new KeyValuePair<string, string>("parent[foo]", "bar"));
            CollectionAssert.Contains(result, new KeyValuePair<string, string>("parent[baz]", "qux"));
        }

        [TestMethod]
        public void TestFlattenParametersWithNestedDictionary()
        {
            var request = new EasyPostRequest("resource");
            var parameters = new Dictionary<string, object> {
                { "foo", new Dictionary<string, object> { { "bar", "baz" } } },
                { "baz", "qux" }
            };
            var result = request.FlattenParameters(parameters, "parent");
            CollectionAssert.Contains(result, new KeyValuePair<string, string>("parent[foo][bar]", "baz"));
            CollectionAssert.Contains(result, new KeyValuePair<string, string>("parent[baz]", "qux"));
        }

        [TestMethod]
        public void TestFlattenParametersWithNestedList()
        {
            var request = new EasyPostRequest("resource");
            var parameters = new Dictionary<string, object> {
                { "foo", new List<Dictionary<string, object>> { new Dictionary<string, object> { { "bar", "baz" } } } },
                { "baz", "qux" }
            };
            var result = request.FlattenParameters(parameters, "parent");
            CollectionAssert.Contains(result, new KeyValuePair<string, string>("parent[foo][0][bar]", "baz"));
            CollectionAssert.Contains(result, new KeyValuePair<string, string>("parent[baz]", "qux"));
        }
    }
}
