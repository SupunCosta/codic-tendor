namespace UPrinceV4.Web.Data;

public class DbUpdator
{
    public int Id { get; set; }
    public string Sql { get; set; }
    public string VersionNo { get; set; }
    public bool ExecutionStatus { get; set; }
}