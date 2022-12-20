using System;
using System.Net;
using System.Threading.Tasks;
using Microsoft.Identity.Client;
using NptExplorer.AzureFunctions.Services.Abstract;

namespace NptExplorer.AzureFunctions.Services.Concrete;

public class GraphService : IGraphService
{
    private readonly IRequestProviderService _requestProviderService;
    private readonly string? _clientId;
    private readonly string? _clientSecret;
    private readonly string? _tenantId;
    private readonly string _url = "https://graph.microsoft.com/v1.0/users";

    public GraphService(IRequestProviderService requestProviderService)
    {
        _requestProviderService = requestProviderService;
        _clientId = Environment.GetEnvironmentVariable("MsGraphClientId");
        _clientSecret = Environment.GetEnvironmentVariable("MsGraphClientSecret");
        _tenantId = Environment.GetEnvironmentVariable("MsGraphTenantId");
    }

    public async Task<bool> DeleteAdUser(string userId)
    {
        try
        {
            var token = await GetToken();
            var result = await _requestProviderService.Delete($"{_url}/{userId}", token);

            return result == HttpStatusCode.NoContent;
        }
        catch
        {
            return false;
        }
    }

    private async Task<string> GetToken()
    {
        var app = ConfidentialClientApplicationBuilder.Create(_clientId)
            .WithAuthority(AzureCloudInstance.AzurePublic, _tenantId)
            .WithClientSecret(_clientSecret)
            .Build();

        var scopes = new[] { "https://graph.microsoft.com/.default" };

        var result = await app
            .AcquireTokenForClient(scopes)
            .ExecuteAsync()
            .ConfigureAwait(false);

        return result.AccessToken;
    }
}