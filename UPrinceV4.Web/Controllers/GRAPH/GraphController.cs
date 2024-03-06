using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Azure.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Graph;
using Microsoft.Graph.Models;
using Newtonsoft.Json;
using UPrinceV4.Shared;
using UPrinceV4.Web.Models;
using UPrinceV4.Web.Repositories.Interfaces;

namespace UPrinceV4.Web.Controllers.GRAPH;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class GraphController : ControllerBase
{
    private readonly GraphServiceClient _graphServiceClient;
    private readonly IGraphRepository _iGraphRepository;
    private readonly ITenantProvider _tenantProvider;

    public GraphController(GraphServiceClient graphServiceClient, ITenantProvider tenantProvider, IGraphRepository iGraphRepository)
    {
        _graphServiceClient = graphServiceClient;
        _tenantProvider = tenantProvider;
        _iGraphRepository = iGraphRepository;
    }

    /// <summary>
    ///     Get the user's profile from MS Graph
    /// </summary>
    /// <returns></returns>
    
    // public async Task<string> Get()
    // {
    //     //HttpContext.VerifyUserHasAnyAcceptedScope(scopeRequiredByApi);
    //
    //     var userProfile = await _graphServiceClient.Me
    //         .GetAsync();
    //
    //     return JsonConvert.SerializeObject(userProfile);
    // }


    [HttpGet("SendInvitation/{PersonId}")]
    public async Task<ActionResult> SendInvitation(string PersonId)
    {
        try
        {
            var _graphParameter = new GraphParameter
            {
                Id = PersonId,
                TenantProvider = _tenantProvider,
                GraphServiceClient = _graphServiceClient
            };

            return Ok(new ApiOkResponse(await _iGraphRepository.SendInvitation(_graphParameter)));
        }
        catch (Exception e)
        {
            //Console.WriteLine(e);
            throw;
        }
    }

    [HttpGet("SendInvitationTest/{PersonId}")]
    public async Task<ActionResult> SendInvitationTest()
    {
        try
        {
            var invitation = new Invitation
            {
                InvitedUserEmailAddress = "ulameri22@gmail.com",
                InviteRedirectUrl = "https://bmengineering.uprince.com",
                InvitedUserMessageInfo = new InvitedUserMessageInfo
                {
                    CustomizedMessageBody = "Welcome to UPrince"
                },
                SendInvitationMessage = true
            };

            var invite = await _graphServiceClient.Invitations.PostAsync(invitation);
            return Ok(new ApiOkResponse(invite));
        }
        catch (Exception e)
        {
            //Console.WriteLine(e);
            throw;
        }
    }
    //
    // [HttpGet("AddToGroup")]
    // public async Task<ActionResult> AddToGroup()
    // {
    //     try
    //     {
    //         var directoryObject = new DirectoryObject
    //         {
    //             Id = "1f94fa45-74ab-4d1c-a407-dd62bbea071e"
    //         };
    //
    //         await _graphServiceClient.Groups["37d3e33d-ff6f-4827-b7f2-32ed8a380c56"].Members.References
    //             .Request()
    //             .AddAsync(directoryObject);
    //
    //         return Ok(new ApiOkResponse("sucess"));
    //     }
    //     catch (Exception e)
    //     {
    //         //Console.WriteLine(e);
    //         throw;
    //     }
    // }

    // [HttpGet("SendEmail")]
    // public async Task<ActionResult> SendEmail()
    // {
    //     var message = new Message
    //     {
    //         Subject = "Meet for lunch?",
    //         Body = new ItemBody
    //         {
    //             ContentType = BodyType.Text,
    //             Content = "The new cafeteria is open."
    //         },
    //         ToRecipients = new List<Recipient>
    //         {
    //             new()
    //             {
    //                 EmailAddress = new EmailAddress
    //                 {
    //                     Address = "shanuka@mickiesoft.com"
    //                 }
    //             }
    //         },
    //         CcRecipients = new List<Recipient>
    //         {
    //             new()
    //             {
    //                 EmailAddress = new EmailAddress
    //                 {
    //                     Address = "achini@mickiesoft.com"
    //                 }
    //             },
    //             new()
    //             {
    //                 EmailAddress = new EmailAddress
    //                 {
    //                     Address = "wasana@mickiesoft.com"
    //                 }
    //             }
    //         }
    //     };
    //
    //     bool saveToSentItems;
    //     saveToSentItems = true;
    //
    //     await _graphServiceClient.Me
    //         .SendMail(message, false)
    //         .Request()
    //         .PostAsync();
    //
    //     return Ok(new ApiOkResponse("ok")); // OID reterns 
    // }


