using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using UPrinceV4.Web.Data;
using UPrinceV4.Web.Models;
using UPrinceV4.Web.Repositories.Interfaces;

namespace UPrinceV4.Web.Repositories;

public class UPrinceCustomerProfileRepository : IUPrinceCustomerProfileRepository
{
    async Task<UPrinceCustomerProfile> IUPrinceCustomerProfileRepository.AddUPrinceCustomerProfile(
        UPrinceCustomerContex uprinceCustomerContext, UPrinceCustomerProfile uprinceCustomerProfile)
    {
        try
        {
            var customer = new UPrinceCustomer();
            uprinceCustomerContext.UprinceCustomer.Update(customer);
            uprinceCustomerContext.SaveChanges();
            uprinceCustomerProfile.UprinceCustomerId = customer.Id;
            uprinceCustomerContext.UprinceCustomerProfile.Update(uprinceCustomerProfile);
            uprinceCustomerContext.SaveChanges();
            return customer.UprinceCustomerProfile;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    bool IUPrinceCustomerProfileRepository.DeleteUPrinceCustomerProfile(
        UPrinceCustomerContex uprinceCustomerContext, int id)
    {
        try
        {
            var uprinceCustomerProfile =
                (from a in uprinceCustomerContext.UprinceCustomerProfile where a.ID == id select a).Single();
            uprinceCustomerContext.UprinceCustomerProfile.Remove(uprinceCustomerProfile);
            uprinceCustomerContext.SaveChanges();
            return true;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    async Task<UPrinceCustomerProfile> IUPrinceCustomerProfileRepository.GetUPrinceCustomerProfileById(
        UPrinceCustomerContex uprinceCustomerContext, int id)
    {
        try
        {
            var profile = (from a in uprinceCustomerContext.UprinceCustomerProfile where a.ID == id select a)
                .Include(u => u.UprinceCustomerLegalAddress).Include(u => u.UprinceCustomerPrimaryContact).Single();
            return profile;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    async Task<PagedResult<T>> IUPrinceCustomerProfileRepository.GetUPrinceCustomerProfilePagedResult<T>(
        UPrinceCustomerContex uprinceCustomerContext, int pageNo)
    {
        try
        {
            var pagedResult = new PagedResult<T>();
            pagedResult.CurrentPage = pageNo;
            var numOfRecords = uprinceCustomerContext.UprinceCustomerProfile.Count();
            var pageCount = numOfRecords / pagedResult.PageSize;
            if (numOfRecords % pagedResult.PageSize != 0) pageCount += 1;

            if (pageCount < pageNo)
                throw new Exception("No more records found");
            pagedResult.PageCount = pageCount;
            var list = uprinceCustomerContext.UprinceCustomerProfile.Skip(pagedResult.PageSize * (pageNo - 1))
                .Take(pagedResult.PageSize).ToList();
            pagedResult.Results = (IList<T>)list;
            return pagedResult;
        }
        catch (Exception e)
        {
            throw e;
        }
    }

    async Task<IEnumerable<UPrinceCustomerProfile>> IUPrinceCustomerProfileRepository.GetUPrinceCustomerProfiles(
        UPrinceCustomerContex uprinceCustomerContext)
    {
        return uprinceCustomerContext.UprinceCustomerProfile.Include(u => u.UprinceCustomerLegalAddress)
            .Include(u => u.UprinceCustomerPrimaryContact).ToList();
    }

    async Task<UPrinceCustomerLocation> IUPrinceCustomerProfileRepository.UpdateUPrinceCustomerProfile(
        UPrinceCustomerContex uprinceCustomerContext, UPrinceCustomerProfile uprinceCustomerProfile)
    {
        throw new NotImplementedException();
    }
}