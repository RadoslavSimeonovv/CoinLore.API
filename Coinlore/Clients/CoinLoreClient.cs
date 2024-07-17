using Dynamo_Coinlore.Models;

namespace Dynamo_Coinlore.Clients
{
    public class CoinLoreClient(IHttpClientFactory httpClientFactory) : ICoinLoreClient
    {
        private readonly IHttpClientFactory _httpClientFactory = httpClientFactory;

        /// <summary>
        /// Coinlore endpoint to fetch information about the coins
        /// </summary>
        /// <returns></returns>
        public async Task<TickersResponseModel?> GetCoinsInfo()
        {
            var client = _httpClientFactory.CreateClient("coinlore");
            var response = await client.GetFromJsonAsync<TickersResponseModel>("/api/tickers/");
         
            return response is null ? null : response;
        }
    }
}
