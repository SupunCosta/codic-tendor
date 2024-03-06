using System.Collections.Generic;
using System.Linq;
using UPrinceV4.Web.Data.CAB;

namespace UPrinceV4.Web.Models;

public class CabUniqueContactDetailsFilter
{
    public IEnumerable<CabUniqueData> FilterByEmail(string email, IEnumerable<CabUniqueData> uniqueDataList)
    {
        var emailList = (IEnumerable<CabEmail>)uniqueDataList;
        uniqueDataList = emailList.Where(p =>
            p.EmailAddress != null && p.EmailAddress.ToLower().Contains(email.ToLower().Replace("'","''")));
        return uniqueDataList;
    }

    public IEnumerable<CabUniqueData> FilterByLandPhone(string landPhone, IEnumerable<CabUniqueData> uniqueDataList)
    {
        var landPhoneList = (IEnumerable<CabLandPhone>)uniqueDataList;
        uniqueDataList = landPhoneList.Where(p =>
            p.LandPhoneNumber != null && p.LandPhoneNumber.ToLower().Contains(landPhone.ToLower()));
        return uniqueDataList;
    }

    public IEnumerable<CabUniqueData> FilterByMobilePhone(string mobilePhone,
        IEnumerable<CabUniqueData> uniqueDataList)
    {
        var mobilePhoneList = (IEnumerable<CabMobilePhone>)uniqueDataList;
        uniqueDataList = mobilePhoneList.Where(p =>
            p.MobilePhoneNumber != null && p.MobilePhoneNumber.ToLower().Contains(mobilePhone.ToLower()));
        return uniqueDataList;
    }

    public IEnumerable<CabUniqueData> FilterBySkype(string skype, IEnumerable<CabUniqueData> uniqueDataList)
    {
        var skypeList = (IEnumerable<CabSkype>)uniqueDataList;
        uniqueDataList = skypeList.Where(p =>
            p.SkypeNumber != null && p.SkypeNumber.ToLower().Contains(skype.ToLower()));
        return uniqueDataList;
    }

    public IEnumerable<CabUniqueData> FilterByWhatsApp(string whatsApp, IEnumerable<CabUniqueData> uniqueDataList)
    {
        var whatsAppList = (IEnumerable<CabWhatsApp>)uniqueDataList;
        uniqueDataList = whatsAppList.Where(p =>
            p.WhatsAppNumber != null && p.WhatsAppNumber.ToLower().Contains(whatsApp.ToLower()));
        return uniqueDataList;
    }

    public bool IsUsedEmail(string email, IEnumerable<CabUniqueData> uniqueDataList)
    {
        var emailList = (IEnumerable<CabEmail>)uniqueDataList;
        var uniqueData = emailList.FirstOrDefault(p =>
            p.EmailAddress != null && p.EmailAddress.ToLower().Equals(email.ToLower()) && p.IsDeleted == false);
        var state = uniqueData != null;
        return state;
    }

    public bool IsUsedLandPhone(string landPhone, IEnumerable<CabUniqueData> uniqueDataList)
    {
        var landPhoneList = (IEnumerable<CabLandPhone>)uniqueDataList;
        var uniqueData = landPhoneList.FirstOrDefault(p =>
            p.LandPhoneNumber != null && p.LandPhoneNumber.ToLower().Equals(landPhone.ToLower()) &&
            p.IsDeleted == false);
        var state = uniqueData != null;
        return state;
    }

    public bool IsUsedMobilePhone(string mobilePhone, IEnumerable<CabUniqueData> uniqueDataList)
    {
        var mobilePhoneList = (IEnumerable<CabMobilePhone>)uniqueDataList;
        var uniqueData = mobilePhoneList.FirstOrDefault(p =>
            p.MobilePhoneNumber != null && p.MobilePhoneNumber.ToLower().Equals(mobilePhone.ToLower()) &&
            p.IsDeleted == false);
        var state = uniqueData != null;
        return state;
    }

    public bool IsUsedSkype(string skype, IEnumerable<CabUniqueData> uniqueDataList)
    {
        var skypeList = (IEnumerable<CabSkype>)uniqueDataList;
        var uniqueData = skypeList.FirstOrDefault(p =>
            p.SkypeNumber != null && p.SkypeNumber.ToLower().Equals(skype.ToLower()) && p.IsDeleted == false);
        var state = uniqueData != null;
        return state;
    }

    public bool IsUsedWhatsApp(string whatsApp, IEnumerable<CabUniqueData> uniqueDataList)
    {
        var whatsAppList = (IEnumerable<CabWhatsApp>)uniqueDataList;
        var uniqueData = whatsAppList.FirstOrDefault(p =>
            p.WhatsAppNumber != null && p.WhatsAppNumber.ToLower().Equals(whatsApp.ToLower()) &&
            p.IsDeleted == false);
        var state = uniqueData != null;
        return state;
    }
}