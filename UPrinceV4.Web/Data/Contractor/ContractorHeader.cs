using System;
using System.Collections.Generic;
using UPrinceV4.Web.Data.StandardMails;

namespace UPrinceV4.Web.Data.Contractor;

public class ContractorHeader
{
    public string Id { get; set; }
    public string Name { get; set; }
    public string Title { get; set; }
    public string SequenceId { get; set; }
    public string ProductItemTypeId { get; set; }
    public string ContractTaxonomyId { get; set; }
    public string ContractorTaxonomyId { get; set; }
    public string StatusId { get; set; }
    public string Division { get; set; }
    public string TenderBudget { get; set; }
    public string CustomerBudget { get; set; }
    public string ContractorId { get; set; }
    public string ProjectSequenceId { get; set; }
    public string ExpectedNumberOfQuotes { get; set; }
    public DateTime? ModifiedDateTime { get; set; }
    public DateTime? CreatedDateTime { get; set; }
    public DateTime? StatusChangeDate { get; set; }
    public string ModifiedBy { get; set; }
    public string CreatedBy { get; set; }
    public string Cu { get; set; }
    public double MeasuringStatus { get; set; } = 0.0;
    public DateTime? ReminderOneDate { get; set; }
    public DateTime? ReminderTwoDate { get; set; }
    public DateTime? ReminderThreeDate { get; set; }
    public DateTime? ReminderFourDate { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public string StandardMailId { get; set; }
    public bool IsInviteSend { get; set; }
    public bool IsPublic { get; set; }

}

public class ContractorHeaderDto
{
    public string Id { get; set; }
    public string Name { get; set; }
    public string SequenceId { get; set; }
    public string ProductTitle { get; set; }
    public string ProductId { get; set; }
    public string ProductItemTypeId { get; set; }
    public string ContractTaxonomyId { get; set; }
    public List<string> ContractorTaxonomyId { get; set; }
    public string StatusId { get; set; }
    public string Division { get; set; }
    public string TenderBudget { get; set; }
    public string CustomerBudget { get; set; }
    public string ContractorId { get; set; }
    public ContractorList ContractorList { get; set; }
    public ContractorDocumentation LotDocumentation { get; set; }
    public TechnicalDocumentation TechnicalDocumentation { get; set; }
    public List<ContractorTenderAward> TenderAwarding { get; set; }
    public string ProjectSequenceId { get; set; }
    public string ExpectedNumberOfQuotes { get; set; }
    public string MeasuringStatus { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public string StandardMailId { get; set; }
    public bool IsInviteSend { get; set; }
    public bool IsPublic { get; set; }

}

public class ContractorDocumentation
{
    public List<ContractorTechInstructionsDocs> TechnicalInstructions { get; set; }
    public List<ContractorTenderDocs> TenderDocuments { get; set; }
    public List<ContractorProvisionalAcceptenceDocs> ProvisionalAcceptance { get; set; }
    public List<ContractorFinalDeliveryDocs> FinalDelivery { get; set; }
}

public class TechnicalDocumentation
{
    public List<ContractorTechDocs> TechnicalDocList { get; set; }
}

public class ContractorList
{
    public string Id { get; set; }
    public List<string> ContractorLot { get; set; }
    public string LotId { get; set; }
    public List<ContractorTeamList> ContractorTeamList { get; set; }
}

public class ContractorFilterDto
{
    public string Title { get; set; }
    public string StatusChangeDate { get; set; }
    public string StatusId { get; set; }
    public string Price { get; set; }
    public Sorter Sorter { get; set; }
}

public class ContractorListDto
{
    public string Id { get; set; }
    public string Title { get; set; }
    public string Status { get; set; }
    public string Price { get; set; }
    public string StatusId { get; set; }
    public string SequenceId { get; set; }
    public string ContractId { get; set; }
    public string ContractSequenceId { get; set; }
    public string Type { get; set; }
    public DateTime StatusChangeDate { get; set; }
    public bool IsZeroState { get; set; }
    public string CompanyId { get; set; }
    public bool IsInviteSend { get; set; }
    public bool IsPublic { get; set; }

}

public class BMLotHeaderGetDto
{
    public string Id { get; set; }
    public string Name { get; set; }
    public string SequenceId { get; set; }
    public string Title { get; set; }
    public string ContractTaxonomyId { get; set; }
    public List<string> ContractorTaxonomyId { get; set; }
    public string Division { get; set; }
    public string TenderBudget { get; set; }
    public string CustomerBudget { get; set; }
    public string ContractorId { get; set; }
    public ContractorProductItemTypeDto ProductItemType { get; set; }
    public ContractorStatusDto Status { get; set; }
    public StandardMailDto StandardMail { get; set; }
    public ContractorList ContractorList { get; set; }
    public ContractorDocumentationget LotDocumentation { get; set; }
    public TechnicalDocumentationGet TechnicalDocumentation { get; set; }
    public List<ContractorsCompanyList> TenderAwarding { get; set; }
    public HistoryLogDto HistoryLog { get; set; }
    public string ProjectSequenceId { get; set; }
    public string ExpectedNumberOfQuotes { get; set; }
    public PublishTender PublishTender { get; set; }
    public string MeasuringStatus { get; set; }
    public List<GetLotStatusChangeTime> TimeTable { get; set; }
    public string StandardMailId { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public bool IsInviteSend { get; set; }
    public bool IsPublic { get; set; }

}

public class ContractorDocumentationget
{
    public List<ContractorTechInstructionsDocsDto> TechnicalInstructions { get; set; }
    public List<ContractorTenderDocsDto> TenderDocuments { get; set; }
    public List<ContractorProvisionalAcceptenceDocsDto> ProvisionalAcceptance { get; set; }
    public List<ContractorFinalDeliveryDocsDto> FinalDelivery { get; set; }
}

public class TechnicalDocumentationGet
{
    public List<ContractorTechDocsDto> TechnicalDocList { get; set; }
}

public class HistoryLogDto
{
    public DateTime CreatedDateTime { get; set; }
    public DateTime ModifiedDateTime { get; set; }
    public string CreatedBy { get; set; }
    public string ModifiedBy { get; set; }
}

public class PublishTender
{
    public int ContractorsInList { get; set; }
    public int DocumentsUploaded { get; set; }
    public string MeasuringStatus { get; set; }
    public string InvitationMail { get; set; }
}

public class AcceptInvitationDto
{
    public string LotId { get; set; }
    public string InvitationId { get; set; }
    public string Approve { get; set; }
}

public class ContractorTeam
{
    public string LotId { get; set; }
    public string SequenceId { get; set; }
    public List<ContractorTeamList> ContractorTeamList { get; set; }
}

public class GetUserInformationDto
{
    public string Organisation { get; set; }
    public string UserName { get; set; }
    public string OrganisationId { get; set; }
    public string UserId { get; set; }
    public ContractingUnit ContractingUnit { get; set; }
}

public class GetUserTypeDto
{
    public bool IsContractor { get; set; }
}

public class ContractingUnit
{
    public string Id { get; set; }
    public string SequnceCode { get; set; }
}

public class GetContractorByIdForMailDto
{
    public string SequenceCode { get; set; }
    public string Customer { get; set; }
    public string Sector { get; set; }
    public string EndDate { get; set; }
    public string StartDate { get; set; }
    public string Description { get; set; }
    public string ContractingUnit { get; set; }
    public string Architect { get; set; }
    public string Id { get; set; }
    public string Approve { get; set; }
    public string Cu { get; set; }
    public string EmailContent { get; set; }
    public bool? IsDownloded { get; set; }
    public bool? IsSubscribed { get; set; }
    public List<DownloadLotLinks> TenderDocs { get; set; }
    public List<DownloadLotLinks> TechnicalDocs { get; set; }

    public string LotTitle { get; set; }
    public DateTime LotStartDate { get; set; }
    public DateTime LotEndDate { get; set; }
    public string Name { get; set; }
    public string ProjectName { get; set; }
    public string LotId { get; set; }
    public bool? IsNotSubscribe { get; set; }

}

public class DownloadLotDocsDto
{
    public string LotId { get; set; }
    public string InvitationId { get; set; }
    public string Approve { get; set; }
    public string CwSequenceId { get; set; }
    public string Comment { get; set; }
    public bool IsNotSubscribe { get; set; } = false;
}

public class DownloadLotLinks
{
    public string Link { get; set; }
    public string Title { get; set; }
}