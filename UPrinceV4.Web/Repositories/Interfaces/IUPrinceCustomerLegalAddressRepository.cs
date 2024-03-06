using System.Collections.Generic;
using System.Threading.Tasks;
using UPrinceV4.Web.Data;

namespace UPrinceV4.Web.Repositories.Interfaces;

public interface IUPrinceCustomerLegalAddressRepository
{
    Task<IEnumerable<UPrinceCustomerLegalAddress>> GetUPrinceCustomerLegalAddress(
        UPrinceCustomerContex uprinceCustomerContexContext);

    Task<UPrinceCustomerLegalAddress> GetUPrinceCustomerLegalAddressById(
        UPrinceCustomerContex uprinceCustomerContexContext, int id);

    Task<int> AddUPrinceCustomerLegalAddress(UPrinceCustomerContex uprinceCustomerContexContext,
        UPrinceCustomerLegalAddress uprinceCustomerLegalAddress);

    Task<UPrinceCustomerLegalAddress> UpdateUPrinceCustomerLegalAddress(
        UPrinceCustomerContex uprinceCustomerContexContext,
        UPrinceCustomerLegalAddress uprinceCustomerLegalAddress);

    bool DeleteUPrinceCustomerLegalAddress(UPrinceCustomerContex uprinceCustomerContexContext, int id);

    Task<IEnumerable<UprinceCustomerLegalAddressHistory>> GetUPrinceCustomerLegalAddressHistory(
        UPrinceCustomerContex context);
}