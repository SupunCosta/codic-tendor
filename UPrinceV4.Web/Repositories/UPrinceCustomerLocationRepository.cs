using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UPrinceV4.Web.Data;
using UPrinceV4.Web.Repositories.Interfaces;

namespace UPrinceV4.Web.Repositories;

public class UPrinceCustomerLocationRepository : IUPrinceCustomerLocationRepository
{
    public async Task<int> AddUPrinceCustomerCustomerLocation(UPrinceCustomerContex uprinceCustomerContext,
        UPrinceCustomerLocation uprinceCustomerLocation)
    {
        try
        {
            //uprinceCustomerLocation.UserId = "admin";//if user available then set user from here
            //uprinceCustomerLocation.Action = HistoryState.ADDED.ToString();
            uprinceCustomerContext.UprinceCustomerLocation.Update(uprinceCustomerLocation);
            uprinceCustomerContext.SaveChanges();
            return uprinceCustomerLocation.ID;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    public bool DeleteUPrinceCustomerLocation(UPrinceCustomerContex uprinceCustomerContext, int id)
    {
        try
        {
            var uprinceCustomerLocation =
                (from a in uprinceCustomerContext.UprinceCustomerLocation where a.ID == id select a).Single();
            //uprinceCustomerLocation.UserId = "admin";//if user available then set user from here
            //uprinceCustomerLocation.Action = HistoryState.DELETED.ToString();
            uprinceCustomerContext.UprinceCustomerLocation.Update(uprinceCustomerLocation);
            uprinceCustomerContext.SaveChanges();
            uprinceCustomerContext.UprinceCustomerLocation.Remove(uprinceCustomerLocation);
            uprinceCustomerContext.SaveChanges();
            return true;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    public async Task<IEnumerable<UprinceCustomerLocationHistory>> GetUPrinceCustomerLegalAddressHistory(
        UPrinceCustomerContex context)
    {
        var lAddresses = context.UprinceCustomerLocationHistory.ToList();
        return lAddresses;
    }

    public async Task<UPrinceCustomerLocation> GetUPrinceCustomerLocationById(
        UPrinceCustomerContex uprinceCustomerContext, int id)
    {
        try
        {
            var uprinceCustomerLocation =
                (from a in uprinceCustomerContext.UprinceCustomerLocation where a.ID == id select a).Single();
            return uprinceCustomerLocation;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    public async Task<IEnumerable<UPrinceCustomerLocation>> GetUPrinceCustomerLocations(
        UPrinceCustomerContex uprinceCustomerContext)
    {
        return uprinceCustomerContext.UprinceCustomerLocation.ToList();
    }

    public async Task<UPrinceCustomerLocation> UpdateUPrinceCustomerLocation(
        UPrinceCustomerContex uprinceCustomerContext, UPrinceCustomerLocation uprinceCustomerLocation)
    {
        throw new NotImplementedException();
    }
}