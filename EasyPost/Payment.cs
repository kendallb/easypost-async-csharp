/*
 * Licensed under The MIT License (MIT)
 *
 * Copyright (c) 2014 EasyPost
 * Copyright (C) 2017 AMain.com, Inc.
 * All Rights Reserved
 */

namespace EasyPost
{
    public class Payment
    {
        /// <summary>
        /// Defines the payment type. Supported values are "SENDER", "THIRD_PARTY", "RECEIVER", "COLLECT". Defaults to SENDER.
        /// </summary>
        public string Type  { get; set; }

        /// <summary>
        /// Setting account number. Required for RECEIVER and THIRD_PARTY.
        /// </summary>
        public string Account { get; set; }

        /// <summary>
        /// Setting postal code that the account is based in. Required for RECEIVER and THIRD_PARTY.
        /// </summary>
        public string PostalCode { get; set; }

        /// <summary>
        /// Setting country code that the account is based in. Required for THIRD_PARTY.
        /// </summary>
        public string Country { get; set; }
    }
}