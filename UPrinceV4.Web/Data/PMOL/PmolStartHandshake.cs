using System.ComponentModel.DataAnnotations.Schema;

namespace UPrinceV4.Web.Data.PMOL;

public class PmolStartHandshake
{
    public string Id { get; set; }
    public string Address { get; set; }
    public double Latitude { get; set; }
    public double Longitude { get; set; }
    [ForeignKey("Pmol")] public string PmolId { get; set; }
    public virtual Pmol Pmol { get; set; }
}

public class PmolStartHandshakeCreateDto
{
    public string LocationId { get; set; }
    public string PmolId { get; set; }
}