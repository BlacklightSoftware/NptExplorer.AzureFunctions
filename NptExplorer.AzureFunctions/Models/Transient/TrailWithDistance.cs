using Microsoft.Net.Http.Headers;

namespace NptExplorer.AzureFunctions.Models.Transient;

public class TrailWithDistance
{
    public Trail Trail { get; set; }
    public double Distance { get; set; }
}