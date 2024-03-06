namespace UPrinceV4.Web.Data.Category;

public class CategoryLevel
{
    public string Id { get; set; }
    public string Name { get; set; }
    public string LevelId { get; set; }
    public string LanguageCode { get; set; }
    public int DisplayOrder { get; set; }
    public bool IsChildren { get; set; } = true;
    public string Image { get; set; }
    public string ParentId { get; set; }
}

public class CategoryLevelCreateDto
{
    public string Id { get; set; }
    public string Name { get; set; }
    public string LevelId { get; set; }

    public int DisplayOrder { get; set; }
    public bool IsChildren { get; set; } = true;
    public string Image { get; set; }
    public string ParentId { get; set; }
    public string hasChildren { get; set; }
}