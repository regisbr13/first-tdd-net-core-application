using NerdStore.Core.Data;

namespace NerdStore.Vendas.Domain.Interfaces
{
    public interface IOrderRepository : IRepository<Order>
    {
        void Add(Order order);
    }
}
