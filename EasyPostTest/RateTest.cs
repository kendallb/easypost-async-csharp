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
    public class RateTest
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
            var fromAddress = new Address {
                Name = "Andrew Tribone",
                Street1 = "480 Fell St",
                Street2 = "#3",
                City = "San Francisco",
                State = "CA",
                Country = "US",
                Zip = "94102",
            };
            var toAddress = new Address {
                Company = "Simpler Postage Inc",
                Street1 = "164 Townsend Street",
                Street2 = "Unit 1",
                City = "San Francisco",
                State = "CA",
                Country = "US",
                Zip = "94107",
            };
            var shipment = new Shipment {
                ToAddress = toAddress,
                FromAddress = fromAddress,
                Parcel = new Parcel {
                    Length = 8,
                    Width = 6,
                    Height = 5,
                    Weight = 10,
                },
                Reference = "ShipmentRef",
            };
            shipment = _client.CreateShipment(shipment).Result;

            var rate = _client.GetRate(shipment.Rates[0].Id).Result;
            Assert.AreEqual(rate.Id, shipment.Rates[0].Id);

            Assert.IsNotNull(rate.Rate);
            Assert.IsNotNull(rate.Currency);
            Assert.IsNotNull(rate.ListRate);
            Assert.IsNotNull(rate.ListCurrency);
        }
    }
}