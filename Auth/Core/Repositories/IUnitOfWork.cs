namespace Auth.Core.Repositories
{
    public interface IUnitOfWork
    {
        Task CompleteAsync();
    }
}
