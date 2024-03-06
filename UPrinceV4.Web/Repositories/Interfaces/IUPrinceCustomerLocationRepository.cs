using System.Collections.Generic;
using System.Threading.Tasks;
using UPrinceV4.Web.Data;

namespace UPrinceV4.Web.Repositories.Interfaces;

public interface IUPrinceCustomerLocationRepository
{
    Task<IEnumerable<UPrinceCustomerLocation>> GetUPrinceCustomerLocations(
        UPrinceCustomerContex uprinceCustomerContexContext);

    Task<UPrinceCustomerLocation> GetUPrinceCustomerLocationById(UPrinceCustomerContex uprinceCustomerContexContext,
        int id);

    Task<int> AddUPrinceCustomerCustomerLocation(UPrinceCustomerContex uprinceCustomerContexContext,
        UPrinceCustomerLocation uprinceCustomerLocation);

    Task<UPrinceCustomerLocation> UpdateUPrinceCustomerLocation(UPrinceCustomerContex uprinceCustomerContexContext,
        UPrinceCustomerLocation uprinceCustomerLocation);

    bool DeleteUPrinceCustomerLocation(UPrinceCustomerContex uprinceCustomerContexContext, int id);

    Task<IEnumerable<UprinceCustomerLocationHistory>> GetUPrinceCustomerLegalAddressHistory(
        UPrinceCustomerContex context);
}