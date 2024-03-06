namespace UPrinceV4.Web.Data.CPC;

public class CpcFilter
{
    public string ResourceTypeId { get; set; }
    public string ResourceFamilyId { get; set; }
    public string ResourceNumber { get; set; }
    public string Title { get; set; }
    public int? Status { get; set; }
    public Sorter Sorter { get; set; }

    public bool isStock { get; set; } = false;
}

public class CpcBORFilter
{
    public string ResourceTypeId { get; set; }
    public string ResourceFamilyId { get; set; }
    public string ResourceNumber { get; set; }
    public string Title { get; set; }
    public string BORId { get; set; }
    public int? Status { get; set; }
    public Sorter Sorter { get; set; }
}

public class CpcPmolFilter
{
    public string ResourceTypeId { get; set; }
    public string ResourceFamilyId { get; set; }
    public string ResourceNumber { get; set; }
    public string Title { get; set; }
    public string PmolId { get; set; }
    public int? Status { get; set; }
    public Sorter Sorter { get; set; }
    public string type { get; set; }
}

public class PBSBORFilter
{
    public string ResourceTypeId { get; set; }
    public string ResourceFamilyId { get; set; }
    public string ResourceNumber { get; set; }
    public string Title { get; set; }
    public string PBSId { get; set; }
    public int? Status { get; set; }
    public Sorter Sorter { get; set; }
}

public class CpcLobourFilterMyEnvDto
{
    public string Cu { get; set; }
    public string Title { get; set; }
    public string ProjectSequenceId { get; set; }

}