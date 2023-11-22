using MongoDB.Entities;
using SearchService.Models;

namespace SearchService.Service;

public class AuctionSvcHttpClient
{
    private readonly HttpClient _httpClient;
    private readonly IConfiguration _configuration;

    public AuctionSvcHttpClient(HttpClient httpClient, IConfiguration configuration)
    {
        _httpClient = httpClient;
        _configuration = configuration;
    }

    public async Task<List<Item>> GetItemsForSearchDb()
    {
        var lastUpdated = await DB.Find<Item, string>()
            .Sort(x => x.Descending(a => a.UpdatedAt))
            .Project(x => x.UpdatedAt.ToString())
            .ExecuteFirstAsync();
        var auctionSvcBaseUrl = _configuration["AuctionServiceUrl"];
        var requestUri = $"{auctionSvcBaseUrl}/api/auctions?date={lastUpdated}";
        return await _httpClient.GetFromJsonAsync<List<Item>>(requestUri);
    }
}
