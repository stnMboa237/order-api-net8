using MediatR;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;
using OrderEntity = OrderSystem.Domain.Entities.Order;
using OrderItemEntity = OrderSystem.Domain.Entities.OrderItem;
using OrderSystem.Application.Features.Order.Interfaces;
using FluentValidation;
using OrderSystem.Domain.Entities;


namespace OrderSystem.Application.Features.Order.Commands.Create
{
    public class CreateOrderCommandHandler : IRequestHandler<CreateOrderCommand, Guid>
    {

        private readonly IOrderRepository _orderRepository;
        private readonly IValidator<CreateOrderCommand> _validator;
        private readonly IClientRepository _clientRepository;

        public CreateOrderCommandHandler(IOrderRepository orderRepository, IValidator<CreateOrderCommand> validator, IClientRepository clientRepository)
        {
            _orderRepository = orderRepository;
            _validator = validator;
            _clientRepository = clientRepository;
        }

        public async Task<Guid> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(request);
            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }

            var clients = await _clientRepository.GetByNameAsync(request.ClientName);
            Client client;
            if (clients == null || clients.Count == 0)
            {
                client = new Client { Id = Guid.NewGuid(), Name = request.ClientName };
                //throw new ArgumentException($"Client with name '{request.ClientName}' does not exist. Please add the client.");
            } else {
                client = clients[0];
            }

            var orderItems = request.Items.Select(item => new OrderItemEntity
            {
                ItemName = item.ItemName,
                Quantity = item.Quantity,
                UnitPrice = item.UnitPrice
            }).ToList();

            var order = new OrderEntity
            {
                Id = request.OrderId,
                Client = client,
                TotalAmount = request.TotalAmount,
                OrderDate = request.OrderDate,
                Items = orderItems
            };

              await _orderRepository.AddAsync(order);
              return request.OrderId;
        }
    }
}
