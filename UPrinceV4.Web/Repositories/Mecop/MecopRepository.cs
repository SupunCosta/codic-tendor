using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using MickiesoftMuiltitenant.Dapper;
using Microsoft.Data.SqlClient;
using Newtonsoft.Json;
using UPrinceV4.Web.Data.Mecops;
using UPrinceV4.Web.Repositories.Interfaces;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace UPrinceV4.Web.Repositories.Mecop;

public class MecopRepository : IMecopRepository
{
    public async Task<string> GetMecopData(MecopParameter mecopParameter)
    {
        try
        {
            var client = new HttpClient();
            client.BaseAddress = new Uri("https://tokio.potteau.com/");
        
            string base64Credentials = Convert.ToBase64String(Encoding.ASCII.GetBytes($"{"Mickie"}:{"MsService01!"}"));

            // Add the Authorization header with the Basic Auth credentials
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", base64Credentials);
        
            var response = await client.GetAsync("odata/omlijsting");
            var jsonString = await response.Content.ReadAsStringAsync();
            
            var myDeserializedClass = JsonConvert.DeserializeObject<MecopResponceData>(jsonString);

            var value = myDeserializedClass.Value;
            
            await using var connection = new SqlConnection(mecopParameter.TenantProvider.GetTenant().ConnectionString) ;
            connection.Open();
            await connection.ExecuteAsync(@"DELETE FROM CustomerOrderToPo");
            var chunkedList = value.Chunk(1000);
            
            foreach (var item in chunkedList)
            {
                connection.BulkInsertAndSelect(data: item, identityInsert: true);
            }
            
            
            return "ok";
        }
        catch (Exception e)
        {
            //Console.WriteLine(e);
            throw;
        }
    }

