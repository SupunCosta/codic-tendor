using System;
using System.Collections.Generic;

namespace UPrinceV4.Web.Data.Comment;

public class CommentCard
{
    public string Id { get; set; }
    public string Title { get; set; }
    public string ArticleNo { get; set; }
    public string CardTitle { get; set; }
    public string LotId { get; set; }
    public string ColumnName { get; set; }
    public string Message { get; set; }
    public string CompanyId { get; set; }
    public DateTime? Date { get; set; }
}

public class CommentCardDto
{
    public string Id { get; set; }
    public string Title { get; set; }
    public string ArticleNo { get; set; }
    public string CardTitle { get; set; }
    public string LotId { get; set; }
    public string ColumnName { get; set; }
    public string Message { get; set; }
    public DateTime Date { get; set; }
    public List<ContractorsCommentCardDto> Contractors { get; set; }
    public List<ContractorsCommentCardDto> Ps { get; set; }
}

public class ContractorsCommentCardDto
{
    public string Id { get; set; }
    public string CommentCardId { get; set; }
    public string PsSequenceId { get; set; }
    public string ContractorId { get; set; }
    public string ContractorName { get; set; }
    public bool Accept { get; set; }

    public string Reporter { get; set; }
    public string Assigner { get; set; }
    public string AssignerId { get; set; }
    public string Creater { get; set; }
    public string CreaterId { get; set; }
    public CommentLogPriorityDto Priority { get; set; }
    public CommentLogFieldDto Field { get; set; }
    public CommentLogSeverityDto Severity { get; set; }
    public CommentLogStatusDto Status { get; set; }
    public CommentChangeTypeDto ChangeType { get; set; }
    public List<CommentsDto> Comments { get; set; }
    public string PsOrderNumber { get; set; }
}

public class CommentsDto
{
    public string Id { get; set; }
    public string Name { get; set; }
    public string Message { get; set; }
    public string CommentCardContractorsId { get; set; }
    public string PersonId { get; set; }
    public string Image { get; set; }
    public string TimeStamp { get; set; }
}

public class AcceptComment
{
    public string Id { get; set; }
    public string Accept { get; set; }
}

public class CommentFilter
{
    public string ArticalNo { get; set; }
    public string Title { get; set; }
    public string LotId { get; set; }
    public Sorter Sorter { get; set; }
}