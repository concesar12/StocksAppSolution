using Microsoft.Extensions.Configuration;
using System.Net.Http;
using System.Text.Json; //for API response
using RepositoryContracts;
using Microsoft.Extensions.Logging;
using Exceptions;
using ServiceContracts.FinnhubService;

namespace Services.FinnhubService
{
    public class FinnhubCompanyProfileService : IFinnhubCompanyProfileService
    {
        //Add finnhub repository
        private readonly IFinnhubRepository _finnhubRepository;
        //Adding logger in here
        private readonly ILogger<FinnhubCompanyProfileService> _logger;
        //Contructor finnhub repository
        public FinnhubCompanyProfileService(IFinnhubRepository finnhubRepository, ILogger<FinnhubCompanyProfileService> logger)
        {
            _logger = logger;
            _finnhubRepository = finnhubRepository;
        }

        public async Task<Dictionary<string, object>?> GetCompanyProfile(string stockSymbol)
        {
            try
            {
                //invoke repository
                Dictionary<string, object>? responseDictionary = await _finnhubRepository.GetCompanyProfile(stockSymbol);

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
