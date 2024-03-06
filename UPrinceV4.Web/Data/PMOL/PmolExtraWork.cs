using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace UPrinceV4.Web.Data.PMOL;

public class PmolExtraWork
{
    public string Id { get; set; }
    public string Description { get; set; }
    [ForeignKey("Pmol")] public string PmolId { get; set; }
    public virtual Pmol Pmol { get; set; }
}

public class PmolExtraWorkCreateDto
{
    public string Id { get; set; }
    public string Description { get; set; }
    public string PmolId { get; set; }
    public List<PmolExtraWorkFilesCreateDto> ExtraWorkFiles { get; set; }
}

public class PmolExtraWorkReadDto
{
    public string Id { get; set; }
    public string Description { get; set; }
    public string PmolId { get; set; }
    public List<PmolExtraWorkFilesReadDto> ImageList { get; set; }
    public List<PmolExtraWorkFilesReadDto> AudioList { get; set; }
}