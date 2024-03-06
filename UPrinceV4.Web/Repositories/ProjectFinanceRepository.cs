using System;
using System.Collections.Generic;
using System.Linq;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using UPrinceV4.Shared;
using UPrinceV4.Web.Data;
using UPrinceV4.Web.Repositories.Interfaces;

namespace UPrinceV4.Web.Repositories;

public class ProjectFinanceRepository : IProjectFinanceRepository
{
    public string CreateProjectFinance( ProjectFinanceCreateDto financedto, ITenantProvider iTenantProvider)
    {
        using var connection = iTenantProvider.orgSqlConnection();
        var sql =
            "INSERT INTO dbo.ProjectFinance ( Id ,TotalBudget ,BudgetLabour ,BudgetMaterial ,CurrencyId ,ProjectId ,Invoiced ,Paid ,CustomerBudgetSpent ,CustomerBudget ,DifferenceEstimatedCostAndTenderBudget ,DifferenceTenderAndCustomer ,ExpectedTotalProjectCost ,ExtraWork ,MinAndExtraWork ,MinWork ,TenderBudget ,ToBeInvoiced ) VALUES ( @Id ,@TotalBudget ,@BudgetLabour ,@BudgetMaterial ,@CurrencyId ,@ProjectId ,@Invoiced ,@Paid ,@CustomerBudgetSpent ,@CustomerBudget ,@DifferenceEstimatedCostAndTenderBudget ,@DifferenceTenderAndCustomer ,@ExpectedTotalProjectCost ,@ExtraWork ,@MinAndExtraWork ,@MinWork ,@TenderBudget ,@ToBeInvoiced )";
        string id = null;
        if (financedto != null)
        {
            if (financedto.BudgetMaterial != null && financedto.BudgetLabour != null &&
                financedto.TotalBudget != null && financedto.BudgetMaterial + financedto.BudgetLabour >
                financedto.TotalBudget)
                throw new Exception(
                    "Total Budget should be higher than the summation of Budget Labour and Budget Material");

            var project = new ProjectFinance
            {
                Id = Guid.NewGuid().ToString(),
                ProjectId = financedto.ProjectId,
                TotalBudget = financedto.TotalBudget,
                CurrencyId = financedto.CurrencyId,
                BudgetMaterial = financedto.BudgetMaterial,
                BudgetLabour = financedto.BudgetLabour,
                Invoiced = financedto.Invoiced,
                Paid = financedto.Paid,
                TenderBudget = financedto.TenderBudget,
                CustomerBudget = financedto.CustomerBudget,
                DifferenceTenderAndCustomer = financedto.DifferenceTenderAndCustomer,
                CustomerBudgetSpent = financedto.CustomerBudgetSpent,
                ToBeInvoiced = financedto.ToBeInvoiced,
                MinWork = financedto.MinWork,
                ExtraWork = financedto.ExtraWork,
                MinAndExtraWork = financedto.MinAndExtraWork,
                ExpectedTotalProjectCost = financedto.ExpectedTotalProjectCost,
                DifferenceEstimatedCostAndTenderBudget = financedto.DifferenceEstimatedCostAndTenderBudget
            };
            // context.ProjectFinance.Add(project);
            // context.SaveChanges();
            connection.ExecuteAsync(sql, project);
            id = project.Id;
        }

        return id;
    }

    public bool DeleteProjectFinance(ApplicationDbContext context, string id)
    {
        throw new NotImplementedException();
    }

    public Currency GetProjectFinanceById(ApplicationDbContext context, string id)
    {
        throw new NotImplementedException();
    }

    public IEnumerable<ProjectFinance> GetProjectFinances(ApplicationDbContext context)
    {
        throw new NotImplementedException();
    }

    public string UpdateProjectFinance( ProjectFinanceUpdateDto financedto, ITenantProvider iTenantProvider)
    {
        
        try
        {
            var options1 = new DbContextOptions<ProjectDefinitionDbContext>();
            var context =
                new ProjectDefinitionDbContext(options1,iTenantProvider);
            if (financedto.Id != null)
            {
                var project = context.ProjectFinance.FirstOrDefault(p => p.Id == financedto.Id);
                project.Id = financedto.Id;
                project.ProjectId = financedto.ProjectId;
                project.TotalBudget = financedto.TotalBudget;
                project.CurrencyId = financedto.CurrencyId;
                project.BudgetMaterial = financedto.BudgetMaterial;
                project.BudgetLabour = financedto.BudgetLabour;
                project.Invoiced = financedto.Invoiced;
                project.Paid = financedto.Paid;
                //project.ChangeByUserId = user.Id;
                //project.Action = HistoryState.UPDATED.ToString();
                project.TenderBudget = financedto.TenderBudget;
                project.CustomerBudget = financedto.CustomerBudget;
                project.DifferenceTenderAndCustomer = financedto.DifferenceTenderAndCustomer;
                project.CustomerBudgetSpent = financedto.CustomerBudgetSpent;
                project.ToBeInvoiced = financedto.ToBeInvoiced;
                project.MinWork = financedto.MinWork;
                project.ExtraWork = financedto.ExtraWork;
                project.MinAndExtraWork = financedto.MinAndExtraWork;
                project.ExpectedTotalProjectCost = financedto.ExpectedTotalProjectCost;
                project.DifferenceEstimatedCostAndTenderBudget = financedto.DifferenceEstimatedCostAndTenderBudget;

                context.ProjectFinance.Update(project);
                context.SaveChanges();
                return project.Id;
            }

            var financeCreateDto = new ProjectFinanceCreateDto
            {
                ProjectId = financedto.ProjectId,
                TotalBudget = financedto.TotalBudget,
                CurrencyId = financedto.CurrencyId,
                BudgetMaterial = financedto.BudgetMaterial,
                BudgetLabour = financedto.BudgetLabour,
                Invoiced = financedto.Invoiced,
                Paid = financedto.Paid,
                TenderBudget = financedto.TenderBudget,
                CustomerBudget = financedto.CustomerBudget,
                DifferenceTenderAndCustomer = financedto.DifferenceTenderAndCustomer,
                CustomerBudgetSpent = financedto.CustomerBudgetSpent,
                ToBeInvoiced = financedto.ToBeInvoiced,
                MinWork = financedto.MinWork,
                ExtraWork = financedto.ExtraWork,
                MinAndExtraWork = financedto.MinAndExtraWork,
                ExpectedTotalProjectCost = financedto.ExpectedTotalProjectCost,
                DifferenceEstimatedCostAndTenderBudget = financedto.DifferenceEstimatedCostAndTenderBudget
            };

            return CreateProjectFinance( financeCreateDto, iTenantProvider);
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }
}