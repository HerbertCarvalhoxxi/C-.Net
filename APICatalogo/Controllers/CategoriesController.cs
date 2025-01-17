using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using APICatalogo.Context;
using APICatalogo.Models;
using APICatalogo.Filters;
using APICatalogo.Repositories;
using APICatalogo.DTOs;
using APICatalogo.DTOs.Mapping;
using APICatalogo.Pagination;
using Newtonsoft.Json;

namespace APICatalogo.Controllers;

[ApiController]
[Route("[controller]")]
public class CategoriesController : ControllerBase
{
    private readonly IUnityOfWork _unityOfWork;
    private readonly ILogger<CategoriesController> _logger;
    public CategoriesController(IUnityOfWork unityOfWork, ILogger<CategoriesController> logger)
    {
        _unityOfWork = unityOfWork;
        _logger = logger;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<CategoryDTO>>> GetCategories()
    {
       IEnumerable<Category> categories = await _unityOfWork.CategoryRepository.GetAllAsync();

       IEnumerable<CategoryDTO> categoriesDTO = categories.ToCategoryDTOList();

        return Ok(categoriesDTO);
    }

    [HttpGet("{id}", Name = "ObterCategoria")]
    public async Task<ActionResult<CategoryDTO>> GetCategory(int id)
    {
        Category category = await _unityOfWork.CategoryRepository.GetAsync(c => c.CategoryId == id);
        

        if (category is null)
        {
            _logger.LogWarning($"Categoria com id= {id} não encontrada...");
            return NotFound($"Categoria com id= {id} não encontrada...");
        }

        CategoryDTO categoryResponse = category.ToCategoryDTO();

        return Ok(categoryResponse);
    }

    [HttpGet("Pagination")]
    public async Task<ActionResult<IEnumerable<Category>>> GetPagination([FromQuery]CategoryParams categoryParams)
    {
        PagedList<Category> categories = await _unityOfWork.CategoryRepository.GetPagedListAsync(categoryParams);
        return Ok(GetCategoriesFil(categories));
    }

    [HttpGet("filter/name/pagination")]
    public async Task<ActionResult<IEnumerable<Category>>> FilterName([FromQuery] FilterNameCategory filterNameCategory)
    {
        PagedList<Category> categories = await _unityOfWork.CategoryRepository.GetFiltedAsync(filterNameCategory);
        IEnumerable<Category> filtedCategories = GetCategoriesFil(categories);
        return Ok(filtedCategories);

    }

    private IEnumerable<Category> GetCategoriesFil(PagedList<Category> categories)
    {
        var metadata = new
        {
            categories.TotalCount,
            categories.PageSize,
            categories.CurrentPage,
            categories.TotalPages,
            categories.HasNext,
            categories.HasPrevious

        };

        Response.Headers.Append("X-Pagination", JsonConvert.SerializeObject(metadata));
        return categories;
    }

    [HttpPost]
    public async Task<ActionResult> CreateCategory(CategoryDTO categoryDTO)
    {
        if (categoryDTO is null)
            return BadRequest("Dados inválidos");

        Category category = categoryDTO.ToCategory();

        _unityOfWork.CategoryRepository.Create(category);
        await _unityOfWork.CommitAsync();

        CategoryDTO categoryResponse = category.ToCategoryDTO();

        return new CreatedAtRouteResult("ObterCategoria", new { id = categoryResponse.CategoryId, categoryResponse });
    }

    [HttpPut("{id}")]
    public async Task<ActionResult> UpdateCategory(int id, CategoryDTO categoryDTO)
    {
        if (categoryDTO.CategoryId != id)
        {
            _logger.LogWarning($"Dados inválidos...");
            return BadRequest("Dados inválidos");
        }

        Category category = categoryDTO.ToCategory();

        _unityOfWork.CategoryRepository.Update(category);
        await _unityOfWork.CommitAsync();

        CategoryDTO categoryResponse = category.ToCategoryDTO();
        
        return Ok(categoryResponse);
    }

    [HttpDelete]
    public async Task<ActionResult> DeleteCategory(Category category)
    {
        Category Deletedcategory = _unityOfWork.CategoryRepository.Delete(category);

        if (category is null)
        {   
            return NotFound();
        }
        await _unityOfWork.CommitAsync();
        return Ok(Deletedcategory);
    }

}
