namespace NptExplorer.AzureFunctions.Models.Transient;

public class GraphToken
{
    public string token_type { get; set; }
    public int expires_in { get; set; }
    public int exp_expires_in { get; set; }
    public string access_token { get; set; }
}