using System.Collections.Generic;

namespace UPrinceV4.Web.Data.Comment;

public class CommentLogDropDownData
{
    public IEnumerable<CommentLogPriorityDto> Priority { get; set; }
    public IEnumerable<CommentLogSeverityDto> Severity { get; set; }
    public IEnumerable<CommentLogStatusDto> Status { get; set; }
    public IEnumerable<CommentLogFieldDto> Field { get; set; }

    public IEnumerable<CommentChangeTypeDto> ChangeType { get; set; }
}