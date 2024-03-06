using System;

namespace UPrinceV4.Web.Data.Comment;

public class CommentCardPs
{
    public string Id { get; set; }
    public string PsSequenceId { get; set; }
    public string Title { get; set; }
    public string ArticleNo { get; set; }
    public string CardTitle { get; set; }
    public string LotId { get; set; }
    public string ColumnName { get; set; }
    public string Message { get; set; }
    public string CompanyId { get; set; }
    public DateTime? Date { get; set; }
}

public class CreateCommentCardPs
{
    public string Id { get; set; }
    public string PsSequenceId { get; set; }
    public string Title { get; set; }
    public string ArticleNo { get; set; }
    public string CardTitle { get; set; }
    public string LotId { get; set; }
    public string ColumnName { get; set; }
    public string Message { get; set; }
    public string CompanyId { get; set; }
    public DateTime? Date { get; set; }
}

public class GetCommentCardPs
{
    public string Id { get; set; }
    public string PsSequenceId { get; set; }
    public string Title { get; set; }
    public string ArticleNo { get; set; }
    public string CardTitle { get; set; }
    public string LotId { get; set; }
    public string ColumnName { get; set; }
    public string Message { get; set; }
    public string CompanyId { get; set; }
    public DateTime? Date { get; set; }
}