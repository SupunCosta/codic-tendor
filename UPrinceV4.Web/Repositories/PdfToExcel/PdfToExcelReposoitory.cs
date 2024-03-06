using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Azure;
using Azure.AI.FormRecognizer.DocumentAnalysis;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using ServiceStack;
using UPrinceV4.Web.Data.PdfToExcel;
using UPrinceV4.Web.Repositories.Interfaces;
using UPrinceV4.Web.Util;

namespace UPrinceV4.Web.Repositories.PdfToExcel;

public class PdfToExcelReposoitory : IPdfToExcelRepository
{
    public async Task<AnalyzeResult> PdfToExcel(PdfToExcelParameter PdfToExcelParameter)
    {
        //var license = new License();
        try
        {
            var dbConnection = new SqlConnection(PdfToExcelParameter.TenantProvider.GetTenant().ConnectionString);
            var endpoint = "https://uprincepdfread.cognitiveservices.azure.com/";
            var apiKey = "f3b8e73c4f244de4a079c25ce691b01c";
            var credential = new AzureKeyCredential(apiKey);
            var client = new DocumentAnalysisClient(new Uri(endpoint), credential);

            // Uri fileUri = new Uri("https://raw.githubusercontent.com/Azure-Samples/cognitive-services-REST-api-samples/master/curl/form-recognizer/sample-layout.pdf");

            //Uri fileUri = new Uri("https://uprincev4dev.blob.core.windows.net/uprincev4dev/PO%20Documents/2/8/2022%206%3A25%3A29%20AMtestttt.pdf");
            var fileUri =
                new Uri(
                    PdfToExcelParameter.URL);

            var wait = new WaitUntil();


            var operation =
                await client.AnalyzeDocumentFromUriAsync(WaitUntil.Completed, "prebuilt-layout", fileUri);


            await operation.WaitForCompletionAsync();

            AnalyzeResult result;
            result = operation.Value;

            var hg = result.Tables.ToList();

            var cells = new List<DocumentTableCell>();

            var tableData = new List<tableData>();


            // for (int i = 0; i < result.Tables.Count; i++)
            // {
            //     DocumentTable table = result.Tables[i];
            //     Console.WriteLine($"  Table {i} has {table.RowCount} rows and {table.ColumnCount} columns.");
            //     cells.AddRange(table.Cells);
            //
            //     if (i == getStartTable(PdfToExcelParameter.PdfType))
            //     {
            //         foreach (DocumentTableCell cell in table.Cells)
            //         {
            //             Console.WriteLine(
            //                 $"    Cell ({cell.RowIndex}, {cell.ColumnIndex}) has kind '{cell.Kind}' and content: '{cell.Content}'.");
            //             switch (PdfToExcelParameter.PdfType)
            //             {
            //                 case "EQUANS":
            //                 {
            //                     if (cell.RowIndex >= 4 && cell.Content.Any())
            //                     {
            //                         ToTable(table, cell, tableData);
            //                     }
            //
            //                     break;
            //                 }
            //                 case "Hvac":
            //                 {
            //                     if (cell.RowIndex >= 2 && cell.Content.Any())
            //                     {
            //                         ToTableHvac(table, cell, tableData);
            //                     }
            //
            //                     break;
            //                 }
            //                 case "Pepsico":
            //                 {
            //                     if (cell.RowIndex >= 0 && cell.Content.Any())
            //                     {
            //                         ToTablePepsico(table, cell, tableData, 0, PdfToExcelParameter);
            //                     }
            //
            //                     break;
            //                 }
            //             }
            //         }
            //     }
            //     else if (i > getStartTable(PdfToExcelParameter.PdfType))
            //     {
            //         foreach (DocumentTableCell cell in table.Cells)
            //         {
            //             Console.WriteLine(
            //                 $"    Cell ({cell.RowIndex}, {cell.ColumnIndex}) has kind '{cell.Kind}' and content: '{cell.Content}'.");
            //
            //             if (PdfToExcelParameter.PdfType == "EQUANS")
            //             {
            //                 if (cell.RowIndex >= 0 && cell.Content.Any())
            //                 {
            //                     ToTable(table, cell, tableData);
            //                 }
            //             }
            //             else if (PdfToExcelParameter.PdfType == "Hvac")
            //             {
            //                 if (cell.RowIndex >= 0 && cell.Content.Any())
            //                 {
            //                     ToTableHvac(table, cell, tableData);
            //                 }
            //             }
            //             else if (PdfToExcelParameter.PdfType == "Pepsico")
            //             {
            //                 if (cell.RowIndex >= 1 && cell.Content.Any())
            //                 {
            //                     ToTablePepsico(table, cell, tableData, 0, PdfToExcelParameter);
            //                 }
            //             }
            //         }
            //     }
            // }
            //
            //
            // // var data =   result.Tables.Where(x => x.Cells.Any(c => c.BoundingRegions.Any(v => v.PageNumber == 1)));
            // //
            // // var bb = data.Where(c=>c.Cells.Any(c => c.Content == "Post nr"));
            //
            // //return cells.Where(v => v.ColumnIndex == 0);
            // var data = tableData.DistinctBy(d => d.RowIndex).ToList();
            // foreach (var pdfData in data)
            // {
            //     var sql =
            //         "INSERT INTO dbo.ContractorPdfData ( Id ,ArticleNo ,CompanyId ,Title ,Unit ,VH ,Quantity ,UnitPrice ,TotalPrice,LotId,CreatedDate ) VALUES ( @Id ,@ArticleNo ,@CompanyId ,@Title ,@Unit ,@VH ,@Quantity ,@UnitPrice ,@TotalPrice, @LotId, @CreatedDate )";
            //
            //     var param = new
            //     {
            //         Id = Guid.NewGuid().ToString(),
            //         ArticleNo = pdfData.ArticleNo,
            //         CompanyId = pdfData.CompanyId,
            //         Title = pdfData.Title,
            //         Unit = pdfData.Unit,
            //         VH = pdfData.VH,
            //         Quantity = pdfData.Quantity,
            //         UnitPrice = pdfData.UnitPrice,
            //         TotalPrice = pdfData.TotalPrice,
            //         LotId = pdfData.LotId,
            //         CreatedDate = pdfData.CreatedDate
            //     };
            //
            //     await dbConnection.ExecuteAsync(sql, param);
            // }

            return result;
        }
        catch (Exception e)
        {
            throw e;
        }
    }

