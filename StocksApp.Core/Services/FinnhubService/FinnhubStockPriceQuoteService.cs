using Microsoft.Extensions.Configuration;
using System.Net.Http;
using System.Text.Json; //for API response
using RepositoryContracts;
using Microsoft.Extensions.Logging;
using Exceptions;
using ServiceContracts.FinnhubService;

namespace Services.FinnhubService
{
    public class FinnhubStockPriceQuoteService : IFinnhubStockPriceQuoteService
    {
        //Add finnhub repository
        private readonly IFinnhubRepository _finnhubRepository;
        //Adding logger in here
        private readonly ILogger<FinnhubStockPriceQuoteService> _logger;
        //Contructor finnhub repository
        public FinnhubStockPriceQuoteService(IFinnhubRepository finnhubRepository, ILogger<FinnhubStockPriceQuoteService> logger)
        {
            _logger = logger;
            _finnhubRepository = finnhubRepository;
        }

        public async Task<Dictionary<string, object>?> GetStockPriceQuote(string stockSymbol) // This is the task that makes the request
        {
            try
            {
                //invoke repository
                Dictionary<string, object>? responseDictionary = await _finnhubRepository.GetStockPriceQuote(stockSymbol);
                //return response dictionary back to the caller
                return responseDictionary;
            }
            catch (Exception ex)
            {
                FinnhubException finnhubException = new FinnhubException("Unable to connect to finnhub", ex);
                throw finnhubException;
            }
        }

    }
}
