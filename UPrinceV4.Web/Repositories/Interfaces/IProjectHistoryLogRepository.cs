using System.Collections.Generic;
using UPrinceV4.Web.Data;

namespace UPrinceV4.Web.Repositories.Interfaces;

internal interface IProjectHistoryLogRepository
{
    IEnumerable<ProjectHistoryLog> GetProjectHistoryLogs(ApplicationDbContext context);
    Currency GetProjectHistoryLogById(ApplicationDbContext context, string id);

    string CreateProjectHistoryLog(ApplicationDbContext context,
        ProjectHistoryLogCreateDto projectHistoryLogCreateDto);

    string UpdateProjectHistoryLog(ApplicationDbContext context,
        ProjectHistoryLogCreateDto projectHistoryLogCreateDto);

    bool DeleteProjectHistoryLog(ApplicationDbContext context, string id);
}