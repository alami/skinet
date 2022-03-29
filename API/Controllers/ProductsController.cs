using Infrastructure.Data;
using Core.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Core.Interfaces;
using Core.Specifications;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    { 

        private readonly IGenericRepository<Product> _productsRepo;
        private readonly IGenericRepository<ProductBrand> _productBrandRepo;
        private readonly IGenericRepository<ProductType> _productTypeRepo;
        public ProductsController(IGenericRepository<Product> productsRepo, IGenericRepository<ProductBrand> productBrandRepo, IGenericRepository<ProductType> productTypeRepo)
        {            
            _productTypeRepo = productTypeRepo;
            _productBrandRepo = productBrandRepo;
            _productsRepo = productsRepo;

        }
        [HttpGet]
        public async Task<ActionResult<List<Product>>> GetProducts(){
            var spec = new ProductsWithTypesAndBrandsSpecification();
            var products = await _productsRepo.ListAsync(spec);
            return Ok(products);
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<Product>>  GetProduct(int id){
            var product = await _productsRepo.GetByIdAsync(id);
            return Ok(product);
        }
        // [HttpGet("brands")]
        // public async Task<ActionResult<List<ProductBrand>>> GetBrands(){
        //     var products = await _productsRepo.GetProductBrandsAsync();
        //     return Ok(products);
        // }
        // [HttpGet("types")]
        // public async Task<ActionResult<List<ProductType>>> GetTypes(){
        //     var products = await _productsRepo.GetProductTypesAsync();
        //     return Ok(products);
        // }
    } 
}