using System.Collections.Generic;
using System.Linq;
using UPrinceV4.Web.Data.CAB;

namespace UPrinceV4.Web.Models;

public class CabPersonFilter
{
    public IEnumerable<CabPerson> FilterByFullName(string fullName, IEnumerable<CabPerson> personList)
    {
        personList = personList.Where(p => p.FullName != null && p.FullName.ToLower().Contains(fullName.ToLower().Replace("'","''")));
        return personList;
    }

    public IEnumerable<CabPerson> FilterByJobTitle(string jobTitle, IEnumerable<CabPerson> personList)
    {
        personList = personList.Where(p => p.PersonCompanyList != null && p.PersonCompanyList.Any()
                                                                       && p.PersonCompanyList.First().JobRole !=
                                                                       null
                                                                       && p.PersonCompanyList.First().JobRole
                                                                           .ToLower().Contains(jobTitle.ToLower().Replace("'","''")));
        return personList;
    }

    public IEnumerable<CabPerson> FilterByMobilePhone(string personMobilePhone, IEnumerable<CabPerson> personList)
    {
        personList = personList.Where(p => p.PersonCompanyList != null && p.PersonCompanyList.Any()
                                                                       && p.PersonCompanyList.First().MobilePhone !=
                                                                       null && p.PersonCompanyList.First()
                                                                           .MobilePhone.MobilePhoneNumber != null
                                                                       && p.PersonCompanyList.First().MobilePhone
                                                                           .MobilePhoneNumber.ToLower()
                                                                           .Contains(personMobilePhone.ToLower()));
        return personList;
    }

    public IEnumerable<CabPerson> FilterByCompany(string company, IEnumerable<CabPerson> personList)
    {
        personList = personList.Where(p => p.PersonCompanyList != null && p.PersonCompanyList.Any()
                                                                       && p.PersonCompanyList.First().Company !=
                                                                       null
                                                                       && p.PersonCompanyList.First().Company
                                                                           .Name != null
                                                                       && p.PersonCompanyList.First().Company.Name
                                                                           .ToLower().Contains(company.ToLower().Replace("'","''")));
        return personList;
    }

    public IEnumerable<CabPerson> FilterByEmail(string email, IEnumerable<CabPerson> personList)
    {
        personList = personList.Where(p => p.PersonCompanyList != null && p.PersonCompanyList.Any()
                                                                       && p.PersonCompanyList.First().Email !=
                                                                       null && p.PersonCompanyList.First().Email
                                                                           .EmailAddress != null
                                                                       && p.PersonCompanyList.First().Email
                                                                           .EmailAddress.ToLower()
                                                                           .Contains(email.ToLower().Replace("'","''")));
        return personList;
    }

    public IEnumerable<CabLandPhone> FilterByLandPhone(string landPhone, IEnumerable<CabLandPhone> landPhoneList)
    {
        landPhoneList = landPhoneList.Where(p => p.LandPhoneNumber.ToLower().Contains(landPhone.ToLower()));
        return landPhoneList;
    }

    public IEnumerable<CabWhatsApp> FilterByWhatsApp(string whatsApp, IEnumerable<CabWhatsApp> whatsAppList)
    {
        whatsAppList = whatsAppList.Where(p => p.WhatsAppNumber.ToLower().Contains(whatsApp.ToLower()));
        return whatsAppList;
    }

    public IEnumerable<CabSkype> FilterBySkype(string skype, IEnumerable<CabSkype> skypeList)
    {
        skypeList = skypeList.Where(p => p.SkypeNumber.ToLower().Contains(skype.ToLower().Replace("'","''")));
        return skypeList;
    }

    public IEnumerable<CabPerson> FilterBySaveState(bool isSaved, IEnumerable<CabPerson> personList)
    {
        personList = personList.Where(p => p.IsSaved == isSaved);
        return personList;
    }
}