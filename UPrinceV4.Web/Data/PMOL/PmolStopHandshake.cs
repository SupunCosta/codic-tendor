using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace UPrinceV4.Web.Data.PMOL;

public class PmolStopHandshake
{
    public string Id { get; set; }
    public string SequenceCode { get; set; }
    public string Name { get; set; }
    public string Link { get; set; }
    [ForeignKey("Pmol")] public string PmolId { get; set; }
    public virtual Pmol Pmol { get; set; }
    public string CabPersonlId { get; set; }
}

public class PmolStopHandshakeCreateDto
{
    public string Name { get; set; }
    public string Link { get; set; }
    public string PmolId { get; set; }
    public string CabId { get; set; }
}

public class PmolStopHandshakeCreateDocumentsDto
{
    public string PmolId { get; set; }
    public List<string> DocLinks { get; set; }
}

public class PmolStopHandshakeReadObj
{
    public string Name { get; set; }
    public string Link { get; set; }
    public string PmolId { get; set; }
}

public class PmolStopHandshakeReadDto
{
    public List<PmolStopHandshakeReadObj> StopHandshakes { get; set; }
    public List<string> DocLinks { get; set; }
}