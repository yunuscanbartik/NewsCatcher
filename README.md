# NewsCatcher
NewsCatcher aggregates news from RSS and related sources, persists them in SQL Server, and exposes a **.NET 8 Web API** for authentication, news browsing, favorites, notifications, and admin-style CRUD operations. Optional **background workers** ingest feeds and consume queue messages.
## Features

- **OTP + JWT authentication** (`/api/Auth/GenerateOtp`, `/api/Auth/GenerateToken`) with resend throttling and email quotas on the server
- **News** CRUD and browse (`/api/News/*`)
- **Categories**, **Tags**, **News–Tag** linking
- **Users**, **User favorites**, **Notifications**, **News statistics**
- **Global rate limiting** plus a stricter limiter on `GenerateOtp`
- **Swagger UI** in development

## Solution layout
| Project | Role |
|--------|------|
| **NewsCatcher.Api** | ASP.NET Core Web API, JWT, rate limiting, Swagger |
| **NewsCatcher.Application** | Business services (auth, news, email, etc.) |
| **NewsCatcher.Domain** | Models, DTOs, interfaces |
| **NewsCatcher.Infrastructure** | SQL Server access, RabbitMQ helpers |
| **NewsCatcherBackgroundService** | Scheduled RSS ingestion |
| **NewsCatcherConsumerService** | RabbitMQ consumer worker |

## Prerequisites
- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- SQL Server (connection string in configuration)
- (Optional) SMTP for OTP emails; RabbitMQ for queue-based flows

## Configuration
Copy or edit `appsettings.json` / `appsettings.Development.json` in **NewsCatcher.Api** (and workers as needed):

- **ConnectionStrings:DefaultConnection** — SQL Server
- **AppSettings:Secret** — JWT signing key (use a strong secret in production)
- **SmtpSettings** — host, port, credentials, `From` for OTP mail
- **Rss** / **RabbitMQ** — where applicable for background services

Do **not** commit real passwords or production secrets. Use [User Secrets](https://learn.microsoft.com/en-us/aspnet/core/security/app-secrets), environment variables, or a secret manager in CI.

## Run the API
```bash
cd NewsCatcher.Api
dotnet restore
dotnet run
```
- HTTP(S) URLs are shown in the console (see `Properties/launchSettings.json`).
- Swagger: `/swagger` (typically enabled in Development).
## Run background services
```bash
cd NewsCatcherBackgroundService
dotnet run
```
```bash
cd NewsCatcherConsumerService
dotnet run
```
Adjust `appsettings.json` feed URLs and queue names for your environment.
## API highlights
| Area | Base route |
|------|----------------|
| Auth | `POST /api/Auth/GenerateOtp`, `POST /api/Auth/GenerateToken` |
| News | `GET/POST/PUT/DELETE /api/News/*` (JWT required for most) |
All user-facing API error strings are in **English**. OTP responses include `remainingTime` (seconds) and `mailSent` when applicable.
## Frontend
A separate UI (e.g. Vite + React) can call this API. Set the API base URL (e.g. `VITE_API_BASE_URL`) and enable **CORS** on the API if the SPA runs on another origin.
## Build
```bash
dotnet build NewsCatcher.sln
```
## License
Specify your license here (e.g. MIT, Apache-2.0) or remove this section if the repository is private and undecided.
## Contributing
Issues and pull requests are welcome. Please keep secrets out of git and align API changes with the existing controller and DTO patterns.
