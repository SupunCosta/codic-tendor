using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;
using UPrinceV4.Web.Data.CAB;

namespace UPrinceV4.Web.Data.CIAW;

public class CiawHeader
{
    public string Id { get; set; }
    public string CabPersonId { get; set; }
    public string CiawStatus { get; set; }
    public DateTime? Date { get; set; }
    public string Project { get; set; }
    public string CreatedBy { get; set; }
    public string ModifiedBy { get; set; }
    public DateTime? CreatedDate { get; set; }
    public DateTime? ModifiedDate { get; set; }
    public string Reference { get; set; }
    public string PmolTeamRoleId { get; set; }
    public string ContractingUnit { get; set; }
    public string PmolId { get; set; }
    public string PresenceRegistrationId { get; set; }
    public int CIAWReferenceId { get; set; }

    [NotMapped] public string CabPersonName { get; set; }

    [NotMapped] public string IsCiawEligible { get; set; }

    [NotMapped] public string ProjectTitle { get; set; }

    public bool IsMailSend { get; set; }
}

public class CiawCreateDto
{
    public DateTime? Date { get; set; }
    public string BuId { get; set; }
}

public class CiawCreateReturnDto
{
    public DateTime? Date { get; set; }
    public RequestedBy RequestedBy { get; set; }
    public string CiawRequested { get; set; }
    public string CiawApproved { get; set; }
}

public class RequestedBy
{
    public string RequestedPerson { get; set; }
    public string RequestedPersonName { get; set; }
}

public class CiawHeaderFilterDto
{
    public string Id { get; set; }
    public string CabPersonId { get; set; }
    public string CiawStatus { get; set; }
    public DateTime? Date { get; set; }
    public string Project { get; set; }
    public string CreatedBy { get; set; }
    public string ModifiedBy { get; set; }
    public DateTime? CreatedDate { get; set; }
    public DateTime? ModifiedDate { get; set; }
    public string Reference { get; set; }
    public string PmolTeamRoleId { get; set; }
    public string ContractingUnit { get; set; }
    public string PmolId { get; set; }
    public string CabPersonName { get; set; }
    public string IsCiawEligible { get; set; } = "1";
    public string ProjectTitle { get; set; }
    public string NationalityId { get; set; }
    public string CIAWReferenceId { get; set; }
    public string CiawSiteCode { get; set; }
    public string VatId { get; set; }
    public string PresenceRegistrationId { get; set; }
    public int IsCiawEligibleOrder { get; set; }
    public SearchPresenceRegistrationItem SearchPresenceRegistrationItem { get; set; }
}

public class CiawFilter
{
    public string CabPerson { get; set; }
    public string CiawStatus { get; set; }

    public string IsCiawEligible { get; set; }
    public DateTime? Date { get; set; }
    public string Project { get; set; }
    public Sorter Sorter { get; set; }
}

public class CiawGetByIdDto
{
    public LastValidation remarks { get; set; }
    public string Id { get; set; }
    public string CabPersonId { get; set; }
    public string CiawStatus { get; set; }
    public DateTime? Date { get; set; }
    public string Project { get; set; }
    public string CreatedBy { get; set; }
    public string ModifiedBy { get; set; }
    public DateTime? CreatedDate { get; set; }
    public DateTime? ModifiedDate { get; set; }
    public string Reference { get; set; }
    public string PmolTeamRoleId { get; set; }
    public string CabPersonName { get; set; }
    public CiawStatusDto Status { get; set; }
    public HistoryLogDto HistoryLog { get; set; }
    public string ProjectTitle { get; set; }
    public string ProjectManager { get; set; }
    public string CiawCode { get; set; }
    public string CiawRegistrationStatus { get; set; }
    public string Nationality { get; set; }
    public CabCertification CabCertification { get; set; }
    public string NationalityId { get; set; }
    public string ProjectCiawCode { get; set; }
    public string Organisation { get; set; }
    public long OrgCiawCode { get; set; }
    public string OrgCountryCode { get; set; }
    public string OrgCountryName { get; set; }
    public string CertificationId { get; set; }
    public string ErrorWarning { get; set; }
    public string CIAWReferenceId { get; set; }
    public bool IsMailSend { get; set; }

    public string JsonString { get; set; }
    public string StatusError { get; set; }
}

public class HistoryLogDto
{
    public DateTime CreatedDate { get; set; }
    public DateTime ModifiedDate { get; set; }
    public string CreatedBy { get; set; }
    public string ModifiedBy { get; set; }
}

public class CiawSendRequest
{
    public List<string> CiawId { get; set; }
}

