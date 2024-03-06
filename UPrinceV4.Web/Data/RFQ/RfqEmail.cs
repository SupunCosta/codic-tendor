using Newtonsoft.Json;

namespace UPrinceV4.Web.Data.RFQ;

public class RfqEmail
{
    [JsonProperty("link")] public string Link { get; set; }

    [JsonProperty("code")] public string Code { get; set; }

    [JsonProperty("rfqTitle")] public string RfqTitle { get; set; }

    [JsonProperty("date")] public string Date { get; set; }

    [JsonProperty("customer")] public string Customer { get; set; }

    [JsonProperty("logoUrl")] public string LogoUrl { get; set; }

    [JsonProperty("year")] public int Year { get; set; }

    [JsonProperty("companyName")] public string CompanyName { get; set; }

    [JsonProperty("emailContentHeader")] public string EmailContentHeader { get; set; }

    [JsonProperty("emailContent")] public string EmailContent { get; set; }

    [JsonProperty("subject")] public string Subject { get; set; }

    [JsonProperty("resourceLeadTime")] public string ResourceLeadTime { get; set; }

    [JsonProperty("unitPrice")] public string UnitPrice { get; set; }

    [JsonProperty("customerJobRole")] public string CustomerJobRole { get; set; }
}

public class RfqCab
{
    public string Id { get; set; }
    public string EmailAddress { get; set; }
    public string Oid { get; set; }
    public string FullName { get; set; }
    public string CompanyName { get; set; }
    public string JobRole { get; set; }
    public string CompanyId { get; set; }
    public string CustomerName { get; set; }
    public string CustomerEmail { get; set; }
    public string Vat { get; set; }
}

public class RfqAccept
{
    public string SequenceId { get; set; }
    public string Signature { get; set; }
    public string FullName { get; set; }
    public string Cu { get; set; }
}