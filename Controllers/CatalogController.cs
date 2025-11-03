using CatalogService.Models;
using Microsoft.AspNetCore.Mvc;

namespace CatalogService.Controllers;

[ApiController]
[Route("[controller]")]
public class CatalogController : ControllerBase
{

    private static List<Product> _products = new List<Product>() {
     new () {
         id = new Guid("7125e019-c469-4dbd-93e5-426de6652523"),
         name = "Salmon Fillet",
         description = "Fresh salmon fillet",
         sku = "SALMON-001",
         brand = "FishmongerX",
         manufacturer = "Fish Supplier",
         model = "Standard",
         image = "https://example.com/salmon.jpg",
         url = "https://example.com/salmon",
         releaseDate = DateTime.Now,
         expires = DateTime.Now.AddDays(3), // Example expiry date 3 days from now
         offers = new List<Offer>
         {
             new Offer
             {
                 id = Guid.NewGuid(),
                 price = 12.99m,
                 priceCurrency = "USD",
                 availability = "InStock",
                 validFrom = DateTime.Now,
                 validThrough = DateTime.Now.AddDays(30),
                 seller = "FishmongerX",
                 condition = "New"
             }
         }
        }
    };

    private readonly ILogger<CatalogController> _logger;

    public CatalogController(ILogger<CatalogController> logger)
    {
        _logger = logger;
    }

    [HttpGet("{productId}", Name = "GetProductById")]
    public Product Get(Guid productId)
    {
        return _products.Where(c => c.id == productId).First();
    }
}