    public async Task<ContractorPdfDataResults> PdfToExcelTest(PdfToExcelParameter pdfToExcelParameter)
    {
        // var license = new License();
        try
        {
            var dataResult = new ContractorPdfDataResults();
            var dbConnection = new SqlConnection(pdfToExcelParameter.TenantProvider.GetTenant().ConnectionString);


            var connectionString = ConnectionString.MapConnectionString(pdfToExcelParameter.ContractingUnitSequenceId,
                pdfToExcelParameter.ProjectSequenceId, pdfToExcelParameter.TenantProvider);
            var connection = new SqlConnection(connectionString);

            var endpoint = "https://uprincepdfread.cognitiveservices.azure.com/";
            var apiKey = "f3b8e73c4f244de4a079c25ce691b01c";
            var credential = new AzureKeyCredential(apiKey);
            var client = new DocumentAnalysisClient(new Uri(endpoint), credential);

            // Uri fileUri = new Uri("https://raw.githubusercontent.com/Azure-Samples/cognitive-services-REST-api-samples/master/curl/form-recognizer/sample-layout.pdf");

            //Uri fileUri = new Uri("https://uprincev4dev.blob.core.windows.net/uprincev4dev/PO%20Documents/2/8/2022%206%3A25%3A29%20AMtestttt.pdf");
            var fileUri =
                new Uri(
                    pdfToExcelParameter.URL);

            var wait = new WaitUntil();

            // var operation =
            //     await client.StartAnalyzeDocumentFromUriAsync("prebuilt-document", fileUri);

            var operation =
                await client.AnalyzeDocumentFromUriAsync(WaitUntil.Completed, "prebuilt-layout", fileUri);

            // var operation =
            //     await client.StartAnalyzeDocumentFromUriAsync("prebuilt-document", fileUri);


            await operation.WaitForCompletionAsync();

            AnalyzeResult result;
            result = operation.Value;

            var cells = new List<DocumentTableCell>();

            var tableData = new List<tableData>();


            var firstRowIndex = 0;
            var firstTableIndex = 0;
            var firstColumnIndex = 0;
            var maxColumn = 0;

            var isFind = false;

            var articleList = dbConnection.Query<string>("SELECT ProductId FROM dbo.PbsProduct").ToList();

            if (pdfToExcelParameter.ContractorId == null)
                pdfToExcelParameter.ContractorId = dbConnection.Query<string>(
                    "Select CompanyId From CabPersonCompany Where Oid = @Oid",
                    new { Oid = pdfToExcelParameter.UserId }).FirstOrDefault();

            for (var i = 0; i < result.Tables.Count; i++)
            {
                var table = result.Tables[i];
                Console.WriteLine($"  Table {i} has {table.RowCount} rows and {table.ColumnCount} columns.");


                foreach (var cell in table.Cells)
                {
                    var cellContent = cell.Content;
                    if (!cellContent.IsNullOrEmpty())
                        if (cellContent.Last() == '.')
                            cellContent = cellContent.Remove(cellContent.Length - 1);

                    if (articleList.Contains(cellContent))
                    {
                        firstRowIndex = cell.RowIndex;
                        firstTableIndex = i;
                        firstColumnIndex = cell.ColumnIndex;
                        isFind = true;

                        maxColumn = result.Tables[i].Cells.MaxBy(t => t.ColumnIndex).ColumnIndex;

                        break;
                    }
                }

                if (isFind) break;
            }


            for (var i = 0; i < result.Tables.Count; i++)
            {
                var table = result.Tables[i];
                Console.WriteLine($"  Table {i} has {table.RowCount} rows and {table.ColumnCount} columns.");
                cells.AddRange(table.Cells);

                if (i == firstTableIndex)
                    foreach (var cell in table.Cells)
                    {
                        Console.WriteLine(
                            $"    Cell ({cell.RowIndex}, {cell.ColumnIndex}) has kind '{cell.Kind}' and content: '{cell.Content}'.");

                        if (cell.RowIndex >= firstRowIndex && cell.Content.Any())
                        {
                            if (cell.RowIndex == 6)
                                ToTablePepsico(table, cell, tableData, firstColumnIndex, pdfToExcelParameter,
                                    maxColumn);
                            else
                                ToTablePepsico(table, cell, tableData, firstColumnIndex, pdfToExcelParameter,
                                    maxColumn);
                        }
                    }
                else if (i > firstTableIndex)
                    foreach (var cell in table.Cells)
                    {
                        Console.WriteLine(
                            $"    Cell ({cell.RowIndex}, {cell.ColumnIndex}) has kind '{cell.Kind}' and content: '{cell.Content}'.");


                        if (cell.RowIndex >= 0 && cell.Content.Any())
                            ToTablePepsico(table, cell, tableData, firstColumnIndex, pdfToExcelParameter, maxColumn);
                    }
            }


            // var data =   result.Tables.Where(x => x.Cells.Any(c => c.BoundingRegions.Any(v => v.PageNumber == 1)));
            //
            // var bb = data.Where(c=>c.Cells.Any(c => c.Content == "Post nr"));

            //return cells.Where(v => v.ColumnIndex == 0);
            var data = tableData.DistinctBy(d => d.RowIndex).ToList();

            if (pdfToExcelParameter.lotId != null && pdfToExcelParameter.ContractorId != null)
            {
                await connection.ExecuteAsync(
                    " Delete From dbo.ContractorPdfData Where LotId =@LotId AND CompanyId = @CompanyId",
                    new { CompanyId = pdfToExcelParameter.ContractorId, LotId = pdfToExcelParameter.lotId });

                await connection.ExecuteAsync(
                    " Delete From dbo.ContractorUploadedFiles Where LotId =@LotId AND CompanyId = @CompanyId",
                    new { CompanyId = pdfToExcelParameter.ContractorId, LotId = pdfToExcelParameter.lotId });
                await connection.ExecuteAsync(
                    " Delete From dbo.ContractorPdfOrginalData Where LotId =@LotId AND CompanyId = @CompanyId",
                    new { CompanyId = pdfToExcelParameter.ContractorId, LotId = pdfToExcelParameter.lotId });
                // " Delete From dbo.ContractorPdfData",
                // new {CompanyId = pdfToExcelParameter.ContractorId, LotId = pdfToExcelParameter.lotId});
            }

            var lotRowNumber = 0;

            foreach (var pdfData in data)
            {
                if (!pdfData.UnitPrice.IsNullOrEmpty())
                {
                    var tt = Regex.Matches(pdfData.UnitPrice, "[0-9],").Count;
                    if (tt > 1) pdfData.UnitPrice = null;
                }

                if (!pdfData.TotalPrice.IsNullOrEmpty())
                {
                    var yy = Regex.Matches(pdfData.TotalPrice, "[0-9],").Count;
                    if (yy > 1) pdfData.TotalPrice = null;
                }

                var sql =
                    "INSERT INTO dbo.ContractorPdfData ( Id ,ArticleNo ,CompanyId ,Title ,Unit ,VH ,Quantity ,UnitPrice ,TotalPrice,LotId,CreatedDate ,PageRowColumn ,PageRow ,LotRowNumber ) VALUES ( @Id ,@ArticleNo ,@CompanyId ,@Title ,@Unit ,@VH ,@Quantity ,@UnitPrice ,@TotalPrice, @LotId, @CreatedDate, @PageRowColumn, @PageRow ,@LotRowNumber)";

                // var sql2 =
                //     "INSERT INTO dbo.ContractorPdfOrginalData ( Id ,ArticleNo ,CompanyId ,Title ,Unit ,VH ,Quantity ,UnitPrice ,TotalPrice,LotId,CreatedDate ,PageRowColumn ,PageRow ,LotRowNumber ) VALUES ( @Id ,@ArticleNo ,@CompanyId ,@Title ,@Unit ,@VH ,@Quantity ,@UnitPrice ,@TotalPrice, @LotId, @CreatedDate, @PageRowColumn, @PageRow ,@LotRowNumber)";


                if (!pdfData.UnitPrice.IsNullOrEmpty()) pdfData.UnitPrice = Regex.Replace(pdfData.UnitPrice, "[.]", "");
                if (!pdfData.TotalPrice.IsNullOrEmpty())
                    pdfData.TotalPrice = Regex.Replace(pdfData.TotalPrice, "[.]", "");
                if (pdfData.UnitPrice.IsNullOrEmpty() || Regex.IsMatch(pdfData.UnitPrice, "[a-zA-z-]"))
                {
                    pdfData.UnitPrice = null;
                }
                else
                {
                    pdfData.UnitPrice = Regex.Replace(pdfData.UnitPrice, ",", ".");
                    pdfData.UnitPrice = Regex.Replace(pdfData.UnitPrice, " ", "");
                }

                if (pdfData.TotalPrice.IsNullOrEmpty() || Regex.IsMatch(pdfData.TotalPrice, "[a-zA-z-]"))
                {
                    pdfData.TotalPrice = null;
                }
                else
                {
                    pdfData.TotalPrice = Regex.Replace(pdfData.TotalPrice, ",", ".");
                    pdfData.TotalPrice = Regex.Replace(pdfData.TotalPrice, " ", "");
                }

                var param = new
                {
                    Id = Guid.NewGuid().ToString(),
                    pdfData.ArticleNo,
                    pdfData.CompanyId,
                    pdfData.Title,
                    pdfData.Unit,
                    pdfData.VH,
                    pdfData.Quantity,
                    UnitPrice = pdfData.UnitPrice?.ToFloat(),
                    TotalPrice = pdfData.TotalPrice?.ToFloat(),
                    pdfData.LotId,
                    pdfData.CreatedDate,
                    pdfData.PageRowColumn,
                    pdfData.PageRow,
                    LotRowNumber = lotRowNumber.ToString().PadLeft(4, '0')
                };

                await connection.ExecuteAsync(sql, param);
                // await connection.ExecuteAsync(sql2, param);


                lotRowNumber++;
            }

            // var rowDataSql =
            //     "INSERT INTO dbo.ContractorPdfRowData ( Id ,Result ,LotId ,ContractorId ,CreatedDate ) VALUES ( @Id ,@Result ,@LotId ,@ContractorId ,@CreatedDate )";
            //
            // var jsonResult = JsonConvert.SerializeObject(result, Formatting.Indented,
            //     new JsonSerializerSettings
            //     {
            //         ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            //     });
            //
            // var sqlParam = new
            // {
            //     Id = Guid.NewGuid().ToString(),
            //     Result = jsonResult,
            //     LotId = pdfToExcelParameter.lotId,
            //     ContractorId = pdfToExcelParameter.ContractorId,
            //     CreatedDate = DateTime.UtcNow
            // }; 
            //
            // await connection.ExecuteAsync(rowDataSql, sqlParam);

            var filesSql =
                "INSERT INTO dbo.ContractorUploadedFiles ( Id ,CompanyId ,LotId ,FileType ) VALUES ( @Id ,@CompanyId ,@LotId ,@FileType )";

            var filesParam = new
            {
                Id = Guid.NewGuid().ToString(),
                CompanyId = pdfToExcelParameter.ContractorId,
                LotId = pdfToExcelParameter.lotId,
                FileType = "pdf"
            };

            await connection.ExecuteAsync(filesSql, filesParam);


            var companyName = dbConnection.Query<string>("SELECT name FROM CabCompany WHERE Id = @Id",
                new { Id = pdfToExcelParameter.ContractorId }).FirstOrDefault();

            // var uploadedItem = await pdfToExcelParameter.GraphServiceClient
            //     .Drive
            //     .Root
            //     //.ItemWithPath("lot/"+ContractorParameter.File.FileName)
            //     .ItemWithPath(pdfToExcelParameter.lotId + "/" + companyName + "/" + pdfToExcelParameter.File.FileName)
            //     .Content
            //     .Request()
            //     .PutAsync<DriveItem>(pdfToExcelParameter.File.OpenReadStream());

            dataResult.ContractorPdfData = data;
            //dataResult.uploadedData = uploadedItem;

            return dataResult;
        }
        catch (Exception e)
        {
            throw e;
        }
    }

