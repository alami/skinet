using Infrastructure.Data;
using Core.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Core.Interfaces;
using Core.Specifications;
using API.Dtos;

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
        public async Task<ActionResult<List<ProductToReturnDto>>> GetProducts(){
            var spec = new ProductsWithTypesAndBrandsSpecification();
            var products = await _productsRepo.ListAsync(spec);
            return products.Select( product => new ProductToReturnDto {
                Id = product.Id,
                Name = product.Name,
                Description =  product.Description,
                Price =  product.Price,
                PictureUrl =  product.PictureUrl,
                ProductType =  product.ProductType.Name,
                ProductBrand =  product.ProductBrand.Name
            }).ToList();
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<ProductToReturnDto>>  GetProduct(int id){
            var spec = new ProductsWithTypesAndBrandsSpecification(id);
            var product = await _productsRepo.GetEntityWithSpec(spec);            
            return new ProductToReturnDto
            {
                Id = product.Id,
                Name = product.Name,
                Description =  product.Description,
                Price =  product.Price,
                PictureUrl =  product.PictureUrl,
                ProductType =  product.ProductType.Name,
                ProductBrand =  product.ProductBrand.Name
            };
        }
        [HttpGet("brands")]
        public async Task<ActionResult<List<ProductBrand>>> GetBrands(){
            return Ok(await _productBrandRepo.ListAllAsync());            
        }
        [HttpGet("types")]
        public async Task<ActionResult<List<ProductType>>> GetTypes(){
            return Ok (await _productTypeRepo.ListAllAsync());
        }
    } 
} 