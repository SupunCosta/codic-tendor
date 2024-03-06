using System.Collections.Generic;

namespace UPrinceV4.Web.Data.Category;

public class Post
{
    public string Id { get; set; }
    public string Title { get; set; }
    public string PostType { get; set; }
    public string Description { get; set; }
    public string Address { get; set; }
    public string Location { get; set; }
    public string FishDetailtype { get; set; }
}

public class PostDto
{
    public string Id { get; set; }
    public string Title { get; set; }
    public string PostType { get; set; }
    public string Description { get; set; }
    public string Address { get; set; }
    public string Location { get; set; }
    public string FishDetailtype { get; set; }
    public List<string> Pictures { get; set; }
}