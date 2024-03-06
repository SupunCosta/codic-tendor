using System.Collections.Generic;
using System.Threading.Tasks;
using UPrinceV4.Web.Data;
using UPrinceV4.Web.Models;

namespace UPrinceV4.Web.Repositories.Interfaces;

public interface IUPrinceCustomerProfileRepository
{
    Task<IEnumerable<UPrinceCustomerProfile>> GetUPrinceCustomerProfiles(
        UPrinceCustomerContex uprinceCustomerContexContext);

    Task<PagedResult<T>> GetUPrinceCustomerProfilePagedResult<T>(UPrinceCustomerContex uprinceCustomerContexContext,
        int pageNo);

    Task<UPrinceCustomerProfile> GetUPrinceCustomerProfileById(UPrinceCustomerContex uprinceCustomerContexContext,
        int id);

    Task<UPrinceCustomerProfile> AddUPrinceCustomerProfile(UPrinceCustomerContex uprinceCustomerContexContext,
        UPrinceCustomerProfile uprinceCustomerProfile);

    Task<UPrinceCustomerLocation> UpdateUPrinceCustomerProfile(UPrinceCustomerContex uprinceCustomerContexContext,
        UPrinceCustomerProfile uprinceCustomerProfile);

    bool DeleteUPrinceCustomerProfile(UPrinceCustomerContex uprinceCustomerContexContext, int id);
}