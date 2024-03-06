using System;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using UPrinceV4.Web.Azure;

namespace UPrinceV4.Web.AzureOptions;

public static class AzureAdServiceCollectionExtensions
{
    public static AuthenticationBuilder AddAzureAdBearer(this AuthenticationBuilder builder)
    {
        return builder.AddAzureAdBearer(_ => { });
    }

    public static AuthenticationBuilder AddAzureAdBearer(this AuthenticationBuilder builder,
        Action<AzureAdOptions> configureOptions)
    {
        builder.Services.Configure(configureOptions);
        builder.Services.AddSingleton<IConfigureOptions<JwtBearerOptions>, ConfigureAzureOptions>();
        builder.AddJwtBearer();
        return builder;
    }

    private class ConfigureAzureOptions : IConfigureNamedOptions<JwtBearerOptions>
    {
        private readonly AzureAdOptions _azureOptions;

        public ConfigureAzureOptions(IOptions<AzureAdOptions> azureOptions)
        {
            _azureOptions = azureOptions.Value;
        }

        public void Configure(string name, JwtBearerOptions options)
        {
            options.Audience = _azureOptions.ClientId;
            options.Authority = _azureOptions.Authority;


            options.TokenValidationParameters.ValidateIssuer = false;
        }

        public void Configure(JwtBearerOptions options)
        {
            Configure(Options.DefaultName, options);
        }
    }
}