using CatalogService.Models;
using CatalogService.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace CatalogService.Controllers;

[ApiController]
[Route("api/products")]
public class CatalogController : ControllerBase
{
    private readonly ILogger<CatalogController> _logger;
    private readonly IProductRepository _productRepository;

    public CatalogController(ILogger<CatalogController> logger, IProductRepository productRepository)
    {
        _logger = logger;
        _productRepository = productRepository;
    }

    [HttpGet]
    public ActionResult<IEnumerable<Product>> GetAll()
    {
        var products = _productRepository.GetAll();
        return Ok(products);
    }

    [HttpGet("{id}", Name = "GetProductById")]
    public ActionResult<Product> Get(Guid id)
    {
        var product = _productRepository.GetById(id);
        if (product == null)
        {
            return NotFound();
        }
        return Ok(product);
    }

    [HttpPost]
    public ActionResult<Product> Create(Product product)
    {
        var createdProduct = _productRepository.Create(product);
        return CreatedAtRoute("GetProductById", new { id = createdProduct.id }, createdProduct);
    }

    [HttpPut("{id}")]
    public ActionResult<Product> Update(Guid id, Product product)
    {
        var updatedProduct = _productRepository.Update(id, product);
        if (updatedProduct == null)
        {
            return NotFound();
        }
        return Ok(updatedProduct);
    }

    [HttpDelete("{id}")]
    public ActionResult Delete(Guid id)
    {
        var deleted = _productRepository.Delete(id);
        if (!deleted)
        {
            return NotFound();
        }
        return NoContent();
    }
}
