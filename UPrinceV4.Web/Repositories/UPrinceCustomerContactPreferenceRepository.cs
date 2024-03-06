using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UPrinceV4.Web.Data;
using UPrinceV4.Web.Repositories.Interfaces;

namespace UPrinceV4.Web.Repositories;

public class UPrinceCustomerContactPreferenceRepository : IUPrinceCustomerContactPreferenceRepository
{
    public async Task<int> AddUPrinceCustomerContactPreference(UPrinceCustomerContex uprinceCustomerContext,
        UPrinceCustomerContactPreference uprinceCustomerContactPreference)
    {
        try
        {
            var upcp =
                (from a in uprinceCustomerContext.UprinceCustomerContactPreference
                    where a.UprinceCustomerId == uprinceCustomerContactPreference.UprinceCustomerId
                    select a).Single();
            upcp.FirstName = uprinceCustomerContactPreference.FirstName;
            upcp.LastName = uprinceCustomerContactPreference.LastName;
            upcp.UprinceCustomerId = uprinceCustomerContactPreference.UprinceCustomerId;
            upcp.UPrinceCustomerJobRoleId = uprinceCustomerContactPreference.UPrinceCustomerJobRoleId;
            upcp.Email = uprinceCustomerContactPreference.Email;
            //upcp.Action = HistoryState.ADDED.ToString();
            //upcp.UserId = "admin";//if user available then set user from here
            uprinceCustomerContext.UprinceCustomerContactPreference.Update(upcp);
            uprinceCustomerContext.SaveChanges();
            return upcp.ID;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    public bool DeleteUPrinceCustomerContactPreference(UPrinceCustomerContex uprinceCustomerContext, int id)
    {
        try
        {
            var uprinceCustomerContactPreference =
                (from a in uprinceCustomerContext.UprinceCustomerContactPreference where a.ID == id select a)
                .Single();
            //uprinceCustomerContactPreference.Action = HistoryState.DELETED.ToString();
            //uprinceCustomerContactPreference.UserId = "admin"; 
            uprinceCustomerContext.UprinceCustomerContactPreference.Update(uprinceCustomerContactPreference);
            uprinceCustomerContext.SaveChanges();
            uprinceCustomerContext.UprinceCustomerContactPreference.Remove(uprinceCustomerContactPreference);
            uprinceCustomerContext.SaveChanges();
            return true;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    public async Task<UPrinceCustomerContactPreference> GetUPrinceCustomerContactPreferenceById(
        UPrinceCustomerContex uprinceCustomerContext, int id)
    {
        try
        {
            return (from a in uprinceCustomerContext.UprinceCustomerContactPreference where a.ID == id select a)
                .Single();
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    public async Task<IEnumerable<UprinceCustomerContactPreferenceHistory>>
        GetUprinceCustomerContactPreferenceHistory(UPrinceCustomerContex context)
    {
        var uprinceCustomerContactPreferenceHistory = context.UprinceCustomerContactPreferenceHistory.ToList();
        return uprinceCustomerContactPreferenceHistory;
    }

    public async Task<IEnumerable<UPrinceCustomerContactPreference>> GetUPrinceCustomerContactPreferences(
        UPrinceCustomerContex uprinceCustomerContexContext)
    {
        return uprinceCustomerContexContext.UprinceCustomerContactPreference.ToList();
    }

    public async Task<UPrinceCustomerContactPreference> UpdateUPrinceCustomerContactPreference(
        UPrinceCustomerContex uprinceCustomerContexContext,
        UPrinceCustomerContactPreference uprinceCustomerContactPreference)
    {
        throw new NotImplementedException();
    }
}