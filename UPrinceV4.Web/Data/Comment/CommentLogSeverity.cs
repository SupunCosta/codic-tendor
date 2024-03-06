namespace UPrinceV4.Web.Data.Comment;

public class CommentLogSeverity
{
    public string Id { get; set; }
    public string Name { get; set; }
    public string LanguageCode { get; set; }
    public string SeverityId { get; set; }
    public int DisplayOrder { get; set; }
}

public class CommentLogSeverityDto
{
    public string Key { get; set; }
    public string Text { get; set; }
}