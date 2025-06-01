# 📅 Date Management System

A full-featured ASP.NET Core MVC web application for managing user events, reminders, timesheets, and leave requests — with secure user authentication and role-based access.

---

## 🚀 Features

### ✅ Authentication & User Roles
- Registration and login (with Identity)
- Role-based access control: Admins and Users
- Secure password hashing using Argon2

### 🧑‍💼 Profile Management
- Update personal information: name, email
- Change password
- Set preferred country (used in calendar/holidays)
- Account deletion support

### 📆 Calendar & Event System
- Monthly/weekly calendar view
- Highlighted holidays (via Nager.Date API)
- Create, edit, delete personal events and reminders
- Filter events and reminders by date

---

## 🛠 Tech Stack

- **ASP.NET Core MVC (C#)**
- **Entity Framework Core** for data access
- **Microsoft Identity** for authentication
- **Bootstrap 5** + custom CSS for responsive design
- **JavaScript** for dynamic front-end behaviors
- **SQL Server** (default) — easily switchable
- **Nager.Date API** for holiday lookups

---

## ⚙️ Getting Started

### Prerequisites

- [.NET 8 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/8.0)
- [SQL Server LocalDB or SQL Express](https://docs.microsoft.com/en-us/sql/database-engine/configure-windows/sql-server-express-localdb)
- [Visual Studio 2022](https://visualstudio.microsoft.com/vs/) with ASP.NET and EF workloads

### Setup Instructions

```bash
git clone https://github.com/your-username/dateManagementHTML.git
```

Open NuGet Package Manager
```bash
Add-Migration InitialSetup
Update-database
```
An Admin account for first time setup will be seeded and account details can be viewed inside appsettings.json

!!! DO NOT FORGET TO CHANGE ADMIN PASSWORD AFTER SETUP !!!
