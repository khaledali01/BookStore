using API.Dtos;
using AutoMapper;
using Core.Entities;
using Core.Interfaces;
using Core.Specifications;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
	[ApiController]
	[Route("api/[controller]")]

	public class ProductsController : ControllerBase
	{
		private readonly IGenericRepository<Product> productsRepo;
		private readonly IGenericRepository<ProductType> productTypeRepo;
		private readonly IGenericRepository<ProductBrand> productBrandRepo;
		private readonly IMapper mapper;

		public ProductsController(
			IGenericRepository<Product> productsRepo,
			IGenericRepository<ProductType> productTypeRepo,
			IGenericRepository<ProductBrand> productBrandRepo, 
			IMapper mapper
			)
		{
			this.productsRepo = productsRepo;
			this.productTypeRepo = productTypeRepo;
			this.productBrandRepo = productBrandRepo;
			this.mapper = mapper;
		}

		[HttpGet]
		public async Task<ActionResult<IReadOnlyList<ProductToReturnDto>>> GetProducts()
		{
			var spec = new ProductsWithTypesAndBrandsSpecification();

			var products = await productsRepo.ListAsync(spec);

			return Ok(mapper
				.Map<IReadOnlyList<Product>, IReadOnlyList<ProductToReturnDto>>(products));
		}

		[HttpGet("{id}")]
		public async Task<ActionResult<ProductToReturnDto>> GetProduct(int id)
		{
			var spec = new ProductsWithTypesAndBrandsSpecification(id);

			var product = await productsRepo.GetEntityWithSpec(spec);

			return mapper.Map<Product, ProductToReturnDto>(product);
		}

		[HttpGet("brands")]
		public async Task<ActionResult<IReadOnlyList<ProductBrand>>> GetProductBrands()
		{
			return Ok(await productBrandRepo.ListAllAsync());
		}

		[HttpGet("types")]
		public async Task<ActionResult<IReadOnlyList<ProductType>>> GetProductTypes()
		{
			return Ok(await productTypeRepo.ListAllAsync());
		}


	}
}