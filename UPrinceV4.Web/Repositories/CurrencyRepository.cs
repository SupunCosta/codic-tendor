using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UPrinceV4.Web.Data;
using UPrinceV4.Web.Repositories.Interfaces;
using UPrinceV4.Web.UserException;
using UPrinceV4.Web.Util;

namespace UPrinceV4.Web.Repositories;

public class CurrencyRepository : ICurrencyRepository
{
    public async Task<int> CreateCurrency(ApplicationDbContext context, CurrencyCreateDto currencyCreateDto)
    {
        try
        {
            if (currencyCreateDto.IsDefault)
            {
                var defaultValue = context.Currency.Where(p => p.IsDefault == true);
                if (defaultValue.Count() != 0)
                {
                    defaultValue.First().IsDefault = false;
                    context.Currency.Update(defaultValue.First());
                }
            }

            var currency = new Currency { IsDefault = currencyCreateDto.IsDefault, Name = currencyCreateDto.Name };
            context.Currency.Add(currency);
            context.SaveChanges();
            return currency.Id;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    public async Task<bool> DeleteCurrency(ApplicationDbContext context, int id)
    {
        try
        {
            var currency = (from a in context.Currency
                where a.Id == id
                select a).Single();
            context.Currency.Remove(currency);
            context.SaveChanges();
            return true;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    public async Task<IEnumerable<Currency>> GetCurrencies(ApplicationDbContext context)
    {
        try
        {
            var currencies = context.Currency.ToList();
            if (!currencies.Any())
            {
                var errorMsg =
                    context.ErrorMessage.FirstOrDefault(e => e.Id == ErrorMessageKey.CurrencyEmpty.ToString());
                if (errorMsg != null) throw new EmptyListException(errorMsg.Message);
            }

            return currencies;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    public async Task<Currency> GetCurrencyById(ApplicationDbContext context, int id)
    {
        try
        {
            var currency = context.Currency.FirstOrDefault(p => p.Id == id);
            return currency;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    public async Task<int> UpdateCurrency(ApplicationDbContext context, Currency cur)
    {
        try
        {
            if (cur.IsDefault)
            {
                var defaultValue = context.Currency.Where(p => p.IsDefault == true);
                if (defaultValue.Count() != 0)
                {
                    defaultValue.First().IsDefault = false;
                    context.Currency.Update(defaultValue.First());
                }
            }

            var currency = new Currency { Id = cur.Id, IsDefault = cur.IsDefault, Name = cur.Name };
            context.Currency.Update(currency);
            context.SaveChanges();
            return currency.Id;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
}