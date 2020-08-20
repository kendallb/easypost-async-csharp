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
    public interface IEasyPostClient
    {
        /// <summary>
        /// True if the requests should be executed using non-async code for backwards compatibility
        /// </summary>
        bool ExecuteNonAsync { get; set; }

        #region Address

        /// <summary>
        /// Retrieve an Address from its id.
        /// </summary>
        /// <param name="id">String representing an Address. Starts with "adr_".</param>
        /// <returns>EasyPost.Address instance.</returns>
        Task<Address> GetAddress(
            string id);

        /// <summary>
        /// Create an Address with optional verifications.
        /// </summary>
        /// <param name="address">Address to create</param>
        /// <param name="verify">Verification flags to to control verification. You can verify the delivery address or the
        /// extended zip4 value. If you use the strict versions an HttpException to be raised if unsucessful.
        /// </param>
        /// <returns>Address instance.</returns>
        Task<Address> CreateAddress(
            Address address,
            VerificationFlags verify = VerificationFlags.None);

        /// <summary>
        /// Verify an address.
        /// </summary>
        /// <returns>EasyPost.Address instance. Check message for verification failures.</returns>
        Task<Address> VerifyAddress(
            Address address,
            string carrier = null);

        #endregion

        #region ApiKey

        /// <summary>
        /// Retrieve a list of API Keys
        /// </summary>
        /// <returns>List of API keys</returns>
        Task<List<ApiKey>> GetApiKeys();

        #endregion

        #region Batch

        /// <summary>
        /// Retrieve a Batch from its id.
        /// </summary>
        /// <param name="id">String representing a Batch. Starts with "batch_".</param>
        /// <returns>Batch instance.</returns>
        Task<Batch> GetBatch(
            string id);

        /// <summary>
        /// Create a Batch.
        /// </summary>
        /// <param name="shipments">Optional list of shipments</param>
        /// <param name="reference">Optional reference</param>
        /// <returns>EasyPost.Batch instance.</returns>
        Task<Batch> CreateBatch(
            IEnumerable<Shipment> shipments = null,
            string reference = null);

        /// <summary>
        /// Add shipments to the batch.
        /// </summary>
        /// <param name="id">Batch id to add the shipments to</param>
        /// <param name="shipmentIds">List of shipment ids to be added.</param>
        /// <returns>Batch instance.</returns>
        Task<Batch> AddShipmentsToBatch(
            string id,
            IEnumerable<string> shipmentIds);

        /// <summary>
        /// Add shipments to the batch.
        /// </summary>
        /// <param name="id">Batch id to add the shipments to</param>
        /// <param name="shipments">List of Shipment objects to be added.</param>
        /// <returns>Batch instance.</returns>
        Task<Batch> AddShipmentsToBatch(
            string id,
            IEnumerable<Shipment> shipments);

        /// <summary>
        /// Remove shipments from the batch.
        /// </summary>
        /// <param name="id">Batch id to add the shipments to</param>
        /// <param name="shipmentIds">List of shipment ids to be removed.</param>
        /// <returns>Batch instance.</returns>
        Task<Batch> RemoveShipmentsFromBatch(
            string id,
            IEnumerable<string> shipmentIds);

        /// <summary>
        /// Remove shipments from the batch.
        /// </summary>
        /// <param name="id">Batch id to add the shipments to</param>
        /// <param name="shipments">List of Shipment objects to be removed.</param>
        /// <returns>Batch instance.</returns>
        Task<Batch> RemoveShipmentsFromBatch(
            string id,
            IEnumerable<Shipment> shipments);

        /// <summary>
        /// Buy labels for all shipments within a batch. The Batch's state must be "created" before purchasing.
        /// </summary>
        /// <param name="id">Batch id to add the shipments to</param>
        /// <returns>Batch instance.</returns>
        Task<Batch> BuyLabelsForBatch(
            string id);

        /// <summary>
        /// Asynchronously generate a label containing all of the Shipment labels belonging to the batch.
        /// </summary>
        /// <param name="id">Batch id to generate the label for</param>
        /// <param name="fileFormat">Format to generate the label in. Valid formats: "pdf", "zpl" and "epl2".</param>
        /// <param name="orderBy">Optional parameter to order the generated label. Ex: "reference DESC"</param>
        /// <returns>Batch instance.</returns>
        Task<Batch> GenerateLabelForBatch(
            string id,
            string fileFormat,
            string orderBy = null);

        /// <summary>
        /// Asychronously generate a scan form for the batch.
        /// </summary>
        /// <param name="id">Batch id to generate the label for</param>
        /// <returns>Batch instance.</returns>
        Task<Batch> GenerateScanFormForBatch(
            string id);

        #endregion

        #region CarrierAccount

        /// <summary>
        /// Get a list of carrier accounts
        /// </summary>
        /// <returns>List of carrier accounts</returns>
        Task<List<CarrierAccount>> ListCarrierAccounts();

        /// <summary>
        /// Retrieve a CarrierAccount from its id.
        /// </summary>
        /// <param name="id">String representing a carrier account. Starts with "ca_".</param>
        /// <returns>CarrierAccount instance.</returns>
        Task<CarrierAccount> GetCarrierAccount(
            string id);

        /// <summary>
        /// Create a CarrierAccount.
        /// </summary>
        /// <param name="carrierAccount">Carriern account details to create</param>
        /// <returns>CarrierAccount instance.</returns>
        Task<CarrierAccount> CreateCarrierAccount(
            CarrierAccount carrierAccount);

        /// <summary>
        /// Update this CarrierAccount.
        /// </summary>
        /// <param name="carrierAccount">Carrier account details</param>
        /// <returns>CarrierAccount instance.</returns>
        Task<CarrierAccount> UpdateCarrierAccount(
            CarrierAccount carrierAccount);

        /// <summary>
        /// Remove this CarrierAccount from your account.
        /// </summary>
        /// <param name="id">Carrier account id</param>
        Task DestroyCarrierAccount(
            string id);

        #endregion

        #region CarrierType

        /// <summary>
        /// Get a list of all carrier types
        /// </summary>
        /// <returns>List of carrier types</returns>
        Task<List<CarrierType>> ListCarrierTypes();

        #endregion

        #region Container

        /// <summary>
        /// Retrieve a Container from its id or reference.
        /// </summary>
        /// <param name="id">String representing a Container. Starts with "container_" if passing an id.</param>
        /// <returns>Container instance.</returns>
        Task<Container> GetContainer(
            string id);

        /// <summary>
        /// Create a Container.
        /// </summary>
        /// <param name="container">Container parameters</param>
        /// <returns>EasyPost.Container instance.</returns>
        Task<Container> CreateContainer(
            Container container);

        #endregion

        #region CustomsInfo

        /// <summary>
        /// Retrieve a CustomsInfo from its id.
        /// </summary>
        /// <param name="id">String representing a CustomsInfo. Starts with "cstinfo_".</param>
        /// <returns>CustomsInfo instance.</returns>
        Task<CustomsInfo> GetCustomsInfo(
            string id);

        /// <summary>
        /// Create a CustomsInfo.
        /// </summary>
        /// <param name="customsInfo">Customs info to create</param>
        /// <returns>EasyPost.CustomsInfo instance.</returns>
        Task<CustomsInfo> CreateCustomsInfo(
            CustomsInfo customsInfo);

        #endregion

        #region CustomsItem

        /// <summary>
        /// Retrieve a CustomsItem from its id.
        /// </summary>
        /// <param name="id">String representing a CustomsItem. Starts with "cstitem_".</param>
        /// <returns>CustomsItem instance.</returns>
        Task<CustomsItem> GetCustomsItem(
            string id);

        /// <summary>
        /// Create a CustomsItem.
        /// </summary>
        /// <param name="customsItem">Customs item parameters</param>
        /// <returns>EasyPost.CustomsItem instance.</returns>
        Task<CustomsItem> CreateCustomsItem(
            CustomsItem customsItem);

        #endregion

        #region Event

        /// <summary>
        /// Retrieve a Event from its id.
        /// </summary>
        /// <param name="id">String representing a Event. Starts with "evt_".</param>
        /// <returns>EasyPost.Event instance.</returns>
        Task<Event> GetEvent(
            string id);

        #endregion

        #region Item

        /// <summary>
        /// Retrieve an Item from its id or reference.
        /// </summary>
        /// <param name="id">String representing a Item. Starts with "item_" if passing an id.</param>
        /// <returns>Item instance.</returns>
        Task<Item> GetItem(
            string id);

        /// <summary>
        /// Create an Item.
        /// </summary>
        /// <param name="item">Item to create</param>
        /// <returns>EasyPost.Item instance.</returns>
        Task<Item> CreateItem(
            Item item);

        /// <summary>
        /// Retrieve a Item from a custom reference.
        /// </summary>
        /// <param name="name">String containing the name of the custom reference to search for.</param>
        /// <param name="value">String containing the value of the custom reference to search for.</param>
        /// <returns>Item instance.</returns>
        Task<Item> GetItemByReference(
            string name,
            string value);

        #endregion

        #region Order

        /// <summary>
        /// Retrieve a Order from its id or reference.
        /// </summary>
        /// <param name="id">String representing a Order. Starts with "order_" if passing an id.</param>
        /// <returns>Order instance.</returns>
        Task<Order> GetOrder(
            string id);

        /// <summary>
        /// Create a Order.
        /// </summary>
        /// <param name="order">Order details</param>
        /// <returns>Order instance.</returns>
        Task<Order> CreateOrder(
            Order order);

        /// <summary>
        /// Purchase the shipments within this order with a carrier and service.
        /// </summary>
        /// <param name="id">Order id to buy</param>
        /// <param name="carrier">The carrier to purchase a shipment from.</param>
        /// <param name="service">The service to purchase.</param>
        /// <returns>Order instance.</returns>
        Task<Order> BuyOrder(
            string id,
            string carrier,
            string service);

        /// <summary>
        /// Purchase a label for this shipment with the given rate.
        /// </summary>
        /// <param name="id">Order id to buy</param>
        /// <param name="rate">Rate object to purchase the shipment with.</param>
        /// <returns>Order instance.</returns>
        Task<Order> BuyOrder(
            string id,
            CarrierRate rate);

        #endregion

        #region Parcel

        /// <summary>
        /// Retrieve a Parcel from its id.
        /// </summary>
        /// <param name="id">String representing a Parcel. Starts with "prcl_".</param>
        /// <returns>Parcel instance.</returns>
        Task<Parcel> GetParcel(
            string id);

        /// <summary>
        /// Create a Parcel.
        /// </summary>
        /// <param name="parcel">Parcel to create</param>
        /// <returns>Parcel instance.</returns>
        Task<Parcel> CreateParcel(
            Parcel parcel);

        #endregion

        #region Pickup

        /// <summary>
        /// Retrieve a Pickup from its id.
        /// </summary>
        /// <param name="id">String representing a Pickup. Starts with "pickup_".</param>
        /// <returns>Pickup instance.</returns>
        Task<Pickup> GetPickup(
            string id);

        /// <summary>
        /// Create a Pickup.
        /// </summary>
        /// <param name="pickup">Pickup to create</param>
        /// <returns>Pickup instance.</returns>
        Task<Pickup> CreatePickup(
            Pickup pickup = null);

        /// <summary>
        /// Purchase this pickup.
        /// </summary>
        /// <param name="id">Pickup id to purchase</param>
        /// <param name="carrier">The name of the carrier to purchase with.</param>
        /// <param name="service">The name of the service to purchase.</param>
        /// <returns>Pickup instance.</returns>
        Task<Pickup> BuyPickup(
            string id,
            string carrier,
            string service);

        /// <summary>
        /// Cancel this pickup.
        /// </summary>
        /// <param name="id">Pickup id to cancel</param>
        /// <returns>Pickup instance.</returns>
        Task<Pickup> CancelPickp(
            string id);

        #endregion

        #region Report

        /// <summary>
        /// Retrieve a Report from its id.
        /// </summary>
        /// <param name="id">String representing a report.</param>
        /// <returns>Report instance.</returns>
        Task<Report> GetReport(
            string id);

        /// <summary>
        /// Retrieve a Report from its id and type.
        /// </summary>
        /// <param name="type">Type of report, e.g. shipment, tracker, payment_log, etc.</param>
        /// <param name="id">String representing a report.</param>
        /// <returns>Report instance.</returns>
        Task<Report> GetReport(
            string type,
            string id);

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
        Task<Report> CreateReport(
            string type,
            Report report = null);

        /// <summary>
        /// Get a paginated list of reports.
        /// </summary>
        /// <param name="type">Type of the report</param>
        /// <param name="options">Options for the pagination function</param>
        /// <returns>Instance of EasyPost.ScanForm</returns>
        Task<ReportList> ListReports(
            string type,
            ReportListOptions options = null);

        #endregion

        #region ScanForm

        /// <summary>
        /// Get a paginated list of scan forms.
        /// </summary>
        /// <param name="options">Options for the pagination function</param>
        /// <returns>Instance of ScanFormList</returns>
        Task<ScanFormList> ListScanForms(
            ScanFormListOptions options = null);

        /// <summary>
        /// Gets a scan form from it's ID
        /// </summary>
        /// <param name="id">ID of the scan form</param>
        /// <returns>Instance of ScanForm</returns>
        Task<ScanForm> GetScanForm(
            string id);

        #endregion

        #region Shipment

        /// <summary>
        /// Get a paginated list of shipments.
        /// </summary>
        /// <param name="options">Options for the pagination function</param>
        /// <returns>Instance of EasyPost.ShipmentList</returns>
        Task<ShipmentList> ListShipments(
            ShipmentListOptions options = null);

        /// <summary>
        /// Retrieve a Shipment from its id.
        /// </summary>
        /// <param name="id">String representing a Shipment. Starts with "shp_".</param>
        /// <returns>Shipment instance.</returns>
        Task<Shipment> GetShipment(
            string id);

        /// <summary>
        /// Create a Shipment.
        /// </summary>
        /// <param name="shipment">Shipment details</param>
        /// <returns>Shipment instance.</returns>
        Task<Shipment> CreateShipment(
            Shipment shipment);

        /// <summary>
        /// Re-populate the rates property for this shipment
        /// </summary>
        /// <param name="shipment">The shipment to regenerate rates for</param>
        Task RegenerateRates(
            Shipment shipment);

        /// <summary>
        /// Buy a label for this shipment with the given rate.
        /// </summary>
        /// <param name="id">The id of the shipment to buy the label for</param>
        /// <param name="rateId">The id of the rate to purchase the shipment with.</param>
        /// <returns>Shipment instance.</returns>
        Task<Shipment> BuyShipment(
            string id,
            string rateId);

        /// <summary>
        /// Buy insurance for a shipment for the given amount.
        /// </summary>
        /// <param name="id">The id of the shipment to buy insurance for</param>
        /// <param name="amount">The amount to insure the shipment for. Currency is provided when creating a shipment.</param>
        /// <returns>Shipment instance.</returns>
        Task<Shipment> BuyInsuranceForShipment(
            string id,
            double amount);

        /// <summary>
        /// Generate a postage label for this shipment.
        /// </summary>
        /// <param name="id">The id of the shipment to generate the label for</param>
        /// <param name="fileFormat">Format to generate the label in. Valid formats: "pdf", "zpl" and "epl2".</param>
        /// <returns>Shipment instance.</returns>
        Task<Shipment> GenerateLabel(
            string id,
            string fileFormat);

        /// <summary>
        /// Send a refund request to the carrier the shipment was purchased from.
        /// </summary>
        /// <param name="id">The id of the shipment to refund</param>
        /// <returns>Shipment instance.</returns>
        Task<Shipment> RefundShipment(
            string id);

        #endregion

        #region Tracker

        /// <summary>
        /// Creates a tracker for a carrier and tracking code
        /// </summary>
        /// <param name="carrier">Carrier</param>
        /// <param name="trackingCode">Tracking code</param>
        /// <returns>Tracker instance.</returns>
        Task<Tracker> CreateTracker(
            string carrier,
            string trackingCode);

        /// <summary>
        /// Retrieve a Tracker from its id.
        /// </summary>
        /// <param name="id">String representing a Tracker. Starts with "trk_".</param>
        /// <returns>Tracker instance.</returns>
        Task<Tracker> GetTracker(
            string id);

        /// <summary>
        /// Get a paginated list of trackers.
        /// </summary>
        /// <param name="options">Options for the pagination function</param>
        /// <returns>Instance of EasyPost.ShipmentList</returns>
        Task<TrackerList> ListTrackers(
            TrackerListOptions options = null);

        #endregion

        #region User

        /// <summary>
        /// Retrieve a User from its id. If no id is specified, it returns the user for the api_key specified.
        /// </summary>
        /// <param name="id">String representing a user. Starts with "user_".</param>
        /// <returns>User instance.</returns>
        Task<User> GetUser(
            string id = null);

        /// <summary>
        /// Create a child user for the account associated with the api_key specified.
        /// </summary>
        /// <param name="userName">Name of the user</param>
        /// <returns>EasyPost.User instance.</returns>
        Task<User> CreateUser(
            string userName);

        /// <summary>
        /// Update the User associated with the api_key specified.
        /// </summary>
        /// <param name="user">User parameters to update</param>
        Task<User> UpdateUser(
            User user);

        /// <summary>
        /// Destroys a user
        /// </summary>
        /// <param name="id">ID of the user</param>
        Task DestroyUser(
            string id);

        #endregion
    }
}