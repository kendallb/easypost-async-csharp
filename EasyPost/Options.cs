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
    public class Options : Resource
    {
        /// <summary>
        /// Setting this option to true, will add an additional handling charge. An Additional Handling charge may be applied to the following:
        /// . Any article that is encased in an outside shipping container made of metal or wood.
        /// . Any item, such as a barrel, drum, pail or tire, that is not fully encased in a corrugated cardboard shipping container.
        /// . Any package with the longest side exceeding 60 inches or its second longest side exceeding 30 inches.
        /// . Any package with an actual weight greater than 70 pounds.
        /// </summary>
        public bool? AdditionalHandling { get; set; }

        /// <summary>
        /// Setting this option to "0", will allow the minimum amount of address information to pass the validation check. Only for USPS postage.
        /// </summary>
        public string AddressValidationLevel { get; set; }

        /// <summary>
        /// Set this option to true if your shipment contains alcohol.
        /// . UPS - only supported for US Domestic shipments
        /// . FedEx - only supported for US Domestic shipments
        /// . Canada Post - Requires adult signature 19 years or older.If you want adult signature 18 years or older, instead use delivery_confirmation: ADULT_SIGNATURE
        /// </summary>
        public bool? Alcohol { get; set; }

        /// <summary>
        /// *This field is deprecated. Use the 'Payment' option object instead.
        /// </summary>
        public string BillReceiverAccount { get; set; }

        /// <summary>
        /// *This field is deprecated. Use the 'Payment' option object instead.
        /// </summary>
        public string BillReceiverPostalCode { get; set; }

        /// <summary>
        /// *This field is deprecated. Use the 'Payment' option object instead.
        /// </summary>
        public string BillThirdPartyAccount { get; set; }

        /// <summary>
        /// *This field is deprecated. Use the 'Payment' option object instead.
        /// </summary>
        public string BillThirdPartyCountry { get; set; }

        /// <summary>
        /// *This field is deprecated. Use the 'Payment' option object instead.
        /// </summary>
        public string BillThirdPartyPostalCode { get; set; }

        /// <summary>
        /// Setting payment type to bill the correct account for purchasing postage.
        /// </summary>
        public Payment Payment { get; set; }

        /// <summary>
        /// Setting this option to true will indicate to the carrier to prefer delivery by drone, if the carrier supports drone delivery.
        /// </summary>
        public bool? ByDrone { get; set; }

        /// <summary>
        /// Setting this to true will add a charge to reduce carbon emissions.
        /// </summary>
        public bool? CarbonNeutral { get; set; }

        /// <summary>
        /// Adding an amount will have the carrier collect the specified amount from the recipient.
        /// </summary>
        public string CodAmount { get; set; }

        /// <summary>
        /// Method for payment. "CASH", "CHECK", "MONEY_ORDER"
        /// </summary>
        public string CodMethod { get; set; }

        /// <summary>
        /// COD address Id
        /// </summary>
        public string CodAddressId { get; set; }

        /// <summary>
        /// Which currency this shipment will show for rates if carrier allows.
        /// </summary>
        public string Currency { get; set; }

        /// <summary>
        /// Incoterm negotiated for shipment. Supported values are
        /// "EXW", "FCA", "CPT", "CIP", "DAT", "DAP", "DDP", "FAS", "FOB", "CFR", and "CIF".
        /// Setting this value to anything other than "DDP" will pass the cost and responsibility of
        /// duties on to the recipient of the package(s), as specified by Incoterms rules.
        /// </summary>
        public string Incoterm { get; set; }

        /// <summary>
        /// If you want to request a signature, you can pass "ADULT_SIGNATURE" or "SIGNATURE". You may also request "NO_SIGNATURE" to leave the package at the door.
        /// . All - some options may be limited for international shipments
        /// </summary>
        public string DeliveryConfirmation { get; set; }

        /// <summary>
        /// Package contents contain dry ice.
        /// . UPS - Need dry_ice_weight to be set
        /// . UPS MailInnovations - Need dry_ice_weight to be set
        /// . FedEx - Need dry_ice_weight to be set
        /// </summary>
        public bool? DryIce { get; set; }

        /// <summary>
        /// If the dry ice is for medical use, set this option to true.
        /// . UPS - Need dry_ice_weight to be set
        /// . UPS MailInnovations - Need dry_ice_weight to be set
        /// </summary>
        public string DryIceMedical { get; set; }

        /// <summary>
        /// Weight of the dry ice in ounces.
        /// . UPS - Need dry_ice to be set
        /// . UPS MailInnovations - Need dry_ice to be set
        /// . FedEx - Need dry_ice to be set
        /// </summary>
        public string DryIceWeight { get; set; }

        /// <summary>
        /// Possible values "ADDRESS_SERVICE_REQUESTED", "FORWARDING_SERVICE_REQUESTED", "CHANGE_SERVICE_REQUESTED", "RETURN_SERVICE_REQUESTED", "LEAVE_IF_NO_RESPONSE"
        /// </summary>
        public string Endorsement { get; set; }

        /// <summary>
        /// Additional cost to be added to the invoice of this shipment. Only applies to UPS currently.
        /// </summary>
        public double FreightCharge { get; set; }

        /// <summary>
        /// This is to designate special instructions for the carrier like "Do not drop!".
        /// </summary>
        public string HandlingInstructions { get; set; }

        /// <summary>
        /// Dangerous goods indicator. Possible values are "ORMD" and "LIMITED_QUANTITY". Applies to USPS, FedEx and DHL eCommerce.
        /// </summary>
        public string Hazmat { get; set; }

        /// <summary>
        /// Package will wait at carrier facility for pickup.
        /// </summary>
        public bool? HoldForPickup { get; set; }

        /// <summary>
        /// This will print an invoice number on the postage label.
        /// </summary>
        public string InvoiceNumber { get; set; }

        /// <summary>
        /// Set the date that will appear on the postage label. Accepts ISO 8601 formatted string including time zone offset.
        /// </summary>
        public DateTime? LabelDate { get; set; }

        /// <summary>
        /// Supported label formats include "PNG", "PDF", "ZPL", and "EPL2". "PNG" is the only format that allows for conversion.
        /// </summary>
        public string LabelFormat { get; set; }

        /// <summary>
        /// Whether or not the parcel can be processed by the carriers equipment.
        /// </summary>
        public bool? Machinable { get; set; }

        // ReSharper disable InconsistentNaming
        /// <summary>
        /// You can optionally print custom messages on labels. The locations of these fields show up on different spots on the carrier's labels.
        /// </summary>
        public string PrintCustom_1 { get; set; }

        /// <summary>
        /// An additional message on the label. Same restrictions as print_custom_1
        /// </summary>
        public string PrintCustom_2 { get; set; }

        /// <summary>
        /// An additional message on the label. Same restrictions as print_custom_1
        /// </summary>
        public string PrintCustom_3 { get; set; }

        /// <summary>
        /// Create a barcode for this custom reference if supported by carrier.
        /// </summary>
        public bool? PrintCustom_1Barcode { get; set; }

        /// <summary>
        /// Create a barcode for this custom reference if supported by carrier.
        /// </summary>
        public bool? PrintCustom_2Barcode { get; set; }

        /// <summary>
        /// Create a barcode for this custom reference if supported by carrier.
        /// </summary>
        public bool? PrintCustom_3Barcode { get; set; }

        /// <summary>
        /// Specify the type of print_custom_1.
        /// FedEx
        /// . (null) - If print_custom_1_code is not provided it defaults to Customer Reference
        /// . PO - Purchase Order Number
        /// . DP - Department Number
        /// . RMA - Return Merchandise Authorization
        ///
        /// UPS
        /// . AJ - Accounts Receivable Customer Account
        /// . AT - Appropriation Number
        /// . BM - Bill of Lading Number
        /// . 9V - Collect on Delivery(COD) Number
        /// . ON - Dealer Order Number
        /// . DP - Department Number
        /// . 3Q - Food and Drug Administration(FDA) Product Code
        /// . IK - Invoice Number
        /// . MK - Manifest Key Number
        /// . MJ - Model Number
        /// . PM - Part Number
        /// . PC - Production Code
        /// . PO - Purchase Order Number
        /// . RQ - Purchase Request Number
        /// . RZ - Return Authorization Number
        /// . SA - Salesperson Number
        /// . SE - Serial Number
        /// . ST - Store Number
        /// . TN - Transaction Reference Number
        /// . EI - Employer’s ID Number
        /// . TJ - Federal Taxpayer ID No.
        /// . SY - Social Security Number
        /// </summary>
        public string PrintCustom_1Code { get; set; }

        /// <summary>
        /// See print_custom_1_code.
        /// </summary>
        public string PrintCustom_2Code { get; set; }

        /// <summary>
        /// See print_custom_1_code.
        /// </summary>
        public string PrintCustom_3Code { get; set; }
        // ReSharper restore InconsistentNaming

        /// <summary>
        /// Set this value to true for delivery on Saturday. When setting the saturday_delivery option, you will only get
        /// rates for services that are eligible for saturday delivery. If no services are available for saturday delivery,
        /// then you will not be returned any rates. You may need to create 2 shipments, one with the saturday_delivery
        /// option set on one without to get all your eligible rates.
        /// </summary>
        public bool? SaturdayDelivery { get; set; }

        /// <summary>
        /// This option allows you to request restrictive rates from USPS. Can set to 'USPS.MEDIAMAIL' or 'USPS.LIBRARYMAIL'.
        /// </summary>
        public string SpecialRatesEligibility { get; set; }

        /// <summary>
        /// You can use this to override the hub ID you have on your account.
        /// </summary>
        public string SmartpostHub { get; set; }

        /// <summary>
        /// The manifest ID is used to group SmartPost packages onto a manifest for each trailer.
        /// </summary>
        public string SmartpostManifest { get; set; }

        /// <summary>
        /// Carrier insurance amount
        /// </summary>
        public double? CarrierInsuranceAmount { get; set; }

        /// <summary>
        /// Carrier notification email
        /// </summary>
        public string CarrierNotificationEmail { get; set; }

        /// <summary>
        /// Carrier notification SMS
        /// </summary>
        public string CarrierNotificationSms { get; set; }

        /// <summary>
        /// Carrier branded labels
        /// </summary>
        public bool? CarrierBranded { get; set; }

        /// <summary>
        /// Commercial invoice format
        /// </summary>
        public string CommercialInvoiceFormat { get; set; }

        /// <summary>
        /// Commercial invoice size
        /// </summary>
        public string CommercialInvoiceSize { get; set; }

        /// <summary>
        /// Cost center
        /// </summary>
        public string CostCenter { get; set; }

        /// <summary>
        /// Customs brokeder address ID
        /// </summary>
        public string CustomsBrokerAddressId { get; set; }

        /// <summary>
        /// Declared value
        /// </summary>
        public double? DeclaredValue { get; set; }

        /// <summary>
        /// Delivered duty paid
        /// </summary>
        public bool? DeliveredDutyPaid { get; set; }

        /// <summary>
        /// Delivert time preference
        /// </summary>
        public string DeliveryTimePreference { get; set; }

        /// <summary>
        /// Duty payment amount
        /// </summary>
        public string DutyPaymentAccount { get; set; }

        /// <summary>
        /// Group
        /// </summary>
        public string Group { get; set; }

        /// <summary>
        /// Image format
        /// </summary>
        public string ImageFormat { get; set; }

        /// <summary>
        /// Label size
        /// </summary>
        public string LabelSize { get; set; }

        /// <summary>
        /// Neutral delivery
        /// </summary>
        public bool? NeutralDelivery { get; set; }

        /// <summary>
        /// Non contract
        /// </summary>
        public bool? NonContract { get; set; }

        /// <summary>
        /// PO sort
        /// </summary>
        public string PoSort { get; set; }

        /// <summary>
        /// Print rate
        /// </summary>
        public bool? PrintRate { get; set; }

        /// <summary>
        /// Return service
        /// </summary>
        public string ReturnService { get; set; }

        /// <summary>
        /// Settlement method
        /// </summary>
        public string SettlementMethod { get; set; }
    }
}