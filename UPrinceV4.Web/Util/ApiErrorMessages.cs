using System.Linq;
using Microsoft.EntityFrameworkCore;
using UPrinceV4.Shared;
using UPrinceV4.Web.Data;

namespace UPrinceV4.Web.Util;

public enum ErrorMessageKey
{
    ProjectManagementLevelEmpty,
    CurrencyEmpty,
    Ok,
    NoAvailableShiftsForTheDay,
    NoAvailableTimeClockForTheId,
    InvalidQrCode,
    NoMoreRecordsFound,
    WorkflowCanNotChange,
    NoOrganisationInYourSearchCriteria,
    NoAvailableQuality,
    NoAvailableRisk,
   
}

public class ApiErrorMessages
{
    public const string ALREADY_EXISTS_MESSAGE = "Already Exists";
    public const string CAN_NOT_FIND_MESSAGE = "Can not Find";
    public const string CAN_NOT_DELETE_MESSAGE = "Can not Delete";
    public const string DELETED_SUCCESSFULLY_MESSAGE = "Deleted Successfully";
    public static ErrorMessage GetErrorMessage(ITenantProvider tenantProvider, ErrorMessageKey key,
        string lang)
    {
        var options = new DbContextOptions<LocalizedDbContext>();
        var applicationDbContext = new LocalizedDbContext(options,   tenantProvider.GetTenant().ConnectionString, tenantProvider);
        var errorMessage = applicationDbContext.ErrorMessage.FirstOrDefault(e => e.Id == key.ToString());
        if (string.IsNullOrEmpty(lang) || lang.Equals(Language.en.ToString())) return errorMessage;

        var localizedData =
            applicationDbContext.LocalizedData.FirstOrDefault(l =>
                l.LanguageCode == lang && l.LocaleCode == key.ToString());
        if (errorMessage != null && localizedData != null) errorMessage.Message = localizedData.Label;

        return errorMessage;
    }
}