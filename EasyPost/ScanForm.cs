/*
 * Licensed under The MIT License (MIT)
 *
 * Copyright (c) 2014 EasyPost
 * Copyright (C) 2017 AMain.com, Inc.
 * All Rights Reserved
 */

using System.Collections.Generic;
using System.Threading.Tasks;

namespace EasyPost
{
    public class ScanForm : EasyPostObject
    {
        /// <summary>
        /// Current status. Possible values are "creating", "created" and "failed"
        /// </summary>
        public string Status { get; set; }

        /// <summary>
        /// Human readable message explaining any failures
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// Address the will be Shipments shipped from
        /// </summary>
        public Address Address { get; set; }

        /// <summary>
        /// Tracking codes included on the ScanForm
        /// </summary>
        public List<string> TrackingCodes { get; set; }

        /// <summary>
        /// Url of the document
        /// </summary>
        public string FormUrl { get; set; }

        /// <summary>
        /// File format of the document
        /// </summary>
        public string FormFileType { get; set; }

        /// <summary>
        /// The id of the associated Batch. Unique, starts with "batch_"
        /// </summary>
        public string BatchId { get; set; }
    }

    /// <summary>
    /// ScanForm API implementation
    /// </summary>
    public partial class EasyPostClient
    {
        /// <summary>
        /// Get a paginated list of scan forms.
        /// </summary>
        /// <param name="options">Options for the pagination function</param>
        /// <returns>Instance of ScanFormList</returns>
        public async Task<ScanFormList> ListScanForms(
            ScanFormListOptions options = null)
        {
            var request = new EasyPostRequest("scan_forms");
            if (options != null) {
                request.AddQueryString(options.AsDictionary());
            }

            var scanFormList = await Execute<ScanFormList>(request);
            scanFormList.Options = options;
            return scanFormList;
        }

        /// <summary>
        /// Gets a scan form from it's ID
        /// </summary>
        /// <param name="id">ID of the scan form</param>
        /// <returns>Instance of ScanForm</returns>
        public async Task<ScanForm> GetScanForm(
            string id)
        {
            var request = new EasyPostRequest("scan_forms/{id}");
            request.AddUrlSegment("id", id);

            return await Execute<ScanForm>(request);
        }
    }
}