using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using UPrinceV4.Web.Data;
using UPrinceV4.Web.Repositories.Interfaces;

namespace UPrinceV4.Web.Repositories;

internal class UPrinceCustomerRepository : IUPrinceCustomerRepository
{
    public async Task<int> AddUPrinceCustomer(UPrinceCustomerContex uprinceCustomerContext,
        UPrinceCustomer uprinceCustomer)
    {
        try
        {
            //uprinceCustomer.UserId = "admin";//if user available then set user from here
            //uprinceCustomer.Action = HistoryState.ADDED.ToString();
            uprinceCustomerContext.UprinceCustomer.Update(uprinceCustomer);
            uprinceCustomerContext.SaveChanges();
            return uprinceCustomer.Id;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    public bool DeleteUPrinceCustomer(UPrinceCustomerContex uprinceCustomerContext, int id)
    {
        try
        {
            var uprinceCustomer = (from a in uprinceCustomerContext.UprinceCustomer
                where a.Id == id
                select a).Single();
            //uprinceCustomer.UserId = "admin";//if user available then set user from here
            //uprinceCustomer.Action = HistoryState.DELETED.ToString();
            uprinceCustomerContext.UprinceCustomer.Update(uprinceCustomer);
            uprinceCustomerContext.SaveChanges();
            uprinceCustomerContext.UprinceCustomer.Remove(uprinceCustomer);
            uprinceCustomerContext.SaveChanges();
            return true;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }


    public async Task<UPrinceCustomer> GetUPrinceCustomerById(UPrinceCustomerContex uprinceCustomerContext, int id)
    {
        try
        {
            var uprinceCustomer = (from a in uprinceCustomerContext.UprinceCustomer where a.Id == id select a)
                .Include(u => u.UprinceCustomerContactPreference).Include(u => u.UprinceCustomerLocations)
                .Include(u => u.UprinceCustomerProfile)
                .Include(u => u.UprinceCustomerProfile.UprinceCustomerLegalAddress)
                .Include(u => u.UprinceCustomerProfile.UprinceCustomerPrimaryContact).Single();
            return uprinceCustomer;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    public async Task<IEnumerable<UPrinceCustomer>> GetUPrinceCustomers(
        UPrinceCustomerContex uprinceCustomerContext)
    {
        return uprinceCustomerContext.UprinceCustomer.Include(u => u.UprinceCustomerContactPreference)
            .Include(u => u.UprinceCustomerLocations).Include(u => u.UprinceCustomerProfile)
            .Include(u => u.UprinceCustomerProfile.UprinceCustomerLegalAddress)
            .Include(u => u.UprinceCustomerProfile.UprinceCustomerPrimaryContact).ToList();
    }

    public async Task<IEnumerable<UprinceCustomerHistory>> GetUPrinceCustomersHistory(
        UPrinceCustomerContex uprinceCustomerContext)
    {
        var uprinceCustomersHistory = uprinceCustomerContext.UprinceCustomerHistory.ToList();
        foreach (var uch in uprinceCustomersHistory)
        {
            var uprinceCustomerProfileHistory =
                uprinceCustomerContext.UprinceCustomerProfileHistory.FirstOrDefault(p =>
                    p.UprinceCustomerId == uch.Id);
            var upLegalAddress = uprinceCustomerContext.UprinceCustomerLegalAddressHistory.FirstOrDefault(a =>
                a.UprinceCustomerProfileId == uprinceCustomerProfileHistory.ID);
            if (uprinceCustomerProfileHistory != null)
            {
                uprinceCustomerProfileHistory.UprinceCustomerLegalAddressHistory = upLegalAddress;
                var upPrimaryContact =
                    uprinceCustomerContext.UprinceCustomerPrimaryContactHistory.FirstOrDefault(p =>
                        p.UprinceCustomerProfileId == uprinceCustomerProfileHistory.ID);
                uprinceCustomerProfileHistory.UprinceCustomerPrimaryContactHistory = upPrimaryContact;
                uch.UprinceCustomerProfileHistory = uprinceCustomerProfileHistory;
            }

            var uprinceCustomerContactPreferenceHistory =
                uprinceCustomerContext.UprinceCustomerContactPreferenceHistory.FirstOrDefault(p =>
                    p.UprinceCustomerId == uch.Id);
            uch.UprinceCustomerContactPreferenceHistory = uprinceCustomerContactPreferenceHistory;

            var uprinceCustomerLocationHistory = uprinceCustomerContext.UprinceCustomerLocationHistory
                .Where(p => p.UprinceCustomerId == uch.Id).ToList();
            uch.UprinceCustomerLocationHistory = uprinceCustomerLocationHistory;
        }

        return uprinceCustomersHistory;
    }

    public async Task<UPrinceCustomer> UpdateUPrinceCustomer(UPrinceCustomerContex uprinceCustomerContext,
        UPrinceCustomer uprinceCustomer)
    {
        var upc = (from a in uprinceCustomerContext.UprinceCustomer
            where a.Id == uprinceCustomer.Id
            select a).Single();
        upc.UprinceCustomerContactPreference = uprinceCustomer.UprinceCustomerContactPreference;
        upc.UprinceCustomerLocations = uprinceCustomer.UprinceCustomerLocations;
        upc.UprinceCustomerProfile = uprinceCustomer.UprinceCustomerProfile;
        //upc.UserId = "admin";//if user available then set user from here
        //upc.Action = HistoryState.UPDATED.ToString();
        uprinceCustomerContext.UprinceCustomer.Update(upc);
        uprinceCustomerContext.SaveChanges();
        return upc;
    }
}