# SkillSquare Backend API 🚀

This is the official backend API for **SkillSquare**, a platform designed to connect users with local skilled professionals. It is built using modern .NET technologies and follows a robust repository pattern architecture.

## 🌐 Live API
**Swagger UI:** [View Live API Documentation](https://skillsquare-live-api-b9czenhchfhxdwbp.centralindia-01.azurewebsites.net/index.html)

## 🛠️ Tech Stack
* **Framework:** ASP.NET Core 8/9 Web API
* **Language:** C#
* **Database:** Azure SQL Database & Entity Framework Core
* **Authentication:** ASP.NET Core Identity & JWT (JSON Web Tokens)
* **Real-time Communication:** SignalR (WebSockets)
* **Cloud Hosting:** Microsoft Azure App Service

## 🏗️ Architecture & Features
* **N-Tier Architecture:** Separation of concerns using Data Access Layer (DAL) and Controllers.
* **Repository Pattern:** Clean abstraction of database operations.
* **Role-Based Authorization:** Distinct access levels for Admins, Providers, and standard Users.
* **Real-time Notifications:** Instant alerts using SignalR Hubs.


## 🧪 Demo Test Credentials
To help reviewers and developers test the API, the following demo accounts are available:
### 👤 Customer
- **Email:** `test@gmail.com`
- **Password:** `Test@123`
### 🛠️ Provider
- **Email:** `provider22@gmail.com`
- **Password:** `Provider@123`
### 🛡️ Admin
- **Email:** `admin@skillsquare.com`
- **Password:** `Admin@123`

## 🔐 Authentication (How to test)
This API uses Bearer JWT for security. To test endpoints in Swagger:
1. Go to the `/api/Auth/login` endpoint.
2. Enter your credentials to receive a JWT Token.
3. Click the green **Authorize** button at the top of the Swagger page.
4. Type `Bearer ` followed by a space, and paste your token.

## 💻 Local Development Setup
1. Clone the repository.
2. Update the `DefaultConnection` string in `appsettings.json` with your local SQL Server details.
3. Ensure you have your `JwtSettings` configured in `appsettings.json`.
4. Run `Update-Database` in the Package Manager Console to apply EF Core migrations.
5. Press `F5` to run the project locally via IIS Express or Kestrel.
