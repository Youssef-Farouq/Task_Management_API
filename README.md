# Task Management API

A secure ASP.NET Core Web API for task management with JWT authentication and role-based authorization.

## Prerequisites

- .NET 7.0 SDK
- SQL Server (or SQL Server Express)
- Visual Studio 2022 or VS Code

## Environment Setup

### Development Environment

1. Clone the repository
2. Open the solution in Visual Studio or VS Code
3. The application will use `appsettings.Development.json` automatically in development
4. Run the following commands:
   ```bash
   dotnet restore
   dotnet ef migrations add InitialCreate
   dotnet ef database update
   dotnet run
   ```

### Production Environment

1. Set the following environment variables:
   ```bash
   # Database Configuration
   DB_SERVER=your_server_name
   DB_NAME=your_database_name
   DB_USER=your_username
   DB_PASSWORD=your_password

   # JWT Configuration
   JWT_SECRET_KEY=your-secure-secret-key-min-32-chars
   JWT_ISSUER=your-api-domain
   JWT_AUDIENCE=your-client-domain
   ```

2. Run the application:
   ```bash
   dotnet publish -c Release
   dotnet run --environment Production
   ```

## API Documentation

Once the application is running, you can access the Swagger documentation at:
- Development: `https://localhost:5001/swagger`
- Production: `https://your-domain/swagger`

## Security Features

- JWT Authentication with 15-minute expiration
- Refresh tokens with 7-day expiration
- Role-based authorization
- Rate limiting (50 requests per minute)
- HTTPS enforcement
- Global exception handling
- Input validation
- SQL injection protection
- XSS protection

## Database Setup

### Local Development
- Uses SQL Server LocalDB
- Database is created automatically on first run
- Connection string is configured in `appsettings.Development.json`

### Production
- Requires SQL Server instance
- Set connection string through environment variables
- Ensure proper database user permissions

## Troubleshooting

1. Database Connection Issues:
   - Verify SQL Server is running
   - Check connection string in environment variables
   - Ensure database user has proper permissions

2. JWT Authentication Issues:
   - Verify JWT environment variables are set
   - Check token expiration
   - Ensure HTTPS is properly configured

3. Rate Limiting:
   - Default: 50 requests per minute
   - Queue limit: 2 requests
   - Returns 429 status code when limit is exceeded 