    public async Task<List<GetContractorPdfData>> GetContractorPdfData(PdfToExcelParameter PdfToExcelParameter)
    {
        var connectionString = ConnectionString.MapConnectionString(PdfToExcelParameter.ContractingUnitSequenceId,
            PdfToExcelParameter.ProjectSequenceId, PdfToExcelParameter.TenantProvider);
        var dbConnection = new SqlConnection(connectionString);
        var tenetConnection = new SqlConnection(PdfToExcelParameter.TenantProvider.GetTenant().ConnectionString);

        List<GetContractorPdfData> data;

        var companyId = tenetConnection.Query<string>("Select CompanyId From CabPersonCompany Where Oid = @Oid",
            new { Oid = PdfToExcelParameter.UserId }).FirstOrDefault();

        if (companyId == PdfToExcelParameter.Configuration.GetValue<string>("CompanyId"))
            // data =  dbConnection.Query<ContractorPdfData>(@"SELECT
            // ContractorPdfData.Id
            //     ,ContractorPdfData.ArticleNo
            //     ,ContractorPdfData.CompanyId
            //     ,ContractorPdfData.Title
            //     ,ContractorPdfData.Unit
            //     ,ContractorPdfData.VH
            //     ,ContractorPdfData.Quantity
            //     ,ContractorPdfData.UnitPrice
            //     ,ContractorPdfData.TotalPrice
            //     FROM dbo.ContractorPdfData
            //     WHERE LEN(ContractorPdfData.Unit) < 3
            // AND ISNUMERIC(ContractorPdfData.Unit) = 0 AND ContractorPdfData.LotId = @LotId 
            // ORDER BY ContractorPdfData.ArticleNo DESC",new {LotId = PdfToExcelParameter.Id}).ToList();
            data = dbConnection.Query<GetContractorPdfData>(@"SELECT
            PublishedContractorsPdfData.Id
                ,PublishedContractorsPdfData.ArticleNo
                ,PublishedContractorsPdfData.CompanyId
                ,PublishedContractorsPdfData.Title
                ,PublishedContractorsPdfData.Unit
                ,PublishedContractorsPdfData.VH
                ,PublishedContractorsPdfData.Quantity
                ,PublishedContractorsPdfData.UnitPrice
                ,PublishedContractorsPdfData.TotalPrice,
                PublishedContractorsPdfData.PageRowColumn + 0 As PageRowColumn,
                PublishedContractorsPdfData.PageRow,
                PublishedContractorsPdfData.LotRowNumber,
                PublishedContractorsPdfData.CreatedDate,
                PublishedContractorsPdfData.LotId,
                PublishedContractorsPdfData.Key1,
                PublishedContractorsPdfData.Value1,
                PublishedContractorsPdfData.Key2,
                PublishedContractorsPdfData.Value2,
                PublishedContractorsPdfData.Key3,
                PublishedContractorsPdfData.Value3,
                PublishedContractorsPdfData.Key4,
                PublishedContractorsPdfData.Value4,
                PublishedContractorsPdfData.Key5,
                PublishedContractorsPdfData.Value5,
                PublishedContractorsPdfData.MeasurementCode,
                PublishedContractorsPdfData.RealArticleNo
                FROM dbo.PublishedContractorsPdfData
                WHERE  PublishedContractorsPdfData.LotId = @LotId
            ORDER BY PublishedContractorsPdfData.LotRowNumber ASC", new { LotId = PdfToExcelParameter.Id }).ToList();
        // var pdfData =  data.GroupBy(c => c.CompanyId);
        // var result = new List<ContractorPdfDataResults>();
        // foreach (var item in pdfData)
        // {
        //     var tt = new ContractorPdfDataResults();
        //     tt.CompanyId = item.Key;
        //     tt.ContractorPdfData = item.ToList();
        //     
        //     result.Add(tt);
        // }
        else
            data = await GetContractorPdfDataFilterContractor(PdfToExcelParameter);

        //return result;
        return data;
    }

    public async Task<List<GetContractorPdfData>> GetContractorPdfDataFilterContractor(
        PdfToExcelParameter PdfToExcelParameter)
    {
        var connectionString = ConnectionString.MapConnectionString(PdfToExcelParameter.ContractingUnitSequenceId,
            PdfToExcelParameter.ProjectSequenceId, PdfToExcelParameter.TenantProvider);
        var dbConnection = new SqlConnection(connectionString);
        var tenetConnection = new SqlConnection(PdfToExcelParameter.TenantProvider.GetTenant().ConnectionString);


        List<GetContractorPdfData> data = null;

        var companyId = tenetConnection.Query<string>("Select CompanyId From CabPersonCompany Where Oid = @Oid",
            new { Oid = PdfToExcelParameter.UserId }).FirstOrDefault();

        if (companyId != null)
            // data =  dbConnection.Query<ContractorPdfData>(@"SELECT
            // ContractorPdfData.Id
            //     ,ContractorPdfData.ArticleNo
            //     ,ContractorPdfData.CompanyId
            //     ,ContractorPdfData.Title
            //     ,ContractorPdfData.Unit
            //     ,ContractorPdfData.VH
            //     ,ContractorPdfData.Quantity
            //     ,ContractorPdfData.UnitPrice
            //     ,ContractorPdfData.TotalPrice
            //     FROM dbo.ContractorPdfData
            //     WHERE LEN(ContractorPdfData.Unit) < 3
            // AND ISNUMERIC(ContractorPdfData.Unit) = 0 AND ContractorPdfData.LotId = @LotId 
            // ORDER BY ContractorPdfData.ArticleNo DESC",new {LotId = PdfToExcelParameter.Id}).ToList();
            data = dbConnection.Query<GetContractorPdfData>(@"SELECT
            ContractorPdfData.Id
                ,ContractorPdfData.ArticleNo
                ,ContractorPdfData.CompanyId
                ,ContractorPdfData.Title
                ,ContractorPdfData.Unit
                ,ContractorPdfData.VH
                ,ContractorPdfData.Quantity
                ,ContractorPdfData.UnitPrice
                ,ContractorPdfData.TotalPrice,
                ContractorPdfData.PageRowColumn + 0 As PageRowColumn,
                ContractorPdfData.PageRow,
                ContractorPdfData.LotRowNumber,
                ContractorPdfData.CreatedDate,
                ContractorPdfData.LotId,
                ContractorPdfData.Key1,
                ContractorPdfData.Value1,
                ContractorPdfData.Key2,
                ContractorPdfData.Value2,
                ContractorPdfData.Key3,
                ContractorPdfData.Value3,
                ContractorPdfData.Key4,
                ContractorPdfData.Value4,
                ContractorPdfData.Key5,
                ContractorPdfData.Value5,
                ContractorPdfData.MeasurementCode,
                ContractorPdfData.RealArticleNo
                FROM dbo.ContractorPdfData
                WHERE  ContractorPdfData.LotId = @LotId AND CompanyId = @CompanyId
            ORDER BY ContractorPdfData.LotRowNumber ASC",
                new { LotId = PdfToExcelParameter.Id, CompanyId = companyId }).ToList();
        // var pdfData =  data.GroupBy(c => c.CompanyId);
        // var result = new List<ContractorPdfDataResults>();
        // foreach (var item in pdfData)
        // {
        //     var tt = new ContractorPdfDataResults();
        //     tt.CompanyId = item.Key;
        //     tt.ContractorPdfData = item.ToList();
        //     
        //     result.Add(tt);
        // }
        //return result;

        return data;
    }

    public async Task<string> GetContractorPdfRowData(PdfToExcelParameter PdfToExcelParameter)
    {
        var connectionString = ConnectionString.MapConnectionString(PdfToExcelParameter.ContractingUnitSequenceId,
            PdfToExcelParameter.ProjectSequenceId, PdfToExcelParameter.TenantProvider);
        var dbConnection = new SqlConnection(connectionString);

        AnalyzeResult data = null;
        var jsonResult = dbConnection
            .Query<string>(
                "SELECT Result FROM dbo.ContractorPdfRowData WHERE LotId = @LotId AND ContractorId = @ContractorId  ORDER BY RevisionNumber DESC",
                new { LotId = PdfToExcelParameter.lotId, PdfToExcelParameter.ContractorId })
            .FirstOrDefault();
        // if (jsonResult != null)
        // {
        //     data = JsonConvert.DeserializeObject<AnalyzeResult>(jsonResult);
        // }


        return jsonResult;
    }

    public async Task<List<ContractorPdfErrorLog>> ContractorPdfErrorLogGetByLotId(
        PdfToExcelParameter PdfToExcelParameter)
    {
        var connectionString = ConnectionString.MapConnectionString(PdfToExcelParameter.ContractingUnitSequenceId,
            PdfToExcelParameter.ProjectSequenceId, PdfToExcelParameter.TenantProvider);
        var dbConnection = new SqlConnection(connectionString);
        var tenetConnection = new SqlConnection(PdfToExcelParameter.TenantProvider.GetTenant().ConnectionString);


        var data = new List<ContractorPdfErrorLog>();

        var companyId = tenetConnection.Query<string>("Select CompanyId From CabPersonCompany Where Oid = @Oid",
            new { Oid = PdfToExcelParameter.UserId }).FirstOrDefault();

        if (companyId == PdfToExcelParameter.Configuration.GetValue<string>("CompanyId"))
        {
            var pdfData = await GetContractorPdfData(PdfToExcelParameter);

            var excelData = dbConnection
                .Query<ContractorPdfErrorLog>(
                    @"SELECT Title ,ParentId ,ArticleNo ,Quantity ,ContractId ,UnitPrice, TotalPrice,MeasurementCode,Unit,Mou,IsExclude,RealArticleNo FROM dbo.CBCExcelLotData where ContractId = @ContractId",
                    new { ContractId = PdfToExcelParameter.Id }).ToList();

            var articleList =
                tenetConnection.Query<string>("SELECT ProductId FROM dbo.PbsProduct").ToList();

            var errorsCbc = excelData.Where(v => v.IsExclude == false).Where(p => articleList.All(p2 => p2 != p.RealArticleNo))
                .ToList();
            errorsCbc.ForEach(c => c.Error = "articleNoNotInCbc");
            errorsCbc.ForEach(c => c.ColumnName = "ArticleNo");

            data.AddRange(errorsCbc);

            pdfData = pdfData.Where(o => excelData.All(g => g.ParentId != o.RealArticleNo)).ToList();

            //var fff = pdfData.Where(o => excelData.All(g => g.ArticleNo != o.ArticleNo));
            var notParents = excelData.Where(o => excelData.All(g => g.ParentId != o.RealArticleNo) && o.IsExclude == false).ToList();
            foreach (var item in notParents)
                if (!ValueCheck(PdfToExcelParameter, item.RealArticleNo))
                {
                    
                    //if (item.MeasurementCode != PdfToExcelParameter.Configuration.GetValue<string>("MeasurementCode"))
                    if (!MeasurementCodeCheck(PdfToExcelParameter, item.MeasurementCode))
                    {

                        if (item.Mou.IsNullOrEmpty())
                        {
                            var listdata = new ContractorPdfErrorLog
                            {
                                ArticleNo = item.ArticleNo,
                                Title = item.Title,
                                Error = "unitErrorEmpty",
                                CreatedDate = DateTime.UtcNow,
                                ColumnName = "Unit",
                                CompanyId = item.CompanyId,
                                LotId = item.LotId,
                                LotRowNumber = item.LotRowNumber
                            };

                            data.Add(listdata);
                        }

                        if (item.Quantity.IsNullOrEmpty())
                        {
                            var listdata = new ContractorPdfErrorLog
                            {
                                ArticleNo = item.ArticleNo,
                                Title = item.Title,
                                Error = "quantityErrorEmpty",
                                CreatedDate = DateTime.UtcNow,
                                ColumnName = "Quantity",
                                CompanyId = item.CompanyId,
                                LotId = item.LotId,
                                LotRowNumber = item.LotRowNumber
                            };

                            data.Add(listdata);
                        }
                        else if (Regex.IsMatch(item.Quantity, "[a-z,A-Z/:!@#$%&*()+]"))
                        {
                            var listdata = new ContractorPdfErrorLog
                            {
                                ArticleNo = item.ArticleNo,
                                Title = item.Title,
                                Error = "quantityErrorNotValid",
                                CreatedDate = DateTime.UtcNow,
                                ColumnName = "Quantity",
                                CompanyId = item.CompanyId,
                                LotId = item.LotId,
                                LotRowNumber = item.LotRowNumber
                            };

                            data.Add(listdata);
                        }
                        else if (!Regex.IsMatch(item.Quantity, "^[1-9]") &&
                                 !Regex.IsMatch(item.Quantity, "0."))
                        {
                            var listdata = new ContractorPdfErrorLog
                            {
                                ArticleNo = item.ArticleNo,
                                Title = item.Title,
                                Error = "QuantityIs0",
                                CreatedDate = DateTime.UtcNow,
                                ColumnName = "Quantity",
                                CompanyId = item.CompanyId,
                                LotId = item.LotId,
                                LotRowNumber = item.LotRowNumber
                            };

                            data.Add(listdata);
                        }

                        // if (item.UnitPrice.ToString().IsNullOrEmpty())
                        // {
                        //     var listdata = new ContractorPdfErrorLog
                        //     {
                        //         ArticleNo = item.ArticleNo,
                        //         Title = item.Title,
                        //         Error = "unitPriceErrorEmpty",
                        //         CreatedDate = DateTime.UtcNow,
                        //         ColumnName = "UnitPrice",
                        //         CompanyId = item.CompanyId,
                        //         LotId = item.LotId,
                        //         LotRowNumber = item.LotRowNumber
                        //     };
                        //
                        //     data.Add(listdata);
                        // }
                        //
                        // else if (!Regex.IsMatch(item.UnitPrice.ToString(), "^[1-9]") &&
                        //     !Regex.IsMatch(item.UnitPrice.ToString(), "0."))
                        // {
                        //     var listdata = new ContractorPdfErrorLog
                        //     {
                        //         ArticleNo = item.ArticleNo,
                        //         Title = item.Title,
                        //         Error = "UnitPriceIs0",
                        //         CreatedDate = DateTime.UtcNow,
                        //         ColumnName = "UnitPrice",
                        //         CompanyId = item.CompanyId,
                        //         LotId = item.LotId,
                        //         LotRowNumber = item.LotRowNumber
                        //     };
                        //
                        //     data.Add(listdata);
                        // }

                        // if (item.TotalPrice.ToString().IsNullOrEmpty())
                        // {
                        //     var listdata = new ContractorPdfErrorLog
                        //     {
                        //         ArticleNo = item.ArticleNo,
                        //         Title = item.Title,
                        //         Error = "totalPriceErrorEmpty",
                        //         CreatedDate = DateTime.UtcNow,
                        //         ColumnName = "TotalPrice",
                        //         CompanyId = item.CompanyId,
                        //         LotId = item.LotId,
                        //         LotRowNumber = item.LotRowNumber
                        //     };
                        //
                        //     data.Add(listdata);
                        // }
                        // else if (!Regex.IsMatch(item.TotalPrice.ToString(), "^[1-9]"))
                        // {
                        //     var listdata = new ContractorPdfErrorLog
                        //     {
                        //         ArticleNo = item.ArticleNo,
                        //         Title = item.Title,
                        //         Error = "totalPriceErrorIs0",
                        //         CreatedDate = DateTime.UtcNow,
                        //         ColumnName = "TotalPrice",
                        //         CompanyId = item.CompanyId,
                        //         LotId = item.LotId,
                        //         LotRowNumber = item.LotRowNumber
                        //     };
                        //
                        //     data.Add(listdata);
                        // }

                        // if (!item.Quantity.IsNullOrEmpty() && !item.UnitPrice.ToString().IsNullOrEmpty() &&
                        //     !item.TotalPrice.ToString().IsNullOrEmpty())
                        //     if (Regex.IsMatch(item.TotalPrice.ToString(), "^[0-9]") &&
                        //         Regex.IsMatch(item.UnitPrice.ToString(), "^[0-9]") &&
                        //         !Regex.IsMatch(item.Quantity, "[a-z,A-Z./:!@#$%&*()+]"))
                        //     {
                        //         // var nn = item.Quantity.ToFloat().ToString("0.00");
                        //         // var tt = item.UnitPrice.ToString("0.00");
                        //         var qq = item.TotalPrice?.ToString("0.00");
                        //         var calculatedPricerounded = (item.Quantity.ToFloat() * item.UnitPrice)?.ToString("0.00");
                        //         if (calculatedPricerounded.ToFloat() != qq.ToFloat())
                        //         {
                        //             var listdata = new ContractorPdfErrorLog
                        //             {
                        //                 ArticleNo = item.ArticleNo,
                        //                 Title = item.Title,
                        //                 Error = "totalPriceErrorWrong",
                        //                 CreatedDate = DateTime.UtcNow,
                        //                 ColumnName = "TotalPrice",
                        //                 CompanyId = item.CompanyId,
                        //                 LotId = item.LotId,
                        //                 LotRowNumber = item.LotRowNumber,
                        //                 TotalPrice = item.TotalPrice,
                        //                 TotalPricerounded = qq.ToFloat(),
                        //                 calculatedPricerounded = calculatedPricerounded.ToFloat(),
                        //                 UnitPrice = item.UnitPrice,
                        //                 Quantity = item.Quantity
                        //             };
                        //
                        //             data.Add(listdata);
                        //         }
                        //     }
                    }
                }

            foreach (var item in pdfData)
            {
                if (!ValueCheck(PdfToExcelParameter, item.RealArticleNo))
                {
                    var lotItem = excelData.FirstOrDefault(x => x.ArticleNo == item.ArticleNo);

                    if (lotItem != null)
                    {

                        var mc = lotItem?.MeasurementCode;
                        if (!lotItem.IsExclude)
                        {

                            if (!excelData.Exists(c => c.ArticleNo == item.ArticleNo.Trim()))
                            {
                                var listdata = new ContractorPdfErrorLog
                                {
                                    ArticleNo = item.ArticleNo,
                                    Title = item.Title,
                                    Error = "articleNotInLot",
                                    CreatedDate = DateTime.UtcNow,
                                    ColumnName = "ArticleNo",
                                    CompanyId = item.CompanyId,
                                    LotId = item.LotId,
                                    LotRowNumber = item.LotRowNumber
                                };

                                data.Add(listdata);
                            }

                            // if (mc != PdfToExcelParameter.Configuration.GetValue<string>("MeasurementCode"))
                            if (!MeasurementCodeCheck(PdfToExcelParameter, mc))
                            {

                                if (item.Unit.IsNullOrEmpty())
                                {
                                    var listdata = new ContractorPdfErrorLog
                                    {
                                        ArticleNo = item.ArticleNo,
                                        Title = item.Title,
                                        Error = "unitErrorEmpty",
                                        CreatedDate = DateTime.UtcNow,
                                        ColumnName = "Unit",
                                        CompanyId = item.CompanyId,
                                        LotId = item.LotId,
                                        LotRowNumber = item.LotRowNumber
                                    };

                                    data.Add(listdata);
                                }
                                else
                                {
                                    if (lotItem.Mou != item.Unit)
                                    {

                                        var listdata = new ContractorPdfErrorLog
                                        {
                                            ArticleNo = item.ArticleNo,
                                            Title = item.Title,
                                            Error = "unitErrorWrong",
                                            CreatedDate = DateTime.UtcNow,
                                            ColumnName = "Unit",
                                            CompanyId = item.CompanyId,
                                            LotId = item.LotId,
                                            LotRowNumber = item.LotRowNumber
                                        };

                                        data.Add(listdata);
                                    }
                                }

                                if (item.Quantity.IsNullOrEmpty())
                                {
                                    var listdata = new ContractorPdfErrorLog
                                    {
                                        ArticleNo = item.ArticleNo,
                                        Title = item.Title,
                                        Error = "quantityErrorEmpty",
                                        CreatedDate = DateTime.UtcNow,
                                        ColumnName = "Quantity",
                                        CompanyId = item.CompanyId,
                                        LotId = item.LotId,
                                        LotRowNumber = item.LotRowNumber
                                    };

                                    data.Add(listdata);


                                }
                                else if (Regex.IsMatch(item.Quantity, "[a-z,A-Z/:!@#$%&*()+]"))
                                {


                                    var listdata = new ContractorPdfErrorLog
                                    {
                                        ArticleNo = item.ArticleNo,
                                        Title = item.Title,
                                        Error = "quantityErrorNotValid",
                                        CreatedDate = DateTime.UtcNow,
                                        ColumnName = "Quantity",
                                        CompanyId = item.CompanyId,
                                        LotId = item.LotId,
                                        LotRowNumber = item.LotRowNumber
                                    };

                                    data.Add(listdata);
                                }
                                else if (!Regex.IsMatch(item.Quantity, "^[1-9]") &&
                                         !Regex.IsMatch(item.Quantity, "0."))
                                {
                                    var listdata = new ContractorPdfErrorLog
                                    {
                                        ArticleNo = item.ArticleNo,
                                        Title = item.Title,
                                        Error = "QuantityIs0",
                                        CreatedDate = DateTime.UtcNow,
                                        ColumnName = "Quantity",
                                        CompanyId = item.CompanyId,
                                        LotId = item.LotId,
                                        LotRowNumber = item.LotRowNumber
                                    };

                                    data.Add(listdata);

                                }
                                else
                                {
                                    if (double.Parse(lotItem.Quantity) != double.Parse(item.Quantity))
                                    {
                                        var listdata2 = new ContractorPdfErrorLog
                                        {
                                            ArticleNo = item.ArticleNo,
                                            Title = item.Title,
                                            Error = "quantityErrorWrong",
                                            CreatedDate = DateTime.UtcNow,
                                            ColumnName = "Quantity",
                                            CompanyId = item.CompanyId,
                                            LotId = item.LotId,
                                            LotRowNumber = item.LotRowNumber
                                        };
                                        data.Add(listdata2);

                                    }

                                }

                                if (item.UnitPrice.ToString().IsNullOrEmpty())
                                {
                                    var listdata = new ContractorPdfErrorLog
                                    {
                                        ArticleNo = item.ArticleNo,
                                        Title = item.Title,
                                        Error = "unitPriceErrorEmpty",
                                        CreatedDate = DateTime.UtcNow,
                                        ColumnName = "UnitPrice",
                                        CompanyId = item.CompanyId,
                                        LotId = item.LotId,
                                        LotRowNumber = item.LotRowNumber
                                    };

                                    data.Add(listdata);
                                }

                                else if (!Regex.IsMatch(item.UnitPrice.ToString(), "^[1-9]") &&
                                         !Regex.IsMatch(item.UnitPrice.ToString(), "0."))
                                {
                                    var listdata = new ContractorPdfErrorLog
                                    {
                                        ArticleNo = item.ArticleNo,
                                        Title = item.Title,
                                        Error = "UnitPriceIs0",
                                        CreatedDate = DateTime.UtcNow,
                                        ColumnName = "UnitPrice",
                                        CompanyId = item.CompanyId,
                                        LotId = item.LotId,
                                        LotRowNumber = item.LotRowNumber
                                    };

                                    data.Add(listdata);
                                }

                                if (item.TotalPrice.ToString().IsNullOrEmpty())
                                {
                                    var listdata = new ContractorPdfErrorLog
                                    {
                                        ArticleNo = item.ArticleNo,
                                        Title = item.Title,
                                        Error = "totalPriceErrorEmpty",
                                        CreatedDate = DateTime.UtcNow,
                                        ColumnName = "TotalPrice",
                                        CompanyId = item.CompanyId,
                                        LotId = item.LotId,
                                        LotRowNumber = item.LotRowNumber
                                    };

                                    data.Add(listdata);
                                }
                                else if (!Regex.IsMatch(item.TotalPrice.ToString(), "^[1-9]") &&  !Regex.IsMatch(item.TotalPrice.ToString(), "0."))
                                {
                                    var listdata = new ContractorPdfErrorLog
                                    {
                                        ArticleNo = item.ArticleNo,
                                        Title = item.Title,
                                        Error = "totalPriceErrorIs0",
                                        CreatedDate = DateTime.UtcNow,
                                        ColumnName = "TotalPrice",
                                        CompanyId = item.CompanyId,
                                        LotId = item.LotId,
                                        LotRowNumber = item.LotRowNumber
                                    };

                                    data.Add(listdata);
                                }

                                if (!item.Quantity.IsNullOrEmpty() && !item.UnitPrice.ToString().IsNullOrEmpty() &&
                                    !item.TotalPrice.ToString().IsNullOrEmpty())
                                    if (Regex.IsMatch(item.TotalPrice.ToString(), "^[1-9]") || Regex.IsMatch(item.TotalPrice.ToString(), "0.") &&
                                        Regex.IsMatch(item.UnitPrice.ToString(), "^[1-9]") || Regex.IsMatch(item.UnitPrice.ToString(), "0.") &&
                                        !Regex.IsMatch(item.Quantity, "[a-z,A-Z./:!@#$%&*()+]") || Regex.IsMatch(item.Quantity, "0."))
                                    {
                                        // var nn = item.Quantity.ToFloat().ToString("0.00");
                                        // var tt = item.UnitPrice.ToString("0.00");
                                        var qq = item.TotalPrice?.ToString("0.00");
                                        var calculatedPricerounded =
                                            (item.Quantity.ToFloat() * item.UnitPrice)?.ToString("0.00");
                                        if (calculatedPricerounded.ToFloat() != qq.ToFloat())
                                        {
                                            var listdata = new ContractorPdfErrorLog
                                            {
                                                ArticleNo = item.ArticleNo,
                                                Title = item.Title,
                                                Error = "totalPriceErrorWrong",
                                                CreatedDate = DateTime.UtcNow,
                                                ColumnName = "TotalPrice",
                                                CompanyId = item.CompanyId,
                                                LotId = item.LotId,
                                                LotRowNumber = item.LotRowNumber,
                                                TotalPrice = item.TotalPrice,
                                                TotalPricerounded = qq.ToFloat(),
                                                calculatedPricerounded = calculatedPricerounded.ToFloat(),
                                                UnitPrice = item.UnitPrice,
                                                Quantity = item.Quantity
                                            };

                                            data.Add(listdata);
                                        }
                                    }
                            }
                        }
                    }
                }
            }
        }
        else
        {
            data = await ContractorPdfErrorLogGetByLotIdFilterContractor(PdfToExcelParameter);
        }

        return data.OrderBy(c => c.ArticleNo).ToList();
    }

    private static bool MeasurementCodeCheck(PdfToExcelParameter PdfToExcelParameter, string mc)
    {
        var s1 = PdfToExcelParameter.Configuration.GetValue<string>("MeasurementCode");
        var s2 = mc;
        var containsBothValues = false;
        if (mc == null)
        {
            return containsBothValues;
        }

        
        var pattern = "[" + Regex.Escape(s1.ToLower()) + "]";
        var result = Regex.Replace(s2.ToLower(), pattern, "");

         containsBothValues = result.Length == s2.Length - s1.Length;

        
        return containsBothValues;
    }
    
    private static bool ValueCheck(PdfToExcelParameter PdfToExcelParameter, string mc)
    {
        var s1 = PdfToExcelParameter.Configuration.GetValue<string>("MeasurementCode");
        var s2 = mc;
        var isValid = int.TryParse(mc, out var intValue) && intValue % 100 == 0;
        
        return isValid;
    }

    public async Task<List<ContractorPdfErrorLog>> ContractorPdfErrorLogGetByLotIdFilterContractor(
        PdfToExcelParameter PdfToExcelParameter)
    {
        var connectionString = ConnectionString.MapConnectionString(PdfToExcelParameter.ContractingUnitSequenceId,
            PdfToExcelParameter.ProjectSequenceId, PdfToExcelParameter.TenantProvider);
        var dbConnection = new SqlConnection(connectionString);
        var tenetConnection = new SqlConnection(PdfToExcelParameter.TenantProvider.GetTenant().ConnectionString);

        var data = new List<ContractorPdfErrorLog>();

        var companyId = tenetConnection.Query<string>("Select CompanyId From CabPersonCompany Where Oid = @Oid",
            new { Oid = PdfToExcelParameter.UserId }).FirstOrDefault();

        if (companyId != null)
        {
            var pdfData = await GetContractorPdfDataFilterContractor(PdfToExcelParameter);

            var excelData = dbConnection
                .Query<ContractorPdfErrorLog>(
                    @"SELECT Title ,ParentId ,ArticleNo ,Quantity ,ContractId,MeasurementCode,Unit,IsExclude,RealArticleNo,Mou FROM dbo.CBCExcelLotdataPublished where ContractId = @ContractId",
                    new { ContractId = PdfToExcelParameter.Id }).ToList();

            var articleList =
                tenetConnection.Query<string>("SELECT ProductId FROM dbo.PbsProduct").ToList();

            var errorsCbc = excelData.Where(v => v.IsExclude == false).Where(p => articleList.All(p2 => p2 != p.RealArticleNo))
                .ToList();
            errorsCbc.ForEach(c => c.Error = "articleNoNotInCbc");
            errorsCbc.ForEach(c => c.ColumnName = "ArticleNo");

            data.AddRange(errorsCbc);

            pdfData = pdfData.Where(o => excelData.All(g => g.ParentId != o.RealArticleNo)).ToList();

            foreach (var item in pdfData)
            {
                if (!ValueCheck(PdfToExcelParameter, item.RealArticleNo))
                {
                    var lotItem = excelData.FirstOrDefault(x => x.ArticleNo == item.ArticleNo);

                    if (lotItem != null)
                    {
                        var mc = lotItem.MeasurementCode;

                        if (!lotItem.IsExclude)
                        {

                            if (!excelData.Exists(c => c.ArticleNo == item.ArticleNo.Trim()))
                            {
                                var listdata = new ContractorPdfErrorLog
                                {
                                    ArticleNo = item.ArticleNo,
                                    Title = item.Title,
                                    Error = "articleNotInLot",
                                    CreatedDate = DateTime.UtcNow,
                                    ColumnName = "ArticleNo",
                                    CompanyId = item.CompanyId,
                                    LotId = item.LotId,
                                    LotRowNumber = item.LotRowNumber
                                };

                                data.Add(listdata);
                            }


                            // if (mc != PdfToExcelParameter.Configuration.GetValue<string>("MeasurementCode"))
                            if (!MeasurementCodeCheck(PdfToExcelParameter, mc))
                            {

                                if (item.Unit.IsNullOrEmpty())
                                {
                                    var listdata = new ContractorPdfErrorLog
                                    {
                                        ArticleNo = item.ArticleNo,
                                        Title = item.Title,
                                        Error = "unitErrorEmpty",
                                        CreatedDate = DateTime.UtcNow,
                                        ColumnName = "Unit",
                                        CompanyId = item.CompanyId,
                                        LotId = item.LotId,
                                        LotRowNumber = item.LotRowNumber
                                    };

                                    data.Add(listdata);
                                }
                                else
                                {
                                    if (lotItem.Mou != item.Unit)
                                    {

                                        var listdata = new ContractorPdfErrorLog
                                        {
                                            ArticleNo = item.ArticleNo,
                                            Title = item.Title,
                                            Error = "unitErrorWrong",
                                            CreatedDate = DateTime.UtcNow,
                                            ColumnName = "Unit",
                                            CompanyId = item.CompanyId,
                                            LotId = item.LotId,
                                            LotRowNumber = item.LotRowNumber
                                        };

                                        data.Add(listdata);
                                    }
                                }

                                if (item.Quantity.IsNullOrEmpty())
                                {
                                    var listdata = new ContractorPdfErrorLog
                                    {
                                        ArticleNo = item.ArticleNo,
                                        Title = item.Title,
                                        Error = "quantityErrorEmpty",
                                        CreatedDate = DateTime.UtcNow,
                                        ColumnName = "Quantity",
                                        CompanyId = item.CompanyId,
                                        LotId = item.LotId,
                                        LotRowNumber = item.LotRowNumber
                                    };

                                    data.Add(listdata);


                                }
                                else if (Regex.IsMatch(item.Quantity, "[a-z,A-Z/:!@#$%&*()+]"))
                                {

                                    var listdata = new ContractorPdfErrorLog
                                    {
                                        ArticleNo = item.ArticleNo,
                                        Title = item.Title,
                                        Error = "quantityErrorNotValid",
                                        CreatedDate = DateTime.UtcNow,
                                        ColumnName = "Quantity",
                                        CompanyId = item.CompanyId,
                                        LotId = item.LotId,
                                        LotRowNumber = item.LotRowNumber
                                    };

                                    data.Add(listdata);
                                }
                                else if (!Regex.IsMatch(item.Quantity, "^[1-9]") &&
                                         !Regex.IsMatch(item.Quantity, "0."))
                                {
                                    var listdata = new ContractorPdfErrorLog
                                    {
                                        ArticleNo = item.ArticleNo,
                                        Title = item.Title,
                                        Error = "QuantityIs0",
                                        CreatedDate = DateTime.UtcNow,
                                        ColumnName = "Quantity",
                                        CompanyId = item.CompanyId,
                                        LotId = item.LotId,
                                        LotRowNumber = item.LotRowNumber
                                    };

                                    data.Add(listdata);
                                }
                                else
                                {
                                    if (double.Parse(lotItem.Quantity) != double.Parse(item.Quantity))
                                    {
                                        var listdata2 = new ContractorPdfErrorLog
                                        {
                                            ArticleNo = item.ArticleNo,
                                            Title = item.Title,
                                            Error = "quantityErrorWrong",
                                            CreatedDate = DateTime.UtcNow,
                                            ColumnName = "Quantity",
                                            CompanyId = item.CompanyId,
                                            LotId = item.LotId,
                                            LotRowNumber = item.LotRowNumber
                                        };
                                        data.Add(listdata2);

                                    }
                                }

                                if (item.UnitPrice.ToString().IsNullOrEmpty())
                                {
                                    var listdata = new ContractorPdfErrorLog
                                    {
                                        ArticleNo = item.ArticleNo,
                                        Title = item.Title,
                                        Error = "unitPriceErrorEmpty",
                                        CreatedDate = DateTime.UtcNow,
                                        ColumnName = "UnitPrice",
                                        CompanyId = item.CompanyId,
                                        LotId = item.LotId,
                                        LotRowNumber = item.LotRowNumber
                                    };

                                    data.Add(listdata);
                                }

                                else if (!Regex.IsMatch(item.UnitPrice.ToString(), "^[1-9]") &&
                                         !Regex.IsMatch(item.UnitPrice.ToString(), "0."))
                                {
                                    var listdata = new ContractorPdfErrorLog
                                    {
                                        ArticleNo = item.ArticleNo,
                                        Title = item.Title,
                                        Error = "UnitPriceIs0",
                                        CreatedDate = DateTime.UtcNow,
                                        ColumnName = "UnitPrice",
                                        CompanyId = item.CompanyId,
                                        LotId = item.LotId,
                                        LotRowNumber = item.LotRowNumber
                                    };

                                    data.Add(listdata);
                                }

                                if (item.TotalPrice.ToString().IsNullOrEmpty())
                                {
                                    var listdata = new ContractorPdfErrorLog
                                    {
                                        ArticleNo = item.ArticleNo,
                                        Title = item.Title,
                                        Error = "totalPriceErrorEmpty",
                                        CreatedDate = DateTime.UtcNow,
                                        ColumnName = "TotalPrice",
                                        CompanyId = item.CompanyId,
                                        LotId = item.LotId,
                                        LotRowNumber = item.LotRowNumber
                                    };

                                    data.Add(listdata);
                                }
                                else if (!Regex.IsMatch(item.TotalPrice.ToString(), "^[1-9]") &&
                                         !Regex.IsMatch(item.TotalPrice.ToString(), "0."))
                                {
                                    var listdata = new ContractorPdfErrorLog
                                    {
                                        ArticleNo = item.ArticleNo,
                                        Title = item.Title,
                                        Error = "totalPriceErrorIs0",
                                        CreatedDate = DateTime.UtcNow,
                                        ColumnName = "TotalPrice",
                                        CompanyId = item.CompanyId,
                                        LotId = item.LotId,
                                        LotRowNumber = item.LotRowNumber
                                    };

                                    data.Add(listdata);
                                }

                                if (!item.Quantity.IsNullOrEmpty() && !item.UnitPrice.ToString().IsNullOrEmpty() &&
                                    !item.TotalPrice.ToString().IsNullOrEmpty())
                                    if (Regex.IsMatch(item.TotalPrice.ToString(), "^[1-9]") || Regex.IsMatch(item.TotalPrice.ToString(), "0.") &&
                                        Regex.IsMatch(item.UnitPrice.ToString(), "^[1-9]") || Regex.IsMatch(item.UnitPrice.ToString(), "0.") &&
                                        !Regex.IsMatch(item.Quantity, "[a-z,A-Z./:!@#$%&*()+]") || Regex.IsMatch(item.Quantity, "0.") )
                                    {
                                        // var nn = item.Quantity.ToFloat().ToString("0.00");
                                        // var tt = item.UnitPrice.ToString("0.00");
                                        var qq = item.TotalPrice?.ToString("0.00");
                                        var calculatedPricerounded =
                                            (item.Quantity.ToFloat() * item.UnitPrice)?.ToString("0.00");
                                        if (calculatedPricerounded.ToFloat() != qq.ToFloat())
                                        {
                                            var listdata = new ContractorPdfErrorLog
                                            {
                                                ArticleNo = item.ArticleNo,
                                                Title = item.Title,
                                                Error = "totalPriceErrorWrong",
                                                CreatedDate = DateTime.UtcNow,
                                                ColumnName = "TotalPrice",
                                                CompanyId = item.CompanyId,
                                                LotId = item.LotId,
                                                LotRowNumber = item.LotRowNumber,
                                                TotalPrice = item.TotalPrice,
                                                TotalPricerounded = qq.ToFloat(),
                                                calculatedPricerounded = calculatedPricerounded.ToFloat()
                                            };

                                            data.Add(listdata);
                                        }
                                    }
                            }
                        }
                    }
                }
            }
        }

        return data;
    }

    public async Task<string> SaveContractorPdfData(PdfToExcelParameter PdfToExcelParameter)
    {
        var connectionString = ConnectionString.MapConnectionString(PdfToExcelParameter.ContractingUnitSequenceId,
            PdfToExcelParameter.ProjectSequenceId, PdfToExcelParameter.TenantProvider);
        var connection = new SqlConnection(connectionString);
        var tenetConnection = new SqlConnection(PdfToExcelParameter.TenantProvider.GetTenant().ConnectionString);


        var companyId = tenetConnection.Query<string>("Select CompanyId From CabPersonCompany Where Oid = @Oid",
            new { Oid = PdfToExcelParameter.UserId }).FirstOrDefault();
        var deleteQuery = "Delete From ContractorPdfData Where LotId = @ContractId AND RealArticleNo NOT IN @ArticleNos ";

        var articleList = PdfToExcelParameter.ExcelLotDataDtoTest.ExcelData.Where(x => x.IsParent)
            .Select(v => v.RealArticleNo).ToList();

        var sb = new StringBuilder(deleteQuery);

        if (companyId != PdfToExcelParameter.Configuration.GetValue<string>("CompanyId"))
        {
            sb.Append(" AND CompanyId = @CompanyId");

            if (PdfToExcelParameter.ExcelLotDataDtoTest.LotId != null)
            {
                await connection.ExecuteAsync(sb.ToString(),
                    new
                    {
                        ContractId = PdfToExcelParameter.ExcelLotDataDtoTest.LotId, CompanyId = companyId,
                        ArticleNos = articleList
                    });

                if (PdfToExcelParameter.ExcelLotDataDtoTest.ExcelData.Any())
                {
                    var lotRowNumber = 0;
                    var query =
                        @"INSERT INTO dbo.CBCExcelLotData ( Id ,Title ,ParentId ,ArticleNo ,Quantity ,ContractId ,Unit, UnitPrice,TotalPrice ,MeasurementCode , Mou , Key1, Value1, Key2, Value2, Key3, Value3, Key4, Value4, Key5, Value5) VALUES ( @Id ,@Title ,@ParentId ,@ArticleNo ,@Quantity ,@ContractId ,@Unit, @UnitPrice,@TotalPrice , @MeasurementCode , @Mou , @Key1, @Value1, @Key2, @Value2, @Key3, @Value3, @Key4, @Value4, @Key5, @Value5);";
                    foreach (var dto in PdfToExcelParameter.ExcelLotDataDtoTest.ExcelData)
                        if (!dto.IsParent)
                        {
                            foreach (var contractorData in dto.Contractors)
                            {
                                var sql =
                                    "INSERT INTO dbo.ContractorPdfData ( Id ,ArticleNo ,CompanyId ,Title ,Unit  ,Quantity ,UnitPrice ,TotalPrice,LotId,CreatedDate ,LotRowNumber,Key1, Value1, Key2, Value2, Key3, Value3, Key4, Value4, Key5, Value5,MeasurementCode,RealArticleNo ) VALUES ( @Id ,@ArticleNo ,@CompanyId ,@Title ,@Unit ,@Quantity ,@UnitPrice ,@TotalPrice, @LotId, @CreatedDate ,@LotRowNumber,@Key1, @Value1, @Key2, @Value2, @Key3, @Value3, @Key4, @Value4, @Key5, @Value5,@MeasurementCode,@RealArticleNo)";

                                var param1 = new
                                {
                                    Id = Guid.NewGuid().ToString(),
                                    dto.ArticleNo,
                                    contractorData.CompanyId,
                                    dto.Title,
                                    contractorData.Unit,
                                    contractorData.Quantity,
                                    UnitPrice = contractorData.UnitPrice?.ToFloat(),
                                    TotalPrice = contractorData.TotalPrice?.ToFloat(),
                                    LotId = PdfToExcelParameter.lotId,
                                    CreatedDate = DateTime.UtcNow,
                                    LotRowNumber = lotRowNumber.ToString().PadLeft(4, '0'),
                                    Key1 = contractorData.Key1,
                                    Value1 = contractorData.Value1,
                                    Key2 = contractorData.Key2,
                                    Value2 = contractorData.Value2,
                                    Key3 = contractorData.Key3,
                                    Value3 = contractorData.Value3,
                                    Key4 = contractorData.Key4,
                                    Value4 = contractorData.Value4,
                                    Key5 = contractorData.Key5,
                                    Value5 = contractorData.Value5,
                                    MeasurementCode = contractorData.MeasurementCode,
                                    RealArticleNo = contractorData.RealArticleNo
                                };

                                await connection.ExecuteAsync(sql, param1);
                            }

                            lotRowNumber++;
                        }
                }

                if (PdfToExcelParameter.ExcelLotDataDtoTest.ContractorData.Any())
                    foreach (var contractData in PdfToExcelParameter.ExcelLotDataDtoTest.ContractorData)
                    {
                        await connection.ExecuteAsync(
                            "Delete From ContractorTotalValues Where LotId = @LotId AND CompanyId = @CompanyId",
                            new { contractData.CompanyId, PdfToExcelParameter.ExcelLotDataDtoTest.LotId });

                        var sql3 =
                            "INSERT INTO dbo.ContractorTotalValues ( Id ,LotId ,CompanyId ,TotalBAFO ,TotalCost ) VALUES ( @Id ,@LotId ,@CompanyId ,@TotalBAFO ,@TotalCost )";

                        var param3 = new
                        {
                            Id = Guid.NewGuid().ToString(),
                            PdfToExcelParameter.ExcelLotDataDtoTest.LotId,
                            contractData.CompanyId,
                            contractData.TotalBAFO,
                            contractData.TotalCost
                        };

                        await connection.ExecuteAsync(sql3, param3);
                    }
            }
        }
        else
        {
            if (PdfToExcelParameter.ExcelLotDataDtoTest.LotId != null)
            {
                await connection.ExecuteAsync("Delete From CBCExcelLotData Where ContractId = @ContractId",
                    new { ContractId = PdfToExcelParameter.ExcelLotDataDtoTest.LotId });
                await connection.ExecuteAsync(sb.ToString(),
                    new { ContractId = PdfToExcelParameter.ExcelLotDataDtoTest.LotId, ArticleNos = articleList });

                if (PdfToExcelParameter.ExcelLotDataDtoTest.ExcelData.Any())
                {
                    var lotRowNumber = 0;
                    var query =
                        @"INSERT INTO dbo.CBCExcelLotData ( Id ,Title ,ParentId ,ArticleNo ,Quantity ,ContractId ,Unit, UnitPrice,TotalPrice ,MeasurementCode , Mou ,Key1, Value1, Key2, Value2, Key3, Value3, Key4, Value4, Key5, Value5, IsExclude, RealArticleNo) VALUES ( @Id ,@Title ,@ParentId ,@ArticleNo ,@Quantity ,@ContractId ,@Unit, @UnitPrice,@TotalPrice , @MeasurementCode , @Mou , @Key1, @Value1, @Key2, @Value2, @Key3, @Value3, @Key4, @Value4, @Key5, @Value5, @IsExclude, @RealArticleNo);";
                    foreach (var dto in PdfToExcelParameter.ExcelLotDataDtoTest.ExcelData)
                    {
                        var parm = new
                        {
                            Id = Guid.NewGuid().ToString(),
                            dto.Title,
                            dto.ParentId,
                            dto.ArticleNo,
                            dto.Quantity,
                            ContractId = PdfToExcelParameter.lotId,
                            dto.Unit,
                            UnitPrice = dto.UnitPrice.ToFloat(),
                            TotalPrice = dto.TotalPrice.ToFloat(),
                            dto.MeasurementCode,
                            dto.Mou,
                            dto.Key1,
                            dto.Value1,
                            dto.Key2,
                            dto.Value2,
                            dto.Key3,
                            dto.Value3,
                            dto.Key4,
                            dto.Value4,
                            dto.Key5,
                            dto.Value5,
                            IsExclude = dto.IsExclude,
                            RealArticleNo = dto.RealArticleNo
                        };


                        await connection.ExecuteAsync(query, parm);

                        if (!dto.IsParent)
                        {
                            foreach (var contractorData in dto.Contractors)
                            {
                                var sql =
                                    "INSERT INTO dbo.ContractorPdfData ( Id ,ArticleNo ,CompanyId ,Title ,Unit  ,Quantity ,UnitPrice ,TotalPrice,LotId,CreatedDate ,LotRowNumber , Key1, Value1, Key2, Value2, Key3, Value3, Key4, Value4, Key5, Value5,MeasurementCode ,RealArticleNo ) VALUES ( @Id ,@ArticleNo ,@CompanyId ,@Title ,@Unit ,@Quantity ,@UnitPrice ,@TotalPrice, @LotId, @CreatedDate ,@LotRowNumber, @Key1, @Value1, @Key2, @Value2, @Key3, @Value3, @Key4, @Value4, @Key5, @Value5 ,@MeasurementCode , @RealArticleNo)";

                                var param1 = new
                                {
                                    Id = Guid.NewGuid().ToString(),
                                    dto.ArticleNo,
                                    contractorData.CompanyId,
                                    dto.Title,
                                    contractorData.Unit,
                                    contractorData.Quantity,
                                    UnitPrice = contractorData.UnitPrice?.ToFloat(),
                                    TotalPrice = contractorData.TotalPrice?.ToFloat(),
                                    LotId = PdfToExcelParameter.lotId,
                                    CreatedDate = DateTime.UtcNow,
                                    LotRowNumber = lotRowNumber.ToString().PadLeft(4, '0'),
                                    MeasurementCode = contractorData.MeasurementCode,
                                    Key1 = contractorData.Key1,
                                    Value1 = contractorData.Value1,
                                    Key2 = contractorData.Key2,
                                    Value2 = contractorData.Value2,
                                    Key3 = contractorData.Key3,
                                    Value3 = contractorData.Value3,
                                    Key4 = contractorData.Key4,
                                    Value4 = contractorData.Value4,
                                    Key5 = contractorData.Key5,
                                    Value5 = contractorData.Value5,
                                    RealArticleNo = contractorData.RealArticleNo

                                };

                                await connection.ExecuteAsync(sql, param1);
                            }

                            lotRowNumber++;
                        }
                    }
                }

                if (PdfToExcelParameter.ExcelLotDataDtoTest.ContractorData.Any())
                    foreach (var contractData in PdfToExcelParameter.ExcelLotDataDtoTest.ContractorData)
                    {
                        await connection.ExecuteAsync(
                            "Delete From ContractorTotalValues Where LotId = @LotId AND CompanyId = @CompanyId",
                            new { contractData.CompanyId, PdfToExcelParameter.ExcelLotDataDtoTest.LotId });

                        var sql3 =
                            "INSERT INTO dbo.ContractorTotalValues ( Id ,LotId ,CompanyId ,TotalBAFO ,TotalCost ) VALUES ( @Id ,@LotId ,@CompanyId ,@TotalBAFO ,@TotalCost )";

                        var param3 = new
                        {
                            Id = Guid.NewGuid().ToString(),
                            PdfToExcelParameter.ExcelLotDataDtoTest.LotId,
                            contractData.CompanyId,
                            contractData.TotalBAFO,
                            contractData.TotalCost
                        };

                        await connection.ExecuteAsync(sql3, param3);
                    }
            }
        }


        return "Ok";
    }


    public async Task<string> SaveUploadedPdfData(PdfToExcelParameter PdfToExcelParameter)
    {
        try
        {
            var connectionString = ConnectionString.MapConnectionString(
                PdfToExcelParameter.ContractingUnitSequenceId,
                PdfToExcelParameter.ProjectSequenceId, PdfToExcelParameter.TenantProvider);

            using var connection = new SqlConnection(connectionString);
            using var dbConnection = new SqlConnection(PdfToExcelParameter.TenantProvider.GetTenant().ConnectionString);


            PdfToExcelParameter.ContractorId = dbConnection.Query<string>(
                "Select CompanyId From CabPersonCompany Where Oid = @Oid",
                new { Oid = PdfToExcelParameter.UserId }).FirstOrDefault();


            if (PdfToExcelParameter.lotId != null && PdfToExcelParameter.ContractorId != null)
                await connection.ExecuteAsync(
                    " Delete From dbo.ContractorPdfData Where LotId =@LotId AND CompanyId = @CompanyId",
                    new { CompanyId = PdfToExcelParameter.ContractorId, LotId = PdfToExcelParameter.lotId });

            foreach (var excelData in PdfToExcelParameter.pdfData.ExcelData)
            {
                var sql =
                    "INSERT INTO dbo.ContractorPdfData ( Id ,ArticleNo ,CompanyId ,Title ,Unit ,VH ,Quantity ,UnitPrice ,TotalPrice,LotId,CreatedDate ,PageRowColumn ,PageRow ,LotRowNumber , MeasurementCode, Key1, Value1, Key2, Value2, Key3, Value3, Key4, Value4, Key5, Value5 , RealArticleNo) VALUES ( @Id ,@ArticleNo ,@CompanyId ,@Title ,@Unit ,@VH ,@Quantity ,@UnitPrice ,@TotalPrice, @LotId, @CreatedDate, @PageRowColumn, @PageRow ,@LotRowNumber,@MeasurementCode, @Key1, @Value1, @Key2, @Value2, @Key3, @Value3, @Key4, @Value4, @Key5, @Value5 , @RealArticleNo )";

                var param = new
                {
                    Id = Guid.NewGuid().ToString(),
                    excelData.ArticleNo,
                    CompanyId = PdfToExcelParameter.ContractorId,
                    excelData.Title,
                    excelData.Unit,
                    excelData.VH,
                    excelData.Quantity,
                    excelData.UnitPrice,
                    excelData.TotalPrice,
                    LotId = PdfToExcelParameter.lotId,
                    CreatedDate = DateTime.UtcNow,
                    excelData.PageRowColumn,
                    excelData.PageRow,
                    LotRowNumber = excelData.LotRowNumber.PadLeft(4, '0'),
                    MeasurementCode = excelData.MeasurementCode,
                    Key1 = excelData.Key1,
                    Value1 = excelData.Value1,
                    Key2 = excelData.Key2,
                    Value2 = excelData.Value2,
                    Key3 = excelData.Key3,
                    Value3 = excelData.Value3,
                    Key4 = excelData.Key4,
                    Value4 = excelData.Value4,
                    Key5 = excelData.Key5,
                    Value5 = excelData.Value5,
                    RealArticleNo = excelData.RealArticleNo
                };

                await connection.ExecuteAsync(sql, param);
            }

            return "Ok";
        }
        catch (Exception e)
        {
            //Console.WriteLine(e);
            throw;
        }
    }

    public async Task<string> ContractorLotExcelUpload(PdfToExcelParameter PdfToExcelParameter)
    {
        try
        {
            var connectionString = ConnectionString.MapConnectionString(
                PdfToExcelParameter.ContractingUnitSequenceId,
                PdfToExcelParameter.ProjectSequenceId, PdfToExcelParameter.TenantProvider);

            using var connection = new SqlConnection(connectionString);
            using var dbConnection = new SqlConnection(PdfToExcelParameter.TenantProvider.GetTenant().ConnectionString);


            PdfToExcelParameter.ContractorId = dbConnection.Query<string>(
                "Select CompanyId From CabPersonCompany Where Oid = @Oid",
                new { Oid = PdfToExcelParameter.UserId }).FirstOrDefault();


            if (PdfToExcelParameter.lotId != null && PdfToExcelParameter.ContractorId != null)
                await connection.ExecuteAsync(
                    " Delete From dbo.ContractorPdfData Where LotId =@LotId AND CompanyId = @CompanyId",
                    new { CompanyId = PdfToExcelParameter.ContractorId, LotId = PdfToExcelParameter.lotId });

            foreach (var excelData in PdfToExcelParameter.pdfData.ExcelData)
            {
                // var artNo = excelData.ArticleNo;
                
                if (excelData.Key1.IsNullOrEmpty())
                {
                    excelData.Key1 = null;
                }
                if (excelData.Value1.IsNullOrEmpty())
                {
                    excelData.Value1 = null;
                }
                if (excelData.Key2.IsNullOrEmpty())
                {
                    excelData.Key2 = null;
                }
                if (excelData.Value2.IsNullOrEmpty())
                {
                    excelData.Value2 = null;
                }
                if (excelData.Key3.IsNullOrEmpty())
                {
                    excelData.Key3 = null;
                }
                if (excelData.Value3.IsNullOrEmpty())
                {
                    excelData.Value3 = null;
                }
                if (excelData.Key4.IsNullOrEmpty())
                {
                    excelData.Key4 = null;
                }
                if (excelData.Value4.IsNullOrEmpty())
                {
                    excelData.Value4 = null;
                }if (excelData.Key5.IsNullOrEmpty())
                {
                    excelData.Key5 = null;
                }
                if (excelData.Value5.IsNullOrEmpty())
                {
                    excelData.Value5 = null;
                }

                var sql =
                    "INSERT INTO dbo.ContractorPdfData ( Id ,ArticleNo ,CompanyId ,Title ,Unit ,VH ,Quantity ,UnitPrice ,TotalPrice,LotId,CreatedDate ,PageRowColumn ,PageRow ,LotRowNumber,Key1, Value1, Key2, Value2, Key3, Value3, Key4, Value4, Key5, Value5 ,MeasurementCode, RealArticleNo) VALUES ( @Id ,@ArticleNo ,@CompanyId ,@Title ,@Unit ,@VH ,@Quantity ,@UnitPrice ,@TotalPrice, @LotId, @CreatedDate, @PageRowColumn, @PageRow ,@LotRowNumber, @Key1, @Value1, @Key2, @Value2, @Key3, @Value3, @Key4, @Value4, @Key5, @Value5 ,@MeasurementCode, @RealArticleNo)";

                // if (excelData.Key1 != null && excelData.Value1 != null)
                // {
                //     artNo = artNo + " / " +  excelData.Value1.Trim();
                // }
                // if (excelData.Key2 != null && excelData.Value2 != null)
                // {
                //     artNo = artNo + " / " +  excelData.Value2.Trim();
                // }
                // if (excelData.Key3 != null && excelData.Value3 != null)
                // {
                //     artNo = artNo + " / " +  excelData.Value3.Trim();
                // }
                // if (excelData.Key4 != null && excelData.Value4 != null)
                // {
                //     artNo = artNo + " / " +  excelData.Value4.Trim();
                // }
                // if (excelData.Key5 != null && excelData.Value5 != null)
                // {
                //     artNo = artNo + " / " +  excelData.Value5.Trim();
                // }

                const string pattern = @"^([^/]+)";

                var match = Regex.Match(excelData.ArticleNo, pattern);
                
                var realArtNo = match.Groups[1].Value.Trim();
                
                var param = new
                {
                    Id = Guid.NewGuid().ToString(),
                    ArticleNo = excelData.ArticleNo,
                    CompanyId = PdfToExcelParameter.ContractorId,
                    excelData.Title,
                    excelData.Unit,
                    excelData.VH,
                    excelData.Quantity,
                    excelData.UnitPrice,
                    excelData.TotalPrice,
                    LotId = PdfToExcelParameter.lotId,
                    CreatedDate = DateTime.UtcNow,
                    excelData.PageRowColumn,
                    excelData.PageRow,
                    LotRowNumber = excelData.LotRowNumber.PadLeft(4, '0'),
                    excelData.Key1,
                    excelData.Value1,
                    excelData.Key2,
                    excelData.Value2,
                    excelData.Key3,
                    excelData.Value3,
                    excelData.Key4,
                    excelData.Value4,
                    excelData.Key5,
                    excelData.Value5,
                    MeasurementCode = excelData.MeasurementCode,
                    RealArticleNo = realArtNo
                };

                await connection.ExecuteAsync(sql, param);
            }

            return "Ok";
        }
        catch (Exception e)
        {
            //Console.WriteLine(e);
            throw;
        }
    }

    private int getStartTable(string pdfType)
    {
        var type = pdfType switch
        {
            "EQUANS" => 1,
            "Hvac" => 0,
            "Pepsico" => 0,
            _ => 0
        };
        return type;
    }

    private static void ToTable(DocumentTable table, DocumentTableCell cell, List<tableData> tableData)
    {
        var newTable = new tableData();
        var rowData = table.Cells.Where(c => c.RowIndex == cell.RowIndex);
        if (rowData.Any())
        {
            newTable.ArticleNo = rowData.FirstOrDefault(r => r.ColumnIndex == 0)?.Content;
            newTable.Title = rowData.FirstOrDefault(r => r.ColumnIndex == 2)?.Content;
            newTable.Unit = rowData.FirstOrDefault(r => r.ColumnIndex == 3)?.Content;
            newTable.VH = rowData.FirstOrDefault(r => r.ColumnIndex == 4)?.Content;
            newTable.Quantity = rowData.FirstOrDefault(r => r.ColumnIndex == 5)?.Content;
            newTable.UnitPrice = rowData.FirstOrDefault(r => r.ColumnIndex == 6)?.Content;
            newTable.TotalPrice = rowData.FirstOrDefault(r => r.ColumnIndex == 7)?.Content;
            newTable.CompanyId = "dmds6eac-c9a0-4f93-8baf-d24948bedfdshfh";

            newTable.RowIndex = cell.RowIndex + "-" +
                                cell.BoundingRegions.FirstOrDefault().PageNumber;
            newTable.PageNumber = cell.BoundingRegions.FirstOrDefault().PageNumber;


            tableData.Add(newTable);
        }
    }

    private static void ToTableHvac(DocumentTable table, DocumentTableCell cell, List<tableData> tableData)
    {
        var newTable = new tableData();
        var rowData = table.Cells.Where(c => c.RowIndex == cell.RowIndex);
        if (rowData.Any())
        {
            if (cell.BoundingRegions.FirstOrDefault().PageNumber == 1)
            {
                newTable.ArticleNo = rowData.FirstOrDefault(r => r.ColumnIndex == 1)?.Content;
                newTable.Title = rowData.FirstOrDefault(r => r.ColumnIndex == 2)?.Content;
                newTable.Unit = rowData.FirstOrDefault(r => r.ColumnIndex == 8)?.Content;
                newTable.VH = rowData.FirstOrDefault(r => r.ColumnIndex == 6)?.Content;
                newTable.Quantity = rowData.FirstOrDefault(r => r.ColumnIndex == 7)?.Content;
                newTable.UnitPrice = rowData.FirstOrDefault(r => r.ColumnIndex == 9)?.Content;
                newTable.TotalPrice = rowData.FirstOrDefault(r => r.ColumnIndex == 10)?.Content;

                newTable.RowIndex = cell.RowIndex + "-" +
                                    cell.BoundingRegions.FirstOrDefault().PageNumber;
                newTable.PageNumber = cell.BoundingRegions.FirstOrDefault().PageNumber;


                tableData.Add(newTable);
            }
            else if (cell.ColumnIndex > 7)
            {
                newTable.ArticleNo = rowData.FirstOrDefault(r => r.ColumnIndex == 0)?.Content;
                newTable.Title = rowData.FirstOrDefault(r => r.ColumnIndex == 1)?.Content;
                newTable.Unit = rowData.FirstOrDefault(r => r.ColumnIndex == 7)?.Content;
                newTable.VH = rowData.FirstOrDefault(r => r.ColumnIndex == 5)?.Content;
                newTable.Quantity = rowData.FirstOrDefault(r => r.ColumnIndex == 6)?.Content;
                newTable.UnitPrice = rowData.FirstOrDefault(r => r.ColumnIndex == 8)?.Content;
                newTable.TotalPrice = rowData.FirstOrDefault(r => r.ColumnIndex == 9)?.Content;

                newTable.RowIndex = cell.RowIndex + "-" +
                                    cell.BoundingRegions.FirstOrDefault().PageNumber;
                newTable.PageNumber = cell.BoundingRegions.FirstOrDefault().PageNumber;


                tableData.Add(newTable);
            }
            else
            {
                newTable.ArticleNo = rowData.FirstOrDefault(r => r.ColumnIndex == 0)?.Content;
                newTable.Title = rowData.FirstOrDefault(r => r.ColumnIndex == 1)?.Content;
                newTable.Unit = rowData.FirstOrDefault(r => r.ColumnIndex == 7)?.Content;
                newTable.VH = rowData.FirstOrDefault(r => r.ColumnIndex == 5)?.Content;
                newTable.Quantity = rowData.FirstOrDefault(r => r.ColumnIndex == 6)?.Content;
                newTable.UnitPrice = rowData.FirstOrDefault(r => r.ColumnIndex == 8)?.Content;
                newTable.TotalPrice = rowData.FirstOrDefault(r => r.ColumnIndex == 9)?.Content;

                newTable.RowIndex = cell.RowIndex + "-" +
                                    cell.BoundingRegions.FirstOrDefault().PageNumber;
                newTable.PageNumber = cell.BoundingRegions.FirstOrDefault().PageNumber;


                tableData.Add(newTable);
            }
        }
    }

    private static void ToTablePepsico(DocumentTable table, DocumentTableCell cell, List<tableData> tableData,
        int firstColumnIndex, PdfToExcelParameter pdfToExcelParameter, int max)
    {
        var newTable = new tableData();
        var rowData = table.Cells.Where(c => c.RowIndex == cell.RowIndex);
        var maxColumn = table.Cells.MaxBy(t => t.ColumnIndex).ColumnIndex;

        if (rowData.Any())
        {
            newTable.ArticleNo = rowData.FirstOrDefault(r => r.ColumnIndex == firstColumnIndex)?.Content;

            if (!newTable.ArticleNo.IsNullOrEmpty())
            {
                if (newTable.ArticleNo.Contains(":unselected:"))
                    newTable.ArticleNo = Regex.Replace(newTable.ArticleNo, ":unselected:", "");
                if (newTable.ArticleNo.Contains(":selected:"))
                    newTable.ArticleNo = Regex.Replace(newTable.ArticleNo, ":selected:", "");
            }
            // if (!newTable.ArticleNo.IsNullOrEmpty())
            // {
            //     newTable.ArticleNo = Regex.Replace(newTable.ArticleNo, "[^0-9.]", "");
            //     newTable.ArticleNo = newTable.ArticleNo.TrimEnd();
            //     if (newTable.ArticleNo != "")
            //     {
            //         if (newTable.ArticleNo.Last() == '.')
            //         {
            //             newTable.ArticleNo = newTable.ArticleNo.Remove(newTable.ArticleNo.Length - 1);
            //         }
            //
            //         if (newTable.ArticleNo != "")
            //         {

            // if (char.IsDigit(newTable.ArticleNo[newTable.ArticleNo.Length-1]))
            // {
            //     newTable.ArticleNo = newTable.ArticleNo.Remove(newTable.ArticleNo.Length - 1);
            // }

            newTable.Title = rowData.FirstOrDefault(r => r.ColumnIndex == firstColumnIndex + 1)?.Content;
            if (!newTable.Title.IsNullOrEmpty())
            {
                if (newTable.Title.Contains(":unselected:"))
                    newTable.Title = Regex.Replace(newTable.Title, ":unselected:", "");
                if (newTable.Title.Contains(":selected:"))
                    newTable.Title = Regex.Replace(newTable.Title, ":selected:", "");
            }

            newTable.Unit = rowData.FirstOrDefault(r => r.ColumnIndex == maxColumn - 2)?.Content;
            if (!newTable.Unit.IsNullOrEmpty())
            {
                if (newTable.Unit.Contains(":unselected:"))
                    newTable.Unit = Regex.Replace(newTable.Unit, ":unselected:", "");
                if (newTable.Unit.Contains(":selected:"))
                    newTable.Unit = Regex.Replace(newTable.Unit, ":selected:", "");
            }

            newTable.VH = rowData.FirstOrDefault(r => r.ColumnIndex == firstColumnIndex + 2)?.Content;
            if (!newTable.VH.IsNullOrEmpty())
            {
                if (newTable.VH.Contains(":unselected:")) newTable.VH = Regex.Replace(newTable.VH, ":unselected:", "");
                if (newTable.VH.Contains(":selected:")) newTable.VH = Regex.Replace(newTable.VH, ":selected:", "");
            }

            newTable.Quantity = rowData.FirstOrDefault(r => r.ColumnIndex == maxColumn - 3)?.Content;
            if (!newTable.Quantity.IsNullOrEmpty())
            {
                if (newTable.Quantity.Contains(":unselected:"))
                    newTable.Quantity = Regex.Replace(newTable.Quantity, ":unselected:", "");
                if (newTable.Quantity.Contains(":selected:"))
                    newTable.Quantity = Regex.Replace(newTable.Quantity, ":selected:", "");
            }

            newTable.UnitPrice = rowData.FirstOrDefault(r => r.ColumnIndex == maxColumn - 1)?.Content;
            if (!newTable.UnitPrice.IsNullOrEmpty())
            {
                newTable.UnitPrice = Regex.Replace(newTable.UnitPrice, "€", "");
                if (newTable.UnitPrice.Contains(":unselected:"))
                    newTable.UnitPrice = Regex.Replace(newTable.UnitPrice, ":unselected:", "");
                if (newTable.UnitPrice.Contains(":selected:"))
                    newTable.UnitPrice = Regex.Replace(newTable.UnitPrice, ":selected:", "");
            }

            newTable.TotalPrice = rowData.FirstOrDefault(r => r.ColumnIndex == maxColumn)?.Content;
            if (!newTable.TotalPrice.IsNullOrEmpty())
            {
                newTable.TotalPrice = Regex.Replace(newTable.TotalPrice, "€", "");

                if (newTable.TotalPrice.Contains(":unselected:"))
                    newTable.TotalPrice = Regex.Replace(newTable.TotalPrice, ":unselected:", "");
                if (newTable.TotalPrice.Contains(":selected:"))
                    newTable.TotalPrice = Regex.Replace(newTable.TotalPrice, ":selected:", "");
            }

            newTable.CompanyId = pdfToExcelParameter.ContractorId;
            newTable.RowIndex = cell.RowIndex + "-" +
                                cell.BoundingRegions.FirstOrDefault().PageNumber;
            newTable.PageNumber = cell.BoundingRegions.FirstOrDefault().PageNumber;
            newTable.LotId = pdfToExcelParameter.lotId;
            newTable.CreatedDate = DateTime.UtcNow;
            newTable.PageRow = cell.BoundingRegions.FirstOrDefault().PageNumber + "-" +
                               cell.RowIndex;
            string page;
            string row;

            if (cell.BoundingRegions.FirstOrDefault().PageNumber < 10)
                page = "0" + cell.BoundingRegions.FirstOrDefault().PageNumber;
            else
                page = cell.BoundingRegions.FirstOrDefault().PageNumber.ToString();

            if (cell.RowIndex < 10)
                row = "0" + cell.RowIndex;
            else
                row = cell.RowIndex.ToString();

            newTable.PageRowColumn = page + row;


            tableData.Add(newTable);
        }
        //   }
        // }
        // }
    }
}