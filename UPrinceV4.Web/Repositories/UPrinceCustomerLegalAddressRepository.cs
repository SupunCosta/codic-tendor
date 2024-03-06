using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UPrinceV4.Web.Data;
using UPrinceV4.Web.Repositories.Interfaces;

namespace UPrinceV4.Web.Repositories;

public class UPrinceCustomerLegalAddressRepository : IUPrinceCustomerLegalAddressRepository
{
    public async Task<int> AddUPrinceCustomerLegalAddress(UPrinceCustomerContex uprinceCustomerContext,
        UPrinceCustomerLegalAddress uprinceCustomerLegalAddress)
    {
        try
        {
            //uprinceCustomerLegalAddress.Action = HistoryState.ADDED.ToString();
            //uprinceCustomerLegalAddress.UserId = "admin";//if user available then set user from here
            uprinceCustomerContext.UprinceCustomerLegalAddress.Update(uprinceCustomerLegalAddress);
            uprinceCustomerContext.SaveChanges();
            return uprinceCustomerLegalAddress.ID;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    public bool DeleteUPrinceCustomerLegalAddress(UPrinceCustomerContex uprinceCustomerContext, int id)
    {
        try
        {
            var uprinceCustomerLegalAddress =
                (from a in uprinceCustomerContext.UprinceCustomerLegalAddress where a.ID == id select a).Single();
            //uprinceCustomerLegalAddress.Action = HistoryState.DELETED.ToString();
            //uprinceCustomerLegalAddress.UserId = "admin";//if user available then set user from here
            uprinceCustomerContext.UprinceCustomerLegalAddress.Update(uprinceCustomerLegalAddress);
            uprinceCustomerContext.SaveChanges();
            uprinceCustomerContext.UprinceCustomerLegalAddress.Remove(uprinceCustomerLegalAddress);
            uprinceCustomerContext.SaveChanges();
            return true;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    public async Task<IEnumerable<UPrinceCustomerLegalAddress>> GetUPrinceCustomerLegalAddress(
        UPrinceCustomerContex uprinceCustomerContext)
    {
        return uprinceCustomerContext.UprinceCustomerLegalAddress.ToList();
    }

    public async Task<UPrinceCustomerLegalAddress> GetUPrinceCustomerLegalAddressById(
        UPrinceCustomerContex uprinceCustomerContexContext, int id)
    {
        try
        {
            var uprinceCustomerLegalAddress = (from a in uprinceCustomerContexContext.UprinceCustomerLegalAddress
                where a.ID == id
                select a).Single();
            return uprinceCustomerLegalAddress;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    public async Task<IEnumerable<UprinceCustomerLegalAddressHistory>> GetUPrinceCustomerLegalAddressHistory(
        UPrinceCustomerContex context)
    {
        var uprinceCustomerLegalAddressHistory = context.UprinceCustomerLegalAddressHistory.ToList();
        return uprinceCustomerLegalAddressHistory;
    }

    public async Task<UPrinceCustomerLegalAddress> UpdateUPrinceCustomerLegalAddress(
        UPrinceCustomerContex uprinceCustomerContexContext, UPrinceCustomerLegalAddress uprinceCustomerLegalAddress)
    {
        throw new NotImplementedException();
    }
}