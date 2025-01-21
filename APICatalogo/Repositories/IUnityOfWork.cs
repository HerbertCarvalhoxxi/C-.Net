namespace APICatalogo.Repositories
{
    public interface IUnityOfWork
    {
        IProductRepository ProductRepository { get; }
        ICategoryRepository CategoryRepository { get; }
        Task CommitAsync();
    }
}
