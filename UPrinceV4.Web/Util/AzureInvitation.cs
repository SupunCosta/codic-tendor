using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Graph.Models;
using Microsoft.Identity.Client;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace UPrinceV4.Web.Util;

public class AzureInvitation
{
    private AzureInvitation()
    {
    }

    public static string SendInvitation(string email, string dispalayName, string messageBody, string senderEmail,
        bool isAccessGranted, string connectionString)
    {
        var user = new InvitationNew
        {
            UserPrincipalName = email,
            AccountEnabled = true,
            DisplayName = dispalayName,
            MailNickname = "dispalayName",
            PasswordProfile = new PasswordProfile
            {
                ForceChangePasswordNextSignIn = true,
                Password = "upince!@#s"
            }
        };


        var invitation = CreateInvitation(email, dispalayName, messageBody, senderEmail, isAccessGranted);

        var result = SendInvitation(invitation, connectionString);
        return result;
    }

    public static async Task<string> GetUserByEmail(string email, string connectionString)
    {
        try
        {
            var accessToken = GenerateToken(connectionString);
            var httpClient = GetHttpClient(accessToken);
            var postResponse = httpClient.GetAsync("https://graph.microsoft.com/v1.0/users/").Result;
            var serverResponse = postResponse.Content.ReadAsStringAsync().Result;
            dynamic jsonObj = JsonConvert.DeserializeObject(serverResponse);
            var values = jsonObj["value"];
            foreach (JObject val in values)
            {
                var userMail = (string)val["mail"];

                if (!string.IsNullOrEmpty(userMail) && email.Equals(userMail))
                {
                    var oid = (string)val["id"];
                    return oid;
                }
            }

            return null;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    public static async Task<string> GetTokenForUserAsync()
    {
        try
        {
            var app = ConfidentialClientApplicationBuilder
                .Create("c902c336-b00e-4cc2-8ddb-6604be49eaec")
                .WithClientSecret("OY_CI2U.i0ka_v5~.cSPwim3c5114NdOh8")
                .WithAuthority(
                    "https://login.microsoftonline.com/7892c08a-ffcd-4a61-a996-72854b657d74/oauth2/v2.0/authorize")
                .Build();
            string[] Scopes = { "https://graph.microsoft.com/User.Read.All" };
            var result = app.AcquireTokenForClient(Scopes)
                .ExecuteAsync().Result;
            return result.AccessToken;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    public static string CreateUser(string email, string dispalayName, string messageBody, string senderEmail,
        bool isAccessGranted, string connectionString)
    {
        var user = new InvitationNew();
        user.UserPrincipalName = email;
        user.AccountEnabled = true;
        user.DisplayName = dispalayName;
        user.MailNickname = "dispalayName";
        user.PasswordProfile = new PasswordProfile
        {
            ForceChangePasswordNextSignIn = true,
            Password = "upince!@#s"
        };


        //var invitation = CreateInvitation(email, dispalayName, messageBody, senderEmail, isAccessGranted);

        var result = CreateUser(user, connectionString);
        return result;
    }

    private static Invitation CreateInvitation(string email, string dispalayName, string messageBody,
        string senderEmail, bool isAccessGranted)
    {
        var invitation = new Invitation();
        invitation.InvitedUserDisplayName = dispalayName;
        invitation.InvitedUserEmailAddress = email;
        invitation.InviteRedirectUrl = "https://uprincev4webdev.azurewebsites.net/";
        invitation.SendInvitationMessage = isAccessGranted;
        var info = new InvitedUserMessageInfo();
        var senderEmailObj = new EmailAddress();
        senderEmailObj.Address = senderEmail;
        var recipient = new Recipient { EmailAddress = senderEmailObj };
        info.CcRecipients = new List<Recipient> { recipient };
        info.CustomizedMessageBody = messageBody;
        invitation.InvitedUserMessageInfo = info;
        //invitation.InvitedUserType = "member";
        return invitation;
    }

    private static string CreateUser(InvitationNew invitation, string connectionString)
    {
        var accessToken = GenerateToken(connectionString);
        var httpClient = GetHttpClient(accessToken);
        HttpContent content = new StringContent(JsonConvert.SerializeObject(invitation));
        content.Headers.Add("ContentType", "application/json");
        var postResponse = httpClient.PostAsync("https://graph.microsoft.com/v1.0/users", content).Result;
        var serverResponse = postResponse.Content.ReadAsStringAsync().Result;

        return serverResponse;
    }

    private static string SendInvitation(Invitation invitation, string connectionString)
    {
        var accessToken = GenerateToken(connectionString);
        var httpClient = GetHttpClient(accessToken);
        HttpContent content = new StringContent(JsonConvert.SerializeObject(invitation));
        content.Headers.Add("ContentType", "application/json");
        var postResponse = httpClient.PostAsync("https://graph.microsoft.com/v1.0/invitations", content).Result;
        var serverResponse = postResponse.Content.ReadAsStringAsync().Result;
        return serverResponse;
    }

    private static HttpClient GetHttpClient(string accessToken)
    {
        var httpClient = new HttpClient();
        httpClient.Timeout = TimeSpan.FromSeconds(300);
        httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
        httpClient.DefaultRequestHeaders.Add("client-request-id", Guid.NewGuid().ToString());
        Console.WriteLine(
            "CorrelationID for the request: {0}",
            httpClient.DefaultRequestHeaders.GetValues("client-request-id").Single());
        return httpClient;
    }

    public static string GenerateToken(string connectionString)
    {
        var query = @"SELECT
                            UPrinceCustomerTenantsInfo.ClientId AS ClientId
                            ,UPrinceCustomerTenantsInfo.ClientSecretKey AS ClientSecretKey
                            ,UPrinceCustomerTenantsInfo.TenantId AS TenantId
                            FROM dbo.UPrinceCustomerTenantsInfo
                            WHERE UPrinceCustomerTenantsInfo.CatelogConnectionString = @connectionString";

        var parameters = new { connectionString };
        InvitationConfigData invitationConfigData = null;
        using (var dbConnection = new SqlConnection(connectionString))
        {
            invitationConfigData = dbConnection.Query<InvitationConfigData>(query, parameters)
                .FirstOrDefault();
            
        }

        var audienceURL = "https://graph.microsoft.com/.default";
        var TokenUrl = "https://login.microsoftonline.com/" + invitationConfigData.TenantId + "/oauth2/v2.0/token";

        var webClient = new WebClient();
        webClient.Headers[HttpRequestHeader.CacheControl] = "no-cache";
        webClient.Headers[HttpRequestHeader.ContentType] = "application/x-www-form-urlencoded";
        webClient.Headers[HttpRequestHeader.Cookie] =
            "flight-uxoptin=true; stsservicecookie=ests; x-ms-gateway-slice=productionb; stsservicecookie=ests";
        var para = "client_id=" + invitationConfigData.ClientId +
                   "&grant_type=client_credentials&client_secret=" + invitationConfigData.ClientSecretKey +
                   "&scope=" + audienceURL;
        var response = webClient.UploadString(TokenUrl, "POST", para);
        dynamic jsonObj = JsonConvert.DeserializeObject(response);
        string token = jsonObj.access_token;
        return token;
    }


    public async Task<string> GenerateTokenWithGraphClient()
    {
        try
        {
            var authentication = new
            {
                Authority =
                    "https://uprinceusermanagementprod.b2clogin.com/7892c08a-ffcd-4a61-a996-72854b657d74/B2C_1_Web_v4_signup",
                Directory = "7892c08a-ffcd-4a61-a996-72854b657d74", /* tenant id */
                Application = "93049a78-e107-4ed4-9ae8-c1ce48f51179", /* client id */
                ClientSecret = "O.-~2iFz1v.zu2EcCj35CpLZmO3U.L8yzL"
            };

            var app = ConfidentialClientApplicationBuilder.Create(authentication.Application)
                .WithClientSecret(authentication.ClientSecret)
                .WithAuthority(AzureCloudInstance.AzurePublic, authentication.Directory)
                .Build();

            var scopes = new[] { "https://graph.microsoft.com/.default" };
            //var scopes = new[] { "https://graph.microsoft.com/User.Read.All" };

            var authenticationResult = await app.AcquireTokenForClient(scopes)
                .ExecuteAsync();
            return authenticationResult.AccessToken;
        }
        catch (Exception ex)
        {
            return ex.ToString();
        }
    }
}

public class Invitation
{
    public Invitation()
    {
        InvitedUserMessageInfo = new InvitedUserMessageInfo();
    }

    public string InvitedUserDisplayName { get; set; }
    public string InvitedUserEmailAddress { get; set; }
    public bool SendInvitationMessage { get; set; }
    public string InviteRedirectUrl { get; set; }
    public string InvitedUserType { get; set; }
    public InvitedUserMessageInfo InvitedUserMessageInfo { get; set; }
}

public class InvitationConfigData
{
    public string ClientId { get; set; }
    public string ClientSecretKey { get; set; }
    public string TenantId { get; set; }
}

public class InvitationNew
{
    public bool AccountEnabled { get; set; }
    public string DisplayName { get; set; }
    public string MailNickname { get; set; }
    public string UserPrincipalName { get; set; }
    public PasswordProfile PasswordProfile { get; set; }
}

public class InvitationNew2
{
    public bool AccountEnabled { get; set; }
    public string DisplayName { get; set; }
    public string MailNickname { get; set; }
    public string UserPrincipalName { get; set; }
    public string city { get; set; }
    public string country { get; set; }
    public string department { get; set; }
    public string givenName { get; set; }
    public string jobTitle { get; set; }
    public string mailNickname { get; set; }
    public string passwordPolicies { get; set; }
    public string officeLocation { get; set; }

    public PasswordProfile PasswordProfile { get; set; }
}