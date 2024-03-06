using System;

namespace UPrinceV4.Web.Data.PMOL;

public class PmolPersonComment
{
    public string Id { get; set; }
    public string CommentCardId { get; set; }
    public string Comment { get; set; }
    public string CommentBy { get; set; }
    public string CommentTo { get; set; }
    public DateTime Date { get; set; }
    public string IsAccept { get; set; }
}

public class PmolPersonCommentDto
{
    public string Id { get; set; }
    public string CommentCardId { get; set; }
    public string Comment { get; set; }
    public string CommentBy { get; set; }
    public string CommentTo { get; set; }
    public string CommentByName { get; set; }
    public string CommentToName { get; set; }
    public DateTime Date { get; set; }
    public string IsAccept { get; set; }
}