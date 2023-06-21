using Entities;
using RepositoryContracts;
using ServiceContracts.DTO;
using ServiceContracts.StocksService;
using Services.Helpers;

namespace Services.StocksService
{
    public class StocksBuyOrdersService : IBuyOrdersService
    {
        //Bring the stocks repository
        private readonly IStocksRepository _stocksRepository;

        //Call constructos to use repository
        public StocksBuyOrdersService(IStocksRepository stocksRepository)
        {
            _stocksRepository = stocksRepository;
        }

        public async Task<BuyOrderResponse> CreateBuyOrder(BuyOrderRequest? buyOrderRequest)
        {
            //Validation: buyOrderRequest can't be null
            if (buyOrderRequest == null)
            {
                throw new ArgumentNullException(nameof(buyOrderRequest));
            }
            //Model validation
            ValidationHelper.ModelValidation(buyOrderRequest);

            //convert buyOrderRequest into BuyOrder type
            BuyOrder buyOrder = buyOrderRequest.ToBuyOrder();

            //generate BuyOrderID
            buyOrder.BuyOrderID = Guid.NewGuid();

            //add buy order object to buy orders list
            BuyOrder buyOrderFromRepo = await _stocksRepository.CreateBuyOrder(buyOrder);

            //convert the BuyOrder object into BuyOrderResponse type and return
            return buyOrder.ToBuyOrderResponse();
        }

        public async Task<List<BuyOrderResponse>> GetBuyOrders()
        {
            List<BuyOrder> buyOrders = await _stocksRepository.GetBuyOrders();

            return buyOrders.Select(temp => temp.ToBuyOrderResponse()).ToList();
        }

    }
}
