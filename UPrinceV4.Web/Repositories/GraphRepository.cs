using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Graph.Models;
using UPrinceV4.Web.Data.CAB;
using UPrinceV4.Web.Repositories.Interfaces;

namespace UPrinceV4.Web.Repositories;

public class GraphRepository : IGraphRepository
{
    public async Task<bool> SendInvitation(GraphParameter GraphParameter)
    {
        CabPersonCompany CabPersonCompany;

        var checkOid =
            @"SELECT CabEmail.EmailAddress AS EmailId ,CabPersonCompany.Oid FROM dbo.CabPersonCompany  LEFT OUTER JOIN dbo.CabEmail ON CabPersonCompany.EmailId = CabEmail.Id WHERE CabPersonCompany.PersonId = @PersonId";

        var parm = new { PersonId = GraphParameter.Id };
        using (var connection = new SqlConnection(GraphParameter.TenantProvider.GetTenant().ConnectionString))
        {
            var Send = false;
            await connection.OpenAsync();
            CabPersonCompany = connection.Query<CabPersonCompany>(checkOid, parm).FirstOrDefault();

            if (CabPersonCompany != null)
                if (CabPersonCompany.Oid == null && CabPersonCompany.EmailId != null)
                {
                    var invitation = new Invitation
                    {
                        InvitedUserEmailAddress = CabPersonCompany.EmailId,
                        InviteRedirectUrl = "https://bmengineering.uprince.com",
                        InvitedUserMessageInfo = new InvitedUserMessageInfo
                        {
                            CustomizedMessageBody = "Welcome to UPrince"
                        },
                        SendInvitationMessage = true
                    };

                    var invite = await GraphParameter.GraphServiceClient.Invitations
                        .PostAsync(invitation);

                    var directoryObject = new DirectoryObject
                    {
                        Id = invite.InvitedUser.Id
                    };
                    var oidUpdate = @"UPDATE dbo.CabPersonCompany SET Oid = @Oid WHERE PersonId = @Id;";

                    await connection.ExecuteAsync(oidUpdate, new { Oid = invite.InvitedUser.Id, GraphParameter.Id });

                    // await GraphParameter.GraphServiceClient.Groups["37d3e33d-ff6f-4827-b7f2-32ed8a380c56"].Members
                    //     .References
                    //     .Request()
                    //     .AddAsync(directoryObject);
                    // await GraphParameter.GraphServiceClient.Groups["37d3e33d-ff6f-4827-b7f2-32ed8a380c56"].Members.r;
                    
                    Send = true;
                }

            return Send;
        }
    }
}