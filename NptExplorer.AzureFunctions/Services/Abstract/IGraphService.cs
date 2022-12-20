using System.Threading.Tasks;

namespace NptExplorer.AzureFunctions.Services.Abstract;

public interface IGraphService
{
    Task<bool> DeleteAdUser(string userId);
}