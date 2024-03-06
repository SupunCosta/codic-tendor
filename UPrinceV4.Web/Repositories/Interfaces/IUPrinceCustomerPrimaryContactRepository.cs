using System.Collections.Generic;
using System.Threading.Tasks;
using UPrinceV4.Web.Data;

namespace UPrinceV4.Web.Repositories.Interfaces;

public interface IUPrinceCustomerPrimaryContactRepository
{
    Task<IEnumerable<UPrinceCustomerPrimaryContact>> GetUPrinceCustomerPrimaryContacts(
        UPrinceCustomerContex uprinceCustomerContexContext);

    Task<UPrinceCustomerPrimaryContact> GetUPrinceCustomerPrimaryContactById(
        UPrinceCustomerContex uprinceCustomerContexContext, int id);

    Task<int> AddUPrinceCustomerPrimaryContact(UPrinceCustomerContex uprinceCustomerContexContext,
        UPrinceCustomerPrimaryContact uprinceCustomerPrimaryContact);

    Task<UPrinceCustomerPrimaryContact> UpdateUPrinceCustomerPrimaryContact(
        UPrinceCustomerContex uprinceCustomerContexContext,
        UPrinceCustomerPrimaryContact uprinceCustomerPrimaryContact);

    bool DeleteUPrinceCustomerPrimaryContact(UPrinceCustomerContex uprinceCustomerContexContext, int id);
}