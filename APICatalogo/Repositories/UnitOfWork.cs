using APICatalogo.Context;

namespace APICatalogo.Repositories;

public class UnitOfWork : IUnityOfWork
{
    private IProductRepository? _productRepository;
    private ICategoryRepository? _categoryRepository;

    public AppDbContext _context;

    public UnitOfWork(AppDbContext context)
    {
        this._context = context;
    }
    public IProductRepository ProductRepository
    {
        get
        {
            return _productRepository = _productRepository ?? new ProductRepository(_context);
        }
    }

    public ICategoryRepository CategoryRepository
    {
        get
        {
            return _categoryRepository = _categoryRepository ?? new CategoryRepository(_context);
        }

    }

    public async Task CommitAsync()
    {
       await _context.SaveChangesAsync();
    }
    public void Dispose()
    {
        _context.Dispose();
    }
}