    // [HttpGet("Drive")]
    // public async Task<ActionResult> Drive()
    // {
    //     var children = await _graphServiceClient.Me.Drive.Root.Children
    //         .Request()
    //         .GetAsync();
    //
    //     return Ok(new ApiOkResponse(children)); // OID reterns 
    // }


    // [HttpPost("DriveCreate")]
    // public async Task<ActionResult> DriveCreate([FromForm] IFormCollection pdf)
    // {
    //     var file = pdf.Files.FirstOrDefault();
    //
    //     var uploadedItem = await _graphServiceClient
    //         .Drive
    //         .Root
    //         .ItemWithPath("lot/" + file.FileName)
    //         .Content
    //         .Request()
    //         .PutAsync<DriveItem>(file.OpenReadStream());
    //
    //     return Ok(new ApiOkResponse(uploadedItem)); // OID reterns 
    // }

    // [HttpPost("DriveCreateWithProperty")]
    // public async Task<ActionResult> DriveCreateWithProperty([FromForm] IFormCollection pdf)
    // {
    //     var file = pdf.Files.FirstOrDefault();
    //
    //     var uploadedItem = await _graphServiceClient
    //         .Drive
    //         .Root
    //         .ItemWithPath("lot/" + file.FileName)
    //         .Content
    //         .Request()
    //         .PutAsync<DriveItem>(file.OpenReadStream());
    //
    //     var fieldValueSet = new FieldValueSet
    //     {
    //         AdditionalData = new Dictionary<string, object>
    //         {
    //             { "Title", "test" },
    //             { "Project", "P0031" },
    //             { "Company", "Mickiesoft" },
    //             { "Location", "Sri Lanka" }
    //         }
    //     };
    //
    //     // await _graphServiceClient.Drives["{drive-id}"].Items["{driveitem-id}"].ListItem.Fields
    //     //     .Request()
    //     //     .UpdateAsync(fieldValueSet);
    //     await _graphServiceClient.Drive.Root.ItemWithPath("lot/" + file.FileName).ListItem.Fields
    //         .Request()
    //         .UpdateAsync(fieldValueSet);
    //     // var  contentTypes = await _graphServiceClient.Sites.Root.ContentTypes
    //     //     .Request()
    //     //     .GetAsync();
    //     // //await _graphServiceClient.Drive.Root.ItemWithPath("lot/"+file.FileName).Content
    //     // //    .Request()
    //     // //    .PutResponseAsync<>(fieldValueSet);
    //
    //     return Ok(new ApiOkResponse(uploadedItem)); // OID reterns 
    // }

    // [HttpPost("DriveUpdateWithProperty")]
    // public async Task<ActionResult> DriveUpdateWithProperty([FromForm] IFormCollection pdf)
    // {
    //     var file = pdf.Files.FirstOrDefault();
    //
    //     var uploadedItem = await _graphServiceClient
    //         .Drive
    //         .Root
    //         .ItemWithPath("LT-0917/" + file.FileName)
    //         .Content
    //         .Request()
    //         .PutAsync<DriveItem>(file.OpenReadStream());
    //
    //     var fieldValueSet = new FieldValueSet
    //     {
    //         AdditionalData = new Dictionary<string, object>
    //         {
    //             { "Title", "test" },
    //             { "Project", "P0031" },
    //             //{"Company", "Mickiesoft"},
    //             // {"Location", "Sri Lanka"}
    //
    //             { "Project_x0020_Title", "values.ProjectTitle" },
    //             { "CuTitle", "COM-0001" },
    //
    //             { "Company", "values.CompanyName" },
    //             { "Lot_x0020_Title", "lotName" },
    //             { "_DCDateCreated", DateTime.UtcNow },
    //             { "Amount", "total" },
    //             { "_Version", "1" },
    //             { "Progress_x0020_Statement_x0020_Title", "1" }
    //         }
    //     };
    //
    //     // await _graphServiceClient.Drives["{drive-id}"].Items["{driveitem-id}"].ListItem.Fields
    //     //     .Request()
    //     //     .UpdateAsync(fieldValueSet);
    //
    //     var uploadedItemX = await _graphServiceClient.Drive.Root.ItemWithPath("LT-0917/" + file.FileName).ListItem
    //         .Fields
    //         .Request()
    //         .UpdateAsync(fieldValueSet);
    //
    //
    //     return Ok(new ApiOkResponse(uploadedItem)); // OID reterns 
    // }

