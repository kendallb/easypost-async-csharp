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
    public class AddressTest
    {
        private EasyPostClient _client;
        private Address _testAddress;

        [TestInitialize]
        public void Initialize()
        {
            _client = new EasyPostClient("NvBX2hFF44SVvTPtYjF0zQ");
            _testAddress = new Address {
                Company = "Simpler Postage Inc",
                Street1 = "164 Townsend Street",
                Street2 = "Unit 1",
                City = "San Francisco",
                State = "CA",
                Country = "US",
                Zip = "94107"
            };
        }

        [TestMethod]
        public void TestRetrieveInvalidId()
        {
            var address = _client.GetAddress("not-an-id").Result;
            Assert.IsNotNull(address.RequestError);
            Assert.AreEqual(address.RequestError.StatusCode, HttpStatusCode.NotFound);
            Assert.AreEqual(address.RequestError.Code, "NOT_FOUND");
            Assert.AreEqual(address.RequestError.Message, "The requested resource could not be found.");
            Assert.AreEqual(address.RequestError.Content, "{\"error\":{\"code\":\"NOT_FOUND\",\"message\":\"The requested resource could not be found.\",\"errors\":[]}}");
        }

        [TestMethod]
        public void TestCreateAndRetrieve()
        {
            var address = _client.CreateAddress(_testAddress).Result;
            Assert.IsNotNull(address.Id);
            Assert.AreEqual(address.Company, "Simpler Postage Inc");
            Assert.IsNull(address.Name);

            var retrieved = _client.GetAddress(address.Id).Result;
            Assert.AreEqual(address.Id, retrieved.Id);
        }

        [TestMethod]
        public void TestCreateWithVerifications()
        {
            var address = _client.CreateAddress(_testAddress, VerificationFlags.Delivery | VerificationFlags.Zip4).Result;
            Assert.IsNotNull(address.Verifications.Delivery);
            Assert.AreEqual(address.Verifications.Delivery.Success, true);
            Assert.IsNotNull(address.Verifications.Zip4);
            Assert.AreEqual(address.Verifications.Zip4.Success, true);

            address = _client.CreateAddress(new Address {
                    Company = "Simpler Postage Inc",
                    Street1 = "123 Fake Street",
                    Zip = "94107"
                },
                VerificationFlags.Delivery | VerificationFlags.Zip4).Result;
            Assert.AreEqual(address.Verifications.Delivery.Success, false);
            Assert.AreEqual(address.Verifications.Zip4.Success, false);
        }

        [TestMethod]
        public void TestCreateWithStrictVerifications()
        {
            var address = _client.CreateAddress(new Address {
                    Company = "Simpler Postage Inc",
                    Street1 = "123 Fake Street",
                    Zip = "94107"
                },
                VerificationFlags.DeliveryStrict | VerificationFlags.Zip4Strict).Result;
            Assert.IsNotNull(address.RequestError);
            Assert.AreEqual(address.RequestError.StatusCode, (HttpStatusCode)422);
            Assert.AreEqual(address.RequestError.Code, "ADDRESS.VERIFY.FAILURE");
            Assert.AreEqual(address.RequestError.Message, "Unable to verify address.");
            Assert.AreEqual(address.RequestError.Errors.Count, 1);
            Assert.AreEqual(address.RequestError.Errors[0].Code, "E.ADDRESS.NOT_FOUND");
            Assert.AreEqual(address.RequestError.Errors[0].Field, "address");
            Assert.AreEqual(address.RequestError.Errors[0].Message, "Address not found");
            Assert.AreEqual(address.RequestError.Errors[0].Suggestion, null);
        }

        [TestMethod]
        public void TestVerify()
        {
            var address = _client.CreateAddress(_testAddress).Result;
            address = _client.VerifyAddress(address).Result;
            Assert.IsNotNull(address.Id);
            Assert.AreEqual(address.Company, "SIMPLER POSTAGE INC");
            Assert.AreEqual(address.Street1, "164 TOWNSEND ST UNIT 1");
            Assert.IsNull(address.Name);
            Assert.IsFalse((bool)address.Residential);
        }

        [TestMethod]
        public void TestVerifyCarrier()
        {
            var address = _client.CreateAddress(_testAddress).Result;
            address = _client.VerifyAddress(address, "usps").Result;
            Assert.IsNotNull(address.Id);
            Assert.AreEqual(address.Company, "SIMPLER POSTAGE INC");
            Assert.AreEqual(address.Street1, "164 TOWNSEND ST UNIT 1");
            Assert.IsNull(address.Name);
        }

        [TestMethod]
        public void TestVerifyBeforeCreate()
        {
            var address = _client.VerifyAddress(_testAddress).Result;
            Assert.IsNotNull(address.Id);
            Assert.AreEqual(address.Company, "SIMPLER POSTAGE INC");
        }

        [TestMethod]
        public void TestVerificationFailure()
        {
            _testAddress.Street1 = "1645456 Townsend Street";
            var address = _client.CreateAddress(_testAddress).Result;
            address = _client.VerifyAddress(address).Result;
            Assert.IsNotNull(address.RequestError);
            Assert.AreEqual(address.RequestError.StatusCode, (HttpStatusCode)422);
            Assert.AreEqual(address.RequestError.Code, "ADDRESS.VERIFY.FAILURE");
            Assert.AreEqual(address.RequestError.Message, "Unable to verify address.");
            Assert.AreEqual(address.RequestError.Errors.Count, 1);
            Assert.AreEqual(address.RequestError.Errors[0].Code, "E.ADDRESS.NOT_FOUND");
            Assert.AreEqual(address.RequestError.Errors[0].Field, "address");
            Assert.AreEqual(address.RequestError.Errors[0].Message, "Address not found");
        }
    }
}