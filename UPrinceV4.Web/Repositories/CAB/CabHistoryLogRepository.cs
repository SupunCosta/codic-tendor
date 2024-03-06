using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using UPrinceV4.Web.Data;
using UPrinceV4.Web.Data.CAB;
using UPrinceV4.Web.Repositories.Interfaces.CAB;

namespace UPrinceV4.Web.Repositories.CAB;

public class CabHistoryLogRepository : ICabHistoryLogRepository
{
    public IEnumerable<CabHistoryLog> GetCabHistoryLog(
        CabHistoryLogRepositoryParameter cabHistoryLogRepositoryParameter)
    {
        try
        {
            var historyList = cabHistoryLogRepositoryParameter.ApplicationDbContext.CabHistoryLog.ToList();
            return historyList;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    public string AddCabHistoryLog(CabHistoryLogRepositoryParameter cabHistoryLogRepositoryParameter)
    {
        try
        {
            var cabHistoryLog = new CabHistoryLog
            {
                Id = Guid.NewGuid().ToString(),
                Action = cabHistoryLogRepositoryParameter.Action,
                ChangedByUserId = cabHistoryLogRepositoryParameter.ChangedUser.Id,
                ChangedTime = DateTime.UtcNow
            };
            var jsonData = "";
            if (cabHistoryLogRepositoryParameter.CabDataDto != null)
            {
                jsonData = JsonConvert.SerializeObject(cabHistoryLogRepositoryParameter.CabDataDto,
                    Formatting.Indented,
                    new JsonSerializerSettings
                    {
                        ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                    });
                cabHistoryLog.PersonId = cabHistoryLogRepositoryParameter.CabDataDto.Person.Id;
                cabHistoryLog.CompanyId = cabHistoryLogRepositoryParameter.CabDataDto.CompanyId;
            }
            else if (cabHistoryLogRepositoryParameter.Company != null)
            {
                jsonData = JsonConvert.SerializeObject(cabHistoryLogRepositoryParameter.Company,
                    Formatting.Indented,
                    new JsonSerializerSettings
                    {
                        ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                    });
                cabHistoryLog.CompanyId = cabHistoryLogRepositoryParameter.Company.Id;
            }
            else if (cabHistoryLogRepositoryParameter.Person != null)
            {
                jsonData = JsonConvert.SerializeObject(cabHistoryLogRepositoryParameter.Person, Formatting.Indented,
                    new JsonSerializerSettings
                    {
                        ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                    });
                cabHistoryLog.PersonId = cabHistoryLogRepositoryParameter.Person.Id;
            }
            else if (cabHistoryLogRepositoryParameter.PersonCompany != null)
            {
                jsonData = JsonConvert.SerializeObject(cabHistoryLogRepositoryParameter.PersonCompany,
                    Formatting.Indented,
                    new JsonSerializerSettings
                    {
                        ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                    });
                cabHistoryLog.PersonId = cabHistoryLogRepositoryParameter.PersonCompany.PersonId;
                cabHistoryLog.CompanyId = cabHistoryLogRepositoryParameter.PersonCompany.CompanyId;
            }

            cabHistoryLog.HistoryLog = jsonData;
            var options = new DbContextOptions<ApplicationDbContext>();
            var dbContext =
                new ApplicationDbContext(options, cabHistoryLogRepositoryParameter.TenantProvider);
            dbContext.CabHistoryLog.Add(cabHistoryLog);
            dbContext.SaveChanges();
            return cabHistoryLog.Id;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.ToString());
        }
    }
}