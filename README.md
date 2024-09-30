# Solution overview
WIP

## Table of contents
- [Solution structure](#solution-structure)
- [Patterns and practices](#patterns-and-practices)
- [Packages and tools](#packages-and-tools)
- [Intended use](#intended-use)

## Warnings
Remember not to expose passwords and/or sensitive information that may be hardcoded into .json/dockerfile/docker-compose.yml files only due to educational purposes.
WIP

## Solution structure

The solution is organized into several projects, each serving a specific purpose:

1. **Api**: This project contains the web API for the application.
2. **Application**: This project contains the business logic and application services.
3. **Infrastructure**: This project contains the data access logic and infrastructure services.
4. **Domain**: This project contains the domain models and enums.
5. **Receptor**: This project is an executable that handles specific tasks, such as message processing.
6. **PrintStatusConsumer**: This project is an executable that consumes print job status messages from Kafka and processes them.
7. **Common**: This project contains shared utilities and common code used across other projects.

## Patterns and practices

The solution follows several design patterns and best practices:

- **Domain-Driven Design (DDD)**: The solution is structured around the domain model, with a clear separation between the domain, application, and infrastructure layers.
- **Repository Pattern**: Used in the `Infrastructure` project to abstract data access logic.
- **Dependency Injection**: Used throughout the solution to manage dependencies and promote loose coupling.
- **CQRS (Command Query Responsibility Segregation)**: Commands and queries are separated to improve scalability and maintainability.

## Packages and tools

The solution leverages several packages and tools to enhance functionality and development experience:

- **Entity Framework Core**: Used for data access and ORM (Object-Relational Mapping).
- **Serilog**: Used for logging.
- **Swashbuckle**: Used for API documentation.
- **Confluent.Kafka**: Used for Kafka messaging.
- **Dapper**: Used for lightweight data access.
- **Microsoft.Extensions.Configuration**: Used for configuration management.
- **System.Data.SqlClient**: Used for SQL Server data access.
- **Docker**: Used for containerization and deployment.

### Key features

- **Web API**: Exposes endpoints for client applications.
- **Business Logic**: Encapsulated in the `Application` project to ensure a clean separation of concerns.
- **Data Access**: Managed through Entity Framework Core and Dapper for efficient database interactions.
- **Messaging**: Kafka integration for handling asynchronous communication.
- **Logging**: Comprehensive logging with Serilog for monitoring and troubleshooting.
- **Configuration**: Centralized configuration management using `Microsoft.Extensions.Configuration`.

### Getting started
WIP
