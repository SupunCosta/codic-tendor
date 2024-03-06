using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using UPrinceV4.Web.Data;
using UPrinceV4.Web.Data.TAX;
using UPrinceV4.Web.Repositories.Interfaces.PS;
using UPrinceV4.Web.Util;

namespace UPrinceV4.Web.Repositories.VAT;

public class VATRepository : IVATRepository
{
    public List<Tax> VATFilter(VATParameter parameter)
    {
        var options = new DbContextOptions<ShanukaDbContext>();
        var connectionString = ConnectionString.MapConnectionString(parameter.ContractingUnitSequenceId,
            parameter.ProjectSequenceId, parameter.TenantProvider);
        List<Tax> vatList;
        using (var context = new ShanukaDbContext(options, connectionString, parameter.TenantProvider))
        {
            vatList = context.Tax.ToList();
        }

        if (vatList == null)
        {
            var cuConnectionString =
                ConnectionString.MapConnectionString(parameter.ContractingUnitSequenceId, null,
                    parameter.TenantProvider);
            using (var context = new ShanukaDbContext(options, cuConnectionString, parameter.TenantProvider))
            {
                vatList = context.Tax.ToList();
            }
        }

        return vatList;
    }
}