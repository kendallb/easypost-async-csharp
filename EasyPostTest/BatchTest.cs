/*
 * Licensed under The MIT License (MIT)
 *
 * Copyright (c) 2014 EasyPost
 * Copyright (C) 2017 AMain.com, Inc.
 * All Rights Reserved
 */

using EasyPost;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace EasyPostTest
{
    [TestClass]
    public class BatchTest
    {
        private EasyPostClient _client;
        private Shipment _testShipment;
        private Shipment _testBatchShipment;
        private Address _fromAddress;
        private Address _toAddress;

        [TestInitialize]
        public void Initialize()
        {
            _client = new EasyPostClient("NvBX2hFF44SVvTPtYjF0zQ");

            _toAddress = new Address {
                Company = "Simpler Postage Inc",
                Street1 = "164 Townsend Street",
                Street2 = "Unit 1",
                City = "San Francisco",
                State = "CA",
                Country = "US",
                Zip = "94107",
            };
            _fromAddress = new Address {
                Name = "Andrew Tribone",
                Street1 = "480 Fell St",
                Street2 = "#3",
                City = "San Francisco",
                State = "CA",
                Country = "US",
                Zip = "94102",
            };
            _testShipment = new Shipment {
                ToAddress = _toAddress,
                FromAddress = _fromAddress,
                Parcel = new Parcel {
                    Length = 8,
                    Width = 6,
                    Height = 5,
                    Weight = 10,
                },
            };
            _testBatchShipment = new Shipment {
                ToAddress = _toAddress,
                FromAddress = _fromAddress,
                Parcel = new Parcel {
                    Length = 8,
                    Width = 6,
                    Height = 5,
                    Weight = 10,
                },
                Carrier = "USPS",
                Service = "Priority",
            };
        }

        [TestMethod]
        public void TestRetrieve()
        {
            var batch = _client.CreateBatch().Result;
            var retrieved = _client.GetBatch(batch.Id).Result;
            Assert.AreEqual(batch.Id, retrieved.Id);
        }

        [TestMethod]
        public void TestAddRemoveShipments()
        {
            var batch = _client.CreateBatch().Result;
            var shipment = _client.CreateShipment(_testShipment).Result;
            var otherShipment = _client.CreateShipment(_testShipment).Result;

            while (batch.State != "created") {
                batch = _client.GetBatch(batch.Id).Result;
            }

            batch = _client.AddShipmentsToBatch(batch.Id, new[] { shipment, otherShipment }).Result;

            while (batch.Shipments == null) {
                batch = _client.GetBatch(batch.Id).Result;
            }
            var shipmentIds = batch.Shipments.Select(ship => ship.Id).ToList();
            Assert.AreEqual(batch.NumShipments, 2);
            CollectionAssert.Contains(shipmentIds, shipment.Id);
            CollectionAssert.Contains(shipmentIds, otherShipment.Id);

            batch = _client.RemoveShipmentsFromBatch(batch.Id, new[] { shipment, otherShipment }).Result;
            Assert.AreEqual(batch.NumShipments, 0);
        }

        public async Task<Batch> CreateBatch()
        {
            return await _client.CreateBatch(new[] { _testBatchShipment }, "EasyPostCSharpTest");
        }

        [TestMethod]
        public void TestCreateThenBuyThenGenerateLabelAndScanForm()
        {
            var batch = CreateBatch().Result;

            Assert.IsNotNull(batch.Id);
            Assert.AreEqual(batch.Reference, "EasyPostCSharpTest");
            Assert.AreEqual(batch.State, "creating");

            while (batch.State == "creating") {
                batch = _client.GetBatch(batch.Id).Result;
            }
            batch = _client.BuyLabelsForBatch(batch.Id).Result;

            while (batch.State == "created") {
                batch = _client.GetBatch(batch.Id).Result;
            }
            Assert.AreEqual(batch.State, "purchased");

            batch = _client.GenerateLabelForBatch(batch.Id, "pdf").Result;
            Assert.AreEqual(batch.State, "label_generating");

            batch = _client.GenerateScanFormForBatch(batch.Id).Result;
        }

        [TestMethod]
        public void TestGenerateLabelWithOrderBy()
        {
            var batch = CreateBatch().Result;

            while (batch.State == "creating") {
                batch = _client.GetBatch(batch.Id).Result;
            }
            batch = _client.BuyLabelsForBatch(batch.Id).Result;

            while (batch.State == "created") {
                batch = _client.GetBatch(batch.Id).Result;
            }
            batch = _client.GenerateLabelForBatch(batch.Id, "pdf", "reference DESC").Result;
            Assert.AreEqual(batch.State, "label_generating");
        }
    }
}