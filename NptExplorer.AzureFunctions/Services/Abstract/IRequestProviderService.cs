using System;
using System.Net;
using System.Threading.Tasks;

namespace NptExplorer.AzureFunctions.Services.Abstract;

public interface IRequestProviderService
{
    Task<T> AttemptAndRetry<T>(Func<Task<T>> action, int numRetries = 3);
    Task<TReturn> Get<TReturn>(string url);
    Task<HttpStatusCode> Delete(string url, string token);
    Task<TReturn> Post<T, TReturn>(string url);
}