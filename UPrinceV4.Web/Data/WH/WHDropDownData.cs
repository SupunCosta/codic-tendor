using System.Collections.Generic;

namespace UPrinceV4.Web.Data.WH;

public class WHDropDownData
{
    public IEnumerable<WHTypeDto> Types { get; set; }
    public IEnumerable<WHStatusDto> Status { get; set; }
    public IEnumerable<WHTaxonomyLevel> WHTaxonomyLevel { get; set; }
}