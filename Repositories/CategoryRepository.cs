using APICatalogo.Context;
using APICatalogo.Models;
using APICatalogo.Pagination;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

namespace APICatalogo.Repositories;

public class CategoryRepository : Repository<Category>, ICategoryRepository
{

    public CategoryRepository(AppDbContext context) : base(context)
    {
    }

    public async Task<PagedList<Category>> GetPagedListAsync(CategoryParams categoryParams)
    {
        IQueryable<Category> categories = _context.Categories.AsQueryable();
        //categories = categories.OrderBy(c => c.CategoryId);
        
        return await PagedList<Category>.ToPagedList(categories, categoryParams.PageNumber, categoryParams.PageSize);
    }

    public async Task<PagedList<Category>> GetFiltedAsync(FilterNameCategory filterNameCategory) 
    {
        IQueryable<Category>? categories = _context.Categories.AsQueryable();

        if (!string.IsNullOrEmpty(filterNameCategory.Name))
        {
            categories = categories?.Where(c => c.Name.Contains(filterNameCategory.Name));
        }
        
        
       return await PagedList<Category>.ToPagedList(categories, filterNameCategory.PageNumber, filterNameCategory.PageSize);
        
    }
}
