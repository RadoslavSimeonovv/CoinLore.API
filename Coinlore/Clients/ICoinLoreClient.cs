using Dynamo_Coinlore.Models;

namespace Dynamo_Coinlore.Clients
{
    public interface ICoinLoreClient
    {
        Task<TickersResponseModel?> GetCoinsInfo();
    }
}
