namespace CatalogService.Models
{
    public class Product
    {
        public Guid Id { get; set; }
        public string? name { get; set; }
        public string? description { get; set; }
        public decimal price { get; set; }
        public string? brand { get; set; }
        public string? manufacturer { get; set; }
        public string? model { get; set; }
        public string? image { get; set; }
        public string? url { get; set; }
        public DateTime releaseDate { get; set; }
        public DateTime? expires { get; set; }
    }
}
