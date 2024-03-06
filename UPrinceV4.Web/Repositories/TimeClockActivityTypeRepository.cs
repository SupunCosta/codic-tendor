using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UPrinceV4.Web.Data;
using UPrinceV4.Web.Repositories.Interfaces;
using UPrinceV4.Web.Util;

namespace UPrinceV4.Web.Repositories;

public class TimeClockActivityTypeRepository : ITimeClockActivityTypeRepository
{
    public async Task<TimeClockActivityTypeLocalizedData> GetTimeClockActivityTypeByTypeId(
        ApplicationDbContext applicationDbContext, int typeId, string lang)
    {
        try
        {
            if (string.IsNullOrEmpty(lang) || lang.ToLower().Contains("en")) lang = "en";

            var timeClockActivityType =
                applicationDbContext.TimeClockActivityTypeLocalizedData.FirstOrDefault(p =>
                    p.TypeId == typeId && p.LanguageCode == lang);
            return timeClockActivityType;
        }
        catch (Exception ex)
        {
            throw ex;
        }

        ;
    }

    public async Task<IEnumerable<TimeClockActivityType>> GetTimeClockActivityTypes(
        ApplicationDbContext applicationDbContext, string lang)
    {
        try
        {
            var timeClockActivityTypes =
                applicationDbContext.TimeClockActivityType.Where(a => a.IsDeleted == false).ToList();
            if (lang == Language.en.ToString() || string.IsNullOrEmpty(lang)) return timeClockActivityTypes;

            foreach (var tcap in timeClockActivityTypes)
            {
                var localizedData = applicationDbContext.LocalizedData.FirstOrDefault(ld =>
                    ld.LocaleCode == tcap.LocaleCode && ld.LanguageCode == lang);
                if (localizedData != null)
                    tcap.Type = localizedData.Label;
                else
                    tcap.Type = null;
            }

            return timeClockActivityTypes;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    public async Task<TimeClockActivityType> GetTimeClockActivityTypeById(ApplicationDbContext applicationDbContext,
        int id, string lang)
    {
        try
        {
            var timeClockActivityType = applicationDbContext.TimeClockActivityType.FirstOrDefault(p => p.Id == id);
            if (lang == Language.en.ToString() || string.IsNullOrEmpty(lang)) return timeClockActivityType;

            var localizedData = applicationDbContext.LocalizedData.FirstOrDefault(ld =>
                ld.LocaleCode == timeClockActivityType.LocaleCode && ld.LanguageCode == lang);
            timeClockActivityType.Type = localizedData.Label;
            return timeClockActivityType;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    public bool DeleteTimeClockActivityType(UPrinceCustomerContex uprinceCustomerContext, int id)
    {
        throw new NotImplementedException();
    }
}