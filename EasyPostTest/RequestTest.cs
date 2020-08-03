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
            _client = new EasyPostClient("cueqNZUb3ldeWTNX7MU3Mel8UXtaAMUi");
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
        public void TestAddBodyWithListOfIResource()
        {
            var request = new EasyPostRequest("resource");
            var address = _client.GetAddress("adr_f1369ed31d114c308f627d8879655bd5").Result;
            request.AddBody(new Dictionary<string, object> { { "foo", new List<Address> { address } } }, "parent");

            var restRequest = request.RestRequest;
            CollectionAssert.Contains(restRequest.Parameters.Select(parameter => parameter.ToString()).ToList(),
                "application/x-www-form-urlencoded=parent%5Bfoo%5D%5B0%5D%5Bstreet1%5D=164%20Townsend%20St&parent%5Bfoo%5D%5B0%5D%5Bstreet2%5D=Unit%201&parent%5Bfoo%5D%5B0%5D%5Bcity%5D=San%20Francisco&parent%5Bfoo%5D%5B0%5D%5Bstate%5D=CA&parent%5Bfoo%5D%5B0%5D%5Bzip%5D=94107&parent%5Bfoo%5D%5B0%5D%5Bcountry%5D=US&parent%5Bfoo%5D%5B0%5D%5Bresidential%5D=False&parent%5Bfoo%5D%5B0%5D%5Bname%5D=EasyPost&parent%5Bfoo%5D%5B0%5D%5Bphone%5D=4154567890&parent%5Bfoo%5D%5B0%5D%5Bid%5D=adr_f1369ed31d114c308f627d8879655bd5&parent%5Bfoo%5D%5B0%5D%5Bobject%5D=Address&parent%5Bfoo%5D%5B0%5D%5Bcreated_at%5D=2015-09-15T16%3A03%3A23-07%3A00&parent%5Bfoo%5D%5B0%5D%5Bupdated_at%5D=2015-09-15T16%3A03%3A23-07%3A00&parent%5Bfoo%5D%5B0%5D%5Bmode%5D=test");
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
