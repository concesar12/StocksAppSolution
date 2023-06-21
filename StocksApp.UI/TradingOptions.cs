namespace StockApp
{
    /// <summary>
    /// Represents Options pattern for "StockPrice" configuration
    /// </summary>
    public class TradingOptions
    {
       public string? DefaultStockSymbol { get; set; } // Like this it can get the value at runtime
        public uint ? DefaultOrderQuantity { get; set; } // This is the default quantity
        public string? Top25PopularStocks { get; set; } // To use all popular stocks
    }
}
