using System.Collections.Generic;
using System.Threading.Tasks;
using UPrinceV4.Web.Data;

namespace UPrinceV4.Web.Repositories.Interfaces;

public interface ITimeClockActivityTypeRepository
{
    Task<IEnumerable<TimeClockActivityType>> GetTimeClockActivityTypes(ApplicationDbContext applicationDbContext,
        string lang);

    Task<TimeClockActivityType> GetTimeClockActivityTypeById(ApplicationDbContext applicationDbContext, int id,
        string lang);

    Task<TimeClockActivityTypeLocalizedData> GetTimeClockActivityTypeByTypeId(
        ApplicationDbContext applicationDbContext, int typeId, string lang);

    bool DeleteTimeClockActivityType(UPrinceCustomerContex uprinceCustomerContext, int id);
}