    // [HttpGet("GetDriveItem")]
    // public async Task<ActionResult> GetDriveItem()
    // {
    //     var uploadedItem = await _graphServiceClient.Drives.Root.ItemWithPath("LT-0917/bmEngineering migrations.txt")
    //         .Request()
    //         .Expand("listItem")
    //         .GetAsync();
    //
    //     return Ok(new ApiOkResponse(uploadedItem)); // OID reterns 
    // }

    // [HttpPost("GetContentTypes")]
    // public async Task<ActionResult> GetContentTypes()
    // {
    //     ISiteContentTypesCollectionPage contentTypes;
    //     contentTypes = await _graphServiceClient.Sites.Root.ContentTypes
    //         .Request()
    //         .GetAsync();
    //     return Ok(new ApiOkResponse(contentTypes)); // OID reterns 
    // }


    // [HttpPost("DriveCreateAndInvite")]
    // public async Task<ActionResult> DriveCreateAndInvite([FromForm] IFormCollection pdf)
    // {
    //     var file = pdf.Files.FirstOrDefault();
    //
    //     var uploadedItem = await _graphServiceClient
    //         .Drive
    //         .Root
    //         .ItemWithPath(file.FileName)
    //         .Content
    //         .Request()
    //         .PutAsync<DriveItem>(file.OpenReadStream());
    //
    //     return Ok(new ApiOkResponse(uploadedItem)); // OID reterns 
    // }


    [HttpGet("GetPhoto")]
    public async Task<string> GetPhoto()
    {
        var stream = await _graphServiceClient.Me.Photo.Content
            .GetAsync();


        if (stream != null)
        {
            var ms = new MemoryStream();
            stream.CopyTo(ms);
            var buffer = ms.ToArray();
            var resultI = Convert.ToBase64String(buffer);
            var imgDataURL = string.Format("data:image/png;base64,{0}", resultI);

            return JsonConvert.SerializeObject(imgDataURL);
        }

        return JsonConvert.SerializeObject("");
    }

    [HttpGet("GetGroups")]
    public async Task<ActionResult> GetGroups()
    {
        // var groups = await _graphServiceClient.Groups
        //     .Request()
        //     .Filter("assignedLicenses/any()")
        //     .Select("id,assignedLicenses")
        //     .GetAsync();

        var result = await _graphServiceClient.Groups.GetAsync((requestConfiguration) =>
        {
            requestConfiguration.QueryParameters.Count = true;
            requestConfiguration.QueryParameters.Filter = "assignedLicenses/any()";
            requestConfiguration.QueryParameters.Select = new string []{ "id","assignedLicenses" };
            
        });
        return Ok(result.Value);
    }


    // [HttpGet("CreateLink")]
    // public async Task<Permission> createLink()
    // {
    //     var type = "view";
    //
    //     var password = "ThisIsMyPrivatePassword";
    //
    //     var scope = "anonymous";
    //
    //     var result = await _graphServiceClient.Me.Drive.Items["{driveItem-id}"]
    //         .CreateLink(type, scope, null, password)
    //         .Request()
    //         .PostAsync();
    //
    //
    //     return result;
    // }

    [HttpGet("AddToTeams")]
    public async Task<ActionResult> AddToTeams()
    {
        var team = new Team
        {
            DisplayName = "My Sample Team",
            Description = "My Sample Team’s Description",
            AdditionalData = new Dictionary<string, object>
            {
                { "template@odata.bind", "https://graph.microsoft.com/v1.0/teamsTemplates('standard')" }
            }
        };

        // var result = await _graphServiceClient.Teams
        //     .AddAsync(team);
        var result = await _graphServiceClient.Teams
            .PostAsync(team);
        return Ok(new ApiOkResponse(result));
    }
    
