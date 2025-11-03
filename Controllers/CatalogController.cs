using CatalogService.Models;
using Microsoft.AspNetCore.Mvc;

namespace CatalogService.Controllers;

[ApiController]
[Route("api/products")]
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

    [HttpGet]
    public ActionResult<IEnumerable<Product>> GetAll()
    {
        return Ok(_products);
    }

    [HttpGet("{id}", Name = "GetProductById")]
    public ActionResult<Product> Get(Guid id)
    {
        var product = _products.FirstOrDefault(c => c.id == id);
        if (product == null)
        {
            return NotFound();
        }
        return Ok(product);
    }

    [HttpPost]
    public ActionResult<Product> Create(Product product)
    {
        if (product.id == Guid.Empty)
        {
            product.id = Guid.NewGuid();
        }
        
        // Ensure offers have IDs if not provided
        if (product.offers != null)
        {
            foreach (var offer in product.offers.Where(o => o.id == Guid.Empty))
            {
                offer.id = Guid.NewGuid();
            }
        }

        _products.Add(product);
        return CreatedAtRoute("GetProductById", new { id = product.id }, product);
    }

    [HttpPut("{id}")]
    public ActionResult<Product> Update(Guid id, Product product)
    {
        var existingProduct = _products.FirstOrDefault(c => c.id == id);
        if (existingProduct == null)
        {
            return NotFound();
        }

        // Update the existing product
        existingProduct.name = product.name;
        existingProduct.description = product.description;
        existingProduct.sku = product.sku;
        existingProduct.brand = product.brand;
        existingProduct.manufacturer = product.manufacturer;
        existingProduct.model = product.model;
        existingProduct.image = product.image;
        existingProduct.url = product.url;
        existingProduct.releaseDate = product.releaseDate;
        existingProduct.expires = product.expires;
        existingProduct.offers = product.offers;

        return Ok(existingProduct);
    }

    [HttpDelete("{id}")]
    public ActionResult Delete(Guid id)
    {
        var product = _products.FirstOrDefault(c => c.id == id);
        if (product == null)
        {
            return NotFound();
        }

        _products.Remove(product);
        return NoContent();
    }
}
