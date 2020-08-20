/*
 * Licensed under The MIT License (MIT)
 *
 * Copyright (c) 2014 EasyPost
 * Copyright (C) 2017 AMain.com, Inc.
 * All Rights Reserved
 */

using System;
using System.Threading.Tasks;
using RestSharp;

namespace EasyPost
{
    public class Report : EasyPostObject
    {
        /// <summary>
        /// "new", "available", "failed", or null
        /// </summary>
        public string Status { get; set; }

        /// <summary>
        /// A date string in YYYY-MM-DD form eg: "2016-02-02"
        /// </summary>
        public DateTime? StartDate { get; set; }

        /// <summary>
        /// A date string in YYYY-MM-DD form eg: "2016-02-03"
        /// </summary>
        public DateTime? EndDate { get; set; }

        /// <summary>
        /// Set true if you would like to include Shipments /Trackers created by child users
        /// </summary>
        public bool IncludeChildren { get; set; }

        /// <summary>
        /// A url that contains a link to the Report. Expires 30 seconds after retrieving this object
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        /// Url expiring time
        /// </summary>
        public DateTime? UrlExpiresAt { get; set; }

        /// <summary>
        /// Set true if you would like to send an email containing the Report
        /// </summary>
        public bool? SendEmail { get; set; }
    }

    /// <summary>
    /// Report API implementation
    /// </summary>
    public partial class EasyPostClient
    {
        /// <summary>
        /// Retrieve a Report from its id.
        /// </summary>
        /// <param name="id">String representing a report.</param>
        /// <returns>Report instance.</returns>
        public async Task<Report> GetReport(
            string id)
        {
            var request = new EasyPostRequest("reports/{id}");
            request.AddUrlSegment("id", id);

            return await Execute<Report>(request);
        }

        /// <summary>
        /// Retrieve a Report from its id and type.
        /// </summary>
        /// <param name="type">Type of report, e.g. shipment, tracker, payment_log, etc.</param>
        /// <param name="id">String representing a report.</param>
        /// <returns>Report instance.</returns>
        public async Task<Report> GetReport(
            string type,
            string id)
        {
            var request = new EasyPostRequest("reports/{type}/{id}");
            request.AddUrlSegment("id", id);
            request.AddUrlSegment("type", type);

            return await Execute<Report>(request);
        }

        /// <summary>
        /// Create a Report.
        /// </summary>
        /// <param name="type">Type of the report</param>
        /// <param name="report">Report parameters. Valid fields:
        /// . StartDate
        /// . EndDate
        /// . IncludeChildren
        /// All invalid keys will be ignored. </param>
        /// <returns>Report instance.</returns>
        public async Task<Report> CreateReport(
            string type,
            Report report = null)
        {
            var request = new EasyPostRequest("reports/{type}", Method.POST);
            request.AddUrlSegment("type", type);
            if (report != null) {
                request.AddQueryString(report.AsDictionary());
            }

            return await Execute<Report>(request);
        }

        /// <summary>
        /// Get a paginated list of reports.
        /// </summary>
        /// <param name="type">Type of the report</param>
        /// <param name="options">Options for the pagination function</param>
        /// <returns>Instance of EasyPost.ScanForm</returns>
        public async Task<ReportList> ListReports(
            string type,
            ReportListOptions options = null)
        {
            var request = new EasyPostRequest("reports/{type}");
            request.AddUrlSegment("type", type);
            if (options != null) {
                request.AddQueryString(options.AsDictionary());
            }

            var reportList = await Execute<ReportList>(request);
            reportList.Options = options;
            reportList.Type = type;
            return reportList;
        }
    }
}