    [AllowAnonymous]
    [HttpGet("Drive")]
    public async Task<ActionResult> Drive()
    {
        try
        {
            var scopes = new[] { "https://graph.microsoft.com/.default" };
            var tenantId = "3d438826-fdde-4b8b-89d1-1b9b4feeaa20";
            var clientId = "f9ec3629-065f-4065-9dee-f42c22ae74e5";
            var clientSecret = "zDo8Q~qJEhJmLFbg-jhF8IDsbDispsdJ8J_bMbPd";
            
            var clientSecretCredential = new ClientSecretCredential(
                tenantId, clientId, clientSecret);
            var graphServiceClient = new GraphServiceClient(clientSecretCredential, scopes);
            var result = await graphServiceClient.Drives.GetAsync();
            return Ok(new ApiOkResponse(result)); // O
        }
        catch (Exception ex)
        {
            return BadRequest(ex);
        }
    }
    
    [AllowAnonymous]
    [HttpGet("DriveItems")]
    public async Task<ActionResult> DriveItemso()
    {
        try
        {
            var scopes = new[] { "https://graph.microsoft.com/.default" };
            var tenantId = "3d438826-fdde-4b8b-89d1-1b9b4feeaa20";
            var clientId = "f9ec3629-065f-4065-9dee-f42c22ae74e5";
            var clientSecret = "zDo8Q~qJEhJmLFbg-jhF8IDsbDispsdJ8J_bMbPd";
            
            var clientSecretCredential = new ClientSecretCredential(
                tenantId, clientId, clientSecret);
            var graphServiceClient = new GraphServiceClient(clientSecretCredential, scopes);
            var result2 = await graphServiceClient.Drives.GetAsync();
            var result33 = await graphServiceClient.Drives[result2.Value[2].Id]
                .List.GetAsync();
            
            var result = await graphServiceClient.Drives[result2.Value[2].Id].Items[result33.Items[0].Id].Children.GetAsync();

            return Ok(new ApiOkResponse(result)); // O
        }
        catch (Exception ex)
        {
            return BadRequest(ex);
        }
    }

    [HttpGet("GetSiteList")]
    public async Task<ActionResult> GetSiteList()
    {
        // var uploadedItem =  await _graphServiceClient.Drive.Root.ItemWithPath("LT-0917/bmEngineering migrations.txt")
        //     .Request()
        //     .Expand("listItem")
        //     .GetAsync();

        var sites = await _graphServiceClient.Sites["root"]
            .GetAsync();

        return Ok(new ApiOkResponse(sites)); // OID reterns 
    }

    [HttpGet("GetListItems")]
    public async Task<ActionResult> GetListItems()
    {
        var sites = await _graphServiceClient.Sites["root"]
            .GetAsync();
    
    
        var lists = await _graphServiceClient.Sites[sites.Id].Lists
            .GetAsync();
    
        return Ok(new ApiOkResponse(lists.Value.Where(x => x.DisplayName == "Documents"))); // OID reterns 
    }

    // [HttpGet("GetColomnList")]
    // public async Task<ActionResult> GetColomnList()
    // {
    //     var sites = await _graphServiceClient.Sites["root"]
    //         .Request()
    //         .GetAsync();
    //
    //
    //     var lists = await _graphServiceClient.Sites[sites.Id].Lists
    //         .Request()
    //         .GetAsync();
    //
    //     var docList = lists.Where(x => x.DisplayName == "Documents").First();
    //
    //     var columns = await _graphServiceClient.Sites[sites.Id].Lists[docList.Id].Columns
    //         .Request()
    //         .GetAsync();
    //
    //     return Ok(new ApiOkResponse(columns)); // OID reterns 
    // }