    public async Task<List<MecopDto>> GetMecopForExel(MecopParameter mecopParameter)
    {
        try
        {
            await using var connection = new SqlConnection(mecopParameter.TenantProvider.GetTenant().ConnectionString);

            var query = @"SELECT
                          Mecop.Id
                         ,Mecop.AankoopOrder
                         ,Mecop.Lijn
                         ,Mecop.BrandWeerstand
                         ,Mecop.Type
                         ,Mecop.Grondlaag
                         ,Mecop.Ncs
                         ,Mecop.MuurDikte
                         ,Mecop.BreedteDeurlijsten
                         ,Mecop.BreedteDeurlijstenVerticaal
                         ,Mecop.BreedteDeurlijstenHorizontaal
                         ,Mecop.OmlijstingenNummeren
                         ,Mecop.HoogteDeur
                         ,Mecop.BreedteDeur
                         ,Mecop.BreedteVleugel
                         ,Mecop.DikteDeur
                         ,Mecop.DraaiRichting
                         ,Mecop.Paumelles
                         ,Mecop.AantalPaumelles
                         ,Mecop.TegenPlaat
                         ,Mecop.SlotUitsparing
                         ,Mecop.KrukHoogte
                         ,Mecop.KabelDoorvoer
                         ,Mecop.HoogteKabelDoorvoer
                         ,Mecop.Extra1
                         ,Mecop.Extra2
                         ,Mecop.Extra3
                         ,Mecop.Extra4
                         ,Mecop.DeurNummer
                         ,Mecop.Aantal
                         ,Mecop.Eenheidsprijs
                         ,Mecop.TotaalPrijs
                         ,Mecop.Taal
                         ,Mecop.Created
                         ,Mecop.Modified
                         ,Mecop.Status
                         ,Mecop.Klant
                         ,Mecop.Norm
                         ,CASE WHEN  mc1.Value IS NULL THEN Mecop.Materiaal
                         ELSE mc1.Value END AS Materiaal
                         ,CASE WHEN  mc2.Value IS NULL THEN Mecop.Ral
                         ELSE mc2.Value END AS Ral
                        FROM dbo.Mecop
                        LEFT OUTER JOIN dbo.MecopConversion mc1
                          ON Mecop.Materiaal = mc1.[Key]
                        LEFT OUTER JOIN dbo.MecopConversion mc2
                          ON Mecop.Ral = mc2.[Key] WHERE Mecop.Status != 'Finished'";

            var result = connection.Query<MecopDto>(query).ToList();

            var meCop = new List<MecopDto>();

            var resultGroupByPo = result.GroupBy(e => e.AankoopOrder).ToList();

            foreach (var me in resultGroupByPo)
            {
                var poId = Guid.NewGuid().ToString();
                var po = new MecopDto()
                {
                    Id = poId,
                    Title = me.Key,
                    NodeLevel = "PurchaseOrder",
                    Status = me.First().Status
                    
                };
                meCop.Add(po);

                var resultGroupByPoRal = me.GroupBy(e => e.Ral).ToList();
                foreach (var r in resultGroupByPoRal)
                {
                    var chunkedList = r.Chunk(300);

                    foreach (var list in chunkedList)
                    {
                        var ralId = Guid.NewGuid().ToString();
                        var ral = new MecopDto()
                        {
                            Id = ralId,
                            Title = r.Key,
                            ParentId = poId,
                            NodeLevel = "Colour",
                            Status = r.First().Status
                        };
                        meCop.Add(ral);

                        meCop.AddRange(list.Select(c => new MecopDto()
                        {
                            Id = c.Id,
                            AankoopOrder = c.AankoopOrder,
                            Lijn = c.Lijn,
                            BrandWeerstand = c.BrandWeerstand,
                            Type = c.Type,
                            Materiaal = c.Materiaal,
                            Grondlaag = c.Grondlaag,
                            Ral = c.Ral,
                            Ncs = c.Ncs,
                            MuurDikte = c.MuurDikte,
                            BreedteDeurlijsten = c.BreedteDeurlijsten,
                            BreedteDeurlijstenVerticaal = c.BreedteDeurlijstenVerticaal,
                            BreedteDeurlijstenHorizontaal = c.BreedteDeurlijstenHorizontaal,
                            OmlijstingenNummeren = c.OmlijstingenNummeren,
                            HoogteDeur = c.HoogteDeur,
                            BreedteDeur = c.BreedteDeur,
                            BreedteVleugel = c.BreedteVleugel,
                            DikteDeur = c.DikteDeur,
                            DraaiRichting = c.DraaiRichting,
                            Paumelles = c.Paumelles,
                            AantalPaumelles = c.AantalPaumelles,
                            TegenPlaat = c.TegenPlaat,
                            SlotUitsparing = c.SlotUitsparing,
                            KrukHoogte = c.KrukHoogte,
                            KabelDoorvoer = c.KabelDoorvoer,
                            HoogteKabelDoorvoer = c.HoogteKabelDoorvoer,
                            Extra1 = c.Extra1,
                            Extra2 = c.Extra2,
                            Extra3 = c.Extra3,
                            Extra4 = c.Extra4,
                            DeurNummer = c.DeurNummer,
                            Aantal = c.Aantal,
                            Eenheidsprijs = c.Eenheidsprijs,
                            TotaalPrijs = c.TotaalPrijs,
                            Taal = c.Taal,
                            Created = c.Created,
                            Modified = c.Modified,
                            Status = c.Status,
                            Klant = c.Klant,
                            Norm = c.Norm,
                            ParentId = ralId,
                            NodeLevel = "LastNode"
                        }));
                    }
                }
            }
            
            return meCop;
        }
        catch (Exception e)
        {
            //Console.WriteLine(e);
            throw;
        }
        
    }

