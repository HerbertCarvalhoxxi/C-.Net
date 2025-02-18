using APICatalogo.Models;
using APICatalogo.Pagination;

namespace APICatalogo.Repositories
{
    public interface ICategoryRepository : IRepository<Category>
    {
        public Task<PagedList<Category>> GetPagedListAsync(CategoryParams categoryParams);
        public Task<PagedList<Category>> GetFiltedAsync(FilterNameCategory filterNameCategory);
    }
}
