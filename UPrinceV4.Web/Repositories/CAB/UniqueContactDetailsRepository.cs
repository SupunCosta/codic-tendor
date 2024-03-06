using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UPrinceV4.Web.Data.CAB;
using UPrinceV4.Web.Models;
using UPrinceV4.Web.Repositories.Interfaces.CAB;

namespace UPrinceV4.Web.Repositories.CAB;

public class UniqueContactDetailsRepository : IUniqueContactDetailsRepository
{
    public async Task<IEnumerable<CabUniqueData>> GetCabUsedUniqueContactDataList(
        UniqueContactDetailsRepositoryParameter uniqueContactDetailsRepositoryParameter)
    {
        IEnumerable<CabUniqueData> dataList = null;
        var uniqueData = uniqueContactDetailsRepositoryParameter.CabUniqueContactDetailsFilterModel;
        var cabUniqueContactDetailsFilter = new CabUniqueContactDetailsFilter();
        if (uniqueData.MobilePhone != null)
        {
            var mobileList = uniqueContactDetailsRepositoryParameter.ApplicationDbContext.CabMobilePhone.ToList();
            dataList = cabUniqueContactDetailsFilter.FilterByMobilePhone(uniqueData.MobilePhone, mobileList);
        }
        else if (uniqueData.Skype != null)
        {
            var skypeList = uniqueContactDetailsRepositoryParameter.ApplicationDbContext.CabSkype.ToList();
            dataList = cabUniqueContactDetailsFilter.FilterBySkype(uniqueData.Skype, skypeList);
        }
        else if (uniqueData.LandPhone != null)
        {
            var landPhoneList = uniqueContactDetailsRepositoryParameter.ApplicationDbContext.CabLandPhone.ToList();
            dataList = cabUniqueContactDetailsFilter.FilterByLandPhone(uniqueData.LandPhone, landPhoneList);
        }
        else if (uniqueData.WhatsApp != null)
        {
            var whatsAppList = uniqueContactDetailsRepositoryParameter.ApplicationDbContext.CabWhatsApp.ToList();
            dataList = cabUniqueContactDetailsFilter.FilterByWhatsApp(uniqueData.WhatsApp, whatsAppList);
        }
        else if (uniqueData.Email != null)
        {
            var emailList = uniqueContactDetailsRepositoryParameter.ApplicationDbContext.CabEmail.ToList();
            dataList = cabUniqueContactDetailsFilter.FilterByEmail(uniqueData.Email, emailList);
        }

        return dataList;
    }

    public async Task<bool> IsUsedUniqueContact(
        UniqueContactDetailsRepositoryParameter uniqueContactDetailsRepositoryParameter)
    {
        var state = false;
        var uniqueData = uniqueContactDetailsRepositoryParameter.CabUniqueContactDetailsFilterModel;
        var cabUniqueContactDetailsFilter = new CabUniqueContactDetailsFilter();
        if (uniqueData.MobilePhone != null)
        {
            var mobileList = uniqueContactDetailsRepositoryParameter.ApplicationDbContext.CabMobilePhone.ToList();
            state = cabUniqueContactDetailsFilter.IsUsedMobilePhone(uniqueData.MobilePhone, mobileList);
        }
        else if (uniqueData.Skype != null)
        {
            var skypeList = uniqueContactDetailsRepositoryParameter.ApplicationDbContext.CabSkype.ToList();
            state = cabUniqueContactDetailsFilter.IsUsedSkype(uniqueData.Skype, skypeList);
        }
        else if (uniqueData.LandPhone != null)
        {
            var landPhoneList = uniqueContactDetailsRepositoryParameter.ApplicationDbContext.CabLandPhone.ToList();
            state = cabUniqueContactDetailsFilter.IsUsedLandPhone(uniqueData.LandPhone, landPhoneList);
        }
        else if (uniqueData.WhatsApp != null)
        {
            var whatsAppList = uniqueContactDetailsRepositoryParameter.ApplicationDbContext.CabWhatsApp.ToList();
            state = cabUniqueContactDetailsFilter.IsUsedWhatsApp(uniqueData.WhatsApp, whatsAppList);
        }
        else if (uniqueData.Email != null)
        {
            var emailList = uniqueContactDetailsRepositoryParameter.ApplicationDbContext.CabEmail.ToList();
            state = cabUniqueContactDetailsFilter.IsUsedEmail(uniqueData.Email, emailList);
        }
        else if (uniqueData.CompanyName != null)
        {
            var companyList = uniqueContactDetailsRepositoryParameter.ApplicationDbContext.CabCompany.ToList();
            state = companyList.Any(c => c.Name.ToLower().Equals(uniqueData.CompanyName.ToLower()));
        }

        return state;
    }
}