public class CiawCancleRequest
{
    public string CiawId { get; set; }
    public string LeaveReasonId { get; set; }
}

public class RegisterRequest
{
    public Item Item { get; set; }
    public string WORKPLACE_ID { get; set; }
    public string CiawId { get; set; }
    public DateTime RegistrationDate { get; set; }
}

public class CiawRequestData
{
    public string INSS { get; set; }
    public string COMPANY_ID { get; set; }
    public string WORKPLACE_ID { get; set; }
    public string Id { get; set; }
    public DateTime Date { get; set; }
    public string CIAWReferenceId { get; set; }
    public string CertificationTaxonomyId { get; set; }
    public string CabCountry { get; set; }
    public string CabCompany { get; set; }
    public string Street { get; set; }
    public string StreetNumber { get; set; }
    public string PostalCode { get; set; }
    public string City { get; set; }
}

public class CertificationDto
{
    public string INSS { get; set; }
    public string CertificationTaxonomyId { get; set; }
}

public class CompanyDataDto
{
    public string COMPANY_ID { get; set; }
    public string CabCountry { get; set; }
    public string CabCompany { get; set; }
    public string CabCountryName { get; set; }
}

public class WorkPlaceIdDto
{
    public string WORKPLACE_ID { get; set; }
}

public class CiawRequestDataDto
{
    public string INSS { get; set; }
    public string COMPANY_ID { get; set; }
    public string WORKPLACE_ID { get; set; }
    public string Id { get; set; }
    public DateTime Date { get; set; }
    public string CIAWReferenceId { get; set; }
    public string CertificationTaxonomyId { get; set; }
    public string CabCountry { get; set; }
    public string CabCompany { get; set; }
}

public class Item
{
    public string INSS { get; set; }
    public long COMPANY_ID { get; set; }
    public string LimosaId { get; set; }
}

public class RegisterRequestList
{
    public List<RegisterRequest> RegisterRequest { get; set; }
    public string AuthKey { get; set; }
}

public class SingleRegisterRequest
{
    public RegisterRequest RegisterRequest { get; set; }
    public string AuthKey { get; set; }
}

public class ResponceItem
{
    public Item1 item { get; set; }
    public bool sectorSpecified { get; set; }
    public string inss { get; set; }
    public int presenceRegistrationId { get; set; }
    public DateTime creationDate { get; set; }
    public string channel { get; set; }
    public LastValidation lastValidation { get; set; }
    public object validationHistory { get; set; }
    public object sender { get; set; }
    public DateTime registrationDate { get; set; }
    public bool registrationDateSpecified { get; set; }
    public List<object> items { get; set; }
    public List<int> itemsElementName { get; set; }
    public string workPlaceId { get; set; }
    public string clientPresenceRegistrationReference { get; set; }
    public PresenceRegistrationSubmitted presenceRegistrationSubmitted { get; set; }
    public List<ErrorList> errorList { get; set; }
}

public class Item1
{
    public DateTime registrationDate { get; set; }
    public bool registrationDateSpecified { get; set; }
    public List<string> items { get; set; }
    public List<int> itemsElementName { get; set; }
    public string workPlaceId { get; set; }
    public string clientPresenceRegistrationReference { get; set; }
}

public class LastValidation
{
    public int status { get; set; }
    public bool validationDateSpecified { get; set; }
    public RemarkList remarkList { get; set; }
}

public class ResponceRoot
{
    public ResponceItem item { get; set; }
}

public class ErrorList
{
    public string CiawId { get; set; }
    public string errorCode { get; set; }
    public string errorDescription { get; set; }
}

public class PresenceRegistrationSubmitted
{
    public DateTime registrationDate { get; set; }
    public bool registrationDateSpecified { get; set; }
    public List<string> items { get; set; }
    public List<int> itemsElementName { get; set; }
    public string workPlaceId { get; set; }
    public string clientPresenceRegistrationReference { get; set; }
}

public class CancelPresences
{
    public int PresenceRegistrationId { get; set; }
    public string CancellationReason { get; set; }
}

public class ErrorRoot
{
    public string message { get; set; }
    public string exceptionMessage { get; set; }
    public string exceptionType { get; set; }
    public string stackTrace { get; set; }
}

public class CiawEmail
{
    [JsonProperty("link")] public string Link { get; set; }

    [JsonProperty("date")] public string Date { get; set; }

    [JsonProperty("employeeName")] public string EmployeeName { get; set; }

