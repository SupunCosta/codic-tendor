using System;
using System.Collections.Generic;

namespace UPrinceV4.Web.Data.Contractor;

public class ContractorPs
{
    public string Id { get; set; }
    public string ArticleNumber { get; set; }
    public string Title { get; set; }
    public string MeasurementCode { get; set; }
    public string QuantityQuotation { get; set; }
    public string UnitPrice { get; set; }
    public string QuantityConsumed { get; set; }
    public string Total { get; set; }
    public string LotId { get; set; }
    public string CompanyId { get; set; }
    public string PsSequenceId { get; set; }
    public string PsOrderNumber { get; set; }
    public bool IsApproved { get; set; } = false;
    public DateTime? ApprovedDate { get; set; }
}

public class ContractorPsDto
{
    public string Id { get; set; }
    public string ArticleNumber { get; set; }
    public string Title { get; set; }
    public string MeasurementCode { get; set; }
    public string QuantityQuotation { get; set; }
    public string UnitPrice { get; set; }
    public string QuantityConsumed { get; set; }
    public string Total { get; set; }
    public string LotId { get; set; }
    public string CompanyId { get; set; }
    public string PsSequenceId { get; set; }
    public string PsOrderNumber { get; set; }
    public bool IsApproved { get; set; } = false;
    public DateTime? ApprovedDate { get; set; }
    public string Key1 { get; set; }
    public string Value1 { get; set; }
    public string Key2 { get; set; }
    public string Value2 { get; set; }
    public string Key3 { get; set; }
    public string Value3 { get; set; }
    public string Key4 { get; set; }
    public string Value4 { get; set; }
    public string Key5 { get; set; }
    public string Value5 { get; set; }
}

public class UploadContractorPs
{
    public string ContractId { get; set; }
    public string ContractorId { get; set; }
    public List<ContractorPsDto> ExcelData { get; set; }
    public string PsOrderNumber { get; set; }
}

public class ContractorsPsList
{
    public string CompanyId { get; set; }
    public string CompanyName { get; set; }
    public bool isWinner { get; set; }
    public string PsSequenceId { get; set; }
    public string PsOrderNumber { get; set; }
    public bool IsError { get; set; }

    public bool IsApproved { get; set; }
    public bool IsPublished { get; set; }
    public DateTime? ApprovedDate { get; set; }
    public float Total { get; set; }
    public int Order { get; set; }
}

public class ExcelLotData
{
    public string Id { get; set; }
    public string Title { get; set; }
    public string ParentId { get; set; }
    public string ArticleNo { get; set; }
    public string Quantity { get; set; }
    public string ContractId { get; set; }
    public string Unit { get; set; }
    public float? UnitPrice { get; set; }
    public float? TotalPrice { get; set; }
    public string MeasurementCode { get; set; }
    public string Mou { get; set; }
    public string Key1 { get; set; }
    public string Value1 { get; set; }
    public string Key2 { get; set; }
    public string Value2 { get; set; }
    public string Key3 { get; set; }
    public string Value3 { get; set; }
    public List<ContractorPs> Ps { get; set; }
    public bool IsExclude { get; set; }
    public string RealArticleNo { get; set; }
}

public class SaveContractorPs
{
    public string ContractId { get; set; }
    public string ContractorId { get; set; }
    public List<ExcelLotData> ExcelData { get; set; }
}