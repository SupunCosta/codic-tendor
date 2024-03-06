using System;
using System.Collections.Generic;

namespace UPrinceV4.Web.Data.Contractor;

public class ContractorLotUploadedDocs
{
    public string Id { get; set; }
    public string LotId { get; set; }
    public string CompanyId { get; set; }
    public string Type { get; set; }
    public string Link { get; set; }
    public string Title { get; set; }
    public DateTime? CreatedDate { get; set; }
    public string FileType { get; set; }
}

public class GetContractorLotUploadedDocs
{
    public string Company { get; set; }
    public IEnumerable<ContractorLotUploadedDocs> ProgressDocuments { get; set; }
    public IEnumerable<ContractorLotUploadedDocs> TechnicalDocuments { get; set; }
}

public class Docs
{
    public IEnumerable<ContractorLotUploadedDocs> ProgressDocuments { get; set; }
    public IEnumerable<ContractorLotUploadedDocs> TechnicalDocuments { get; set; }
}