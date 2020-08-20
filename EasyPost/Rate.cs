/*
 * Licensed under The MIT License (MIT)
 *
 * Copyright (c) 2014 EasyPost
 * Copyright (C) 2017 AMain.com, Inc.
 * All Rights Reserved
 */

using System;
using System.Threading.Tasks;

namespace EasyPost
{
    public class CarrierRate : EasyPostObject
    {
        /// <summary>
        /// Service level/name. See here for details:
        /// https://www.easypost.com/docs/api#service-levels
        /// </summary>
        public string Service { get; set; }

        /// <summary>
        /// Name of carrier
        /// </summary>
        public string Carrier { get; set; }

        /// <summary>
        /// ID of the CarrierAccount record used to generate this rate
        /// </summary>
        public string CarrierAccountId { get; set; }

        /// <summary>
        /// ID of the Shipment this rate belongs to
        /// </summary>
        public string ShipmentId { get; set; }

        /// <summary>
        /// The actual rate quote for this service
        /// </summary>
        public double Rate { get; set; }

        /// <summary>
        /// Currency for the rate
        /// </summary>
        public string Currency { get; set; }

        /// <summary>
        /// The retail rate is the in-store rate given with no account
        /// </summary>
        public double RetailRate { get; set; }

        /// <summary>
        /// Currency for the retail rate
        /// </summary>
        public string RetailCurrency { get; set; }

        /// <summary>
        /// The list rate is the non-negotiated rate given for having an account with the carrier
        /// </summary>
        public double ListRate { get; set; }

        /// <summary>
        /// Currency for the list rate
        /// </summary>
        public string ListCurrency { get; set; }

        /// <summary>
        /// Delivery days for this service
        /// </summary>
        public int? DeliveryDays { get; set; }

        /// <summary>
        /// Date for delivery
        /// </summary>
        public DateTime? DeliveryDate { get; set; }

        /// <summary>
        /// Indicates if delivery window is guaranteed (true) or not (false)
        /// </summary>
        public bool DeliveryDateGuaranteed { get; set; }

        /// <summary>
        /// *This field is deprecated and should be ignored.
        /// </summary>
        public int? EstDeliveryDays { get; set; }
    }

    /// <summary>
    /// Rate API implementation
    /// </summary>
    public partial class EasyPostClient
    {
        /// <summary>
        /// Retrieve a Rate from its id.
        /// </summary>
        /// <param name="id">String representing a Rate. Starts with "rate_".</param>
        /// <returns>CarrierRate instance.</returns>
        public async Task<CarrierRate> GetRate(
            string id)
        {
            var request = new EasyPostRequest("rates/{id}");
            request.AddUrlSegment("id", id);

            return await Execute<CarrierRate>(request);
        }
    }
}