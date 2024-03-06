using System.Collections.Generic;

namespace UPrinceV4.Web.Data.Contractor;

public class CBCExcelLotData
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
    public string Key4 { get; set; }
    public string Value4 { get; set; }
    public string Key5 { get; set; }
    public string Value5 { get; set; }
    public bool IsExclude { get; set; }
    public string RealArticleNo { get; set; }

}

public class CBCExcelLotDataDto
{
    public string ContractId { get; set; }
    public List<ExcelLotDataDto> ExcelData { get; set; }
}

public class ExcelLotDataDto
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
    public IEnumerable<Contractors> Contractors { get; set; }
    public string MeasurementCode { get; set; }
    public string Mou { get; set; }
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
    public string RealArticleNo { get; set; }

}

public class Contractors
{
    public string ArticleNo { get; set; }
    public string Title { get; set; }
    public string CompanyId { get; set; }
    public string Quantity { get; set; }
    public string UnitPrice { get; set; }
    public string TotalPrice { get; set; }
}

public class ContractorsCompanyList
{
    public string CompanyId { get; set; }
    public string CompanyName { get; set; }
    public bool Errors { get; set; }
    public float TotalBAFO { get; set; }
    public float TotalCost { get; set; }
    public bool isWinner { get; set; }
    public string FileType { get; set; }
}

public class SaveCBCExcelLotDataDto
{
    public string LotId { get; set; }
    public List<ExcelLotDataDtoTest> ExcelData { get; set; }
    public List<ContractorTotalValuesDto> ContractorData { get; set; }
}

public class ExcelLotDataDtoTest
{
    public string Id { get; set; }
    public string Title { get; set; }
    public string ParentId { get; set; }
    public string ArticleNo { get; set; }
    public string Quantity { get; set; }
    public string ContractId { get; set; }
    public IEnumerable<ExcelLotDataDtoTest> Contractors { get; set; }
    public string CompanyId { get; set; }
    public string UnitPrice { get; set; }
    public string TotalPrice { get; set; }
    public string Unit { get; set; }

    public bool isError { get; set; }
    public string MeasurementCode { get; set; }
    public string Mou { get; set; }
    public string UId { get; set; }
    public float? SubTotal { get; set; }
    public bool IsParent { get; set; }
    public IEnumerable<ContractorPs> Ps { get; set; }
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
    public bool IsMostExpensive { get; set; }
    public bool IsExclude { get; set; }
    public string RealArticleNo { get; set; }

}

public class SubTotalDto
{
    public float SubTotal { get; set; }
    public int Count { get; set; }
}

public class AwardWinner
{
    public string CompanyId { get; set; }
    public string CompanyName { get; set; }
    public bool Errors { get; set; }
    public float TotalBAFO { get; set; }
    public float TotalCost { get; set; }
    public bool IsWinner { get; set; }
    public string LotId { get; set; }
}

public class CBCExcelLotDataParent
{
    public string Id { get; set; }
    public string Title { get; set; }
    public string ParentId { get; set; }
    public string ArticleNo { get; set; }
    public string Quantity { get; set; }
    public string ContractId { get; set; }
    public string Unit { get; set; }
    public float UnitPrice { get; set; }
    public float TotalPrice { get; set; }
    public string MeasurementCode { get; set; }
    public string Mou { get; set; }
    public List<CBCExcelLotDataParent> Children { get; set; }
    public string Key1 { get; set; }
    public string Value1 { get; set; }
    public string Key2 { get; set; }
    public string Value2 { get; set; }
    public string Key3 { get; set; }
    public string Value3 { get; set; }
}

public class FieldValues
{
    public string CompanyName { get; set; }
    public string ProjectTitle { get; set; }
}

public class IsUnresolvedCommentDto
{
    public string ArticleNo { get; set; }
    public string IsUnresolvedComment { get; set; } = "0";
}

public class IsUnresolvedCommentData
{
    public string ArticleNo { get; set; }
    public string ContractorId { get; set; }
    public string Accept { get; set; }
}