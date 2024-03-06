namespace UPrinceV4.Web.Data.Comment;

public class CommentChangeType
{
    public string Id { get; set; }
    public string Name { get; set; }
    public string LanguageCode { get; set; }
    public string TypeId { get; set; }
    public int DisplayOrder { get; set; }
}

public class CommentChangeTypeDto
{
    public string Key { get; set; }
    public string Text { get; set; }
}