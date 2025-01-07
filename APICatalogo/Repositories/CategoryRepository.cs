using APICatalogo.Context;
using APICatalogo.Models;
using APICatalogo.Pagination;
using Microsoft.EntityFrameworkCore;

namespace APICatalogo.Repositories;

public class CategoryRepository : Repository<Category>, ICategoryRepository
{

    public CategoryRepository(AppDbContext context) : base(context)
    {
    }

    public PagedList<Category> GetPagedList(CategoryParams categoryParams)
    {
        IQueryable<Category> categories = GetAll().OrderBy(c => c.Name).AsQueryable();
        PagedList<Category> orderItems = PagedList<Category>.ToPagedList(categories, categoryParams.PageNumber, categoryParams.PageSize);
        return orderItems;
    }
}
