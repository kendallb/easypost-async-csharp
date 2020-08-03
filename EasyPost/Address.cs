/*
 * Licensed under The MIT License (MIT)
 *
 * Copyright (c) 2014 EasyPost
 * Copyright (C) 2017 AMain.com, Inc.
 * All Rights Reserved
 */

using System.Threading.Tasks;
using RestSharp;

namespace EasyPost
{
    /// <summary>
    /// EasyPost Address objects are used to represent people, places, and organizations in a number of contexts.
    /// </summary>
    public class Address : EasyPostObject
    {
        /// <summary>
        /// First line of the address
        /// </summary>
        public string Street1 { get; set; }

        /// <summary>
        /// Second line of the address
        /// </summary>
        public string Street2 { get; set; }

        /// <summary>
        /// City the address is located in
        /// </summary>
        public string City { get; set; }

        /// <summary>
        /// State or province the address is located in
        /// </summary>
        public string State { get; set; }

        /// <summary>
        /// ZIP or postal code the address is located in
        /// </summary>
        public string Zip { get; set; }

        /// <summary>
        /// Two letter ISO 3166 country code for the country the address is located in
        /// </summary>
        public string Country { get; set; }

        /// <summary>
        /// Whether or not this address would be considered residential
        /// </summary>
        public bool? Residential { get; set; }

        /// <summary>
        /// The specific designation for the address (only relevant if the address is a carrier facility)
        /// </summary>
        public string CarrierFacility { get; set; }

        /// <summary>
        /// Name of the person. Both name and company can be included
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Name of the organization. Both name and company can be included
        /// </summary>
        public string Company { get; set; }

        /// <summary>
        /// Phone number to reach the person or organization
        /// </summary>
        public string Phone { get; set; }

        /// <summary>
        /// Email to reach the person or organization
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// Federal tax identifier of the person or organization
        /// </summary>
        public string FederalTaxId { get; set; }

        /// <summary>
        /// State tax identifier of the person or organization
        /// </summary>
        public string StateTaxId { get; set; }

        /// <summary>
        /// The result of any verifications requested
        /// </summary>
        public Verifications Verifications { get; set; }
    }

    /// <summary>
    /// Address API implementation
    /// </summary>
    public partial class EasyPostClient
    {
        /// <summary>
        /// Retrieve an Address from its id.
        /// </summary>
        /// <param name="id">String representing an Address. Starts with "adr_".</param>
        /// <returns>EasyPost.Address instance.</returns>
        public async Task<Address> GetAddress(
            string id)
        {
            var request = new EasyPostRequest("addresses/{id}");
            request.AddUrlSegment("id", id);

            return await Execute<Address>(request);
        }

        /// <summary>
        /// Create an Address with optional verifications.
        /// </summary>
        /// <param name="address">Address to create</param>
        /// <param name="verify">Verification flags to to control verification. You can verify the delivery address or the
        /// extended zip4 value. If you use the strict versions an HttpException to be raised if unsucessful.
        /// </param>
        /// <returns>Address instance.</returns>
        public async Task<Address> CreateAddress(
            Address address,
            VerificationFlags verify = VerificationFlags.None)
        {
            var request = new EasyPostRequest("addresses", Method.POST);
            request.AddBody(address.AsDictionary(), "address");

            if ((verify & VerificationFlags.Delivery) != 0) {
                request.AddParameter("verify[]", "delivery", ParameterType.QueryString);
            }
            if ((verify & VerificationFlags.Zip4) != 0) {
                request.AddParameter("verify[]", "zip4", ParameterType.QueryString);
            }
            if ((verify & VerificationFlags.DeliveryStrict) != 0) {
                request.AddParameter("verify_strict[]", "delivery", ParameterType.QueryString);
            }
            if ((verify & VerificationFlags.Zip4Strict) != 0) {
                request.AddParameter("verify_strict[]", "zip4", ParameterType.QueryString);
            }

            return await Execute<Address>(request);
        }

        public class AddressWrapper
        {
            public Address Address { get; set; }
        }

        /// <summary>
        /// Verify an address.
        /// </summary>
        /// <returns>Address instance. Check message for verification failures.</returns>
        public async Task<Address> VerifyAddress(
            Address address,
            string carrier = null)
        {
            if (address.Id == null) {
                address = await CreateAddress(address);
            }

            var request = new EasyPostRequest("addresses/{id}/verify");
            request.AddUrlSegment("id", address.Id);

            if (carrier != null) {
                request.AddParameter("carrier", carrier, ParameterType.QueryString);
            }

            var result = await Execute<AddressWrapper>(request);
            return result?.Address;
        }
    }
}