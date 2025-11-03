namespace CatalogService.Models
{
    public class Offer
    {
        public Guid id { get; set; }
        public decimal price { get; set; }
        public string? priceCurrency { get; set; }
        public string? availability { get; set; }
        public DateTime? validFrom { get; set; }
        public DateTime? validThrough { get; set; }
        public string? seller { get; set; }
        public string? condition { get; set; }
    }
}