    // [HttpPost("SendEmailTest")]
    // public async Task<ActionResult> SendEmailTest()
    // {
    //     ISiteContentTypesCollectionPage contentTypes;
    //
    //     var message = new Message
    //     {
    //         Subject = "Meet for lunch?",
    //         Body = new ItemBody
    //         {
    //             ContentType = BodyType.Text,
    //             Content = "The new cafeteria is open."
    //         },
    //         ToRecipients = new List<Recipient>
    //         {
    //             new()
    //             {
    //                 EmailAddress = new EmailAddress
    //                 {
    //                     Address = "shanuka@mickiesoft.com"
    //                 }
    //             }
    //         },
    //         CcRecipients = new List<Recipient>
    //         {
    //             new()
    //             {
    //                 EmailAddress = new EmailAddress
    //                 {
    //                     Address = "supun@mickiesoft.com"
    //                 }
    //             }
    //         }
    //     };
    //
    //     var saveToSentItems = false;
    //     await _graphServiceClient.Me.SendMail(message, saveToSentItems)
    //         .Request()
    //         .PostAsync();
    //
    //
    //     return Ok(new ApiOkResponse("ok")); // OID reterns 
    // }

    // [HttpGet("GetMembersOfTheGroups")]
    // public async Task<IGraphServiceGroupsCollectionPage> GetMembersOfTheGroups()
    // {
    //     var groups = await _graphServiceClient.Groups
    //         .Request()
    //         .Filter("assignedLicenses/any()")
    //         .Select("id,assignedLicenses")
    //         .GetAsync();
    //
    //     const string securityGroupName = "test";
    //     var securityGroup = await _graphServiceClient.Groups.Request().Filter($"displayName eq '{securityGroupName}'")
    //         .GetAsync();
    //     if (securityGroup != null && securityGroup.Count > 0)
    //     {
    //         var members = new List<DirectoryObject>();
    //         var securityGroupId = securityGroup[0].Id;
    //         var securityGroupMembers = await _graphServiceClient.Groups[securityGroupId].Members.Request().GetAsync();
    //         members.AddRange(securityGroupMembers.CurrentPage);
    //         while (securityGroupMembers.NextPageRequest != null)
    //         {
    //             securityGroupMembers = await securityGroupMembers.NextPageRequest.GetAsync();
    //             members.AddRange(securityGroupMembers.CurrentPage);
    //         }
    //
    //         foreach (var member in members) Console.WriteLine($"{member.Id} - {member.GetType().Name}");
    //     }
    //     else
    //     {
    //         Console.WriteLine($"Security group '{securityGroupName}' not found.");
    //     }
    //
    //     return groups;
    // }
    
