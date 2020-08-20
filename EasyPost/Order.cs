/*
 * Licensed under The MIT License (MIT)
 *
 * Copyright (c) 2014 EasyPost
 * Copyright (C) 2017 AMain.com, Inc.
 * All Rights Reserved
 */

using System.Collections.Generic;
using System.Threading.Tasks;
using RestSharp;

namespace EasyPost
{
    public class Order : EasyPostObject
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
        /// The shipper's address, defaults to from_address
        /// </summary>
        public Address ReturnAddress { get; set; }

        /// <summary>
        /// The buyer's address, defaults to to_address
        /// </summary>
        public Address BuyerAddress { get; set; }

        /// <summary>
        /// All associated Shipment objects
        /// </summary>
        public List<Shipment> Shipments { get; set; }

        /// <summary>
        /// All associated Rate objects
        /// </summary>
        public List<CarrierRate> Rates { get; set; }

        /// <summary>
        /// Any carrier errors encountered during rating
        /// </summary>
        public List<EasyPostMessage> Messages { get; set; }

        /// <summary>
        /// Set true to create as a return
        /// </summary>
        public bool? IsReturn { get; set; }

        /// <summary>
        /// Customs information
        /// </summary>
        public CustomsInfo CustomsInfo { get; set; }

        /// <summary>
        /// Carrier accounts
        /// </summary>
        public List<CarrierAccount> CarrierAccounts { get; set; }

        /// <summary>
        /// Containers
        /// </summary>
        public List<Container> Containers { get; set; }

        /// <summary>
        /// Items in the order
        /// </summary>
        public List<Item> Items { get; set; }
    }

    /// <summary>
    /// Order API implementation
    /// </summary>
    public partial class EasyPostClient
    {
        /// <summary>
        /// Retrieve a Order from its id or reference.
        /// </summary>
        /// <param name="id">String representing a Order. Starts with "order_" if passing an id.</param>
        /// <returns>Order instance.</returns>
        public async Task<Order> GetOrder(
            string id)
        {
            var request = new EasyPostRequest("orders/{id}");
            request.AddUrlSegment("id", id);

            return await Execute<Order>(request);
        }

        /// <summary>
        /// Create a Order.
        /// </summary>
        /// <param name="order">Order details</param>
        /// <returns>Order instance.</returns>
        public async Task<Order> CreateOrder(
            Order order)
        {
            var request = new EasyPostRequest("orders", Method.POST);
            request.AddBody(order.AsDictionary(), "order");

            return await Execute<Order>(request);
        }

        /// <summary>
        /// Purchase the shipments within this order with a carrier and service.
        /// </summary>
        /// <param name="id">Order id to buy</param>
        /// <param name="carrier">The carrier to purchase a shipment from.</param>
        /// <param name="service">The service to purchase.</param>
        /// <returns>Order instance.</returns>
        public async Task<Order> BuyOrder(
            string id,
            string carrier,
            string service)
        {
            var request = new EasyPostRequest("orders/{id}/buy", Method.POST);
            request.AddUrlSegment("id", id);
            request.AddBody(new List<KeyValuePair<string, string>>() {
                new KeyValuePair<string, string>("carrier", carrier),
                new KeyValuePair<string, string>("service", service)
            });

            return await Execute<Order>(request);
        }

        /// <summary>
        /// Purchase a label for this shipment with the given rate.
        /// </summary>
        /// <param name="id">Order id to buy</param>
        /// <param name="rate">Rate object to purchase the shipment with.</param>
        /// <returns>Order instance.</returns>
        public async Task<Order> BuyOrder(
            string id,
            CarrierRate rate)
        {
            return await BuyOrder(id, rate.Carrier, rate.Service);
        }
    }
}