using System;

namespace UPrinceV4.Web.Data.Contractor;

public class ContractorPsMinusPlusWork
{
    public string Id { get; set; }
    public string LotId { get; set; }
    public int PsOrderNumber { get; set; }
    public string Status { get; set; }
    public DateTime? Date { get; set; }
    public string TotalPrice { get; set; }
    public string Url { get; set; }
    public string Version { get; set; }
    public bool IsApproved { get; set; }
}

public class GetContractorPsMinusPlusWork
{
    public string Id { get; set; }
    public string LotId { get; set; }
    public int PsOrderNumber { get; set; }
    public string Status { get; set; }
    public DateTime? Date { get; set; }
    public string TotalPrice { get; set; }
    public string Url { get; set; }
    public string Version { get; set; }
    public bool IsApproved { get; set; }
}

public class ContractorPsMinusPlusWorkDto
{
    public string LotId { get; set; }
    public int PsOrderNumber { get; set; }
    public string Status { get; set; }
    public string TotalPrice { get; set; }
    public string Version { get; set; }
}