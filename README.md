# OrderSystem API

A robust .NET 8 Core API project implementing **CQRS**, **MediatR**, **FluentValidation**, **Exception Middleware Handling**, **Logging**, and **XUnit Tests** for managing orders in a system. The API provides CRUD (Create, Read, Update, Delete) functionality for managing orders, with the ability to filter by date and customer name.

## Features

- **CRUD Operations**: 
  - **Create** a new order.
  - **Read** orders by ID or list with filters (e.g., by customer name and date range).
  - **Update** an existing order.
  - **Delete** an order.
  
- **Order Structure**:
  - Each order has a unique **ID**.
  - The **client name** associated with the order.
  - **Order date**.
  - A list of **order items** with details:
    - Item name
    - Quantity
    - Unit price
  - The **total value** of the order, calculated automatically based on the items.

- **Filtering**: You can filter orders by:
  - **Customer name**.
  - **Order date range** (start and end dates).

  Note: By default, this project uses an in-memory database for testing purposes.

## Technologies

- **.NET 8 Core**: The framework for building the web API.
- **CQRS (Command Query Responsibility Segregation)**: Separate models for reading and writing data.
- **MediatR**: Library for implementing the mediator pattern in CQRS, promoting loose coupling.
- **FluentValidation**: Provides easy and powerful validation rules for requests.
- **Exception Middleware Handling**: Custom error handling and global exception logging.
- **Logging**: Integrated logging with options for debugging and production environments.
- **XUnit**: Unit testing framework for testing commands, queries, and services.
- **Swagger**: API documentation and interactive testing interface.

## Running with Docker (using Docker Compose)

You can also run the API using Docker Compose. The following steps will help you get started:

1. Make sure **Docker** and **Docker Compose** are installed on your machine.

2. Build and start the containers using Docker Compose:
   ```bash
   docker-compose up --build

3. This will run the application and expose the API on port 5001.

4. Navigate to http://localhost:5001/swagger to view the Swagger documentation and interact with the API.

Note: By default, this project uses an in-memory database for testing purposes.

## Diagrama de Dados

### Tabela: `Client`
A tabela `Client` armazena informações sobre os clientes que realizaram os pedidos. 

| Campo         | Tipo    | Descrição                      |
|---------------|---------|--------------------------------|
| `Id`          | `Guid`  | Identificador único do cliente |
| `Name`        | `string`| Nome do cliente                |

### Tabela: `Order`
A tabela `Order` armazena os pedidos realizados pelos clientes.

| Campo         | Tipo    | Descrição                              |
|---------------|---------|----------------------------------------|
| `Id`          | `Guid`  | Identificador único do pedido          |
| `ClientId`    | `Guid`  | Identificador do cliente (chave estrangeira para `Client`) |
| `OrderDate`   | `DateTime`| Data do pedido                        |
| `TotalAmount` | `decimal` | Valor total do pedido (calculado automaticamente) |

### Tabela: `OrderItem`
A tabela `OrderItem` armazena os itens dos pedidos.

| Campo         | Tipo    | Descrição                              |
|---------------|---------|----------------------------------------|
| `Id`          | `Guid`  | Identificador único do item do pedido  |
| `OrderId`     | `Guid`  | Identificador do pedido (chave estrangeira para `Order`) |
| `ItemName`    | `string`| Nome do item                           |
| `Quantity`    | `int`   | Quantidade do item                     |
| `UnitPrice`   | `decimal`| Preço unitário do item                |

### Relacionamentos

- **`Client` → `Order`**: Um cliente pode ter muitos pedidos (`One-to-Many`), e cada pedido pertence a um único cliente.
- **`Order` → `OrderItem`**: Um pedido pode ter vários itens de pedido (`One-to-Many`), e cada item de pedido pertence a um único pedido.

