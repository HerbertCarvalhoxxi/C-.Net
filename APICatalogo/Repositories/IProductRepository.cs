using APICatalogo.Models;
using APICatalogo.Pagination;

namespace APICatalogo.Repositories
{
    public interface IProductRepository : IRepository<Product>
    {
        //IEnumerable<Product> GetProductsPagination(ProductParams productParams);
        PagedList<Product> GetProductsPagination(ProductParams productParams);
        IEnumerable<Product> GetProductsCategories(int id);
    }
}
