using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using UPrinceV4.Shared;
using UPrinceV4.Web.Data;
using UPrinceV4.Web.Data.PO;
using UPrinceV4.Web.Repositories.Interfaces;
using UPrinceV4.Web.Util;

namespace UPrinceV4.Web.Repositories;

public class WorkflowStateRepository : IWorkflowStateRepository
{
    public bool DeleteWorkflowStateType(ApplicationDbContext applicationDbContext, int id)
    {
        throw new NotImplementedException();
    }

    public async Task<WorkflowState> GetWorkflowStateById(ApplicationDbContext applicationDbContext, int id,
        string lang)
    {
        if (lang == Language.en.ToString() || string.IsNullOrEmpty(lang))
        {
            var workflowState = applicationDbContext.WorkflowState.Where(w => w.Id == id).ToList()
                .FirstOrDefault();
            return workflowState;
        }
        else
        {
            var workflowState = applicationDbContext.WorkflowState.FirstOrDefault(p => p.Id == id);
            var localizedData = applicationDbContext.LocalizedData.FirstOrDefault(ld =>
                ld.LocaleCode == workflowState.LocaleCode && ld.LanguageCode == lang);
            if (localizedData == null)
                return applicationDbContext.WorkflowState.Where(w => w.Id == id).ToList().FirstOrDefault();

            workflowState.State = localizedData.Label;
            return workflowState;
        }
    }

    public async Task<IEnumerable<WorfFlowStatusLocalizedDto>> GetWorkflowStates(ApplicationDbContext applicationDbContext,
        string lang,ITenantProvider iTenantProvider)
    {
        try
        {
            await using var connection = iTenantProvider.Connection();

            // var workflowStates = applicationDbContext.WorkflowState.Where(a => a.IsDeleted == false).ToList();
            // if (lang == Language.en.ToString() || string.IsNullOrEmpty(lang) || lang.ToLower().Contains("en"))
            //     return workflowStates;
            //
            // foreach (var ws in workflowStates)
            // {
            //     var localizedData = applicationDbContext.LocalizedData.FirstOrDefault(ld =>
            //         ld.LocaleCode == ws.LocaleCode && ld.LanguageCode == lang);
            //     ws.State = localizedData.Label;
            // }

            var workflowStates =
                connection.Query<WorfFlowStatusLocalizedDto>(
                    "SELECT StatusId AS Id, Name AS State,LanguageCode FROM dbo.WorfFlowStatusLocalizedData WHERE LanguageCode = @lang",
                    new { lang = lang });

            return workflowStates;
        }
        catch (Exception ex)
        {
            throw ex;
        }

        ;
    }
}