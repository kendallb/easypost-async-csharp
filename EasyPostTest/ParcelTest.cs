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
    public class ParcelTest
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
            var parcel = _client.CreateParcel(new Parcel {
                Length = 10,
                Width = 20,
                Height = 5,
                Weight = 1.8,
            }).Result;
            var retrieved = _client.GetParcel(parcel.Id).Result;
            Assert.AreEqual(parcel.Id, retrieved.Id);
        }

        [TestMethod]
        public void TestPredefinedPackage()
        {
            var parcel = new Parcel { Weight = 1.8, PredefinedPackage = "SMALLFLATRATEBOX" };
            var shipment = new Shipment { Parcel = parcel };
            shipment = _client.CreateShipment(shipment).Result;

            Assert.AreEqual(null, shipment.Parcel.Height);
            Assert.AreEqual("SMALLFLATRATEBOX", shipment.Parcel.PredefinedPackage);
        }
    }
}