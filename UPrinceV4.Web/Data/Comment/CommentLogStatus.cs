namespace UPrinceV4.Web.Data.Comment;

public class CommentLogStatus
{
    public string Id { get; set; }
    public string Name { get; set; }
    public string LanguageCode { get; set; }
    public string StatusId { get; set; }
    public int DisplayOrder { get; set; }
}

public class CommentLogStatusDto
{
    public string Key { get; set; }
    public string Text { get; set; }
}