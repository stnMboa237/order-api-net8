using MediatR;
using System.Threading;
using System.Threading.Tasks;
using OrderSystem.Application.Features.Order.Interfaces;
using OrderSystem.Domain.Entities;

namespace OrderSystem.Application.Features.Order.Commands.Delete
{
    public class DeleteOrderCommandHandler : IRequestHandler<DeleteOrderCommand, bool>
    {
        private readonly IOrderRepository _orderRepository;

        public DeleteOrderCommandHandler(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }

        public async Task<bool> Handle(DeleteOrderCommand request, CancellationToken cancellationToken)
        {
            var order = await _orderRepository.GetByIdAsync(request.OrderId);
            if (order == null)
            {
                return false;
            }

            await _orderRepository.DeleteAsync(request.OrderId);
            return true;
        }
    }
}