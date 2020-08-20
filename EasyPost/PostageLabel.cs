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
    public class PostageLabel : EasyPostObject
    {
        public int DateAdvance { get; set; }
        public string IntegratedForm { get; set; }
        public DateTime LabelDate { get; set; }
        public int LabelResolution { get; set; }
        public string LabelSize { get; set; }
        public string LabelType { get; set; }
        public string LabelUrl { get; set; }
        public string LabelFile { get; set; }
        public string LabelFileType { get; set; }
        public string LabelPdfUrl { get; set; }
        public string LabelEpl2Url { get; set; }
        public string LabelZplUrl { get; set; }
    }
}