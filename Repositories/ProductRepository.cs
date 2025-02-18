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

        public async Task<PagedList<Product>> GetProductsPaginationAsync(ProductParams productParams)
        {
            IQueryable<Product> products = _context.Products.AsQueryable();
            //IQueryable<Product> orderProducts = products.OrderBy(p => p.ProductId).AsQueryable();
            return await PagedList<Product>.ToPagedList(products, productParams.PageNumber, productParams.PageSize);
            //Cria um pagedList usando ToPagedList
            
        }

        public async Task<PagedList<Product>> GetFiltedPricesAsync(FilterPriceProducts filterPriceProducts)
        {
            IQueryable<Product> products = _context.Products.AsQueryable();
            if (filterPriceProducts.Price.HasValue && !string.IsNullOrEmpty(filterPriceProducts.CriterionPrice))
            {
                if (filterPriceProducts.CriterionPrice.Equals("maior", StringComparison.OrdinalIgnoreCase))
                {
                    products = products.Where(p => p.Price > filterPriceProducts.Price.Value).OrderBy(p => p.Price);
                }
                if(filterPriceProducts.CriterionPrice.Equals("menor", StringComparison.OrdinalIgnoreCase))
                {
                    products = products.Where(p => p.Price < filterPriceProducts.Price.Value).OrderBy(p => p.Price);
                }
                if(filterPriceProducts.CriterionPrice.Equals("igual", StringComparison.OrdinalIgnoreCase))
                {
                    products = products.Where(p => p.Price == filterPriceProducts.Price.Value);
                }
            }
            //IQueryable<Product> orderProducts = products.OrderBy(p => p.ProductId).AsQueryable();
            return await PagedList<Product>.ToPagedList(products, filterPriceProducts.PageNumber, filterPriceProducts.PageSize);
            
        }
    }
}
