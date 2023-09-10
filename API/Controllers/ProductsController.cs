using Infrastructure.Data;
using Core.Entities;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Core.Interfaces;
using Core.Specifications;
using API.DTOs;
using System.Security.Cryptography.Xml;
using System.Linq;
using AutoMapper;

namespace API.Controllers
{
    [ApiController]
    [Microsoft.AspNetCore.Mvc.Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly IProductRepository _repo;

        public IGenericRepository<Product> _productRepo { get; }
        public IGenericRepository<ProductBrand> _productBrandRepo { get; }
        public IGenericRepository<ProductType> _productTypeRepo { get; }
        public IMapper _mapper { get; }

        public ProductsController(IGenericRepository<Product> productRepo,IGenericRepository<ProductBrand> productBrandRepo,IGenericRepository<ProductType> productTypeRepo
        ,IMapper mapper)
        {
            _productRepo = productRepo;
            _productBrandRepo = productBrandRepo;
            _productTypeRepo = productTypeRepo;
            _mapper=mapper;
        }
        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<ProductToReturnDto>>> GetProducts()
        {
            var spec=new ProductWithTpesAndBrandSpecification();
            var products= await _productRepo.ListAsync(spec);
           
          return Ok( _mapper.Map<IReadOnlyList<Product>,IReadOnlyList<ProductToReturnDto>>(products));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ProductToReturnDto>> GetProduct(int id)
        {
           var spec=new ProductWithTpesAndBrandSpecification(id);
            var products= await _productRepo.GetEntityWithSpec(spec);
            return Ok(_mapper.Map<Product,ProductToReturnDto>(products));
        }

          [HttpGet("brands")]
        public async Task<ActionResult<IReadOnlyList<Product>>> GetProductBrands()
        {
            return Ok(await _productBrandRepo.ListAllAsync());
        }

        [HttpGet("types")]
        public async Task<ActionResult<IReadOnlyList<Product>>> GetProductTypes()
        {
            return Ok(await _productTypeRepo.ListAllAsync());
        }
    }
}