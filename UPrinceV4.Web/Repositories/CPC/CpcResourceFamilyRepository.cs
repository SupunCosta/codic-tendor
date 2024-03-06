using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Dapper;
using Microsoft.Data.SqlClient;
using UPrinceV4.Web.Data.CPC;
using UPrinceV4.Web.Repositories.Interfaces.CPC;
using UPrinceV4.Web.Util;

namespace UPrinceV4.Web.Repositories.CPC;

public class CpcResourceFamilyRepository : ICpcResourceFamilyRepository
{
    public async Task<List<DatabasesEx>> CreateCpcResourceFamily(CpcResourceFamilyParameters cpcRFParameters)
    {
        try
        {
            var exceptionLst = new List<DatabasesEx>();

            var connectionString = ConnectionString.MapConnectionString(cpcRFParameters.ContractingUnitSequenceId,
                null, cpcRFParameters.TenantProvider);

            await using var connection = new SqlConnection(connectionString);

            await using var dbconnection =
                new SqlConnection(cpcRFParameters.TenantProvider.GetTenant().ConnectionString);


            var id = connection
                .Query<string>(
                    "SELECT Id  FROM dbo.CpcResourceFamily WHERE  Id = @CpcResourceFamilyId",
                    new { cpcRFParameters.cpcResourceFamily.CpcResourceFamilyId }).FirstOrDefault();

            // var project = dbconnection
            //     .Query<ProjectDefinition>("SELECT * FROM dbo.ProjectDefinition WHERE IsDeleted = 0").ToList();
            
             var result = new List<Databases>();
            
            const string pattern = @"Initial Catalog=([^;]+)";
            var val = "master";

            var masterDb = Regex.Replace(connectionString, pattern, $"Initial Catalog={val}");
            
            using (var connection2 = new SqlConnection(masterDb))
            {
                result = connection2
                    .Query<Databases>(
                        @"select [name] as DatabaseName from sys.databases WHERE name NOT IN('master', 'MsalTokenCacheDatabase', 'UPrinceV4EinsteinCatelog', 'UPrinceV4UATCatelog') order by name")
                    .ToList();
            }
            

            if (id == null)
            {
                


                    var param = new
                    {
                        cpcRFParameters.cpcResourceFamily.Id,
                        cpcRFParameters.cpcResourceFamily.Label,
                        LanguageCode = cpcRFParameters.Lang,
                        cpcRFParameters.cpcResourceFamily.ParentId,
                        cpcRFParameters.cpcResourceFamily.CpcResourceFamilyId
                    };

                    var insert =
                        @"INSERT INTO dbo.CpcResourceFamilyLocalizedData ( Id ,Label ,LanguageCode ,CpcResourceFamilyId ,ParentId ) VALUES ( @Id ,@Label ,@LanguageCode ,@CpcResourceFamilyId ,@ParentId );";

                    foreach (var prDb in result.Select(i =>
                                 Regex.Replace(connectionString, pattern, $"Initial Catalog={i.DatabaseName}")))
                    {
                        try
                        {
                            await using var pconnection = new SqlConnection(prDb);

                            await pconnection.ExecuteAsync(
                                "INSERT INTO dbo.CpcResourceFamily ( Id ,LocaleCode ,ParentId ,DisplayOrder ,Title ) VALUES ( @CpcResourceFamilyId ,@Label ,@ParentId ,'0',@Label );",
                                param);

                            if (param.LanguageCode == "en")
                            {
                                await pconnection.ExecuteAsync(insert, param);

                                var newParam = new
                                {
                                    Id = Guid.NewGuid().ToString(),
                                    cpcRFParameters.cpcResourceFamily.Label,
                                    LanguageCode = "nl",
                                    cpcRFParameters.cpcResourceFamily.ParentId,
                                    cpcRFParameters.cpcResourceFamily.CpcResourceFamilyId
                                };
                                await pconnection.ExecuteAsync(insert, newParam);


                            }
                            else 
                            {
                                await pconnection.ExecuteAsync(insert, param);

                                var newParam = new
                                {
                                    Id = Guid.NewGuid().ToString(),
                                    cpcRFParameters.cpcResourceFamily.Label,
                                    LanguageCode = "en",
                                    cpcRFParameters.cpcResourceFamily.ParentId,
                                    cpcRFParameters.cpcResourceFamily.CpcResourceFamilyId
                                };
                                await pconnection.ExecuteAsync(insert, newParam);
                            }
                        
                        }catch (Exception ex)
                        {
                            var mDatabasesEx = new DatabasesEx();
                            mDatabasesEx.DatabaseName = prDb;
                            mDatabasesEx.Exception = ex;
                            exceptionLst.Add(mDatabasesEx);
                        }
                    }
                

            }

            else
            {
                var param1 = new
                {
                    cpcRFParameters.cpcResourceFamily.Label,
                    LanguageCode = cpcRFParameters.Lang,
                    cpcRFParameters.cpcResourceFamily.ParentId,
                    cpcRFParameters.cpcResourceFamily.CpcResourceFamilyId
                };

                var insert = @"UPDATE dbo.CpcResourceFamilyLocalizedData 
                                SET Label = @Label
                                WHERE
                                  CpcResourceFamilyId = @CpcResourceFamilyId
                                ;";

                
                foreach  (var prDb in result.Select(i => Regex.Replace(connectionString, pattern, $"Initial Catalog={i.DatabaseName}")))
                {
                    try
                    {
                        
                        await using var pconnection = new SqlConnection(prDb);

                        await pconnection.ExecuteAsync(insert, param1);
                    
                    }catch (Exception ex)
                    {
                        var mDatabasesEx = new DatabasesEx();
                        mDatabasesEx.DatabaseName = prDb;
                        mDatabasesEx.Exception = ex;
                        exceptionLst.Add(mDatabasesEx);
                    }
                }

            }

            return exceptionLst;
        }
        catch (Exception e)
        {
            //Console.WriteLine(e);
            throw;
        }
    }
}