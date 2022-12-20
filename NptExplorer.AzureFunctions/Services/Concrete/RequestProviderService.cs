using Newtonsoft.Json;
using NptExplorer.AzureFunctions.Services.Abstract;
using Polly;
using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace NptExplorer.AzureFunctions.Services.Concrete;

public class RequestProviderService : IRequestProviderService
{
    public Task<T> AttemptAndRetry<T>(Func<Task<T>> action, int numRetries = 3)
    {
        return Policy.Handle<HttpRequestException>().WaitAndRetryAsync(numRetries, PollyRetryAttempt).ExecuteAsync(action);

        static TimeSpan PollyRetryAttempt(int attemptNumber) => TimeSpan.FromMilliseconds(Math.Pow(2, attemptNumber));
    }

    public async Task<TReturn> Get<TReturn>(string url)
    {
        using var client = CreateHttpClient(url);
        var response = await client.GetAsync(url);
        return ConsumeResponse<TReturn>(response);
    }

    public async Task<HttpStatusCode> Delete(string url, string token)
    {
        using var client = CreateHttpClient(url);
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var response = await client.DeleteAsync(url);
        return response.StatusCode;
    }

    public async Task<TReturn> Post<T, TReturn>(string url)
    {
        using var client = CreateHttpClient(url);
        var response = await client.PostAsync(url, null);
        return ConsumeResponse<TReturn>(response);
    }

    private HttpClient CreateHttpClient(string url)
    {
        var httpClientHandler = new HttpClientHandler();

#if DEBUG
        httpClientHandler.ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => true; //no SSL check needed yet
#endif

        var httpClient = new HttpClient(httpClientHandler);
        httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

        return httpClient;
    }

    private T ConsumeResponse<T>(HttpResponseMessage hrm)
    {
        return hrm.IsSuccessStatusCode && hrm.StatusCode != HttpStatusCode.Conflict
            ? JsonConvert.DeserializeObject<T>(hrm.Content.ReadAsStringAsync().Result)
            :
            throw new Exception($"StatusCode: {hrm.StatusCode}");
    }
}