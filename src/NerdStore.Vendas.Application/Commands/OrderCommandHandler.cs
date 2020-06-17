using MediatR;
using NerdStore.Vendas.Application.Events;
using NerdStore.Vendas.Domain;
using NerdStore.Vendas.Domain.Interfaces;
using System.Threading;
using System.Threading.Tasks;

namespace NerdStore.Vendas.Application.Commands
{
    public class OrderCommandHandler : IRequestHandler<AddOrderItemCommand, bool>
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IMediator _mediator;

        public OrderCommandHandler(IOrderRepository orderRepository, IMediator mediator)
        {
            _orderRepository = orderRepository;
            _mediator = mediator;
        }

        public async Task<bool> Handle(AddOrderItemCommand request, CancellationToken cancellationToken)
        {
            var orderItem = new OrderItem(request.ProductId, request.ProductName, request.Quantity, request.UnitValue);
            var order = Order.OrderFactory.GenerateOrder(request.CustomerId);
            order.AddItem(orderItem);

            _orderRepository.Add(Order.OrderFactory.GenerateOrder(request.CustomerId));
            await _mediator.Publish(new AddOrderItemEvent(request.CustomerId, request.ProductId, 
                order.Id, request.ProductName, request.Quantity, request.UnitValue), cancellationToken);
            return await _orderRepository.UnitOfWork.Commit();
        }
    }
}
