using System.Collections.Generic;

namespace UPrinceV4.Web.Data.Contractor;

public class ContractorDropDownData
{
    public IEnumerable<ContractorStatusDto> Status { get; set; }
    public IEnumerable<ContractorFileTypeDto> FileType { get; set; }
    public IEnumerable<ContractorProductItemTypeDto> ProductItemType { get; set; }
}