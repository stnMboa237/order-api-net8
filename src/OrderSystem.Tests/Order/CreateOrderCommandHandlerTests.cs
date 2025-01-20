using FluentAssertions;
using Moq;
using OrderSystem.Application.Features.Order.Commands.Create;
using OrderSystem.Application.Features.Order.Interfaces;
using OrderSystem.Domain.Entities;
using OrderEntity = OrderSystem.Domain.Entities.Order;
using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace OrderSystem.Tests.Application.Features.Order.Commands
{
    public class CreateOrderCommandHandlerTests
    {
        private readonly Mock<IOrderRepository> _mockOrderRepository;
        private readonly Mock<IClientRepository> _mockClientRepository;
        private readonly Mock<IValidator<CreateOrderCommand>> _mockValidator;
        private readonly CreateOrderCommandHandler _handler;

        public CreateOrderCommandHandlerTests()
        {
            _mockOrderRepository = new Mock<IOrderRepository>();
            _mockClientRepository = new Mock<IClientRepository>();
            _mockValidator = new Mock<IValidator<CreateOrderCommand>>();
            _handler = new CreateOrderCommandHandler(_mockOrderRepository.Object, _mockValidator.Object, _mockClientRepository.Object);
        }

        [Fact]
        public async Task Handle_ShouldCreateOrder_WhenClientExists()
        {
            // Arrange
            var command = new CreateOrderCommand
            {
                OrderId = Guid.NewGuid(),
                ClientName = "Test Client",
                OrderDate = DateTime.UtcNow,
                Items = new List<CreateOrderItemCommand>
                {
                    new CreateOrderItemCommand { ItemName = "Item1", Quantity = 1, UnitPrice = 50.00m },
                    new CreateOrderItemCommand { ItemName = "Item2", Quantity = 1, UnitPrice = 50.00m }
                }
            };

            // Mock validation
            _mockValidator.Setup(v => v.ValidateAsync(It.IsAny<CreateOrderCommand>(), It.IsAny<CancellationToken>()))
                          .ReturnsAsync(new FluentValidation.Results.ValidationResult());

            // Mock client repository (client exists)
            _mockClientRepository.Setup(r => r.GetByNameAsync(command.ClientName))
                .ReturnsAsync(new List<Client> { new Client { Id = Guid.NewGuid(), Name = command.ClientName } });

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.Should().Be(command.OrderId);
            _mockOrderRepository.Verify(r => r.AddAsync(It.Is<OrderEntity>(o => o.Client.Name == command.ClientName && o.Items.Count == command.Items.Count)), Times.Once);
        }

        [Fact]
        public async Task Handle_ShouldCreateNewClient_WhenClientDoesNotExist()
        {
            // Arrange
            var command = new CreateOrderCommand
            {
                OrderId = Guid.NewGuid(),
                ClientName = "New Client",
                OrderDate = DateTime.UtcNow,
                Items = new List<CreateOrderItemCommand>
                {
                    new CreateOrderItemCommand { ItemName = "Item1", Quantity = 1, UnitPrice = 150.00m }
                }
            };

            // Mock validation
            _mockValidator.Setup(v => v.ValidateAsync(It.IsAny<CreateOrderCommand>(), It.IsAny<CancellationToken>()))
                          .ReturnsAsync(new FluentValidation.Results.ValidationResult());

            // Mock client repository (client does not exist)
            _mockClientRepository.Setup(r => r.GetByNameAsync(command.ClientName))
                .ReturnsAsync(new List<Client>());

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.Should().Be(command.OrderId);
            _mockOrderRepository.Verify(r => r.AddAsync(It.Is<OrderEntity>(o => o.Client.Name == command.ClientName && o.Items.Count == command.Items.Count)), Times.Once);
            _mockClientRepository.Verify(r => r.GetByNameAsync(command.ClientName), Times.Once); // Verify if GetByNameAsync was called
        }

        [Fact]
        public async Task Handle_ShouldThrowValidationException_WhenCommandIsInvalid()
        {
            // Arrange
            var command = new CreateOrderCommand
            {
                OrderId = Guid.NewGuid(),
                ClientName = "Invalid Client",
                OrderDate = DateTime.UtcNow,
                Items = new List<CreateOrderItemCommand>() // No items
            };

            // Mock validation to return errors
            var validationResult = new FluentValidation.Results.ValidationResult(
                new List<FluentValidation.Results.ValidationFailure>
                {
                    new FluentValidation.Results.ValidationFailure("Items", "At least one item is required.")
                });
            _mockValidator.Setup(v => v.ValidateAsync(It.IsAny<CreateOrderCommand>(), It.IsAny<CancellationToken>()))
                          .ReturnsAsync(validationResult);

            // Act
            Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<ValidationException>()
                     .WithMessage("Validation failed: \n -- Items: At least one item is required. Severity: Error");
        }
    }
}