using Entities;
using System.ComponentModel.DataAnnotations;

namespace ServiceContracts.DTO
{
    public class SellOrderResponse :IOrderResponse
    {
        public Guid SellOrderID { get; set; }
        public string? StockSymbol { get; set; }
        [Required(ErrorMessage = "Stock Name can't be null or empty")]
        public string? StockName { get; set; }
        public DateTime DateAndTimeOfOrder { get; set; }
        public uint Quantity { get; set; }
        public double Price { get; set; }
        public OrderType TypeOfOrder => OrderType.SellOrder;
        public double TradeAmount { get; set; }

        public override string ToString()
        {
            return $"Sell order Id: {SellOrderID}, Stock Symbol: {StockSymbol}, Stock name: {StockName}, Date and time of order: {DateAndTimeOfOrder.ToString("dd MM yyyy")}, Quantity:{Quantity}, Price: ${Price}, Trade Amount: {TradeAmount}";
        }

        /// <summary>
        /// Checks if the current object and other (parameter) object values match
        /// </summary>
        /// <param name="obj">Other object of SellOrderResponse class, to compare</param>
        /// <returns>True or false determines whether current object and other objects match</returns>
        public override bool Equals(object? obj)
        {
            if (obj == null) return false;
            if (obj is not SellOrderResponse) return false;

            SellOrderResponse other = (SellOrderResponse)obj;
            return SellOrderID == other.SellOrderID && StockSymbol == other.StockSymbol && StockName == other.StockName && DateAndTimeOfOrder == other.DateAndTimeOfOrder && Quantity == other.Quantity && Price == other.Price;
        }

        /// <summary>
        /// Returns an int value that represents unique stock id of the current object
        /// </summary>
        /// <returns>unique int value</returns>
        public override int GetHashCode()
        {
            return StockSymbol.GetHashCode();
        }


    }

    public static class SellOrderExtensions
    {
        public static SellOrderResponse ToSellOrderResponse(this SellOrder sellOrder)
        {
            return new SellOrderResponse()
            {
                SellOrderID = sellOrder.SellOrderID,
                StockSymbol = sellOrder.StockSymbol,
                StockName = sellOrder.StockName,
                Price = sellOrder.Price,
                DateAndTimeOfOrder = sellOrder.DateAndTimeOfOrder,
                Quantity = sellOrder.Quantity,
                TradeAmount = sellOrder.Price * sellOrder.Quantity
            };
        }
    }
}
