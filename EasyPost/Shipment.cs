/*
 * Licensed under The MIT License (MIT)
 *
 * Copyright (c) 2014 EasyPost
 * Copyright (C) 2017 AMain.com, Inc.
 * All Rights Reserved
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RestSharp;

namespace EasyPost
{
    public class Shipment : EasyPostObject
    {
        /// <summary>
        /// The destination address
        /// </summary>
        public Address ToAddress { get; set; }

        /// <summary>
        /// The origin address
        /// </summary>
        public Address FromAddress { get; set; }

        /// <summary>
        /// The shipper's address, defaults to FromAddress
        /// </summary>
        public Address ReturnAddress { get; set; }

        /// <summary>
        /// The buyer's address, defaults to ToAddress
        /// </summary>
        public Address BuyerAddress { get; set; }

        /// <summary>
        /// The dimensions and weight of the package
        /// </summary>
        public Parcel Parcel { get; set; }

        /// <summary>
        /// Information for the processing of customs
        /// </summary>
        public CustomsInfo CustomsInfo { get; set; }

        /// <summary>
        /// Document created to manifest and scan multiple shipments
        /// </summary>
        public ScanForm ScanForm { get; set; }

        /// <summary>
        /// All associated Form objects
        /// </summary>
        public List<Form> Forms { get; set; }

        /// <summary>
        /// The associated Insurance object
        /// </summary>
        public string Insurance { get; set; }

        /// <summary>
        /// All associated Rate objects
        /// </summary>
        public List<CarrierRate> Rates { get; set; }

        /// <summary>
        /// The specific rate purchased for the shipment, or null if unpurchased or purchased through another mechanism
        /// </summary>
        public CarrierRate SelectedRate { get; set; }

        /// <summary>
        /// The associated PostageLabel object
        /// </summary>
        public PostageLabel PostageLabel { get; set; }

        /// <summary>
        /// Any carrier errors encountered during rating, discussed in more depth below
        /// </summary>
        public List<EasyPostMessage> Messages { get; set; }

        /// <summary>
        /// All of the options passed to the shipment, discussed in more depth below
        /// </summary>
        public Options Options { get; set; }

        /// <summary>
        /// Set true to create as a return, discussed in more depth below
        /// </summary>
        public bool? IsReturn { get; set; }

        /// <summary>
        /// If purchased, the tracking code will appear here as well as within the Tracker object
        /// </summary>
        public string TrackingCode { get; set; }

        /// <summary>
        /// The USPS zone of the shipment, if purchased with USPS
        /// </summary>
        public string UspsZone { get; set; }

        /// <summary>
        /// The current tracking status of the shipment
        /// </summary>
        public string Status { get; set; }

        /// <summary>
        /// The associated Tracker object
        /// </summary>
        public Tracker Tracker { get; set; }

        /// <summary>
        /// The associated Fee objects charged to the billing user account
        /// </summary>
        public List<Fee> Fees { get; set; }

        /// <summary>
        /// The current status of the shipment refund process. Possible values are "submitted", "refunded", "rejected".
        /// </summary>
        public string RefundStatus { get; set; }

        /// <summary>
        /// The ID of the batch that contains this shipment, if any
        /// </summary>
        public string BatchId { get; set; }

        /// <summary>
        /// Not sure what this is for, but it's in the non-async C# library
        /// </summary>
        public string OrderId { get; set; }

        /// <summary>
        /// The current state of the associated BatchShipment
        /// </summary>
        public string BatchStatus { get; set; }

        /// <summary>
        /// The current message of the associated BatchShipment
        /// </summary>
        public string BatchMessage { get; set; }

        /// <summary>
        /// Carrier accounts to use
        /// </summary>
        public List<CarrierAccount> CarrierAccounts { get; set; }

        /// <summary>
        /// Optional carrier to select for this shipment. I believe this only works for creation of shipments when creating a batch.
        /// </summary>
        public string Carrier { get; set; }

        /// <summary>
        /// Optional service to select for this shipment. I believe this only works for creation of shipments when creating a batch.
        /// </summary>
        public string Service { get; set; }

        /// <summary>
        /// Get the lowest rate for the shipment. Optionally whitelist/blacklist carriers and services from the search.
        /// </summary>
        /// <param name="includeCarriers">Carriers whitelist.</param>
        /// <param name="includeServices">Services whitelist.</param>
        /// <param name="excludeCarriers">Carriers blacklist.</param>
        /// <param name="excludeServices">Services blacklist.</param>
        /// <returns>EasyPost.Rate instance or null if no rate was found.</returns>
        public CarrierRate LowestRate(
            IEnumerable<string> includeCarriers = null,
            IEnumerable<string> includeServices = null,
            IEnumerable<string> excludeCarriers = null,
            IEnumerable<string> excludeServices = null)
        {
            if (Rates == null) {
                throw new ArgumentException("Shipment rates cannot be null!", nameof(Rates));
            }
            var result = Rates.Select(p => p);
            if (includeCarriers != null) {
                result = result.Where(rate => includeCarriers.Contains(rate.Carrier));
            }
            if (includeServices != null) {
                result = result.Where(rate => includeServices.Contains(rate.Service));
            }
            if (excludeCarriers != null) {
                result = result.Where(rate => !excludeCarriers.Contains(rate.Carrier));
            }
            if (excludeServices != null) {
                result = result.Where(rate => !excludeServices.Contains(rate.Service));
            }
            return result.OrderBy(rate => rate.Rate).FirstOrDefault();
        }
    }

    /// <summary>
    /// Shipment API implementation
    /// </summary>
    public partial class EasyPostClient
    {
        /// <summary>
        /// Get a paginated list of shipments.
        /// </summary>
        /// <param name="options">Options for the pagination function</param>
        /// <returns>Instance of EasyPost.ShipmentList</returns>
        public async Task<ShipmentList> ListShipments(
            ShipmentListOptions options = null)
        {
            var request = new EasyPostRequest("shipments");
            if (options != null) {
                request.AddQueryString(options.AsDictionary());
            }

            var shipmentList = await Execute<ShipmentList>(request);
            shipmentList.Options = options;
            return shipmentList;
        }

        /// <summary>
        /// Retrieve a Shipment from its id.
        /// </summary>
        /// <param name="id">String representing a Shipment. Starts with "shp_".</param>
        /// <returns>Shipment instance.</returns>
        public async Task<Shipment> GetShipment(
            string id)
        {
            var request = new EasyPostRequest("shipments/{id}");
            request.AddUrlSegment("id", id);

            return await Execute<Shipment>(request);
        }

        /// <summary>
        /// Create a Shipment.
        /// </summary>
        /// <param name="shipment">Shipment details</param>
        /// <returns>Shipment instance.</returns>
        public async Task<Shipment> CreateShipment(
            Shipment shipment)
        {
            var request = new EasyPostRequest("shipments", Method.POST);
            request.AddBody(shipment.AsDictionary(), "shipment");

            return await Execute<Shipment>(request);
        }

        /// <summary>
        /// Re-populate the rates property for this shipment
        /// </summary>
        /// <param name="shipment">The shipment to regenerate rates for</param>
        public async Task RegenerateRates(
            Shipment shipment)
        {
            var request = new EasyPostRequest("shipments/{id}/rates");
            request.AddUrlSegment("id", shipment.Id);

            var response = await Execute<Shipment>(request);
            shipment.Rates = response.Rates;
        }

        /// <summary>
        /// Buy a label for this shipment with the given rate.
        /// </summary>
        /// <param name="id">The id of the shipment to buy the label for</param>
        /// <param name="rateId">The id of the rate to purchase the shipment with.</param>
        /// <returns>Shipment instance.</returns>
        public async Task<Shipment> BuyShipment(
            string id,
            string rateId)
        {
            var request = new EasyPostRequest("shipments/{id}/buy", Method.POST);
            request.AddUrlSegment("id", id);
            request.AddBody(new Dictionary<string, object> { { "id", rateId } }, "rate");

            return await Execute<Shipment>(request);
        }

        /// <summary>
        /// Buy insurance for a shipment for the given amount.
        /// </summary>
        /// <param name="id">The id of the shipment to buy insurance for</param>
        /// <param name="amount">The amount to insure the shipment for. Currency is provided when creating a shipment.</param>
        /// <returns>Shipment instance.</returns>
        public async Task<Shipment> BuyInsuranceForShipment(
            string id,
            double amount)
        {
            var request = new EasyPostRequest("shipments/{id}/insure", Method.POST);
            request.AddUrlSegment("id", id);
            request.AddBody(new List<KeyValuePair<string, string>> {
                new KeyValuePair<string, string>("amount", amount.ToString())
            });

            return await Execute<Shipment>(request);
        }

        /// <summary>
        /// Generate a postage label for this shipment.
        /// </summary>
        /// <param name="id">The id of the shipment to generate the label for</param>
        /// <param name="fileFormat">Format to generate the label in. Valid formats: "pdf", "zpl" and "epl2".</param>
        /// <returns>Shipment instance.</returns>
        public async Task<Shipment> GenerateLabel(
            string id,
            string fileFormat)
        {
            var request = new EasyPostRequest("shipments/{id}/label");
            request.AddUrlSegment("id", id);

            // This is a GET, but uses the request body, so use ParameterType.GetOrPost instead.
            request.AddParameter("file_format", fileFormat, ParameterType.GetOrPost);

            return await Execute<Shipment>(request);
        }

        /// <summary>
        /// Send a refund request to the carrier the shipment was purchased from.
        /// </summary>
        /// <param name="id">The id of the shipment to refund</param>
        /// <returns>Shipment instance.</returns>
        public async Task<Shipment> RefundShipment(
            string id)
        {
            var request = new EasyPostRequest("shipments/{id}/refund");
            request.AddUrlSegment("id", id);

            return await Execute<Shipment>(request);
        }
    }
}