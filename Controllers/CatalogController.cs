using CatalogService.Interfaces;
using CatalogService.Models;
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
        _logger.LogInformation("Getting all products");
        try
        {
            var products = _productRepository.GetAll();
            _logger.LogInformation("Retrieved {ProductCount} products", products.Count());
            return Ok(products);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while retrieving all products");
            throw;
        }
    }

    [HttpGet("{id}", Name = "GetProductById")]
    public ActionResult<Product> Get(Guid id)
    {
        _logger.LogInformation("Getting product with ID: {ProductId}", id);
        try
        {
            var product = _productRepository.GetById(id);
            if (product == null)
            {
                _logger.LogWarning("Product with ID {ProductId} not found", id);
                return NotFound();
            }
            _logger.LogInformation("Successfully retrieved product with ID: {ProductId}", id);
            return Ok(product);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while retrieving product with ID: {ProductId}", id);
            throw;
        }
    }

    [HttpPost]
    public ActionResult<Product> Create(Product product)
    {
        _logger.LogInformation("Creating new product with name: {ProductName}", product.name);
        try
        {
            var createdProduct = _productRepository.Create(product);
            _logger.LogInformation("Successfully created product with ID: {ProductId}", createdProduct.id);
            return CreatedAtRoute("GetProductById", new { id = createdProduct.id }, createdProduct);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while creating product with name: {ProductName}", product.name);
            throw;
        }
    }

    [HttpPut("{id}")]
    public ActionResult<Product> Update(Guid id, Product product)
    {
        _logger.LogInformation("Updating product with ID: {ProductId}", id);
        try
        {
            var updatedProduct = _productRepository.Update(id, product);
            if (updatedProduct == null)
            {
                _logger.LogWarning("Product with ID {ProductId} not found for update", id);
                return NotFound();
            }
            _logger.LogInformation("Successfully updated product with ID: {ProductId}", id);
            return Ok(updatedProduct);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while updating product with ID: {ProductId}", id);
            throw;
        }
    }

    [HttpDelete("{id}")]
    public ActionResult Delete(Guid id)
    {
        _logger.LogInformation("Deleting product with ID: {ProductId}", id);
        try
        {
            var deleted = _productRepository.Delete(id);
            if (!deleted)
            {
                _logger.LogWarning("Product with ID {ProductId} not found for deletion", id);
                return NotFound();
            }
            _logger.LogInformation("Successfully deleted product with ID: {ProductId}", id);
            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while deleting product with ID: {ProductId}", id);
            throw;
        }
    }
}
