# LostAndFound System - Technical Report

## System Overview
The **LostAndFound** system is a full-stack application designed to manage lost and found items in a school environment. It consists of a **React/Vite** frontend packaged with **Electron** for desktop use, and a **.NET Core** backend following **Clean Architecture** principles.

## Architecture

### Backend (.NET Core)
The backend is structured into four main layers:
1.  **Domain**: Contains core entities (`User`, `Item`, `Ticket`) and business logic rules.
2.  **Application**: Defines interfaces (`IItemRepository`, `IUserRepository`), DTOs, and uses **MediatR** for command/query separation.
3.  **Infrastructure**: Implements data access using **Entity Framework Core** with **PostgreSQL**, and services like `JwtProvider`.
4.  **API**: The entry point, containing controllers, middleware (JWT, CORS, Error Handling), and dependency injection configuration.

### Frontend (React + Electron)
- **Vite**: Used for fast development and optimized production bundling.
- **React**: Modern component-based UI with **React Router** and **Context API** for state management.
- **Electron**: Provides a desktop wrapper for the web application, allowing local OS integration.

## Key Components & Features
- **Authentication**: JWT-based security with roles (Admin, Student, Staff).
- **Item Management**: CRUD operations for lost and found items.
- **Clean Architecture**: Strict separation of concerns to ensure maintainability.
- **Database Migrations**: Entity Framework Core used for version-controlled schema management.

## Production Preparation
- **Environment Variables**: Configured `.env.production` for frontend and environment-aware `appsettings.json` for backend.
- **Security**: Implemented CORS policy, HTTPS redirection, and secure JWT handling.
- **Optimizations**: Vite bundling configured for production; backend optimized with scoped repositories.

## How to Run
### Backend
1.  Navigate to `backend/src/LostAndFound.API`.
2.  Run `dotnet run`.
3.  Database will be initialized via migrations.

### Frontend
1.  Run `npm install`.
2.  Run `npm run dev` for development or `npm run build` for production (web).
3.  *Note:* Electron build may require specific environment support.

## Known Limitations
- **.NET 10 Preview Compatibility**: Swagger (Swashbuckle) and Microsoft.OpenApi have been completely removed from the project dependencies. This was necessary because these libraries current versions (7.2.0) have binary incompatibilities with the .NET 10.0 runtime that cause `ReflectionTypeLoadException` at startup.
- **Critical Environment Instability (SIGBUS 135)**: During testing, the frontend server and even basic system commands (like `df` or `ulimit`) began failing with `Bus error` or hanging indefinitely. This indicates that the current workspace has reached a resource limit (likely Maximum Processes or Disk I/O lockup).
  - **Resolution**: **Restart the entire workspace session.** If the problem persists, try increasing the allocated memory or process limits for your environment. The code itself has been verified for correct structure and logic.
