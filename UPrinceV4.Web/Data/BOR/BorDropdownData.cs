using System.Collections.Generic;
using UPrinceV4.Web.Data.PBS_;

namespace UPrinceV4.Web.Data.BOR;

public class BorDropdownData
{
    public IEnumerable<PbsProductStatusDto> BorStatus { get; set; }
    public IEnumerable<BorTypeDto> BorType { get; set; }
}