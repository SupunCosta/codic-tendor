using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UPrinceV4.Web.Data;
using UPrinceV4.Web.Repositories.Interfaces;

namespace UPrinceV4.Web.Repositories;

public class UPrinceCustomerJobRoleRepository : IUPrinceCustomerJobRoleRepository
{
    public async Task<int> AddUPrinceCustomerJobRole(UPrinceCustomerContex uprinceCustomerContext,
        UPrinceCustomerJobRole uprinceCustomerJobRole)
    {
        try
        {
            //uprinceCustomerJobRole.UserId = "admin";//if user available then set user from here
            //uprinceCustomerJobRole.Action = HistoryState.ADDED.ToString();
            uprinceCustomerContext.UprinceCustomerJobRole.Update(uprinceCustomerJobRole);
            uprinceCustomerContext.SaveChanges();
            return uprinceCustomerJobRole.ID;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    public bool DeleteUPrinceCustomerJobRole(UPrinceCustomerContex uprinceCustomerContext, int id)
    {
        try
        {
            var uprinceCustomerJobRole =
                (from a in uprinceCustomerContext.UprinceCustomerJobRole where a.ID == id select a).Single();
            //uprinceCustomerJobRole.UserId = "admin";//if user available then set user from here
            //uprinceCustomerJobRole.Action = HistoryState.DELETED.ToString();
            uprinceCustomerContext.UprinceCustomerJobRole.Update(uprinceCustomerJobRole);
            uprinceCustomerContext.SaveChanges();
            uprinceCustomerContext.UprinceCustomerJobRole.Remove(uprinceCustomerJobRole);
            uprinceCustomerContext.SaveChanges();
            return true;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    public async Task<UPrinceCustomerJobRole> GetUPrinceCustomerJobRoleById(
        UPrinceCustomerContex uprinceCustomerContext, int id)
    {
        try
        {
            var jobRole =
                (from a in uprinceCustomerContext.UprinceCustomerJobRole where a.ID == id select a).Single();
            return jobRole;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    public async Task<IEnumerable<UPrinceCustomerJobRole>> GetUPrinceCustomerJobRoles(
        UPrinceCustomerContex uprinceCustomerContext)
    {
        return uprinceCustomerContext.UprinceCustomerJobRole.ToList();
    }

    public async Task<UPrinceCustomerJobRole> UpdateUPrinceCustomerJobRole(
        UPrinceCustomerContex uprinceCustomerContext, UPrinceCustomerJobRole uprinceCustomerJobRole)
    {
        var jobRole =
            uprinceCustomerContext.UprinceCustomerJobRole.FirstOrDefault(j => j.ID == uprinceCustomerJobRole.ID);
        //jobRole.UserId = "admin";//if user available then set user from here
        //jobRole.Action = HistoryState.UPDATED.ToString();
        jobRole.Role = uprinceCustomerJobRole.Role;
        uprinceCustomerContext.UprinceCustomerJobRole.Update(jobRole);
        uprinceCustomerContext.SaveChanges();
        return jobRole;
    }

    public async Task<IEnumerable<UprinceCustomerJobRoleHistory>> GetUPrinceCustomersHistory(
        UPrinceCustomerContex uprinceCustomerContext)
    {
        var uprinceCustomersJobRoleHistory = uprinceCustomerContext.UprinceCustomerJobRoleHistory.ToList();
        return uprinceCustomersJobRoleHistory;
    }

    public bool DeleteUPrinceCustomerJobRoleHistory(UPrinceCustomerContex uprinceCustomerContext)
    {
        //var state = uprinceCustomerContexContext.Database.ExecuteSqlCommand("TRUNCATE TABLE ["+"UprinceCustomerJobRoleHistory"+"]");
        return true;
    }
}