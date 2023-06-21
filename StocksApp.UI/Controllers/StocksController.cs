using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using ServiceContracts.FinnhubService;
using StockApp.Models;

namespace StockApp.Controllers
{
    [Route("[controller]")]
    public class StocksController :Controller
    {
        //Bring the app json options to have the stocks
        private readonly TradingOptions _tradingOptions;
        //Bring Finnhub service to call the methods
        private readonly IFinnhubStocksService _finnhubService;
        //Bring logs to live in here
        private readonly ILogger<StocksController> _logger;

        /// <summary>
        /// Constructor for TradeController that executes when a new object is created for the class
        /// </summary>
        /// <param name="tradingOptions">Injecting TradeOptions config through Options pattern</param>
        /// <param name="finnhubService">Injecting FinnhubService</param>
        /// 
        public StocksController(IOptions<TradingOptions> tradingOptions, IFinnhubStocksService finnhubService, ILogger<StocksController> logger)
        {
            //Initialize trading options
            _tradingOptions = tradingOptions.Value;
            //Initialize finnhubService
            _finnhubService = finnhubService;
            //Initialize logger
            _logger = logger;
        }

        [Route("/")]
        [Route("[action]/{stock?}")]
        [Route("~/[action]/{stock?}")]
        public async Task<IActionResult> Explore(string? stock, bool showAll = false)
        {
            //Log information in the controller
            _logger.LogInformation($"Entering to the explorer controller values: stock: {stock}, showall: {showAll}");
            //get company profile from API server
            List<Dictionary<string, string>>? stockDictionary = await _finnhubService.GetStocks();

            //Prepare local list to be fill of stocks
            List<Stock> stocks = new List<Stock>();

            //Validation of the get info to the dictionary
            if (stockDictionary is not null)
            {
                //filter stocks
                if (!showAll && _tradingOptions.Top25PopularStocks != null)
                {
                    string[]? Top25PopularStocksList = _tradingOptions.Top25PopularStocks.Split(",");
                    if (Top25PopularStocksList is not null)
                    {
                        stockDictionary = stockDictionary
                         .Where(temp => Top25PopularStocksList.Contains(Convert.ToString(temp["symbol"])))
                         .ToList();
                    }
                }

                //convert dictionary objects into Stock objects
                stocks = stockDictionary
                 .Select(temp => new Stock() { StockName = Convert.ToString(temp["description"]), StockSymbol = Convert.ToString(temp["symbol"]) })
                .ToList();
            }

            ViewBag.Stock = stock;
            return View(stocks);
        }
    }
}
