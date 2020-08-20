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
    /// <summary>
    /// Define the address verification flags
    /// </summary>
    [Flags]
    public enum VerificationFlags
    {
        None = 0x00,
        Delivery = 0x01,
        DeliveryStrict = 0x02,
        Zip4 = 0x04,
        Zip4Strict = 0x08,
    }
}