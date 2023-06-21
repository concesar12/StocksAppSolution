using Entities;
using Microsoft.EntityFrameworkCore;
using RepositoryContracts;

namespace Repositories
{
    public class StocksRepository :IStocksRepository
    {
        //Bring the DB to be accesed
        private readonly ApplicationDbContext _db;

        /// <summary>
        /// Constructor of StocksRepository class that executes when a new object is created for the class
        /// </summary>
        /// <param name="stockDbContext">DB related to the stock with buy and sell</param>
        public StocksRepository(ApplicationDbContext stockDbContext)
        {
            _db = stockDbContext;
        }

        /// <summary>
        /// Add buy order object to buy orders list
        /// </summary>
        /// <param name="buyOrder"></param>
        /// <returns></returns>
        public async Task<BuyOrder> CreateBuyOrder(BuyOrder buyOrder)
        {
            _db.Add(buyOrder);
            await _db.SaveChangesAsync();

            return buyOrder;
        }

        /// <summary>
        /// Add sell order object to buy orders list
        /// </summary>
        /// <param name="sellOrder"></param>
        /// <returns></returns>
        public async Task<SellOrder> CreateSellOrder(SellOrder sellOrder)
        {
            _db.Add(sellOrder);
            await  _db.SaveChangesAsync();

            return sellOrder;
        }

        public async Task<List<BuyOrder>> GetBuyOrders()
        {
            List<BuyOrder> buyOrders = await _db.BuyOrders.OrderByDescending(temp => temp.DateAndTimeOfOrder).ToListAsync();
            return buyOrders;
        }

        public async Task<List<SellOrder>> GetSellOrders()
        {
            List<SellOrder> sellOrders = await _db.SellOrders.OrderByDescending(temp => temp.DateAndTimeOfOrder).ToListAsync();
            return sellOrders;
        }
    }
}