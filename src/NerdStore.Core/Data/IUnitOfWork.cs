using NerdStore.Core.DomainObjects;
using System.Threading.Tasks;

namespace NerdStore.Core.Data
{
    public interface IUnitOfWork
    {
        Task<bool> Commit();
    }

    public interface IRepository<T> where T : IAgregateRoot
    {
        IUnitOfWork UnitOfWork { get; }
    }
}
