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
    public ActionResult<IEnumerable<CategoryDTO>> GetCategories()
    {
       IEnumerable<Category> categories = _unityOfWork.CategoryRepository.GetAll();

       IEnumerable<CategoryDTO> categoriesDTO = categories.ToCategoryDTOList();

        return Ok(categoriesDTO);
    }

    [HttpGet("{id}", Name = "ObterCategoria")]
    public ActionResult<CategoryDTO> GetCategory(int id)
    {
        Category category = _unityOfWork.CategoryRepository.Get(c => c.CategoryId == id);
        

        if (category is null)
        {
            _logger.LogWarning($"Categoria com id= {id} não encontrada...");
            return NotFound($"Categoria com id= {id} não encontrada...");
        }

        CategoryDTO categoryResponse = category.ToCategoryDTO();

        return Ok(categoryResponse);
    }

    [HttpGet("Pagination")]
    public ActionResult<List<Category>> GetPagination([FromQuery]CategoryParams categoryParams)
    {
        PagedList<Category> categories = _unityOfWork.CategoryRepository.GetPagedList(categoryParams);

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
    public ActionResult CreateCategory(CategoryDTO categoryDTO)
    {
        if (categoryDTO is null)
            return BadRequest("Dados inválidos");

        Category category = categoryDTO.ToCategory();

        _unityOfWork.CategoryRepository.Create(category);
        _unityOfWork.Commit();

        CategoryDTO categoryResponse = category.ToCategoryDTO();

        return new CreatedAtRouteResult("ObterCategoria", new { id = categoryResponse.CategoryId, categoryResponse });
    }

    [HttpPut("{id}")]
    public ActionResult UpdateCategory(int id, CategoryDTO categoryDTO)
    {
        if (categoryDTO.CategoryId != id)
        {
            _logger.LogWarning($"Dados inválidos...");
            return BadRequest("Dados inválidos");
        }

        Category category = categoryDTO.ToCategory();

        _unityOfWork.CategoryRepository.Update(category);
        _unityOfWork.Commit();

        CategoryDTO categoryResponse = category.ToCategoryDTO();
        
        return Ok(categoryResponse);
    }

    [HttpDelete]
    public ActionResult DeleteCategory(Category category)
    {
        Category Deletedcategory = _unityOfWork.CategoryRepository.Delete(category);

        if (category is null)
        {   
            return NotFound();
        }
        _unityOfWork.Commit();
        return Ok(Deletedcategory);
    }

}
