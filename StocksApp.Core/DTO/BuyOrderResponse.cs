using Entities;
using System.ComponentModel.DataAnnotations;

namespace ServiceContracts.DTO
{
    public class BuyOrderResponse : IOrderResponse
    {
        public Guid BuyOrderID { get; set; }
        public string StockSymbol { get; set; }
        [Required(ErrorMessage = "Stock name can't be blank")]
        public string StockName { get; set; }
        public DateTime DateAndTimeOfOrder { get; set; }
        public uint Quantity { get; set; }
        public double Price { get; set; }
        public OrderType TypeOfOrder => OrderType.BuyOrder;
        public double TradeAmount { get; set; }


        public override string ToString()
        {
            return $"Buy order Id: {BuyOrderID}, Stock Symbol: {StockSymbol}, Stock name: {StockName}, Date and time of order: {DateAndTimeOfOrder.ToString("dd MM yyyy")}, Quantity:{Quantity}, Price: ${Price}, Trade Amount: {TradeAmount}";
        }

        public override bool Equals(object? obj)
        {
            if (obj ==null) return false;
            if (obj is not BuyOrderResponse) return false;

            BuyOrderResponse other = obj as BuyOrderResponse;
            return BuyOrderID == other.BuyOrderID && StockSymbol == other.StockSymbol && StockName == other.StockName && Price == other.Price && TradeAmount == other.TradeAmount && DateAndTimeOfOrder == other.DateAndTimeOfOrder && StockSymbol == other.StockSymbol;
        }

        public override int GetHashCode()
        {
            return StockSymbol.GetHashCode();
        }

    }
    public static class BuyOrderExtensions
    {
        public static BuyOrderResponse ToBuyOrderResponse(this BuyOrder buyOrder)
        {
            return new BuyOrderResponse()
            {
                BuyOrderID = buyOrder.BuyOrderID,
                StockSymbol = buyOrder.StockSymbol,
                StockName = buyOrder.StockName,
                Price = buyOrder.Price,
                DateAndTimeOfOrder = buyOrder.DateAndTimeOfOrder,
                Quantity = buyOrder.Quantity,
                TradeAmount = buyOrder.Price * buyOrder.Quantity
            };
        }
    }

    
}
