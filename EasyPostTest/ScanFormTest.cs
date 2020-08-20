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
    public class ScanFormTest
    {
        private EasyPostClient _client;

        [TestInitialize]
        public void Initialize()
        {
            _client = new EasyPostClient("NvBX2hFF44SVvTPtYjF0zQ");
        }

        [TestMethod]
        public void TestScanFormList()
        {
            var scanFormList = _client.ListScanForms(new ScanFormListOptions {
                PageSize = 1,
            }).Result;
            Assert.AreNotEqual(null, scanFormList.ScanForms[0].BatchId);
            Assert.AreNotEqual(0, scanFormList.ScanForms.Count);
            var nextScanFormList = scanFormList.Next(_client).Result;
            Assert.AreNotEqual(scanFormList.ScanForms[0].Id, nextScanFormList.ScanForms[0].Id);
        }

        [TestMethod]
        public void TestGetScanForm()
        {
            var scanForm = _client.GetScanForm("sf_e35ae7fc59bb4482ae32efc663267104").Result;
            Assert.AreEqual(scanForm.Id, "sf_e35ae7fc59bb4482ae32efc663267104");
        }
    }
}