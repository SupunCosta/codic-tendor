using System.Collections.Generic;

namespace UPrinceV4.Web.Data.PBS_;

public class PbsDropdownData
{
    public IEnumerable<PbsToleranceStateDto> ToleranceStates { get; set; }
    public IEnumerable<PbsProductStatusDto> ProductStates { get; set; }
    public IEnumerable<PbsProductItemTypeDto> Itemtypes { get; set; }
    public IEnumerable<GetPbsExperienceDto> Experiences { get; set; }
    public IEnumerable<GetPbsSkillDto> Skills { get; set; }
}