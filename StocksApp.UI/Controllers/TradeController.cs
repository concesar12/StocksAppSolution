using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using StockApp.Models;
using ServiceContracts.DTO;
using ServiceContracts.FinnhubService;
using ServiceContracts.StocksService;
using StockApp.Filters.ActionFilters;
using Rotativa;
using Rotativa.AspNetCore;

namespace StockApp.Controllers
{
    //The [Route("[controller]")] attribute is used to define the base route for all actions in this controller.
    //Here, [controller] is a placeholder that will be replaced with the name of the controller, which is "Trade" in this case.
    [Route("[controller]")]
    public class TradeController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly TradingOptions _tradeOptions;
        private readonly IBuyOrdersService _stocksBuyOrdersService;
        private readonly ISellOrdersService _stocksSellOrdersService;
        private readonly IFinnhubSearchStocksService _finnhubSeachStocksService;
        private readonly IFinnhubCompanyProfileService _finnhubCompanyProfileService;
        private readonly IFinnhubStockPriceQuoteService _finnhubStockPriceQuoteService;

        /// <summary>
        /// Constructor for TradeController that executes when a new object is created for the class
        /// </summary>
        /// <param name="finnhubService"></param>
        /// <param name="stocksService">Injecting StocksService</param>
        /// <param name="configuration"></param>
        /// <param name="tradingOptions"></param>
        public TradeController (IOptions<TradingOptions> tradingOptions,
            IBuyOrdersService stocksBuyOrdersService,
            ISellOrdersService stocksSellOrdersService,
            IFinnhubSearchStocksService finnhubSearchStocksService,
            IFinnhubCompanyProfileService finnhubCompanyProfileService,
            IFinnhubStockPriceQuoteService finnhubStockPriceQuoteService,
            IConfiguration configuration
            )
        {
            _tradeOptions = tradingOptions.Value;
            _configuration = configuration;
            _stocksBuyOrdersService = stocksBuyOrdersService;
            _stocksSellOrdersService = stocksSellOrdersService;
            _finnhubSeachStocksService = finnhubSearchStocksService;
            _finnhubCompanyProfileService = finnhubCompanyProfileService;
            _finnhubStockPriceQuoteService = finnhubStockPriceQuoteService;
        }

        [Route("[action]/{stockSymbol}")] // specifies that the action method can be accessed using a URL that matches the name of the action method. 
        [Route("~/[controller]/{stockSymbol}")] //  specifies that the controller can be accessed using a URL that matches the name of the controller, but with a leading tilde (~) character.
        public async Task<IActionResult> Index(string stockSymbol)
        {
            //reset stock symbol if not exists
            if (string.IsNullOrEmpty(stockSymbol))
                stockSymbol = "MSFT";

            //get company profile from API server
            Dictionary<string, object>? companyProfileDictionary = await _finnhubCompanyProfileService.GetCompanyProfile(stockSymbol);

            //get stock price quotes from API server
            Dictionary<string, object>? stockQuoteDictionary = await _finnhubStockPriceQuoteService.GetStockPriceQuote(stockSymbol);


            //create model object
            StockTrade stockTrade = new StockTrade() { StockSymbol = stockSymbol };

            //load data from finnHubService into model object
            if (companyProfileDictionary != null && stockQuoteDictionary != null)
            {
                stockTrade = new StockTrade() { StockSymbol = companyProfileDictionary["ticker"].ToString(), StockName = companyProfileDictionary["name"].ToString(), Quantity = _tradeOptions.DefaultOrderQuantity ?? 0, Price = Convert.ToDouble(stockQuoteDictionary["c"].ToString()) };
            }

            //Send Finnhub token to view
            ViewBag.FinnhubToken = _configuration["FinnhubToken"];

            return View(stockTrade);
        }

        [Route("[action]")]
        public async Task<IActionResult> Orders()
        {
            //invoke service methods
            List<BuyOrderResponse> buyOrderResponses = await _stocksBuyOrdersService.GetBuyOrders();
            List<SellOrderResponse> sellOrderResponses = await _stocksSellOrdersService.GetSellOrders();

            //Create model object
            Orders orders = new Orders() { BuyOrders = buyOrderResponses, SellOrders = sellOrderResponses };

            //Now add in the viewbag the options read to the view
            ViewBag.TradingOptions = _tradeOptions;
            
            return View(orders);
        }

        [Route("[action]")]
        [HttpPost]
        //Filter for model validation
        [TypeFilter(typeof(CreateOrderActionFilter))]
        public async Task<IActionResult> SellOrder(SellOrderRequest orderRequest)
        {
            //Invoke service method
            SellOrderResponse sellOrderResponse = await _stocksSellOrdersService.CreateSellOrder(orderRequest);

            return RedirectToAction(nameof(Orders));
        }

        [Route("[action]")]
        [HttpPost]
        //Filter for model validation
        [TypeFilter(typeof(CreateOrderActionFilter))]
        public async Task<IActionResult> BuyOrder(BuyOrderRequest orderRequest)
        {
            
            //Invoke service method
            BuyOrderResponse buyOrderResponse = await _stocksBuyOrdersService.CreateBuyOrder(orderRequest);

            return RedirectToAction(nameof(Orders));
        }

        [Route("[action]")]
        public async Task<IActionResult> TradesPDF()
        {
            //get list of buys and sells orders
            List<IOrderResponse> orders = new List<IOrderResponse>();
            orders.AddRange(await _stocksBuyOrdersService.GetBuyOrders());
            orders.AddRange(await _stocksSellOrdersService.GetSellOrders());
            orders = orders.OrderByDescending(temp => temp.DateAndTimeOfOrder).ToList();

            ViewBag.TradingOptions = _tradeOptions;

            //Return view as pdf
            return new ViewAsPdf("TradesPDF", orders, ViewData)
            {
                PageMargins = new Rotativa.AspNetCore.Options.Margins() { Top = 20, Right = 20, Bottom = 20, Left = 20 },
                PageOrientation = Rotativa.AspNetCore.Options.Orientation.Landscape
            };
        }
    }
}
