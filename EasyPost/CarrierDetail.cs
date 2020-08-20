/*
 * Licensed under The MIT License (MIT)
 *
 * Copyright (c) 2014 EasyPost
 * Copyright (C) 2017 AMain.com, Inc.
 * All Rights Reserved
 */

using System;

namespace EasyPost
{
    public class CarrierDetail : Resource
    {
        /// <summary>
        /// The guaranteed delivery date
        /// </summary>
        public DateTime? GuaranteedDeliveryDate { set; get; }

        /// <summary>
        /// Original location
        /// </summary>
        public string OriginLocation { get; set; }

        /// <summary>
        /// Destination location
        /// </summary>
        public string DestinationLocation { get; set; }

        /// <summary>
        /// The service level the associated shipment was shipped with (if available)
        /// </summary>
        public string Service { set; get; }

        /// <summary>
        /// The type of container the associated shipment was shipped in (if available)
        /// </summary>
        public string ContainerType { set; get; }

        /// <summary>
        /// The estimated delivery date as provided by the carrier, in the local time zone (if available)
        /// </summary>
        public DateTime? EstDeliveryDateLocal { set; get; }

        /// <summary>
        /// The estimated delivery time as provided by the carrier, in the local time zone (if available)
        /// </summary>
        public DateTime? EstDeliveryTimeLocal { set; get; }

        /// <summary>
        /// The alternate identifier for this package as provided by the carrier (if available)
        /// </summary>
        public string AlternateIdentifier { get; set; }

        /// <summary>
        /// The date and time of the first attempt by the carrier to deliver the package (if available)
        /// </summary>
        public DateTime? InitialDeliveryAttempt { get; set; }
    }
}