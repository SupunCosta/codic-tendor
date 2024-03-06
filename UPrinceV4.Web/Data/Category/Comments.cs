namespace UPrinceV4.Web.Data.Category;

public class Comments
{
    public string Id { get; set; }
    public string PostId { get; set; }
    public string Comment { get; set; }
    public string LevelId { get; set; }
    public int DisplayOrder { get; set; }
    public bool IsChildren { get; set; } = true;
    public string ParentId { get; set; }
}

public class CommentsDto
{
    public string Id { get; set; }
    public string PostId { get; set; }
    public string Comment { get; set; }
    public string LevelId { get; set; }
    public int DisplayOrder { get; set; }
    public bool IsChildren { get; set; }
    public string ParentId { get; set; }
    public string HasChildren { get; set; }
}