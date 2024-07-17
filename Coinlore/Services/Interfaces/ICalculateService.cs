using System.Text;

namespace Dynamo_Coinlore.Services.Interfaces
{
    public interface ICalculateService
    {
        Task<decimal> GetInitialPortfolioValue();

        Task<StringBuilder> CalculateCoinChangePercentage();

        Task<string> GetPortfolioChangePercentage();

        Task LivePortfolioChangePercentage(int minutesPeriod);
    }
}
