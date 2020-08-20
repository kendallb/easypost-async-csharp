# EasyPost Async .Net Client Library

EasyPost Async is a simple shipping API using the .net 4.5 Task Async API. You can sign up for an account at https://easypost.com

## Documentation

Up-to-date documentation at: https://www.easypost.com/docs/api/csharp

## Installation

The easiest way to add EasyPost Async to your project is with the NuGet package manager.

    PM> Install-Package EasyPost-Async

See NuGet docs for instructions on installing via the [dialog](http://docs.nuget.org/docs/start-here/managing-nuget-packages-using-the-dialog) or the [console](http://docs.nuget.org/docs/start-here/using-the-package-manager-console).

## Usage

The EasyPost Async API consists of many object types. There are several attributes that are consistent across all objects:

* `Id` -- Guaranteed unique identifier of the object.
* `CreatedAt`/`UpdatedAt`  -- Timestamps of creation and last update time.

### Configuration

The EasyPost Async API is initialized using your API key via the EasyPostClient class. The EasyPostClient class exposes all the functions of the API via a single IEasyPostClient interface that is fully mockable for unit testing.

```cs
using EasyPost;

var client = new EasyPostClient("ApiKey");
```

### Error Handling

Error handling for task based Async operations is different than normal operations due to differences in how exception handling works. For this reason the EasyPost Async library does not throw exceptions on error but rather relies on the caller examining the RequestError field of the response. This is filled in with details about the error:

```cs
using EasyPost;

var address = _client.GetAddress("not-an-id").Result;
if (address.RequestError != null) {
    var requestError = address.RequestError;
    var statusCode = requestError.StatusCode;
    var errorCode = requestError.Code;
    var errorMessage = requestError.Message;
    var errorList = requestError.Errors;
}
```

### [Address Verification](https://www.easypost.com/docs/api/csharp#create-and-verify-addresses)

An `Address` can be verified using one or many verifications [methods](https://www.easypost.com/docs/api/csharp#verifications-object). If `Address` is created without strict verifications the object will still be created, otherwise an `HttpException` will be raised.

```cs
using EasyPost;

var address = new Address {
    Company = "Simpler Postage Inc",
    Street1 = "164 Townsend Street",
    Street2 = "Unit 1",
    City = "San Francisco",
    State = "CA",
    Country = "US",
    Zip = "94107",
};

var address = client.CreateAddress(address, VerificationFlags.Delivery);

if (address.Verifications.Delivery.Success) {
    // successful verification
} else {
    // unsuccessful verification
}
```

```cs
using EasyPost;

Address address = new Address {
    C1ompany = "Simpler Postage Inc",
    Street1 = "164 Townsend Street",
    Street2 = "Unit 1",
    City = "San Francisco",
    State = "CA",
    Country = "US",
    Zip = "94107",
};

var address = client.CreateAddress(address, VerificationFlags.DeliveryStrict);
if (address.RequestError != null) {
    // unsuccessful verification
}

// successful verification
```

### [Rating](https://www.easypost.com/docs/api/csharp#rates)

Rating is available through the `Shipment` object. Since we do not charge for rating there are rate limits for this action if you do not eventually purchase the `Shipment`. Please contact us at support@easypost.com if you have any questions.

```cs
var fromAddress = new Address { Zip = "14534" };
var toAddress = new Address { Zip = "94107" };

var parcel = new Parcel {
    Length = 8,
    Width = 6,
    Height = 5,
    Weight = 10
};

var shipment = new Shipment {
    FromAddress = fromAddress,
    ToAddress = toAddress,
    Parcel = parcel
};

shipment = client.CreateShipment(shipment);

foreach (var rate in shipment.Rates) {
    // process rates
}
```

### [Postage Label](https://www.easypost.com/docs/api/csharp#buy-a-shipment) Generation

Purchasing a shipment will generate a `PostageLabel` and any customs `Form`s that are needed for shipping.

```cs
var fromAddress = new Address { Id = "adr_..." };
var toAddress = new Address {
    Company = "EasyPost",
    Street1 = "164 Townsend Street",
    Street2 = "Unit 1",
    City = "San Francisco",
    State = "CA",
    Country = "US",
    Zip = "94107"
};

var parcel = new Parcel {
    Length = 8,
    Width = 6,
    Height = 5,
    Weight = 10
};

var item = new CustomsItem { Description = "description" };
var info = new CustomsInfo {
    CustomsCertify = "TRUE",
    EelPfc = "NOEEI 30.37(a)",
    CustomsItems = new List<CustomsItem> { item }
};

var options = new Options { Label_format = "PDF" };

var shipment = new Shipment {
    FromAddress = fromAddress,
    ToAddress = toAddress,
    Parcel = parcel,
    CustomsInfo = info,
    Options = options
};

shipment = client.BuyShipment(shipment.Id, shipment.LowestRate(
    includeServices: new[] { Service.Priority },
    excludeCarriers: new[] { Carrier.USPS }
));

shipment.PostageLabel.Url; // https://easypost-files.s3-us-west-2.amazonaws.com/files/postage_label/20160826/8e77c397d47b4d088f1c684b7acd802a.png

foreach (var form in shipment.Forms) {
    // process forms
}
```

### Asynchronous Batch Processing

The `Batch` object allows you to perform operations on multiple `Shipment`s at once. This includes scheduling a `Pickup`, creating a `ScanForm` and consolidating labels. Operations performed on a `Batch` are asynchronous and take advantage of our [webhoook](https://www.easypost.com/docs/api/csharp#events) infrastructure.

```cs
using EasyPost;

var shipment = new Shipment {
    FromAddress = fromAddress,
    ToAddress = toAddress,
    Parcel = parcel,
    Options = options
};

var batch = client.CreateBatch(new[] { _testBatchShipment }, "MyReference");
```

This will produce two webhooks. One `batch.Created` and one `batch.Updated`. Process each `Batch` [state](https://www.easypost.com/docs/api/csharp#batch-object) according to your business logic.

```cs
using EasyPost;

var batch = client.GetBatch(batch.Id);

batch = _client.GenerateLabelForBatch(batch.Id, "zpl"); // populates batch.label_url asynchronously
```

Consume the subsequent `batch.Updated` webhook to process further.

### Reporting Issues

If you have an issue with the client feel free to open an issue on [GitHub](https://github.com/EasyPost/easypost-csharp/issues). If you have a general shipping question or a questions about EasyPost's service please contact support@easypost.com for additional assitance.
