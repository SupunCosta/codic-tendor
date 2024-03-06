using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UPrinceV4.Web.Data;
using UPrinceV4.Web.Repositories.Interfaces;

namespace UPrinceV4.Web.Repositories;

public class UPrinceCustomerPrimaryContactRepository : IUPrinceCustomerPrimaryContactRepository
{
    public async Task<int> AddUPrinceCustomerPrimaryContact(UPrinceCustomerContex uprinceCustomerContext,
        UPrinceCustomerPrimaryContact uprinceCustomerPrimaryContact)
    {
        try
        {
            uprinceCustomerContext.UprinceCustomerPrimaryContact.Update(uprinceCustomerPrimaryContact);
            uprinceCustomerContext.SaveChanges();
            return uprinceCustomerPrimaryContact.ID;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    public bool DeleteUPrinceCustomerPrimaryContact(UPrinceCustomerContex uprinceCustomerContext, int id)
    {
        try
        {
            var uprinceCustomerPrimaryContact =
                (from a in uprinceCustomerContext.UprinceCustomerPrimaryContact where a.ID == id select a).Single();
            uprinceCustomerContext.UprinceCustomerPrimaryContact.Remove(uprinceCustomerPrimaryContact);
            uprinceCustomerContext.SaveChanges();
            return true;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    public async Task<UPrinceCustomerPrimaryContact> GetUPrinceCustomerPrimaryContactById(
        UPrinceCustomerContex uprinceCustomerContext, int id)
    {
        try
        {
            var primaryContact =
                (from a in uprinceCustomerContext.UprinceCustomerPrimaryContact where a.ID == id select a).Single();
            return primaryContact;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    public async Task<IEnumerable<UPrinceCustomerPrimaryContact>> GetUPrinceCustomerPrimaryContacts(
        UPrinceCustomerContex uprinceCustomerContext)
    {
        return uprinceCustomerContext.UprinceCustomerPrimaryContact.ToList();
    }

    public async Task<UPrinceCustomerPrimaryContact> UpdateUPrinceCustomerPrimaryContact(
        UPrinceCustomerContex uprinceCustomerContexContext,
        UPrinceCustomerPrimaryContact uprinceCustomerPrimaryContact)
    {
        throw new NotImplementedException();
    }
}