/*
 * Licensed under The MIT License (MIT)
 *
 * Copyright (c) 2014 EasyPost
 * Copyright (C) 2017 AMain.com, Inc.
 * All Rights Reserved
 */

using EasyPost;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace EasyPostTest
{
    [TestClass]
    public class CustomsItemTest
    {
        private EasyPostClient _client;

        [TestInitialize]
        public void Initialize()
        {
            _client = new EasyPostClient("NvBX2hFF44SVvTPtYjF0zQ");
        }

        [TestMethod]
        public void TestCreateAndRetrieve()
        {
            var item = _client.CreateCustomsItem(new CustomsItem {
                Description= "TShirt",
                Quantity = 1,
                Weight = 8,
                Value = 10.0,
                Currency = "USD",
            }).Result;
            var retrieved = _client.GetCustomsItem(item.Id).Result;
            Assert.AreEqual(item.Id, retrieved.Id);
            Assert.AreEqual(retrieved.Value, 10.0);
            Assert.AreEqual(retrieved.Currency, "USD");
        }
    }
}