     public async Task<List<MecopDto>> GetMecopForExelNew(MecopParameter mecopParameter)
    {
        try
        {
            await using var connection = new SqlConnection(mecopParameter.TenantProvider.GetTenant().ConnectionString);

            var query = @"SELECT Id ,AankoopOrder ,Lijn ,BrandWeerstand ,Type ,Materiaal ,Grondlaag ,Ral ,Ncs ,MuurDikte ,BreedteDeurlijsten ,BreedteDeurlijstenVerticaal ,BreedteDeurlijstenHorizontaal ,OmlijstingenNummeren ,HoogteDeur ,BreedteDeur ,BreedteVleugel ,DikteDeur ,DraaiRichting ,Paumelles ,AantalPaumelles ,TegenPlaat ,SlotUitsparing ,KrukHoogte ,KabelDoorvoer ,HoogteKabelDoorvoer ,Extra1 ,Extra2 ,Extra3 ,Extra4 ,DeurNummer ,Aantal ,Eenheidsprijs ,TotaalPrijs ,Taal ,Created ,Modified ,Status ,Klant ,Norm FROM dbo.CustomerOrderToPo where CustomerOrderToPo.Status != 'Finished'";

            var result = connection.Query<MecopDto>(query).ToList();
            
            var conversion = connection.Query<MecopConversion>("SELECT * FROM dbo.CustomerOrderToPoConversion order by [Key]").ToList();

            var meCop = new List<MecopDto>();

            var resultGroupByPo = result.GroupBy(e => e.AankoopOrder).ToList();

            foreach (var me in resultGroupByPo)
            {
                var count = 1;
                var poId = Guid.NewGuid().ToString();
                var po = new MecopDto()
                {
                    Id = poId,
                    Title = me.Key,
                    NodeLevel = "PurchaseOrder",
                    Status = me.First().Status
                    
                };
                meCop.Add(po);

                var resultGroupByPoRal = me.GroupBy(e => e.Ral).ToList();
                foreach (var r in resultGroupByPoRal)
                {
                    int chunk = 6;
                    var chunkedList = r.Chunk(chunk);
                    
                    foreach (var list in chunkedList)
                    {
                        var ralId = Guid.NewGuid().ToString();
                        var ral = new MecopDto()
                        {
                            Id = ralId,
                            Title = r.Key + " (" + count + "-" + (count + list.Length - 1) + "/" + me.Count() + ")",
                            ParentId = poId,
                            NodeLevel = "Colour",
                            Status = r.First().Status
                        };

                        count += chunk;
                        
                        meCop.Add(ral);

                        // foreach (var c in list)
                        // {
                        //     if (c.Id == "1067")
                        //     {
                        //         var Materiaal = conversion
                        //             .Where(e =>
                        //                 e.Customer == c.Klant && e.Attribute == "Materiaal" && e.Language == c.Taal).Where(e => e.Key == c.Materiaal || e.Key == c.Materiaal + " " + c.Norm )
                        //             .Select(e => e.Value).FirstOrDefault()?.ToString() ?? c.Materiaal;
                        //
                        //     }
                        //
                        // }
                        meCop.AddRange(list.Select(c => new MecopDto()
                        {
                            Id = c.Id,
                            AankoopOrder = c.AankoopOrder,
                            Lijn = conversion
                                .Where(e => e.Customer == c.Klant && string.Equals(e.Attribute, "Lijn", StringComparison.CurrentCultureIgnoreCase) && e.Language == c.Taal && e.Key == c.Lijn)
                                .Select(e => e.Value)
                                .FirstOrDefault()?.ToString() ?? c.Lijn,
                        
                            BrandWeerstand = conversion
                                .Where(e => e.Customer == c.Klant && string.Equals(e.Attribute, "BrandWeerstand", StringComparison.CurrentCultureIgnoreCase) && e.Language == c.Taal && string.Equals(e.Norm, c.Norm, StringComparison.CurrentCultureIgnoreCase) && e.Key == c.BrandWeerstand)
                                .Select(e => e.Value)
                                .FirstOrDefault()?.ToString() ?? c.BrandWeerstand,
                        
                            Type = conversion
                                .Where(e => e.Customer == c.Klant && string.Equals(e.Attribute, "Type", StringComparison.CurrentCultureIgnoreCase) && e.Language == c.Taal && string.Equals(e.Norm, c.Norm, StringComparison.CurrentCultureIgnoreCase) && e.Key == c.Type)
                                .Select(e => e.Value)
                                .FirstOrDefault()?.ToString() ?? c.Type,
                            
                            DraaiRichting = conversion
                                .Where(e => e.Customer == c.Klant && string.Equals(e.Attribute, "DraaiRichting", StringComparison.CurrentCultureIgnoreCase) && e.Language == c.Taal && string.Equals(e.Norm, c.Norm, StringComparison.CurrentCultureIgnoreCase) && e.Key == c.DraaiRichting)
                                .Select(e => e.Value)
                                .FirstOrDefault()?.ToString() ?? c.DraaiRichting,
                        
                            Materiaal = conversion
                                .Where(e => e.Customer == c.Klant && string.Equals(e.Attribute, "Materiaal", StringComparison.CurrentCultureIgnoreCase) && e.Language == c.Taal && e.Key == c.Materiaal)
                                .Select(e => e.Value)
                                .FirstOrDefault()?.ToString() ?? c.Materiaal,
                        
                            Grondlaag = conversion
                                .Where(e => e.Customer == c.Klant && string.Equals(e.Attribute, "Grondlaag", StringComparison.CurrentCultureIgnoreCase) && e.Language == c.Taal && e.Key == c.Grondlaag)
                                .Select(e => e.Value)
                                .FirstOrDefault()?.ToString() ?? c.Grondlaag,
                        
                            Ral = conversion
                                .Where(e => e.Customer == c.Klant && string.Equals(e.Attribute, "Ral", StringComparison.CurrentCultureIgnoreCase) && e.Language == c.Taal && e.Key == c.Ral)
                                .Select(e => e.Value)
                                .FirstOrDefault()?.ToString() ?? c.Ral,
                        
                            Ncs = conversion
                                .Where(e => e.Customer == c.Klant && string.Equals(e.Attribute, "Ncs", StringComparison.CurrentCultureIgnoreCase) && e.Language == c.Taal && e.Key == c.Ncs)
                                .Select(e => e.Value)
                                .FirstOrDefault()?.ToString() ?? c.Ncs,
                        
                            MuurDikte = conversion
                                .Where(e => e.Customer == c.Klant && string.Equals(e.Attribute, "MuurDikte", StringComparison.CurrentCultureIgnoreCase) && e.Language == c.Taal && e.Key == c.MuurDikte)
                                .Select(e => e.Value)
                                .FirstOrDefault()?.ToString() ?? c.MuurDikte,
                        
                            BreedteDeurlijsten = conversion
                                .Where(e => e.Customer == c.Klant && string.Equals(e.Attribute, "BreedteDeurlijsten", StringComparison.CurrentCultureIgnoreCase) && e.Language == c.Taal && e.Key == c.BreedteDeurlijsten)
                                .Select(e => e.Value)
                                .FirstOrDefault()?.ToString() ?? c.BreedteDeurlijsten,
                        
                            BreedteDeurlijstenVerticaal = conversion
                                .Where(e => e.Customer == c.Klant && string.Equals(e.Attribute, "BreedteDeurlijstenVerticaal", StringComparison.CurrentCultureIgnoreCase) && e.Language == c.Taal && e.Key == c.BreedteDeurlijstenVerticaal)
                                .Select(e => e.Value)
                                .FirstOrDefault()?.ToString() ?? c.BreedteDeurlijstenVerticaal,
                        
                            BreedteDeurlijstenHorizontaal = conversion
                                .Where(e => e.Customer == c.Klant && string.Equals(e.Attribute, "BreedteDeurlijstenHorizontaal", StringComparison.CurrentCultureIgnoreCase) && e.Language == c.Taal && e.Key == c.BreedteDeurlijstenHorizontaal)
                                .Select(e => e.Value)
                                .FirstOrDefault()?.ToString() ?? c.BreedteDeurlijstenHorizontaal,
                            
                            OmlijstingenNummeren = c.OmlijstingenNummeren,
                            
                            HoogteDeur = conversion
                                .Where(e => e.Customer == c.Klant && string.Equals(e.Attribute, "HoogteDeur", StringComparison.CurrentCultureIgnoreCase) && e.Language == c.Taal && e.Key == c.HoogteDeur)
                                .Select(e => e.Value)
                                .FirstOrDefault()?.ToString() ?? c.HoogteDeur,
                        
                            BreedteDeur = conversion
                                .Where(e => e.Customer == c.Klant && string.Equals(e.Attribute, "BreedteDeur", StringComparison.CurrentCultureIgnoreCase) && e.Language == c.Taal && e.Key == c.BreedteDeur)
                                .Select(e => e.Value)
                                .FirstOrDefault()?.ToString() ?? c.BreedteDeur,
                        
                            BreedteVleugel = conversion
                                .Where(e => e.Customer == c.Klant && string.Equals(e.Attribute, "BreedteVleugel", StringComparison.CurrentCultureIgnoreCase) && e.Language == c.Taal && e.Key == c.BreedteVleugel)
                                .Select(e => e.Value)
                                .FirstOrDefault()?.ToString() ?? c.BreedteVleugel,
                        
                            DikteDeur = conversion
                                .Where(e => e.Customer == c.Klant && string.Equals(e.Attribute, "DikteDeur", StringComparison.CurrentCultureIgnoreCase) && e.Language == c.Taal && e.Key == c.DikteDeur)
                                .Select(e => e.Value)
                                .FirstOrDefault()?.ToString() ?? c.DikteDeur,
                            
                            Paumelles = conversion
                                .Where(e => e.Customer == c.Klant && string.Equals(e.Attribute, "Paumelles", StringComparison.CurrentCultureIgnoreCase) && e.Language == c.Taal && e.Key == c.Paumelles)
                                .Select(e => e.Value)
                                .FirstOrDefault()?.ToString() ?? c.Paumelles,
                        
                            AantalPaumelles = conversion
                                .Where(e => e.Customer == c.Klant && string.Equals(e.Attribute, "AantalPaumelles", StringComparison.CurrentCultureIgnoreCase) && e.Language == c.Taal && e.Key == c.AantalPaumelles)
                                .Select(e => e.Value)
                                .FirstOrDefault()?.ToString() ?? c.AantalPaumelles,
                        
                            TegenPlaat = conversion
                                .Where(e => e.Customer == c.Klant && string.Equals(e.Attribute, "TegenPlaat", StringComparison.CurrentCultureIgnoreCase) && e.Language == c.Taal && e.Key == c.TegenPlaat)
                                .Select(e => e.Value)
                                .FirstOrDefault()?.ToString() ?? c.TegenPlaat,
                        
                            SlotUitsparing = conversion
                                .Where(e => e.Customer == c.Klant && string.Equals(e.Attribute, "SlotUitsparing", StringComparison.CurrentCultureIgnoreCase) && e.Language == c.Taal && e.Key == c.SlotUitsparing)
                                .Select(e => e.Value)
                                .FirstOrDefault()?.ToString() ?? c.SlotUitsparing,
                        
                            KrukHoogte = conversion
                                .Where(e => e.Customer == c.Klant && string.Equals(e.Attribute, "KrukHoogte", StringComparison.CurrentCultureIgnoreCase) && e.Language == c.Taal && e.Key == c.KrukHoogte)
                                .Select(e => e.Value)
                                .FirstOrDefault()?.ToString() ?? c.KrukHoogte,
                        
                            KabelDoorvoer = conversion
                                .Where(e => e.Customer == c.Klant && string.Equals(e.Attribute, "KabelDoorvoer", StringComparison.CurrentCultureIgnoreCase) && e.Language == c.Taal && e.Key == c.KabelDoorvoer)
                                .Select(e => e.Value)
                                .FirstOrDefault()?.ToString() ?? c.KabelDoorvoer,
                        
                            HoogteKabelDoorvoer = conversion
                                .Where(e => e.Customer == c.Klant && string.Equals(e.Attribute, "HoogteKabelDoorvoer", StringComparison.CurrentCultureIgnoreCase) && e.Language == c.Taal && e.Key == c.HoogteKabelDoorvoer)
                                .Select(e => e.Value)
                                .FirstOrDefault()?.ToString() ?? c.HoogteKabelDoorvoer,
                        
                            Extra1 = conversion
                                .Where(e => e.Customer == c.Klant && string.Equals(e.Attribute, "Extra1", StringComparison.CurrentCultureIgnoreCase) && e.Language == c.Taal && e.Key == c.Extra1)
                                .Select(e => e.Value)
                                .FirstOrDefault()?.ToString() ?? c.Extra1,
                        
                            Extra2 = conversion
                                .Where(e => e.Customer == c.Klant && string.Equals(e.Attribute, "Extra2", StringComparison.CurrentCultureIgnoreCase) && e.Language == c.Taal && e.Key == c.Extra2)
                                .Select(e => e.Value)
                                .FirstOrDefault()?.ToString() ?? c.Extra2,
                        
                            Extra3 = conversion
                                .Where(e => e.Customer == c.Klant && string.Equals(e.Attribute, "Extra3", StringComparison.CurrentCultureIgnoreCase) && e.Language == c.Taal && e.Key == c.Extra3)
                                .Select(e => e.Value)
                                .FirstOrDefault()?.ToString() ?? c.Extra3,
                        
                            Extra4 = conversion
                                .Where(e => e.Customer == c.Klant && string.Equals(e.Attribute, "Extra4", StringComparison.CurrentCultureIgnoreCase) && e.Language == c.Taal && e.Key == c.Extra4)
                                .Select(e => e.Value)
                                .FirstOrDefault()?.ToString() ?? c.Extra4,
                        
                            DeurNummer = conversion
                                .Where(e => e.Customer == c.Klant && string.Equals(e.Attribute, "DeurNummer", StringComparison.CurrentCultureIgnoreCase) && e.Language == c.Taal && e.Key == c.DeurNummer)
                                .Select(e => e.Value)
                                .FirstOrDefault()?.ToString() ?? c.DeurNummer,
                        
                            Aantal = conversion
                                .Where(e => e.Customer == c.Klant && string.Equals(e.Attribute, "Aantal", StringComparison.CurrentCultureIgnoreCase) && e.Language == c.Taal && e.Key == c.Aantal)
                                .Select(e => e.Value)
                                .FirstOrDefault()?.ToString() ?? c.Aantal,
                        
                            Eenheidsprijs = conversion
                                .Where(e => e.Customer == c.Klant && string.Equals(e.Attribute, "Eenheidsprijs", StringComparison.CurrentCultureIgnoreCase) && e.Language == c.Taal && e.Key == c.Eenheidsprijs)
                                .Select(e => e.Value)
                                .FirstOrDefault()?.ToString() ?? c.Eenheidsprijs,
                        
                            TotaalPrijs = conversion
                                .Where(e => e.Customer == c.Klant && string.Equals(e.Attribute, "TotaalPrijs", StringComparison.CurrentCultureIgnoreCase) && e.Language == c.Taal && e.Key == c.TotaalPrijs)
                                .Select(e => e.Value)
                                .FirstOrDefault()?.ToString() ?? c.TotaalPrijs,
                        
                            Taal = conversion
                                .Where(e => e.Customer == c.Klant && string.Equals(e.Attribute, "Taal", StringComparison.CurrentCultureIgnoreCase) && e.Language == c.Taal && e.Key == c.Taal)
                                .Select(e => e.Value)
                                .FirstOrDefault()?.ToString() ?? c.Taal,
                            
                            Created = c.Created,
                            
                            Modified = c.Modified,
                            
                            Status = conversion
                                .Where(e => e.Customer == c.Klant && string.Equals(e.Attribute, "Status", StringComparison.CurrentCultureIgnoreCase) && e.Language == c.Taal && e.Key == c.Status)
                                .Select(e => e.Value)
                                .FirstOrDefault()?.ToString() ?? c.Status,
                        
                            Klant = c.Klant,
                            Norm = c.Norm,
                            ParentId = ralId,
                            NodeLevel = "LastNode"
                        }));
                        
                    }
                }
            }
            
            return meCop;
        }
        catch (Exception e)
        {
            //Console.WriteLine(e);
            throw;
        }
        
    }
     
