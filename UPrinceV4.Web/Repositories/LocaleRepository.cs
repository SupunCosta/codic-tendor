using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using UPrinceV4.Shared;
using UPrinceV4.Web.Data;
using UPrinceV4.Web.Models;
using UPrinceV4.Web.Repositories.Interfaces;

namespace UPrinceV4.Web.Repositories;

public class LocaleRepository : ILocaleRepository
{
    public async Task<string> CreateLocale(ApplicationDbContext context, ITenantProvider tenantProvider,
        IFormCollection locale)
    {
        try
        {
            var localeName = new LocaleName();
            localeName.Id = Guid.NewGuid().ToString();

            var client = new FileClient();
            var url = client.PersistPhoto(localeName.Id, tenantProvider, locale.Files.First());

            localeName.Country = locale["Country"];
            localeName.Icon = url;
            localeName.Language = locale["Language"];
            localeName.Locale = locale["Locale"];
            localeName.TenantId = int.Parse(locale["TenantId"]);
            context.Locales.Add(localeName);
            context.SaveChanges();
            return localeName.Id;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    public async Task<bool> DeleteLocale(ApplicationDbContext context, string id)
    {
        try
        {
            var locale = (from a in context.Locales
                where a.Id == id
                select a).Single();
            context.Locales.Remove(locale);
            context?.SaveChanges();
            return true;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    public async Task<IEnumerable<LocaleName>> GetLocale(ApplicationDbContext context)
    {
        return context.Locales.ToList();
    }

    public async Task<LocaleName> GetLocaleById(ApplicationDbContext context, string id)
    {
        try
        {
            return context.Locales.First(l => l.Id == id);
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    public async Task<string> UpdateLocale(ApplicationDbContext context, ITenantProvider tenantProvider,
        LocaleNameUpdateDto locale)
    {
        var localeName = new LocaleName();
        try
        {
            localeName.Id = locale.Id;

            var client = new FileClient();
            var url = client.PersistPhoto(localeName.Id, tenantProvider, locale.Icon);

            localeName.Country = locale.Country;
            localeName.Icon = url;
            localeName.Language = locale.Language;
            localeName.Locale = locale.Locale;
            localeName.TenantId = locale.TenantId;
            context.Locales.Add(localeName);
            context.SaveChanges();
            return localeName.Id;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
}