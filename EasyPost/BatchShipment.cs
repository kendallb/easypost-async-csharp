/*
 * Licensed under The MIT License (MIT)
 *
 * Copyright (c) 2014 EasyPost
 * Copyright (C) 2017 AMain.com, Inc.
 * All Rights Reserved
 */

namespace EasyPost
{
    public class BatchShipment : Resource
    {
        /// <summary>
        /// The id of the Shipment.
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// The current status. Possible values are "postage_purchased", "postage_purchase_failed", "queued_for_purchase", and "creation_failed"
        /// </summary>
        public string BatchStatus { get; set; }

        /// <summary>
        /// A human readable message for any errors that occurred during the Batch's life cycle
        /// </summary>
        public string BatchMessage { get; set; }
    }
}