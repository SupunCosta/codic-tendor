using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace UPrinceV4.Web.Data.Contractor;

public class ContractorTeamList
{
    public string Id { get; set; }
    public string Name { get; set; }
    public string Company { get; set; }
    public string CompanyId { get; set; }
    public string RoleId { get; set; }
    public bool? InvitationMail { get; set; } = false;
    public string InvitationId { get; set; }
    public string CabPersonId { get; set; }
    public string CabPersonName { get; set; }
    public string RoleName { get; set; }
    public string LotContractorId { get; set; }
    public string Count { get; set; }
    public bool? IsManual { get; set; } = false;
    public string? Approve { get; set; } = "0";

    public bool? IsDownloded { get; set; } = false;
    public bool? IsSubscribed { get; set; } = false;

    [NotMapped]
    public bool? IsNotSubscribe { get; set; } = false;

    [NotMapped]
    public bool IsUploaded { get; set; }

    [NotMapped]
    public string FileType { get; set; }

}

public class ContractorTeamListDto
{
    public string Id { get; set; }
    public string Name { get; set; }
    public string Company { get; set; }
    public string CompanyId { get; set; }
    public string RoleId { get; set; }
    public bool? InvitationMail { get; set; } = false;
    public string InvitationId { get; set; }
    public string CabPersonId { get; set; }
    public string CabPersonName { get; set; }
    public string RoleName { get; set; }
    public string LotContractorId { get; set; }
    public string Count { get; set; }
    public bool? IsManual { get; set; } = false;
    public string? Approve { get; set; } = "0";
    public bool? IsDownloded { get; set; } = false;
    public bool? IsSubscribed { get; set; } = false;
    
    public bool? IsNotSubscribe { get; set; } = false;
    public DateTime? ReminderOneDate { get; set; }
    public DateTime? ReminderTwoDate { get; set; }
    public DateTime? ReminderThreeDate { get; set; }
    public DateTime? ReminderFourDate { get; set; }
    public string ConstructorWorkFlowSequenceId { get; set; }
    public string SequenceId { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public string LotTitle { get; set; }
    public string LotId { get; set; }
}