    public async Task<List<MecopMetaData>> GetMecopMetaDataForExel(MecopParameter mecopParameter)
    {
        await using var connection = new SqlConnection(mecopParameter.TenantProvider.GetTenant().ConnectionString);

        var result = connection.Query<MecopMetaData>(@"SELECT * FROM dbo.CustomerOrderToPoMetaData").ToList();

        return result;
    }
    
    public async Task<List<string>> MecopStatusUpdate(MecopParameter mecopParameter)
    {
        try
        {
            await using var connection = new SqlConnection(mecopParameter.TenantProvider.GetTenant().ConnectionString);

            var idList = new List<string>();
            if (mecopParameter.MecopStatusUpdateDto.NodeLevel == "Colour")
            {
                idList = connection
                    .Query<string>(@"SELECT Id FROM dbo.CustomerOrderToPo WHERE AankoopOrder = @ParentTitle AND Ral = @Ral",
                        new {mecopParameter.MecopStatusUpdateDto.ParentTitle,Ral = mecopParameter.MecopStatusUpdateDto.Title}).ToList();
            }
            else
            {
                idList = connection.Query<string>(@"SELECT Id FROM dbo.CustomerOrderToPo WHERE AankoopOrder = @AankoopOrder",new {AankoopOrder = mecopParameter.MecopStatusUpdateDto.Title}).ToList();
            }

            var status = new StatusUpdate()
            {
                Status = "Aangemaakt"
            };
            
            if (mecopParameter.MecopStatusUpdateDto.IsFinished)
            {
                status.Status = "Finished";
            }

            foreach (var i in idList)
            {
                var client = new HttpClient();
                client.BaseAddress = new Uri("https://tokio.potteau.com/");
        
                string base64Credentials = Convert.ToBase64String(Encoding.ASCII.GetBytes($"{"Mickie"}:{"MsService01!"}"));
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", base64Credentials);
                var payload = JsonSerializer.Serialize(status);
                var content = new StringContent(payload, Encoding.UTF8, "application/json");
            
                var response = await client.PatchAsync("odata/omlijsting/"+i+"",content);
            }

            await GetMecopData(mecopParameter);
            return idList;

        }
        catch (Exception e)
        {
            //Console.WriteLine(e);
            throw;
        }
    }
    
    public class StatusUpdate
    {
        public string Status { get; set; }
    }
}