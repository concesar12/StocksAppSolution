using Microsoft.Extensions.Configuration;
using System.Net.Http;
using System.Text.Json; //for API response
using RepositoryContracts;
using Microsoft.Extensions.Logging;
using Exceptions;
using ServiceContracts.FinnhubService;

namespace Services.FinnhubService
{
    public class FinnhubSearchStocksService : IFinnhubSearchStocksService
    {
        //Add finnhub repository
        private readonly IFinnhubRepository _finnhubRepository;
        //Adding logger in here
        private readonly ILogger<FinnhubSearchStocksService> _logger;
        //Contructor finnhub repository
        public FinnhubSearchStocksService(IFinnhubRepository finnhubRepository, ILogger<FinnhubSearchStocksService> logger)
        {
            _logger = logger;
            _finnhubRepository = finnhubRepository;
        }

        public async Task<Dictionary<string, object>?> SearchStocks(string stockSymbolToSearch)
        {
            try
            {
                //invoke repository
                Dictionary<string, object>? responseDictionary = await _finnhubRepository.SearchStocks(stockSymbolToSearch);
                //return response dictionary back to the caller\
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
