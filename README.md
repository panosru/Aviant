Aviant Library - Modern .NET Libraries for Clean and Scalable Applications
=====================

Overview
--------

The [Aviant Library](https://github.com/panosru/Aviant) is a collection of modern, well-designed, and easy-to-use .NET libraries aimed at helping developers build clean, scalable, and maintainable applications with minimal boilerplate code. As of now, Aviant Library is only accessible by adding it as source code to your project, rather than using NuGet packages. However, I am working on publishing each project as an independent NuGet package in the near future. Unfortunately, documentation is not yet available, but you can find use cases and examples in [CleanDDDArchitecture](https://github.com/panosru/CleanDDDArchitecture)

Features
------

Some key features of the Aviant Library include:

1. **Clean Architecture**: The library is designed following the Clean Architecture principles, ensuring a clear separation of concerns and making it easy to extend and maintain over time.

2. **Domain-Driven Design (DDD)**:
    - **Transfer Objects**: Provides Data Transfer Objects (DTOs) and Entity Transfer Objects (ETOs) for effective data exchange between layers.
    - **Value Objects**: Offers value objects for modeling domain concepts effectively.

3. **Event Sourcing**:
    - **Aggregates and Aggregate Roots**: Support for creating and managing aggregates and aggregate roots in event-driven systems.
    - **Domain Events**: A framework for handling domain events and event sourcing.
    - **Event Bus**: A publish-subscribe mechanism for managing domain events.
    - **Persistence**: Tools for storing and retrieving event-sourced data.
    - **Use Cases Pattern**: Implements the use case pattern for orchestrating application logic.
    - **Orchestrator**: Facilitates the coordination of complex operations.
    - **Service Events**: Managing events within service-oriented architectures.
    - **Commands/Queries**: Provides infrastructure for implementing commands and queries.
    - **Unit of Work**: Offers a unit of work pattern for transaction management.

4. **Identity**:
    - **Users and Roles**: Features for managing user identities and roles.
    - **JWT Authentication**: Supports JSON Web Token (JWT) authentication for secure access control.

5. **Jobs**:
    - **Background Job Runner**: Utilizes Hangfire for managing and executing background jobs efficiently.

6. **Persistence**:
    - **CQRS (Command Query Responsibility Segregation)**: Separates command and query responsibilities for scalable and efficient data access.
    - **Use Case Pattern**: Implements the use case pattern for application logic.
    - **Repositories**: Provides repositories for data access.
    - **Orchestration**: Supports orchestrating complex operations.

7. **Email**:
    - **Generic Email Service**: Offers a generic email service with an SMTP client factory for sending emails.

8. **Kernel**:
    - **Application Events**: A mechanism for handling application-level events.
    - **Behaviors**: Includes pre and post-processors for behaviors like logging, performance monitoring, handling unhandled exceptions, and data validation.
    - **Commands and Queries**: Infrastructure for implementing command and query patterns.
    - **Custom Exceptions**: Supports custom exceptions for specific application needs.
    - **Aspect-Oriented Programming**: Incorporates aspects and provides tools for aspect-oriented programming.
    - **Retry Mechanism**: Utilizes Polly for implementing a robust retry mechanism.
    - **Specification Pattern**: Implements the specification pattern for query composition.
    - **Use Case Pattern**: Facilitates the use case pattern for orchestrating application logic.
    - **Entities**: Provides base classes and utilities for creating domain entities.
    - **Extensions**: Includes extensions for object mapping, collections, dictionaries, enumerables, lists, I/O operations, JSON handling, LINQ enhancements, and reflection utilities.
    - **Message Queue System**: Offers a message queue system for asynchronous communication.
    - **Custom Objects**: Provides custom objects for handling timing, such as date and time, clock, localization, etc.
    - **Cross-Cutting Concerns Helpers**: Tools and utilities for managing cross-cutting concerns efficiently.

Usage
-----

To get started with using the Aviant Library in your .NET projects, follow these steps:

1. **Add the library to your project** - Include the necessary NuGet packages or download the source code from GitHub.
2. **Import required namespaces** - Import the relevant namespace(s) into your C# code to access the functionalities provided by the Aviant Library components.
3. **Use the libraries in your application logic** - Leverage the various modules and classes within the library to implement clean, maintainable, and scalable features for your applications.

Contribution
-------------

I welcome contributions from the community! If you have ideas or improvements related to the Aviant Library, please submit a pull request on GitHub. I encourage code refactoring, new feature implementation, unit test creation, documentation updates, and any other efforts that can help improve the library for everyone's benefit.

Contact
----------

For questions, feedback, or collaboration inquiries related to the Aviant Library, please reach out via:

* [GitHub Issues](https://github.com/panosru/Aviant/issues)