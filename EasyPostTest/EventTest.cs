/*
 * Licensed under The MIT License (MIT)
 *
 * Copyright (c) 2014 EasyPost
 * Copyright (C) 2017 AMain.com, Inc.
 * All Rights Reserved
 */

using System.Net;
using EasyPost;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace EasyPostTest
{
    [TestClass]
    public class EventTest
    {
        private EasyPostClient _client;

        [TestInitialize]
        public void Initialize()
        {
            _client = new EasyPostClient("NvBX2hFF44SVvTPtYjF0zQ");
        }

        [TestMethod]
        public void TestRetrieve()
        {
            // Events are archived after some time. Lets at least make sure we get a 404.
            var e = _client.GetEvent("evt_d0000c1a9c6c4614949af6931ea9fac8").Result;
            Assert.IsNotNull(e.RequestError);
            Assert.AreEqual(e.RequestError.StatusCode, HttpStatusCode.NotFound);
            Assert.AreEqual(e.RequestError.Code, "EVENT.NOT_FOUND");
            Assert.AreEqual(e.RequestError.Message, "The event(s) could not be found.");
            Assert.AreEqual(e.RequestError.Errors.Count, 0);

        }
    }
}