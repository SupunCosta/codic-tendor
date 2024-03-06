using System;
using System.Collections.Generic;
using UPrinceV4.Web.Data;
using UPrinceV4.Web.Repositories.Interfaces;

namespace UPrinceV4.Web.Repositories;

public class ProjectHistoryLogRepository : IProjectHistoryLogRepository
{
    public string CreateProjectHistoryLog(ApplicationDbContext context,
        ProjectHistoryLogCreateDto projectHistoryLogCreateDto)
    {
        throw new NotImplementedException();
    }

    public bool DeleteProjectHistoryLog(ApplicationDbContext context, string id)
    {
        throw new NotImplementedException();
    }

    public Currency GetProjectHistoryLogById(ApplicationDbContext context, string id)
    {
        throw new NotImplementedException();
    }

    public IEnumerable<ProjectHistoryLog> GetProjectHistoryLogs(ApplicationDbContext context)
    {
        throw new NotImplementedException();
    }

    public string UpdateProjectHistoryLog(ApplicationDbContext context,
        ProjectHistoryLogCreateDto projectHistoryLogCreateDto)
    {
        throw new NotImplementedException();
    }
}