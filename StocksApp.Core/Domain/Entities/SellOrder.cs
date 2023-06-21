using System.ComponentModel.DataAnnotations;

namespace Entities
{
    public class SellOrder
    {
        [Key]
        public Guid SellOrderID { get; set; }

        [StringLength(15)]
        [Required(ErrorMessage = "Stock symbol can't be blank")]
        public string StockSymbol { get; set; }
        
        [StringLength(40)]
        [Required(ErrorMessage = "Stock Name can't be blank")]
        public string StockName { get; set; }

        public DateTime DateAndTimeOfOrder { get; set; }

        [Range(1, 100000, ErrorMessage = "{0} should be between {1} and ${2}")]
        public uint Quantity { get; set; }

        [Range(1, 100000, ErrorMessage = "{0} should be between {1} and ${2}")]
        public double Price { get; set; }
    }
}
