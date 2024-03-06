using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UPrinceV4.Web.Data;
using UPrinceV4.Web.Repositories.Interfaces;
using UPrinceV4.Web.UserException;
using UPrinceV4.Web.Util;

namespace UPrinceV4.Web.Repositories;

public class CountryRepository : ICountryRepository
{
    public async Task<IEnumerable<Country>> GetCountryList(CountryRepositoryParameter countryRepositoryParameter)
    {
        try
        {
            var countryList = countryRepositoryParameter.ApplicationDbContext.Country.OrderBy(n => n.CountryName)
                .ToList();
            if (countryList.Count == 0)
            {
                var errorMsg = countryRepositoryParameter.ApplicationDbContext.ErrorMessage.FirstOrDefault(e =>
                    e.Id == ErrorMessageKey.ProjectManagementLevelEmpty.ToString());
                if (errorMsg != null) throw new EmptyListException(errorMsg.Message);
            }

            if (countryRepositoryParameter.Lang == Language.en.ToString() ||
                string.IsNullOrEmpty(countryRepositoryParameter.Lang))
                return countryList;

            foreach (var country in countryList)
            {
                var localizedData =
                    countryRepositoryParameter.ApplicationDbContext.LocalizedData.FirstOrDefault(ld =>
                        ld.LocaleCode == country.LocaleCode &&
                        ld.LanguageCode == countryRepositoryParameter.Lang);
                if (localizedData != null) country.CountryName = localizedData.Label;
            }

            return countryList;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
}