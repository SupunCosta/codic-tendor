using System.Collections.Generic;
using UPrinceV4.Shared;
using UPrinceV4.Web.Data;

namespace UPrinceV4.Web.Repositories.Interfaces;

public interface IProjectFinanceRepository
{
    IEnumerable<ProjectFinance> GetProjectFinances(ApplicationDbContext context);
    Currency GetProjectFinanceById(ApplicationDbContext context, string id);

    string CreateProjectFinance( ProjectFinanceCreateDto projectFinanceCreateDto, ITenantProvider iTenantProvider);

    string UpdateProjectFinance( ProjectFinanceUpdateDto projectFinanceCreateDto, ITenantProvider iTenantProvider);

    bool DeleteProjectFinance(ApplicationDbContext context, string id);
}