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
    public class ReportTest
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
            var report = _client.CreateReport("shipment", new Report {
                // Unfortunately, this can only be run once a day. If you need to test more than that change the date here.
                //EndDate = DateTime.Parse("2016-06-01"),
            }).Result;
            Assert.IsNotNull(report.Id);

            var retrieved = _client.GetReport("shipment", report.Id).Result;
            Assert.AreEqual(report.Id, retrieved.Id);

            retrieved = _client.GetReport(report.Id).Result;
            Assert.AreEqual(report.Id, retrieved.Id);
        }

        [TestMethod]
        public void TestList()
        {
            var reportList = _client.ListReports("shipment", new ReportListOptions {
                PageSize = 1,
            }).Result;
            Assert.AreNotEqual(0, reportList.Reports.Count);

            var nextReportList = reportList.Next(_client).Result;
            Assert.AreNotEqual(reportList.Reports[0].Id, nextReportList.Reports[0].Id);
        }
    }
}