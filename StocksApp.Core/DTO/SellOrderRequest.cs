using Entities;
using System.ComponentModel.DataAnnotations;


namespace ServiceContracts.DTO
{
    public class SellOrderRequest : IValidatableObject
    {
        [Required(ErrorMessage = "Stock symbol can't be empty")]
        public string StockSymbol { get; set; }
        [Required(ErrorMessage = "Stock name can't be empty")]
        public string StockName { get; set; }
        public DateTime DateAndTimeOfOrder { get; set; }
        [Range(1, 100000, ErrorMessage = "{0} should be between {1} and ${2}")]
        public uint Quantity { get; set; }
        [Range(1, 100000, ErrorMessage = "{0} should be between {1} and ${2}")]
        public double Price { get; set; }


        public SellOrder ToSellOrder()
        {
            return new SellOrder { StockSymbol = StockSymbol, StockName = StockName, Price = Price, DateAndTimeOfOrder = DateAndTimeOfOrder, Quantity = Quantity };
        }

        /// <summary>
        /// Model class-level validation using IValidatableObject
        /// </summary>
        /// <param name="validationContext">ValidationContext to validate</param>
        /// <returns>Returns validation errors as ValidationResult</returns>
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            List<ValidationResult> results = new List<ValidationResult>();

            //Date of order should be less than Jan 01, 2000
            if (DateAndTimeOfOrder < Convert.ToDateTime("2000-01-01"))
            {
                results.Add(new ValidationResult("Date of the order should not be older than Jan 01, 2000."));
            }

            return results;
        }
    }
}
