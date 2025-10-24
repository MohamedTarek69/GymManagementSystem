<h1 align="center">💪 Gym Management System</h1>
<p align="center">
  <b>ASP.NET Core MVC | Entity Framework Core | SQL Server</b>  
</p>
<p align="center">
  A complete web-based system for managing <b>gym members, trainers, sessions, and plans</b>.  
</p>

---

## 🏗️ Project Overview
The **Gym Management System** is designed to help gyms organize their daily operations.  
It allows admins to manage members, trainers, sessions, and memberships efficiently in a centralized dashboard.

### 🎯 Goals
- Centralize management of members, trainers, and plans.  
- Simplify scheduling and booking processes.  
- Provide real-time insights through a dashboard.  
- Maintain data consistency and validation across modules.

---

## ✨ Main Features
| Module | Description |
|--------|--------------|
| 👨‍🏫 Trainer Management | Add, update, delete, and view trainers with specialties. |
| 🧍 Member Management | Manage member profiles, health records, and memberships. |
| 🧾 Plan Management | Create, edit, deactivate (Soft Delete), and view plans. |
| 🗓️ Session Management | Full CRUD operations with trainer and category assignments. |
| 🎟️ Booking System | Manage session bookings and attendance. |
| 📊 Dashboard | Provides analytics and gym statistics. |

---

## 🧱 Architecture
The project follows a **Three-Layer Architecture**:
- Presentation Layer → ASP.NET MVC (Razor Views + Bootstrap)
- Business Logic Layer → Services (TrainerService, SessionService, etc.)
- Data Access Layer → EF Core + Repository Pattern + Unit of Work


---

## 🧰 Tech Stack
| Category | Technology |
|-----------|-------------|
| **Backend** | ASP.NET Core MVC |
| **ORM** | Entity Framework Core |
| **Database** | Microsoft SQL Server |
| **Frontend** | Razor Views + Bootstrap + Custom CSS |
| **Design Patterns** | Repository, Unit of Work, Dependency Injection |
| **Libraries** | AutoMapper |

---

## 🧬 Entities Overview

### 👤 Member
- Contains `Id`, `Name`, `Email`, `Phone`, `Gender`, `JoinDate`, `Photo`, and `Address`.
- Relationships:
  - 1️⃣ Has **one HealthRecord**  
  - 1️⃣ Has **one Membership (Plan)**  
  - ♻️ Can attend many Sessions  

### 🩺 HealthRecord
- Fields: `Height`, `Weight`, `BloodType`, `Note`, `LastUpdate`  
- Linked one-to-one with **Member**

### 🏋️ Trainer
- Fields: `Name`, `Email`, `Phone`, `HireDate`, `Specialties`
- Conducts multiple **Sessions**

### 💼 Plan
- Fields: `Name`, `Description`, `DurationDays`, `Price`, `IsActive`
- Assigned to multiple **Members**

### 🏷️ Category
- Fields: `CategoryName` (e.g., Yoga, Cardio)
- Associated with multiple **Sessions**

### ⏰ Session
- Fields: `Description`, `Capacity`, `StartDate`, `EndDate`
- Linked to one **Trainer** and one **Category**
- Attended by many **Members**

---

## ⚖️ Business Rules Highlights

### Members
- Email & phone are **unique and validated**.  
- Egyptian phone format: `(010|011|012|015)XXXXXXXX`  
- Health record required upon registration.  
- Cannot delete members with active bookings.

### Trainers
- Cannot delete trainers with future sessions.  
- Must have at least one specialty.  
- HireDate auto-generated.

### Sessions
- Capacity between **1–25**.  
- EndDate must be after StartDate.  
- Cannot delete upcoming sessions.

### Plans
- Cannot modify active plans.  
- Duration: **1–365 days**.  

### Bookings & Memberships
- Only active members can book sessions.  
- No duplicate active memberships.  
- Automatic EndDate calculation.  

---

## 🧩 MVC Controllers Overview

| Controller | Responsibilities |
|-------------|------------------|
| **HomeController** | Dashboard & overview. |
| **MemberController** | Manage member CRUD, profiles, health records. |
| **TrainerController** | Manage trainers & their specialties. |
| **SessionController** | CRUD for sessions, scheduling, booking logic. |
| **PlanController** | Manage and activate/deactivate plans. |
| **AccountController** | Login, logout, and access control. |

---

## 🔐 Identity Module
### 👨‍💼 ApplicationUser
- Fields: `FirstName`, `LastName`, `UserName`, `Email`, `Phone`
- Each user can have multiple roles.

### 🧩 IdentityRole
- Fields: `Name`, `NormalizedName`, `ConcurrencyStamp`
- Each role can have multiple users.

---

## 🗄️ Database Design
**Core Tables:**  
- `Members`, `Trainers`, `Plans`, `Sessions`, `Categories`  
**Junction Tables:**  
- `Bookings` (Members ↔ Sessions)  
- `Memberships` (Members ↔ Plans)  
**Supporting:**  
- `HealthRecords` (One-to-One with Member)

---

## 🧠 Key Tools & Concepts
- 🧭 **AutoMapper** — map between Entities and ViewModels.  
- 💉 **Dependency Injection** — for cleaner architecture.  
- 🗃️ **Repository & Unit of Work** — abstraction over EF Core.  
- 🪶 **Soft Delete** — for plan deactivation instead of permanent removal.

