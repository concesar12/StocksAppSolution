using Microsoft.Extensions.Configuration;
using System.Net.Http;
using System.Text.Json; //for API response
using RepositoryContracts;
using Microsoft.Extensions.Logging;
using Exceptions;
using ServiceContracts.FinnhubService;

namespace Services.FinnhubService
{
    public class FinnhubStocksService : IFinnhubStocksService
    {
        //Add finnhub repository
        private readonly IFinnhubRepository _finnhubRepository;
        //Adding logger in here
        private readonly ILogger<FinnhubStocksService> _logger;
        //Contructor finnhub repository
        public FinnhubStocksService(IFinnhubRepository finnhubRepository, ILogger<FinnhubStocksService> logger)
        {
            _logger = logger;
            _finnhubRepository = finnhubRepository;
        }

        public async Task<List<Dictionary<string, string>>?> GetStocks()
        {
            try
            {
                _logger.LogInformation("This is entering the getstocks method in finhub service");
                List<Dictionary<string, string>>? responseListStocks = await _finnhubRepository.GetStocks();
                return responseListStocks;
            }
            catch (Exception ex)
            {
                FinnhubException finnhubException = new FinnhubException("Unable to connect to finnhub", ex);
                throw finnhubException;
            }
        }

    }
}
