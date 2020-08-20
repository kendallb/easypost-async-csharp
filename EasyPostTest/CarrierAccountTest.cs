/*
 * Licensed under The MIT License (MIT)
 *
 * Copyright (c) 2014 EasyPost
 * Copyright (C) 2017 AMain.com, Inc.
 * All Rights Reserved
 */

using Microsoft.VisualStudio.TestTools.UnitTesting;
using EasyPost;

namespace EasyPostTest
{
    [TestClass]
    public class CarrierAccountTest
    {
        private EasyPostClient _client;

        [TestInitialize]
        public void Initialize()
        {
            _client = new EasyPostClient("GxhY479LTioDWsGcEtSAfQ");
        }

        [TestMethod]
        public void TestRetrieve()
        {
            var account = _client.GetCarrierAccount("ca_7642d249fdcf47bcb5da9ea34c96dfcf").Result;
            Assert.AreEqual("ca_7642d249fdcf47bcb5da9ea34c96dfcf", account.Id);
        }

        [TestMethod]
        public void TestCrud()
        {
            var account = _client.CreateCarrierAccount(new CarrierAccount {
                Type = "DhlExpressAccount",
                Description = "description",
            }).Result;

            Assert.IsNotNull(account.Id);
            Assert.AreEqual(account.Type, "DhlExpressAccount");

            account.Reference = "new-reference";
            account = _client.UpdateCarrierAccount(account).Result;
            Assert.AreEqual("new-reference", account.Reference);

            _client.DestroyCarrierAccount(account.Id).Wait();

            account = _client.GetCarrierAccount(account.Id).Result;
            Assert.IsNotNull(account.RequestError);
            Assert.AreEqual(account.RequestError.Code, "NOT_FOUND");
        }

        [TestMethod]
        public void TestList()
        {
            var accounts = _client.ListCarrierAccounts().Result;
            Assert.AreEqual(accounts[0].Id, "ca_7642d249fdcf47bcb5da9ea34c96dfcf");
        }
    }
}