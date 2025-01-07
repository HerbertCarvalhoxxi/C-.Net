using APICatalogo.Context;
using APICatalogo.Models;
using APICatalogo.Pagination;
using Microsoft.EntityFrameworkCore;

namespace APICatalogo.Repositories
{
    public class ProductRepository : Repository<Product>, IProductRepository
    {
        public ProductRepository(AppDbContext context) : base(context)
        {
        }

        //public IEnumerable<Product> GetProductsPagination(ProductParams productParams)
        //{
        //  return GetAll().OrderBy(x => x.Name)
        //    .Skip((productParams.PageNumber - 1) * productParams.PageSize)
        //  .Take(productParams.PageSize).ToList();
        //}

        public PagedList<Product> GetProductsPagination(ProductParams productParams)
        {
            IQueryable<Product> products = GetAll().OrderBy(x => x.ProductId).AsQueryable(); //recebe IQueryable de products
            PagedList<Product> Orderproducts = PagedList<Product>.ToPagedList(products, productParams.PageNumber, productParams.PageSize);
            //Cria um pagedList usando ToPagedList
            return Orderproducts;
        }

        public IEnumerable<Product> GetProductsCategories(int id)
        {
            
            return GetAll().Where(c=> c.CategoryId == id);
        }
    }
}
