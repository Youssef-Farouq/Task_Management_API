# Task Manager API

A robust .NET 8.0 Web API for managing tasks with authentication and authorization features.

## Project Overview

This is a Task Management API built with ASP.NET Core 8.0 that provides endpoints for user authentication, task management, and user management. The application uses JWT (JSON Web Tokens) for authentication and Entity Framework Core for data persistence.

## Technology Stack

- **Framework**: .NET 8.0
- **Database**: SQL Server with Entity Framework Core
- **Authentication**: JWT (JSON Web Tokens)
- **API Documentation**: Swagger/OpenAPI
- **Password Hashing**: BCrypt.Net-Next

## Project Structure

```
TaskManager/
├── Controllers/         # API endpoints
├── Models/             # Data models
│   ├── TaskItem.cs     # Task entity
│   ├── User.cs         # User entity
│   └── RefreshToken.cs # Refresh token entity
├── Data/               # Database context and configurations
├── Middleware/         # Custom middleware components
├── Migrations/         # Database migrations
├── Properties/         # Project properties
├── Program.cs          # Application entry point and configuration
└── appsettings.json    # Application configuration
```

## Key Features

1. **Authentication & Authorization**
   - JWT-based authentication
   - Refresh token mechanism
   - Secure password hashing with BCrypt

2. **Task Management**
   - Create, read, update, and delete tasks
   - Task status tracking
   - Task assignment to users

3. **User Management**
   - User registration and login
   - User profile management
   - Role-based access control

4. **Security Features**
   - Rate limiting
   - Global exception handling
   - CORS policy configuration
   - HTTPS redirection

## API Documentation

The API documentation is available through Swagger UI when running the application. Access it at `/swagger` endpoint.

## Setup and Installation

1. **Prerequisites**
   - .NET 8.0 SDK
   - SQL Server
   - Visual Studio 2022 or VS Code

2. **Configuration**
   - Update the connection string in `appsettings.json`
   - Configure JWT settings in `appsettings.json`:
     ```json
     "Jwt": {
       "Key": "your-secret-key",
       "Issuer": "your-issuer",
       "Audience": "your-audience"
     }
     ```

3. **Database Setup**
   - Run Entity Framework migrations:
     ```bash
     dotnet ef database update
     ```

4. **Running the Application**
   ```bash
   dotnet run
   ```

## Security Considerations

- JWT tokens are used for authentication
- Passwords are hashed using BCrypt
- Rate limiting is implemented to prevent abuse
- CORS is configured for specific origins
- HTTPS is enforced
- Global exception handling for secure error responses

## Dependencies

- Microsoft.AspNetCore.Authentication.JwtBearer (8.0.0)
- Microsoft.EntityFrameworkCore (8.0.0)
- Microsoft.EntityFrameworkCore.SqlServer (8.0.0)
- BCrypt.Net-Next (4.0.3)
- Swashbuckle.AspNetCore (6.5.0)

## Development

The project follows standard .NET development practices and includes:
- Entity Framework Core for data access
- Repository pattern for data operations
- Middleware for cross-cutting concerns
- Swagger for API documentation

## Contributing

1. Fork the repository
2. Create a feature branch
3. Commit your changes
4. Push to the branch
5. Create a Pull Request

## License

This project is licensed under the MIT License. 
