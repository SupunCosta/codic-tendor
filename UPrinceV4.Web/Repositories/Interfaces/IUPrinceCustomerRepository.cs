using System.Collections.Generic;
using System.Threading.Tasks;
using UPrinceV4.Web.Data;

namespace UPrinceV4.Web.Repositories.Interfaces;

public interface IUPrinceCustomerRepository
{
    Task<IEnumerable<UPrinceCustomer>> GetUPrinceCustomers(UPrinceCustomerContex uprinceCustomerContexContext);
    Task<UPrinceCustomer> GetUPrinceCustomerById(UPrinceCustomerContex uprinceCustomerContexContext, int id);

    Task<int> AddUPrinceCustomer(UPrinceCustomerContex uprinceCustomerContexContext,
        UPrinceCustomer uprinceCustomer);

    Task<UPrinceCustomer> UpdateUPrinceCustomer(UPrinceCustomerContex uprinceCustomerContexContext,
        UPrinceCustomer uprinceCustomer);

    bool DeleteUPrinceCustomer(UPrinceCustomerContex uprinceCustomerContexContext, int id);

    Task<IEnumerable<UprinceCustomerHistory>> GetUPrinceCustomersHistory(
        UPrinceCustomerContex uprinceCustomerContexContext);
}