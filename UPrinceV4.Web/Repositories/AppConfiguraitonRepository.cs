using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using UPrinceV4.Shared;
using UPrinceV4.Web.Data;
using UPrinceV4.Web.Models;
using UPrinceV4.Web.Repositories.Interfaces;
using UPrinceV4.Web.Util;

namespace UPrinceV4.Web.Repositories;

public class AppConfigurationRepository : IAppConfigurationRepository
{
    public async Task<string> Configure(ApplicationDbContext context, ITenantProvider tenantProvider,
        IFormCollection csvFile)
    {
        var result = "";
        var appConfigurationData = new AppConfigurationData();
        var client = new FileClient();
        var fileName = csvFile.Files.FirstOrDefault()?.FileName;
        var url = client.PersistPhoto(fileName, tenantProvider, csvFile.Files.FirstOrDefault());
        appConfigurationData.Name = fileName;
        appConfigurationData.Url = url;
        var feedCsvData = new FeedCsvData();
        result = feedCsvData.LoadDataToDatabase(context, appConfigurationData.Url, fileName);
        appConfigurationData.IsConfigured = true;
        context.AppConfigurationData.Add(appConfigurationData);
        await context.SaveChangesAsync();

        return result;
    }
}