# Contacts API  
## Project Description  

Contacts API is a web application for managing contacts. It performs CRUD operations on two related tables: Contact and Person. The implementation follows Clean Architecture principles, ensuring clear layer separation and modularity of the code.  

## Key Features  

- CRUD operations for Contact and Person entities  
- Interaction with the API via Swagger UI for convenient testing  
- The project structure is organized into four separate projects plus unit tests  

## Project Architecture  

The project is divided into several layers:  
- **Contacts-API.Domain** — data models and database context.  
- **Contacts-API.Application** — application business logic (CQRS, command, and query handlers).  
- **Contacts-API.Infrastructure** — repositories and data access.  
- **Contacts-API.API** — API layer providing application functionality through HTTP (Web API).  
- **Contacts-API.Tests** — unit tests for system components.  

## Project Deployment  

To deploy the project in a Docker container, run the following command from the root directory:  

```
docker-compose up --build
```

This command will create containers, initialize the database, and populate it with test data.  

> [!IMPORTANT]  
> After successful startup, the API will be available via `Swagger UI` for testing all methods.  

## Tools and Technologies  

- ASP.NET Core Web API (.NET 8)  
- Swagger UI for API documentation and testing  
- PostgreSQL and EF Core for database interaction  
- Docker for containerization  
- MediatR for CQRS implementation  
- XUnit, FluentAssertions, FakeItEasy for unit testing  