    // [HttpGet("GetUserSecurityGroups")]
    // public async Task<ActionResult> GetUserSecurityGroups()
    // {
    //     var token =
    //         "eyJ0eXAiOiJKV1QiLCJhbGciOiJSUzI1NiIsImtpZCI6Ii1LSTNROW5OUjdiUm9meG1lWm9YcWJIWkdldyJ9.eyJhdWQiOiJkZTJlZDhmYy0yM2I1LTRjODctYWVmNi1jMDBlMzg1NTYwMTgiLCJpc3MiOiJodHRwczovL2xvZ2luLm1pY3Jvc29mdG9ubGluZS5jb20vM2Q0Mzg4MjYtZmRkZS00YjhiLTg5ZDEtMWI5YjRmZWVhYTIwL3YyLjAiLCJpYXQiOjE2OTU3MTg2NDAsIm5iZiI6MTY5NTcxODY0MCwiZXhwIjoxNjk1NzQ3NzQwLCJhY2N0IjoxLCJhaW8iOiJBVVFBdS84VUFBQUFqYWtYYThJZTNDZ2crS0lteXJFbUd6RVBnL1hUSS82dkhIWExwbHBBUE1ZU3FvQ3VycmE5RkVWcEJJa3pWb1kzUksrS3I4ZWthOUpydWRGWjc4OWVZQT09IiwiYXV0aF90aW1lIjoxNjk1NzE4ODk2LCJhenAiOiJkZTJlZDhmYy0yM2I1LTRjODctYWVmNi1jMDBlMzg1NTYwMTgiLCJhenBhY3IiOiIwIiwiZW1haWwiOiJzdXB1bkBtaWNraWVzb2Z0LmNvbSIsImZhbWlseV9uYW1lIjoiQ29zdGEiLCJnaXZlbl9uYW1lIjoiU3VwdW4iLCJpZHAiOiJodHRwczovL3N0cy53aW5kb3dzLm5ldC85ZDMzNmY3OC0yYjJjLTRlN2QtYmE2Mi0wNzlmNDdlYTEyNTEvIiwiaXBhZGRyIjoiMjQwMjpkMDAwOjgxM2M6NDAxMDo4MWExOjIwZTU6MmNkZDo1YzgwIiwibG9naW5faGludCI6Ik8uQ2lRNFpEVmpabUUwWmkweE5EZzFMVFEyTUdZdFltRTBNQzAzTVdFM056SXhOMlJtWmpRU0pEbGtNek0yWmpjNExUSmlNbU10TkdVM1pDMWlZVFl5TFRBM09XWTBOMlZoTVRJMU1Sb1VjM1Z3ZFc1QWJXbGphMmxsYzI5bWRDNWpiMjBnR2c9PSIsIm5hbWUiOiJTdXB1biBDb3N0YSIsIm9pZCI6ImEwZjRjMTRlLTcyYjktNDVjYi05NWQwLWYyNzdmNDVjMDkwNyIsInByZWZlcnJlZF91c2VybmFtZSI6InN1cHVuQG1pY2tpZXNvZnQuY29tIiwicmgiOiIwLkFZSUFKb2hEUGQ3OWkwdUowUnViVC02cUlQellMdDYxSTRkTXJ2YkFEamhWWUJpQ0FQcy4iLCJzY3AiOiJhY2Nlc3NfYXNfdXNlciIsInNpZCI6ImI5YzUwNzFhLTI0Y2YtNGQ1NS1iNjAyLTM3ZWNiOGZhZjUxZCIsInN1YiI6ImlHYl8xWXh3M1g0VHI2cGppOUs0X1NDTUYzcWRxUGRqN1RTVm1WYWdXeUkiLCJ0ZW5hbnRfY3RyeSI6IkJFIiwidGVuYW50X3JlZ2lvbl9zY29wZSI6IkVVIiwidGlkIjoiM2Q0Mzg4MjYtZmRkZS00YjhiLTg5ZDEtMWI5YjRmZWVhYTIwIiwidXRpIjoiM0U3bThjWmE1a3VGS2s0NmozTVNBQSIsInZlciI6IjIuMCIsInZlcmlmaWVkX3ByaW1hcnlfZW1haWwiOlsic3VwdW5AbWlja2llc29mdC5jb20iXSwidmVyaWZpZWRfc2Vjb25kYXJ5X2VtYWlsIjpbInN1cHVuQE1pY2tpZVNvZnQub25taWNyb3NvZnQuY29tIl0sInhtc190cGwiOiJlbiJ9.n8cuRkTBg_pH1nDDGPVwMahazYTkBsU0WWq8nWQjnmeWX-sljRTAKNqfQkUThwjYGV1eiGzHN7ym8A58OEHnAuuK_cWE_rCIdy2oY9a6Is8KdQ8E5_N5kTqUTYclxJgAzc5ZzLneji2G3SnIbNoxWg9VxrOCsxVeJdm7La9dCcuqOeHTm6yGGlhXvox0ayKzwAlmJ3g-C1rzzgBgb2tCYsxyc2A1pmsa_5bKhLETCWllbVbdfM2LhTv5apeFtZduBQfQISboWit49D9ZOLvCdY9XihfZis1WzZtaF6dBj7ln8ziD1StzhiW8x9RoCe8b5dnUk0-RuMRqBzmiFz_TkQ";
    //     var graphClient = new GraphServiceClient(
    //         new DelegateAuthenticationProvider(requestMessage =>
    //         {
    //             requestMessage.Headers.Authorization =
    //                 new AuthenticationHeaderValue("Bearer", token);
    //             return Task.CompletedTask;
    //         }));
    //
    //                 
    //     var groups = await graphClient.Me.MemberOf.Request()
    //         .GetAsync();
    //
    //     var securityGroups = groups.Where(x =>
    //     {
    //         var group = x as Group;
    //         return group is { SecurityEnabled: true };
    //     });
    //     return Ok(new ApiOkResponse(securityGroups)); // OID reterns 
    // }
}