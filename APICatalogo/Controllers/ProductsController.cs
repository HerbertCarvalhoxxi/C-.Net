using APICatalogo.Context;
using APICatalogo.DTOs;
using APICatalogo.Models;
using APICatalogo.Pagination;
using APICatalogo.Repositories;
using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace APICatalogo.Controller;

[ApiController]
[Route("[controller]")]
public class ProductsController : ControllerBase
{
    private readonly IUnityOfWork _unityOfWork;
    private readonly IMapper _mapper;

    public ProductsController(IUnityOfWork unityOfWork, IMapper mapper)
    {
        _unityOfWork = unityOfWork;
        _mapper = mapper;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<ProductDTO>>> GetProducts()
    {
        IEnumerable<Product> products = await _unityOfWork.ProductRepository.GetAllAsync();
        IEnumerable<ProductDTO> productsDTO = _mapper.Map<IEnumerable<ProductDTO>>(products);
        return Ok(productsDTO);
    }


    [HttpGet("{id}", Name = "ObterProduto")]
    public async Task<ActionResult<ProductDTO>> GetProduct(int id)
    {
        Product product = await _unityOfWork.ProductRepository.GetAsync(p => p.ProductId == id);
        if (product is null)
        {
            return NotFound();
        }

        ProductDTO productDTO = _mapper.Map<ProductDTO>(product);

        return productDTO;
    }

    [HttpGet("pagination")]
    public async Task<ActionResult<IEnumerable<ProductDTO>>> GetPaginated([FromQuery] ProductParams productParams)
    {
        PagedList<Product> products = await _unityOfWork.ProductRepository.GetProductsPaginationAsync(productParams); //usa o método do repositorio e retorna pagedList

        return Ok(GetProductsPag(products));
    }

    private IEnumerable<ProductDTO> GetProductsPag(PagedList<Product> products)
    {
        var metadata = new
        {
            products.TotalCount,
            products.PageSize,
            products.CurrentPage,
            products.TotalPages,
            products.HasNext,
            products.HasPrevious

        };

        Response.Headers.Append("X-Pagination", JsonConvert.SerializeObject(metadata));

        IEnumerable<ProductDTO> productsDTO = _mapper.Map<IEnumerable<ProductDTO>>(products);
        return productsDTO;
    }

    [HttpGet("filter/preco/pagination")]
    public async Task<ActionResult<IEnumerable<Product>>> GetFiltedList([FromQuery]FilterPriceProducts filterPriceProducts)
    {
        PagedList<Product> products = await _unityOfWork.ProductRepository.GetFiltedPricesAsync(filterPriceProducts);
        IEnumerable<ProductDTO> filtedProducts = GetProductsPag(products);
        return Ok(filtedProducts);
    }

    [HttpPost]
    public async Task<ActionResult<ProductDTO>> CreateProduct(ProductDTO productDTO)
    {
        if (productDTO is null)
            return BadRequest();

        Product product = _mapper.Map<Product>(productDTO);
        _unityOfWork.ProductRepository.Create(product);
        await _unityOfWork.CommitAsync();
        return new CreatedAtRouteResult("ObterProduto", new { id = productDTO.ProductId }, productDTO);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<ProductDTO>> UpdateProduct(int id, ProductDTO productDTO)
    {
        if (productDTO.ProductId != id)
        {
            return BadRequest();
        }
       Product product = _mapper.Map<Product>(productDTO);
       Product updatedProduct = _unityOfWork.ProductRepository.Update(product);
       await _unityOfWork.CommitAsync();
       ProductDTO updatedProductDTO = _mapper.Map<ProductDTO>(updatedProduct);

        return Ok(updatedProductDTO);
    }

    [HttpPatch("{id}/UpdateParcial")]
    public async Task<ActionResult<ProductDTOResponse>> Patch(int id, JsonPatchDocument<ProductDTORequest> patchProduct)
    {
        if(patchProduct is null || id < 0)
        {
            return BadRequest();
        }

        Product product = await _unityOfWork.ProductRepository.GetAsync(p => p.ProductId == id); //busca o produto que será alterado

        if (product == null)
        {
            return BadRequest();
        }

        ProductDTORequest productRequest = _mapper.Map<ProductDTORequest>(product); //transfere os dados de product para productRequest (os campos iguais das entidades)

        patchProduct.ApplyTo(productRequest, ModelState); //Aplica os dados enviados no corpo da requisição para productRequest

        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        _mapper.Map(productRequest, product); //mapeia novamente, mas com os dados de productRequest, inseridos com applyto
        _unityOfWork.ProductRepository.Update(product);
        await _unityOfWork.CommitAsync();
        return Ok(_mapper.Map<ProductDTOResponse>(product));

    }

    [HttpDelete("{id}")]
    public async Task<ActionResult<ProductDTO>> DeleteProduct(ProductDTO productDTO)
    {
       if (productDTO is null)
        {
            return NotFound();
        }

        Product produto = _mapper.Map<Product>(productDTO);
        Product deletedProduct = _unityOfWork.ProductRepository.Delete(produto);
        await _unityOfWork.CommitAsync();


        return Ok(produto);

    }
}
