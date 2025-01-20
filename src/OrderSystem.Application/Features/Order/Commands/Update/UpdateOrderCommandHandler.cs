using MediatR;
using System.Threading;
using System.Threading.Tasks;
using OrderSystem.Application.Features.Order.Interfaces;
using OrderSystem.Domain.Entities;
using System.Linq;
using FluentValidation;

namespace OrderSystem.Application.Features.Order.Commands.Update
{
    public class UpdateOrderCommandHandler : IRequestHandler<UpdateOrderCommand, bool>
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IValidator<UpdateOrderCommand> _validator;
        private readonly IClientRepository _clientRepository;


        public UpdateOrderCommandHandler(IOrderRepository orderRepository, IValidator<UpdateOrderCommand> validator, IClientRepository clientRepository)
        {
            _orderRepository = orderRepository;
            _validator = validator;
            _clientRepository = clientRepository;

        }

        public async Task<bool> Handle(UpdateOrderCommand request, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(request);
            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }

            var client = await _clientRepository.GetByNameAsync(request.ClientName);
            if (client == null || client.Count == 0)
            {
                throw new ArgumentException($"Client with name '{request.ClientName}' does not exist. Please add the client.");
            }

            var order = await _orderRepository.GetByIdAsync(request.OrderId);
            if (order == null)
            {
                return false; 
            }

            order.Client = client[0];
            order.OrderDate = request.OrderDate;

            order.Items.Clear();
            var newItems = request.Items.Select(item => new OrderItem
            {
                ItemName = item.ItemName,
                Quantity = item.Quantity,
                UnitPrice = item.UnitPrice
            }).ToList();

            order.Items.AddRange(newItems);
            order.TotalAmount = request.TotalAmount;

            await _orderRepository.UpdateAsync(order);

            return true;
        }
    }
}