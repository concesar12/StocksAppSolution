namespace StockApp.Models
{
    /// <summary>
    /// Stock model class is to be use for the search stock and get stocks
    /// </summary>
    public class Stock
    {
        public string? StockSymbol { get; set; }
        public string? StockName { get; set; }

        public override bool Equals(object? obj)
        {
            if (obj == null) return false;
            if (obj is not Stock) return false;

            Stock other = (Stock)obj;
            return StockSymbol == other.StockSymbol && StockName == other.StockName;
        }

        public override int GetHashCode()
        {
            throw new NotImplementedException();
        }
    }
}
