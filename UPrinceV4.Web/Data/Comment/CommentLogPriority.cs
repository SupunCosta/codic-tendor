namespace UPrinceV4.Web.Data.Comment;

public class CommentLogPriority
{
    public string Id { get; set; }
    public string Name { get; set; }
    public string LanguageCode { get; set; }
    public string PriorityId { get; set; }
    public int DisplayOrder { get; set; }
}

public class CommentLogPriorityDto
{
    public string Key { get; set; }
    public string Text { get; set; }
}