using System.Collections.Generic;

namespace UPrinceV4.Web.Data.PMOL;

public class PmolDropdown
{
    public IEnumerable<PmolDropdownDto> Status { get; set; }
    public IEnumerable<PmolDropdownDto> Type { get; set; }
}

public class PmolDropdownDto
{
    public string Key { get; set; }
    public string Text { get; set; }
}