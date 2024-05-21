using System.ComponentModel.DataAnnotations;

namespace api.Dtos.Stock
{
    public class UpdateStockDto
    {
        [Required]
        [MaxLength(10, ErrorMessage = "Symbol must be at least 10 character long")]
        public string Symbol { get; set; } = string.Empty;
        [Required]
        [MaxLength(10, ErrorMessage = "Company Name must be at least 10 character long")]
        public string CompanyName { get; set; } = string.Empty;
        [Required]
        [Range(0, 1000000, ErrorMessage = "Purchase must be between 0 and 1000000")] public decimal Purchase { get; set; }
        [Required]
        [Range(0.001, 100, ErrorMessage = "Purchase must be between 0 and 1000000")]
        public decimal LastDiv { get; set; }
        [Required]
        [MaxLength(100, ErrorMessage = "Industry must be at most 100 characters long")]
        public string Industry { get; set; } = string.Empty;
        [Required]
        [Range(0, 1000000, ErrorMessage = "Market Cap must be between 0 and 1000000")]
        public long MarketCap { get; set; }
    }
}
