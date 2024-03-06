namespace UPrinceV4.Web.Data.Comment;

public class CommentCardContractorPs
{
    public string Id { get; set; }
    public string PsSequenceId { get; set; }
    public string CommentCardId { get; set; }
    public string ContractorId { get; set; }
    public string Accept { get; set; } = "0";
    public string Priority { get; set; }
    public string Severity { get; set; }
    public string Status { get; set; }
    public string Field { get; set; }
    public string Reporter { get; set; }
    public string Assigner { get; set; }
    public string CreaterId { get; set; }
    public string ChangeType { get; set; }
}