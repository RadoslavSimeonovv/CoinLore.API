using Dynamo_Coinlore.Models;
using Dynamo_Coinlore.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Dynamo_Coinlore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CoinLoreController(
        ICalculateService calculateService
        ) : ControllerBase
    {
        private readonly ICalculateService _calculateService = calculateService;

        [HttpGet("initialPortfolio")]
        [ProducesResponseType(typeof(decimal), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetInitialPortfolioValueAsync()
        {
            var response = await _calculateService.GetInitialPortfolioValue();

            return Ok(response);
        }

        [HttpGet("coinPercentageChange")]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetCoinPercentageChangeAsync()
        {
            var response = await _calculateService.CalculateCoinChangePercentage();

            return Ok(response.ToString());
        }

        [HttpGet("portfolioPercentageChange")]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetPortfolioPercentageChangeAsync()
        {
            var response = await _calculateService.GetPortfolioChangePercentage();

            return Ok(response);
        }

        [HttpPost("livePercentageChanges")]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetLivePercentageChanges([FromBody] LivePercentageChangeRequest request)
        {
            var response = await _calculateService.LivePortfolioChangePercentage(request.Limit);

            return Ok(response.ToString());
        }
    }
}
