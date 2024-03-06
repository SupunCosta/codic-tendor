using System.Collections.Generic;

namespace UPrinceV4.Web.Data.PO;

public class PODropdownData
{
    public IEnumerable<POTypeDto> Types { get; set; }
    public IEnumerable<POStatusDto> States { get; set; }
    public IEnumerable<PORequestTypeDto> RequestTypes { get; set; }
}