# 🏋️‍♂️ Gym Management System

A **web-based application** for managing gym operations — including member management, trainer scheduling, session booking, and membership plans — built using **ASP.NET Core MVC** with **Entity Framework Core** and **SQL Server**.

---

## 📋 Overview

**Description:**  
This system provides a centralized platform for gym management, allowing admins to handle members, trainers, plans, and sessions efficiently.

**Goals:**
- Centralize Members and Plans management.  
- Manage Trainers and Session schedules.  
- Track memberships and bookings.  
- Provide analytics and reports via dashboard.

---

## ⚙️ Features

- 👨‍🏫 **Trainer Management:** Full CRUD operations.  
- 🧍‍♂️ **Member Management:** Add, update, delete, and view members.  
- 💪 **Plans Management:** Update, deactivate (Soft Delete), and view plans.  
- 🧾 **Membership Management:** Assign training plans to members.  
- 🗓️ **Session Management:** Full CRUD operations with scheduling.  
- 📅 **Session Booking:** Organize and book sessions with members.  
- 📊 **Dashboard:** Provides analytics and reports.

---

## 🧱 Architecture (Three-Layer)

- **Presentation Layer:** ASP.NET MVC Controllers + Razor Views (Bootstrap for UI).  
- **Business Logic Layer:** Services (e.g., `TrainerService`, `SessionService`) handling core logic.  
- **Data Access Layer:** Repository Pattern wrapping EF Core `DbContext`.

---

## 🧰 Technology Stack

| Layer | Technology |
|-------|-------------|
| **Backend** | ASP.NET Core MVC |
| **ORM** | Entity Framework Core |
| **Database** | Microsoft SQL Server |
| **Frontend** | Razor Views + Bootstrap + Custom CSS |
| **Patterns** | Repository, Unit of Work, Dependency Injection |
| **Libraries** | AutoMapper |

---

## 🧬 Entities Overview

### 🧍 Member
- `Id`, `Name`, `Email`, `Phone`, `DateOfBirth`, `Gender`, `Address`, `JoinDate`, `Photo`
- Each Member:
  - Has one **HealthRecord**
  - Subscribes to one **Plan**
  - Can attend many **Sessions**

### 💊 HealthRecord
- `Height`, `Weight`, `BloodType`, `Note`, `LastUpdate`
- Each belongs to one **Member**

### 👨‍🏫 Trainer
- `Id`, `Name`, `Email`, `Phone`, `DateOfBirth`, `Gender`, `Address`, `Specialties`, `HireDate`
- A Trainer can conduct many **Sessions**

### 🧾 Plan
- `Id`, `Name`, `Description`, `DurationDays`, `Price`, `IsActive`
- A Plan can be assigned to many Members

### 🏷️ Category
- `Id`, `CategoryName`
- A Category can be associated with many Sessions

### 🕒 Session
- `Id`, `Description`, `Capacity`, `StartDate`, `EndDate`
- Conducted by one **Trainer** and belongs to one **Category**

---

## 🧩 Business Rules

### Members
- Email and phone must be unique and valid.  
- Egyptian phone validation: `(010|011|012|015)XXXXXXXX`  
- Cannot delete members with active bookings.  
- JoinDate auto-calculated.  
- Health record required at registration.

### Trainers
- Email and phone must be unique and valid.  
- Cannot delete trainers with future sessions.  
- Must have at least one specialty.  
- HireDate auto-calculated.

### Sessions
- Capacity: 1–25 participants.  
- EndDate must be after StartDate.  
- Requires valid Trainer & Category.  
- Cannot delete future sessions.

### Plans
- Cannot deactivate/update plans with active memberships.  
- Duration: 1–365 days.

### Bookings
1. Member must have active membership.  
2. Session must have available capacity.  
3. Member cannot book same session twice.  
4. Only future sessions can be booked.  
5. Attendance marked only for ongoing sessions.

### Memberships
1. No duplicate active memberships per member.  
2. Only active plans can be assigned.  
3. EndDate = StartDate + DurationDays.  
4. Auto-status: *Active* or *Expired*.  

---

## 🧠 MVC Components

### `HomeController`
- `Index()` → Dashboard & Statistics

### `MemberController`
- `Index()`, `Create()`, `MemberDetails()`, `HealthRecordDetails()`, `Edit()`, `Delete()`

### `TrainerController`
- `Index()`, `Create()`, `Details()`, `Edit()`, `Delete()`

### `SessionController`
- `Index()`, `Create()`, `Details()`, `Edit()`, `Delete()`

### `PlanController`
- `Index()`, `Details()`, `Edit()`, `Activate()`

### `AccountController`
- `Login()`, `Logout()`, `AccessDenied()`

---

## 🔐 Identity Module

### ApplicationUser
- `Id`, `FirstName`, `LastName`, `UserName`, `Email`, `Phone`
- One user → many roles.

### IdentityRole
- `Id`, `Name`, `NormalizedName`, `ConcurrencyStamp`
- One role → many users.

---

## 🧾 Database Schema Highlights

- **GymUser (Abstract)** → Shared fields for Members & Trainers.  
- **Booking** → Junction table (Members–Sessions).  
- **Membership** → Junction table (Members–Plans).  
- **HealthRecord** → Linked one-to-one with Member.  
- **Category** → Seeded data (Cardio, Yoga, etc.).

---

## 🚀 Key Tools

- **AutoMapper** for ViewModel ↔ Entity mapping.  
- **Dependency Injection** for clean architecture.  
- **Repository Pattern** for reusable data access logic.

---

## 👨‍💻 Developed For
**ASP.NET Course Project — Gym Management System**

---

## 🏁 Author
**Mohamed Tarek**
