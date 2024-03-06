using System;
using System.Collections.Generic;
using Microsoft.Graph.Models;

namespace UPrinceV4.Web.Data.PdfToExcel;

public class ContractorPdfData
{
    public string Id { get; set; }
    public string ArticleNo { get; set; }
    public string CompanyId { get; set; }
    public string Title { get; set; }
    public string Unit { get; set; }
    public string VH { get; set; }
    public string Quantity { get; set; }
    public float? UnitPrice { get; set; }
    public float? TotalPrice { get; set; }
    public string LotId { get; set; }
    public DateTime? CreatedDate { get; set; }
    public string PageRowColumn { get; set; }
    public string PageRow { get; set; }
    public string LotRowNumber { get; set; }
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
    public bool IsExclude { get; set; }
    public string MeasurementCode { get; set; }
    public string RealArticleNo { get; set; }
}

public class ContractorPdfDataResults
{
    public DriveItem uploadedData { get; set; }
    public List<tableData> ContractorPdfData { get; set; }
}

public class tableData
{
    public string ArticleNo { get; set; }
    public string Title { get; set; }
    public string Unit { get; set; }
    public string VH { get; set; }
    public string Quantity { get; set; }
    public string UnitPrice { get; set; }
    public string TotalPrice { get; set; }
    public string CompanyId { get; set; }
    public string LotId { get; set; }
    public DateTime? CreatedDate { get; set; }
    public string RowIndex { get; set; }
    public int PageNumber { get; set; }
    public string PageRowColumn { get; set; }
    public string PageRow { get; set; }
    public string LotRowNumber { get; set; }
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
    public bool IsExclude { get; set; }
    public string MeasurementCode { get; set; }
    public string RealArticleNo { get; set; }

}

public class GetContractorPdfData
{
    public string Id { get; set; }
    public string ArticleNo { get; set; }
    public string CompanyId { get; set; }
    public string Title { get; set; }
    public string Unit { get; set; }
    public string VH { get; set; }
    public string Quantity { get; set; }
    public float? UnitPrice { get; set; }
    public float? TotalPrice { get; set; }
    public string LotId { get; set; }
    public DateTime? CreatedDate { get; set; }
    public int PageRowColumn { get; set; }
    public string PageRow { get; set; }
    public string LotRowNumber { get; set; }
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
    public bool IsExclude { get; set; }
    public string MeasurementCode { get; set; }
    public string RealArticleNo { get; set; }

}

public class GetContractorPdfRowData
{
    public string LotId { get; set; }
    public string ContractorId { get; set; }
}

public class ContractorPdfErrorLog
{
    public string ArticleNo { get; set; }
    public string CompanyId { get; set; }
    public string Title { get; set; }
    public string ColumnName { get; set; }
    public DateTime? CreatedDate { get; set; }
    public string Error { get; set; }
    public string LotId { get; set; }
    public string LotRowNumber { get; set; }
    public float? TotalPrice { get; set; }
    public float TotalPricerounded { get; set; }
    public float calculatedPricerounded { get; set; }
    public string ParentId { get; set; }
    public string Quantity { get; set; }
    public float? UnitPrice { get; set; }
    public string PsSequenceId { get; set; }
    public string MeasurementCode { get; set; }
    public string Unit { get; set; }
    public string Mou { get; set; }
    public bool IsExclude { get; set; }
    public string RealArticleNo { get; set; }

    
}

public class ContractorLotExcelData
{
    public List<tableData> ExcelData { get; set; }
    public string ContractId { get; set; }
    public string ContractorId { get; set; }
}

public class SaveUploadedPdfData
{
    public List<GetContractorPdfData> ExcelData { get; set; }
    public string ContractId { get; set; }
    public string ContractorId { get; set; }
}

public class GetLotTotalPrices
{
    public string Customer { get; set; }
    public float? TotalCost { get; set; }
    public string ExtraInfo { get; set; }
}

public class GetTotalPriceErrorsDto
{
    public string CompanyId { get; set; }
    public string CompanyName { get; set; }
    public List<TotalPriceErrors> Errors { get; set; }
}

public class TotalPriceErrors
{
    public string Id { get; set; }
    public string ArticleNo { get; set; }
    public string Title { get; set; }
    public string Unit { get; set; }
    public string Quantity { get; set; }
    public float? UnitPrice { get; set; }
    public float? TotalPrice { get; set; }
    public string LotId { get; set; }
    public float? CorrectTotalPrice { get; set; }

}