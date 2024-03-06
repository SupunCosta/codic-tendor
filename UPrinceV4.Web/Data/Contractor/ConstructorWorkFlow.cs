using System;
using System.Collections.Generic;

namespace UPrinceV4.Web.Data.Contractor;

public class ConstructorWorkFlow
{
    public string Id { get; set; }
    public string SequenceId { get; set; }
    public string Division { get; set; }
    public string Lot { get; set; }
    public string Contractor { get; set; }
    public string Price { get; set; }
    public string Status { get; set; }
    public DateTime? ModifiedDateTime { get; set; }
    public DateTime? CreatedDateTime { get; set; }
    public DateTime? StatusChangeDate { get; set; }
    public string ModifiedBy { get; set; }
    public string CreatedBy { get; set; }
    public string CabCompanyId { get; set; }
    public string Title { get; set; }
    public string TypeId { get; set; }
    public double Version { get; set; } = 0.0;
    public bool IsInviteSend { get; set; }
    public bool MeasuringStateReceived { get; set; }
}

public class ConstructorLotInfoDto
{
    public string Id { get; set; }
    public string SequenceId { get; set; }
    public string Division { get; set; }
    public string Lot { get; set; }
    public string Contractor { get; set; }
    public string Price { get; set; }
    public ConstructorWorkFlowStatusDto Status { get; set; }
    public string CompanyId { get; set; }
    
    public ContractorAccreditationAndSupplierDto AccreditationAndSupplier { get; set; }
   public List<ConstructorTeamList> ConstructorTeamList { get; set; }
}

public class ContractorAccreditationAndSupplierDto
{
    public List<ContractorSupplierListDto> ContractorSupplierList { get; set; }
    public List<ContractorAccreditationDto> ContractorAccreditation { get; set; }
}

public class ConstructorWorkFlowDto
{
    public string Id { get; set; }
    public string SequenceId { get; set; }
    public string Division { get; set; }
    public string Lot { get; set; }
    public string Title { get; set; }
    public string Contractor { get; set; }
    public string Price { get; set; }
    public DateTime? ModifiedDateTime { get; set; }
    public DateTime? CreatedDateTime { get; set; }
    public DateTime? StatusChangeDate { get; set; }
    public string ModifiedBy { get; set; }
    public string CreatedBy { get; set; }
    public List<ContractList> ContractList { get; set; }
    public ConstructorWorkFlowStatusDto Status { get; set; }
    public HistoryLogDto HistoryLog { get; set; }
    public ContractorAccreditationAndSupplierDto AccreditationAndSupplier { get; set; }
    public string CabCompanyId { get; set; }
    public List<ConstructorTeamList> ConstructorTeamList { get; set; }
    public string ContractTitle { get; set; }
    public string TypeId { get; set; }
    public string TotalPrice { get; set; }
    public string LotSequenceId { get; set; }
    public List<GetConstructorWfStatusChangeTime> TimeTable { get; set; }
    public string Version { get; set; }
    public string CabPersonId { get; set; }
    public bool IsInviteSend { get; set; }
    public string LotStatusId { get; set; }
    public string LotDocLink { get; set; }

}

public class ContractList
{
    public string Id { get; set; }
    public string Title { get; set; }
    public string Price { get; set; }
    public string SequenceId { get; set; }
    public bool IsZeroState { get; set; }
    public string CabCompanyId { get; set; }
    public bool IsPsUpload { get; set; }
}

public class ConstructorByTaxonomyDto
{
    public string FullName { get; set; }
    public string CompanyName { get; set; }
    public string JobRole { get; set; }
}

public class ConstructorTeamList
{
    public string Id { get; set; }
    public string CabPersonName { get; set; }
    public string JobTitle { get; set; }
    public string CompanyName { get; set; }
    public string CabPersonId { get; set; }
    public string CompanyId { get; set; }
    public PersonCompanyDto PersonCompany { get; set; }
    public CompanyDto Company { get; set; }
}

public class PersonCompanyDto
{
    public string Id { get; set; }
    public string JobTitle { get; set; }
}

public class CompanyDto
{
    public string Id { get; set; }
    public string Name { get; set; }
}

public class ConstructorTeamListFilter
{
    public string FullName { get; set; }
    public string CompanyId { get; set; }
    public string LotId { get; set; }
}

public class ConstructorWorkFlowDelete
{
    public string Lot { get; set; }
    public List<string> CabCompanyId { get; set; }
}

public class GetContractingUnitByUser
{
    public string Key { get; set; }
    public string SequenceCode { get; set; }
    public string Name { get; set; }
}

public class GetProjectByUser
{
    public string Key { get; set; }
    public string SequenceCode { get; set; }
    public string Name { get; set; }
    public string Title { get; set; }
    public string ContractingUnitId { get; set; }
}

public class FilterByUser
{
    public string Title { get; set; }
}