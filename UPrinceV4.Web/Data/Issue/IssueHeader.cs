using System;
using System.Collections.Generic;
using UPrinceV4.Web.Data.WBS;

namespace UPrinceV4.Web.Data.Issue;

public class IssueHeader
{
    public string Id { get; set; }
    public string Title { get; set; }
    public string Type { get; set; }
    public string Status { get; set; }
    public string Priority { get; set; }
    public string Severity { get; set; }
    public DateTime? DateRaised  { get; set; }
    public DateTime? LastUpdate  { get; set; }
    public DateTime? DecisionDate  { get; set; }
    public DateTime? ClosureDate  { get; set; }
    public string RaisedBy  { get; set; }
    public DateTime? CreatedDateTime  { get; set; }
    public DateTime? UpdatedDateTime  { get; set; }
    public string CreatedBy  { get; set; }
    public string UpdatedBy  { get; set; }
    public string ProductId  { get; set; }
    public string WbsId  { get; set; }
    public string Responsible  { get; set; }
    public string Decision  { get; set; }
    
}

public class IssueHeaderCreateDto
{
    public string Id { get; set; }
    public string Title { get; set; }
    public string Type { get; set; }
    public string Status { get; set; }
    public string Priority { get; set; }
    public string Severity { get; set; }
    public DateTime? DateRaised  { get; set; }
    public DateTime? LastUpdate  { get; set; }
    public DateTime? DecisionDate  { get; set; }
    public DateTime? ClosureDate  { get; set; }
    public string RaisedBy  { get; set; }
    public string RaisedByName  { get; set; }
    public DateTime? CreatedDateTime  { get; set; }
    public DateTime? UpdatedDateTime  { get; set; }
    public string CreatedBy  { get; set; }
    public string UpdatedBy  { get; set; }
    public string ProductId  { get; set; }
    public string WbsId  { get; set; }
    public string Responsible  { get; set; }
    public string ResponsibleName  { get; set; }
    public string Decision  { get; set; }
    public List<TagDto> Tags { get; set; }
    public List<DocDto> Documents { get; set; }
    public List<IssueTo> ToPerson { get; set; }
    public List<IssueCc> Cc { get; set; }
    public List<WbsTaskFilterResults> Tasks { get; set; }
    public List<WbsConversationDto> Conversations { get; set; }


}

public class TagDto
{ 
    public string Id { get; set; }
    public string Name { get; set; }
}
public class DocDto
{ 
    public string Id { get; set; }
    public string Url { get; set; }
}

public class IssueFilterDto
{ 
    public string Title { get; set; }
    public string Status { get; set; }
    public string Priority { get; set; }
    public string Severity { get; set; }
    public string Type { get; set; }
    public string Responsible { get; set; }
    public string ProductTitle { get; set; }
    public Sorter Sorter { get; set; }
    public string Project { get; set; }
    public DateTime? StartDate  { get; set; }
    public DateTime? EndDate { get; set; }


}

public class IssueDropDownData
{ 
    public IEnumerable<IssueDropDownDto> Type { get; set; }
    public IEnumerable<IssueDropDownDto> Status { get; set; }
    public IEnumerable<IssueDropDownDto> Priority { get; set; }
    public IEnumerable<IssueDropDownDto> Severity { get; set; }

}

public class CabPersonIssueDto
{ 
    public string Oid { get; set; }
    public string FullName { get; set; }
    public string Id { get; set; }
}

public class IssueFilterResults
{
    public string Id { get; set; }
    public string Title { get; set; }
    public string Type { get; set; }
    public string Status { get; set; }
    public string Priority { get; set; }
    public string Severity { get; set; }
    public DateTime? DateRaised  { get; set; }
    public DateTime? LastUpdate  { get; set; }
    public DateTime? DecisionDate  { get; set; }
    public DateTime? ClosureDate  { get; set; }
    public string RaisedBy  { get; set; }
    public DateTime? CreatedDateTime  { get; set; }
    public DateTime? UpdatedDateTime  { get; set; }
    public string CreatedBy  { get; set; }
    public string UpdatedBy  { get; set; }
    public string ProductId  { get; set; }
    public string WbsId  { get; set; }
    public string Responsible  { get; set; }
    public string Decision  { get; set; }
    public string TypeName { get; set; }
    public string StatusName { get; set; }
    public string PriorityName { get; set; }
    public string SeverityName { get; set; }
    public string ResponsibleName  { get; set; }
    public int? TotalAttachment { get; set; }
    public int? TotalMail { get; set; }
    public int? TotalInstructions { get; set; }
    public int? TotalCheckedInstructions { get; set; }
    public List<IssueTo> ToPerson { get; set; }
    public List<IssueCc> Cc { get; set; }
    public string ProjectSequenceId  { get; set; }
    public string Cu  { get; set; }
    public string ProjectTitle  { get; set; }
    public string ProductTitle  { get; set; }

    
}