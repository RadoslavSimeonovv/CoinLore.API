using Dynamo_Coinlore.Clients;
using Dynamo_Coinlore.Services.Interfaces;
using System.Text;

namespace Dynamo_Coinlore.Services
{
    public class CalculateService(
        IFileService fileReader,
        ICoinLoreClient coinLoreClient) 
        : ICalculateService
    {
        private readonly IFileService _fileReader = fileReader;
        private readonly ICoinLoreClient _coinLoreClient = coinLoreClient;

        /// <summary>
        /// Calculate initial portfolio value
        /// </summary>
        /// <returns></returns>
        public async Task<decimal> GetInitialPortfolioValue()
            => await CalculateInitialPortfolioValue();

        /// <summary>
        /// Calculate coin change percentage
        /// </summary>
        /// <returns></returns>
        public async Task<StringBuilder> CalculateCoinChangePercentage()
        {
            var coinChangePercentageList = new StringBuilder();
            var coinsInfo = await _coinLoreClient.GetCoinsInfo().ConfigureAwait(false);
            var fileLines = await _fileReader.ReadCoinsFile().ConfigureAwait(false);

            if (coinsInfo != null)
            {
                foreach (var line in fileLines)
                {
                    var initialCoin = line.Split("|");
                    var currentCoin = coinsInfo.Data.SingleOrDefault(x => x.Symbol == initialCoin[1]);

                    if (currentCoin != null)
                    {
                        var changePercentage = (Convert.ToDecimal(currentCoin.PriceUsd) - Convert.ToDecimal(initialCoin[2])) * 100 / Convert.ToDecimal(initialCoin[2]);
                        await WriteInLogFile($"Calculated the {currentCoin.Symbol} coin change percentage betwen initial value {initialCoin[2]} and current value {currentCoin.PriceUsd} which is {Convert.ToInt32(changePercentage)}%\n");
                        coinChangePercentageList.Append($"Difference between intiial price {initialCoin[2]} and current price {currentCoin.PriceUsd} is {Convert.ToInt32(changePercentage)}%\n");
                    }
                }
            }

            return coinChangePercentageList;
        }

        /// <summary>
        /// Portofolio change percentage between initial and current
        /// </summary>
        /// <returns></returns>
        public async Task<string> GetPortfolioChangePercentage()
        {
            var calculation = await CalculatePortfolioChangePercentage().ConfigureAwait(false);
            return $"Difference between initial portfolio value {calculation.Item1} and current portfolio value {calculation.Item2} is {Convert.ToInt32(calculation.Item3)}%";
        }

        /// <summary>
        /// Live porfolio change percentage
        /// </summary>
        /// <param name="minutesPeriod"></param>
        /// <returns></returns>
        public async Task<StringBuilder> LivePortfolioChangePercentage(int minutesPeriod)
        {
            while (true)
            {
                var timer = new PeriodicTimer(TimeSpan.FromSeconds(minutesPeriod));

                while (await timer.WaitForNextTickAsync())
                {
                    await CalculatePortfolioChangePercentage().ConfigureAwait(false);
                }
            }
        }

        private async Task<Tuple<decimal, decimal, decimal>> CalculatePortfolioChangePercentage()
        {
            var initialPortfolioValue = await CalculateInitialPortfolioValue();
            var currentPortfolioValue = await CalculateCurrentPortfolioValue();
            var changePercentage = (Convert.ToDecimal(currentPortfolioValue) - Convert.ToDecimal(initialPortfolioValue)) * 100 / Convert.ToDecimal(initialPortfolioValue);

            await WriteInLogFile($"Calculated the portfolio change percentage between initial value {initialPortfolioValue} and current portfolio value {currentPortfolioValue} which is {Convert.ToInt32(changePercentage)}%\n");
            return new Tuple<decimal, decimal, decimal>(initialPortfolioValue, currentPortfolioValue, changePercentage);
        }

        private async Task<decimal> CalculateInitialPortfolioValue()
        {
            decimal initialPortfolioValue = 0;
            var fileLines = await _fileReader.ReadCoinsFile();

            foreach (var line in fileLines)
            {
                var initialCoin = line.Split("|");
                initialPortfolioValue += Convert.ToDecimal(initialCoin[0]) * Convert.ToDecimal(initialCoin[2]);
            }

            await WriteInLogFile($"Calculated initial portofolio value: {initialPortfolioValue}\n");
            return initialPortfolioValue;
        }

        private async Task<decimal> CalculateCurrentPortfolioValue()
        {
            decimal currentPortfolioValue = 0;

            var coinsInfo = await _coinLoreClient.GetCoinsInfo();
            var fileLines = await _fileReader.ReadCoinsFile();

            if (coinsInfo != null)
            {
                foreach (var line in fileLines)
                {
                    var initialCoin = line.Split("|");
                    var currentCoin = coinsInfo.Data.SingleOrDefault(x => x.Symbol == initialCoin[1]);

                    if (currentCoin != null)
                    {
                        currentPortfolioValue += Convert.ToDecimal(initialCoin[0]) * Convert.ToDecimal(currentCoin.PriceUsd);
                    }
                }
            }

            await WriteInLogFile($"Calculated current portofolio value: {currentPortfolioValue}\n");
            return currentPortfolioValue;
        }

        private async Task WriteInLogFile(string logMessage)
        {
            var path = Path.Combine(Environment.CurrentDirectory, @"logs.txt");
            await _fileReader.WriteInLogFile(path, logMessage);
        }
    }
}
