/*
 * Licensed under The MIT License (MIT)
 *
 * Copyright (c) 2014 EasyPost
 * Copyright (C) 2017 AMain.com, Inc.
 * All Rights Reserved
 */

using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using EasyPost;

namespace EasyPostTest
{
    [TestClass]
    public class OrderTest
    {
        private EasyPostClient _client;
        private Order _testOrder;
        private List<Shipment> _testShipments;
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
            _testShipments = new List<Shipment> {
                new Shipment {
                    Parcel = new Parcel {
                        Length = 8,
                        Width = 6,
                        Height = 5,
                        Weight = 18,
                    },
                },
                new Shipment {
                    Parcel = new Parcel {
                        Length = 9,
                        Width = 5,
                        Height = 4,
                        Weight = 18,
                    },
                },
            };
            _testOrder = new Order {
                ToAddress = _toAddress,
                FromAddress = _fromAddress,
                Reference = "OrderRef",
                Shipments = _testShipments,
            };
        }

        [TestMethod]
        public void TestCreateAndRetrieveOrder()
        {
            var order = _client.CreateOrder(_testOrder).Result;

            Assert.IsNotNull(order.Id);
            Assert.AreEqual(order.Reference, "OrderRef");

            var retrieved = _client.GetOrder(order.Id).Result;
            Assert.AreEqual(order.Id, retrieved.Id);
        }

        [TestMethod]
        public void TestBuyOrder()
        {
            var order = _client.CreateOrder(_testOrder).Result;
            order = _client.BuyOrder(order.Id, "USPS", "Priority").Result;
            Assert.IsNotNull(order.Shipments[0].PostageLabel);
        }

        [TestMethod]
        public void TestOrderCarrierAccounts()
        {
            _testOrder.CarrierAccounts = new List<CarrierAccount> {
                new CarrierAccount {
                    Id = "ca_7642d249fdcf47bcb5da9ea34c96dfcf",
                }
            };
            var order = _client.CreateOrder(_testOrder).Result;

            Assert.IsNotNull(order.Id);
            Assert.AreEqual(order.Reference, "OrderRef");
            CollectionAssert.AreEqual(new HashSet<string>(order.Shipments.SelectMany(s => s.Rates).Select(r => r.CarrierAccountId)).ToList(),
                new List<string> { "ca_7642d249fdcf47bcb5da9ea34c96dfcf" });
            Assert.AreEqual(3, order.Rates.Count);

            _testOrder.CarrierAccounts = null;
            var largeOrder = _client.CreateOrder(_testOrder).Result;
            Assert.IsTrue(order.Rates.Count < largeOrder.Rates.Count);
        }
    }
}