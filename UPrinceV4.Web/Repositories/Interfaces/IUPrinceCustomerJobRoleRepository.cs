using System.Collections.Generic;
using System.Threading.Tasks;
using UPrinceV4.Web.Data;

namespace UPrinceV4.Web.Repositories.Interfaces;

public interface IUPrinceCustomerJobRoleRepository
{
    Task<IEnumerable<UPrinceCustomerJobRole>> GetUPrinceCustomerJobRoles(
        UPrinceCustomerContex uprinceCustomerContexContext);

    Task<UPrinceCustomerJobRole> GetUPrinceCustomerJobRoleById(UPrinceCustomerContex uprinceCustomerContexContext,
        int id);

    Task<int> AddUPrinceCustomerJobRole(UPrinceCustomerContex uprinceCustomerContexContext,
        UPrinceCustomerJobRole uprinceCustomerJobRole);

    Task<UPrinceCustomerJobRole> UpdateUPrinceCustomerJobRole(UPrinceCustomerContex uprinceCustomerContexContext,
        UPrinceCustomerJobRole uprinceCustomerJobRole);

    bool DeleteUPrinceCustomerJobRole(UPrinceCustomerContex uprinceCustomerContexContext, int id);

    public Task<IEnumerable<UprinceCustomerJobRoleHistory>> GetUPrinceCustomersHistory(
        UPrinceCustomerContex uprinceCustomerContext);

    bool DeleteUPrinceCustomerJobRoleHistory(UPrinceCustomerContex uprinceCustomerContexContext);
}