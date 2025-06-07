# Clean Architecture

## Key Layers and Responsibilities

## 1. API Layer (Web/API Interface Layer)
### Responsibilities:

1. Handles HTTP requests and responses (controllers, endpoints).
2. Maps HTTP requests to application layer commands and queries.
3. Maps application layer responses to HTTP responses.
4. Contains minimal logic, focusing on routing, serialization, and deserialization.
5. Handles API-specific concerns such as authentication/authorization (routing), versioning, Swagger/OpenAPI documentation, CORS, request validation, exception handling, etc.

### Should NOT:

1. Contain any business or domain logic.
2. Reference infrastructure implementations directly.

## 2. Application Layer

### Responsibilities:

1. Contains application-specific business logic and orchestration (use cases).
2. Defines use cases, commands, queries, handlers (CQRS), and application services.
3. Orchestrates the flow, interacting with domain layer for business rules and with infrastructure layer for persistence and integrations (by abstraction).
4. Implements transaction management for use cases, if needed.
5. Handles coordination among multiple domain objects/aggregates.
6. Contains interfaces (abstractions) for persistence, messaging, and external services used by the application.

### Should NOT:

1. Contain implementation details of infrastructure (only references abstractions/interfaces).
2. Directly reference API/web concerns (should be decoupled from delivery mechanisms).

## 3. Domain Layer
### Responsibilities:

1. Contains the core business logic and rules (the heart of the system).
2. Defines entities, aggregates, value objects, domain events, domain services, and repository interfaces (not implementations).
3. Contains validation logic that relates to the domain.
4. Models ubiquitous language (business vocabulary).
5. Isolated from infrastructure, application, and API layers (pure POCOs/POJOs/Entity Classes).

### Should NOT:

1. Reference or depend on any other layer or frameworks/libraries that are not strictly necessary for the domain logic.

## 4. Infrastructure Layer
### Responsibilities:

1. Implements interfaces defined in the application and domain layers for:
2. Persistence (database access, ORMs, etc.).
3. External services (APIs, file systems, messaging, email, etc.).
4. Framework-specific concerns (logging, caching, etc.).
5. Handles I/O, third-party integrations, and frameworks.
6. May include data mappings, DTOs, adapters, migrations.

### Should NOT:

1. Contain business or workflow logic.
2. Reference the API layer (it can reference the domain and application layers as needed for implementations).

## Example Structure
```sh
+-----------------+
|    API Layer    |  <-- HTTP, GRPC, GraphQL, etc.
+-----------------+
        |
        v
+----------------------+
| Application Layer    |  <-- Use Cases, Orchestration
+----------------------+
        |
        v
+----------------------+
|    Domain Layer      |  <-- Entities, Rules, Logic
+----------------------+
        |
        v
+----------------------+
| Infrastructure Layer |  <-- DB, Messaging, File System
+----------------------+
```

## Typical request flow:

```sh

(Client Request)
    ↓        
(API Layer: Validate DTO shape, Map DTO → Command/Query)
    ↓
(Application Layer: Use Case/Command/Query Handler)
    ↓
(Domain Layer: Business Logic/Validation)
    ↓
(Application Layer: Calls abstractions for repositories/services)
    ↓
(Infrastructure Layer:
    - Implements repository interfaces, external services, database, messaging, file storage, etc.
    - Returns data/results to application layer.)
    ↓
(Application Layer: Receives data, finalizes response/use case result)
    ↓
(API Layer: Map Result/Entity → DTO, Serialize for Response)
    ↓
(Client Response)

```
### Solution Structure

```sh
MyProject
│
├── src
│   ├── MyProject.Api
│   ├── MyProject.Application
│   ├── MyProject.Domain
│   └── MyProject.Infrastructure
│
└── tests
    ├── MyProject.Api.Tests
    ├── MyProject.Application.Tests
    ├── MyProject.Domain.Tests
    └── MyProject.Infrastructure.Tests
```