using System.Collections.Generic;
using UPrinceV4.Web.Data.PMOL;

namespace UPrinceV4.Web.Data.PS;

public class PsDropdownData
{
    public IEnumerable<PmolDropdownDto> Status { get; set; }
    public IEnumerable<PmolDropdownDto> Type { get; set; }
    public string ProjectCompletionDate { get; set; }
    public PsCustomerReadDto Customer { get; set; }
}

public class projectForPsDto
{
    public string Id { get; set; }
    public string EndDate { get; set; }
    public string ContactPersonName { get; set; }
    public string ContactPersonEmail { get; set; }
    public string Customer { get; set; }
}