    [JsonProperty("subject")] public string Subject { get; set; }
    [JsonProperty("projectSequenceCode")] public string ProjectSequenceCode { get; set; }
    [JsonProperty("projectCiawCode")] public string ProjectCiawCode { get; set; }
    [JsonProperty("organisation")] public string Organisation { get; set; }

    [JsonProperty("organisationCountryCode")]
    public string OrganisationCountryCode { get; set; }

    [JsonProperty("limosa")] public string Limosa { get; set; }
    [JsonProperty("ciawCode")] public string CiawCode { get; set; }
    [JsonProperty("datePmol")] public string DatePmol { get; set; }

    [JsonProperty("companyEnterpriseNumber")]
    public string CompanyEnterpriseNumber { get; set; }

    [JsonProperty("workPlaceZipCode")] public string WorkPlaceZipCode { get; set; }
    [JsonProperty("reason")] public string Reason { get; set; }
}

public class CiawEmail2
{
    [JsonProperty("firstName")] public string FirstName { get; set; }

    [JsonProperty("streetNumber")] public string StreetNumber { get; set; }

    [JsonProperty("postalCode")] public string PostalCode { get; set; }

    [JsonProperty("subject")] public string Subject { get; set; }
    [JsonProperty("vatCode")] public string VatCode { get; set; }
    [JsonProperty("workingContractor")] public string WorkingContractor { get; set; }
    [JsonProperty("adminAssistant")] public string AdminAssistant { get; set; }
    [JsonProperty("country")] public string Country { get; set; }
    [JsonProperty("reason")] public string Reason { get; set; }
}

public class CiawcertificateDate
{
    public string PersonId { get; set; }
    public string CertificationTaxonomyId { get; set; }

    public DateTime EndDate { get; set; }
}

public class CiawSiteCodeList
{
    public string ProjectSequenceCode { get; set; }
    public string ProjectTitle { get; set; }
    public string VatId { get; set; }
    public string CiawSiteCode { get; set; }
}

public class CiawSearchDto
{
    public SearchPresenceRegistrationCriteria searchPresenceRegistrationCriteria { get; set; }
}

public class SearchPresenceRegistrationCriteria
{
    public long CompanyID { get; set; }
}

// Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
public class ItemResonce
{
    public DateTime registrationDate { get; set; }
    public bool registrationDateSpecified { get; set; }
    public string inss { get; set; }
    public int companyID { get; set; }
    public bool companyIDSpecified { get; set; }
    public string limosaId { get; set; }
    public string vatNumber { get; set; }
    public string workPlaceId { get; set; }
    public string clientPresenceRegistrationReference { get; set; }
}

public class LastValidationResponse
{
    public int status { get; set; }
    public DateTime validationDate { get; set; }
    public bool validationDateSpecified { get; set; }
    public RemarkList remarkList { get; set; }
}

public class RemarkList
{
    public List<string> remark { get; set; }
}

public class SearchPresenceRegistrationItem
{
    public ItemResonce item { get; set; }
    public int sector { get; set; }
    public bool sectorSpecified { get; set; }
    public string inss { get; set; }
    public int presenceRegistrationId { get; set; }
    public DateTime creationDate { get; set; }
    public string channel { get; set; }
    public LastValidationResponse lastValidation { get; set; }

    public Sender sender { get; set; }
}

public class Sender
{
    public string senderId { get; set; }
    public int senderCompanyId { get; set; }
    public bool senderCompanyIdSpecified { get; set; }
    public string senderUserType { get; set; }
}

public class PresenceRegistrationTypeItem
{
    public DateTime registrationDate { get; set; }
    public bool registrationDateSpecified { get; set; }
    public string inss { get; set; }
    public int companyID { get; set; }
    public bool companyIDSpecified { get; set; }
    public object limosaId { get; set; }
    public object vatNumber { get; set; }
    public string workPlaceId { get; set; }
    public object clientPresenceRegistrationReference { get; set; }
}

public class PresenceRegistrationType
{
    public PresenceRegistrationTypeItem item { get; set; }
    public int sector { get; set; }
    public bool sectorSpecified { get; set; }
    public string inss { get; set; }
    public int presenceRegistrationId { get; set; }
    public DateTime creationDate { get; set; }
    public string channel { get; set; }
    public LastValidation lastValidation { get; set; }
    public List<ValidationHistory> validationHistory { get; set; }
    public Sender sender { get; set; }
}

public class PresenceRegistrationTypeResponce
{
    public PresenceRegistrationType presenceRegistrationType { get; set; }
}

public class ValidationHistory
{
    public int status { get; set; }
    public DateTime validationDate { get; set; }
    public bool validationDateSpecified { get; set; }
    public RemarkList remarkList { get; set; }
}