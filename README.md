#  Feline Gallery

> A full-featured online art gallery platform for discovering, browsing, and purchasing feline-themed artwork — built with ASP.NET Core 8 MVC.

[![.NET](https://img.shields.io/badge/.NET-8.0-512BD4?style=flat-square&logo=dotnet)](https://dotnet.microsoft.com/)
[![ASP.NET Core MVC](https://img.shields.io/badge/ASP.NET_Core-MVC-512BD4?style=flat-square&logo=dotnet)](https://docs.microsoft.com/en-us/aspnet/core/mvc/)
[![SQLite](https://img.shields.io/badge/Database-SQLite-003B57?style=flat-square&logo=sqlite)](https://www.sqlite.org/)
[![Entity Framework Core](https://img.shields.io/badge/ORM-Entity_Framework_Core_8-512BD4?style=flat-square)](https://docs.microsoft.com/en-us/ef/core/)
[![License](https://img.shields.io/badge/License-Apache_2.0-blue?style=flat-square)](LICENSE)

---

## 📖 Table of Contents

- [About](#about)
- [Features](#features)
- [Tech Stack](#tech-stack)
- [Architecture](#architecture)
- [Getting Started](#getting-started)
- [Default Credentials](#default-credentials)
- [Project Structure](#project-structure)
- [Deployment](#deployment)
- [Contributing](#contributing)
- [License](#license)

---

## About

Feline Gallery is a full-stack ASP.NET Core 8 MVC web application that serves as an online gallery platform for cat-themed art. It supports two user roles — **Admin** and **Customer** — with a rich claims-based authorization system that controls fine-grained access to features like artwork management, exhibitions, artist profiles, order processing, and more.

The platform was designed with a clean separation of concerns using the **Repository Pattern**, **ViewModels**, and **Service layers**, making it maintainable and extensible.

---

## Features

### Customer-Facing
- Browse artworks by category, artist, and exhibition
- View detailed artwork and artist profile pages
- Place and track orders
- Register / Login with secure cookie-based authentication
- Responsive UI across devices

### Admin Panel (`/Admin`)
- **Dashboard** — overview of gallery activity
- **Artwork Management** — create, edit, delete artworks with image uploads
- **Category Management** — organize artworks into categories
- **Artist Management** — manage artist profiles and bios
- **Exhibition Management** — schedule and manage gallery exhibitions
- **Order Management** — view and process customer orders
- **Role & Claims-Based Access** — fine-grained permission control per admin action

---

## Tech Stack

| Layer | Technology |
|---|---|
| Framework | ASP.NET Core 8 MVC |
| Language | C# 12 |
| ORM | Entity Framework Core 8 |
| Query Layer | Dapper |
| Database | SQLite |
| Authentication | ASP.NET Core Identity |
| Authorization | Claims-based policies |
| Frontend | Razor Views, HTML5, CSS3, JavaScript |
| Session | In-memory distributed session |

---

## Architecture

```
FelineGallery/
├── Areas/
│   └── Admin/               # Admin area (controllers, views)
├── Authorization/
│   ├── Handler/             # IAuthorizationHandler implementations
│   └── Requirement/         # Custom authorization requirements & policies
├── Controllers/             # Customer-facing MVC controllers
├── Data/
│   └── ApplicationDbContext # EF Core DbContext
├── Migrations/              # EF Core database migrations
├── Models/
│   ├── Interfaces/          # Repository contracts
│   └── Repositories/        # Repository implementations
├── Services/                # Business logic & claims service
├── ViewModels/              # Strongly-typed view models
├── Views/                   # Razor views
└── wwwroot/                 # Static assets (CSS, JS, images)
```

The application follows the **Repository Pattern** — controllers never touch the DbContext directly. All data access goes through repository interfaces, making testing and swapping implementations straightforward.

---

## Getting Started

### Prerequisites

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- [Visual Studio 2022](https://visualstudio.microsoft.com/) (or VS Code with C# extension)
- Git

### Local Setup

```bash
# 1. Clone the repository
git clone https://github.com/Noorumms/FelineGallery.git
cd FelineGallery

# 2. Restore dependencies
dotnet restore

# 3. Apply database migrations
dotnet ef database update

# 4. Run the application
dotnet run
```

The app will be available at `https://localhost:5001` (or the port shown in your terminal).

> The database is seeded automatically on first run — an admin account is created and all roles/claims are configured.

---

## Default Credentials

| Role | Email | Password |
|------|-------|----------|
| Admin | `admin@felinegallery.com` | `Admin@123` |

> **Security note:** Change the admin password immediately after first login in any production environment.

---

## Project Structure — Key Files

| File | Purpose |
|------|---------|
| `Program.cs` | App bootstrap, DI registration, middleware pipeline, seeding |
| `Data/ApplicationDbContext.cs` | EF Core DbContext with Identity |
| `Authorization/` | Custom claims types, permissions, policies, and handlers |
| `Services/ClaimsService.cs` | Assigns role-specific claims to users on registration |
| `appsettings.json` | App configuration and connection strings |

---

## Deployment

### Deploy to Railway

This app is configured for one-click deployment to [Railway](https://railway.app).

**Prerequisites:** A Railway account (free tier works) and your repo pushed to GitHub.

#### Steps

1. Go to [railway.app](https://railway.app) → **New Project** → **Deploy from GitHub repo**
2. Select `Noorumms/FelineGallery`
3. Railway will auto-detect the .NET project and start building
4. Once deployed, go to **Settings → Networking → Generate Domain** to get a public URL
5. The app auto-runs migrations and seeds the admin account on first boot

#### Environment Variables (Railway Dashboard → Variables)

No required environment variables for basic deployment. The app uses SQLite with an internal file path.

If you want to override settings, you can set:

```
ASPNETCORE_ENVIRONMENT=Production
```

#### Notes on SQLite in Production

SQLite works well for low-to-medium traffic. The database file is stored at the app's content root. Note that Railway's filesystem is **ephemeral** — if the container restarts, the SQLite file resets. For persistent data in production, consider:
- Mounting a Railway **Volume** (persistent disk)
- Migrating to a hosted PostgreSQL database

---

## Contributing

Contributions, issues and feature requests are welcome.

1. Fork the repository
2. Create a feature branch (`git checkout -b feature/your-feature`)
3. Commit your changes (`git commit -m 'Add some feature'`)
4. Push to the branch (`git push origin feature/your-feature`)
5. Open a Pull Request

---

## License

Distributed under the Apache 2.0 License. See [`LICENSE`](LICENSE) for more information.

---

<p align="center">Built with ❤️ using ASP.NET Core 8</p>
