using APICatalogo.Models;
using APICatalogo.Pagination;

namespace APICatalogo.Repositories
{
    public interface IProductRepository : IRepository<Product>
    {
        //IEnumerable<Product> GetProductsPagination(ProductParams productParams);
        Task<PagedList<Product>> GetProductsPaginationAsync(ProductParams productParams);
        Task<PagedList<Product>> GetFiltedPricesAsync(FilterPriceProducts filterPriceProducts);
        
    }
}
