# Experiment 11: Smart Campus Service Portal – Feature Extension

A complete, professional, and fully functional **Smart Campus Service Portal** built by extending the reference architecture with **ASP.NET MVC 5**, **ASP.NET Web API 2**, **ADO.NET** (`System.Data.SqlClient`), and **SQL Server**.

---

## 📋 System Features

1. **Smart Campus Dashboard**:
   - 5 KPI summary cards: Total Requests, Pending, In Progress, Completed, Rejected.
   - Interactive Graphical Statistics with Chart.js:
     - Status Distribution (Doughnut Chart)
     - Requests by Service Category (Bar Chart)
   - Recent Service Activities feed with status badges and quick action shortcuts.
2. **Full CRUD & Workflow Management**:
   - Create Service Request with client and server-side validation.
   - List all requests with dynamic search (Student name, title, department) and multi-filter (Category and Status).
   - View complete request details with student metadata and technician notes.
   - Edit ticket information.
   - Dedicated **Update Status** module with Admin Remarks.
   - Safe deletion with confirmation prompt.
3. **Pure ADO.NET Data Access Layer**:
   - `SqlConnection`, `SqlCommand`, `SqlDataReader`, `ExecuteReader()`, `ExecuteNonQuery()`, `ExecuteScalar()`.
   - Parameterized SQL queries to completely prevent SQL Injection.
   - Strictly no Entity Framework or ORM used.
4. **Unified Database & Architecture**:
   - Both MVC Controllers and REST Web API share the exact same database (`SmartCampusDB`) and DAL (`ServiceRequestDAL`).
   - Automated database verification and table creation with realistic seed data on application start.
5. **RESTful Web API (Web API 2)**:
   - `GET /api/ServiceApi` - Retrieve all requests.
   - `GET /api/ServiceApi/{id}` - Retrieve request by ID.
   - `POST /api/ServiceApi` - Create a new service request.
   - `PUT /api/ServiceApi/{id}` - Update existing request.
   - `DELETE /api/ServiceApi/{id}` - Delete request by ID.
   - `PUT /api/ServiceApi/UpdateStatus/{id}` - Update status.

---

## 🏗️ Project Structure

```
SmartCampusPortal/
│── App_Start/
│   ├── RouteConfig.cs             # MVC route registrations
│   └── WebApiConfig.cs            # Web API route & JSON formatters
│── Controllers/
│   ├── HomeController.cs          # Dashboard controller
│   ├── ServiceRequestController.cs# Full MVC CRUD & Status controller
│   └── ServiceApiController.cs    # RESTful Web API controller
│── DAL/
│   └── ServiceRequestDAL.cs       # Pure ADO.NET SQL Server operations
│── Models/
│   ├── ServiceRequest.cs          # Service request domain model
│   └── DashboardViewModel.cs      # KPI & graphical aggregation model
│── Views/
│   ├── Home/
│   │   └── Index.cshtml           # Smart Campus Dashboard
│   ├── ServiceRequest/
│   │   ├── Index.cshtml           # Filterable Service Requests List
│   │   ├── Create.cshtml          # Create Service Request Form
│   │   ├── Edit.cshtml            # Edit Request Form
│   │   ├── Details.cshtml         # View Request Details
│   │   ├── UpdateStatus.cshtml    # Update Status & Remarks Form
│   │   └── Delete.cshtml          # Delete Confirmation
│   ├── Shared/
│   │   └── _Layout.cshtml         # Modern sidebar layout with Bootstrap 5
│   └── _ViewStart.cshtml
│── Database/
│   └── SmartCampusDB.sql          # SQL Server DDL and DML scripts
│── Global.asax / Global.asax.cs   # App lifecycle & DB auto-initializer
│── Web.config                     # Database connection strings & settings
└── SmartCampusPortal.csproj       # Project file
```

---

## 🗄️ Database Schema (`SmartCampusDB`)

```sql
CREATE TABLE ServiceRequests (
    RequestId INT IDENTITY(1,1) PRIMARY KEY,
    StudentName NVARCHAR(100) NOT NULL,
    StudentEmail NVARCHAR(100) NOT NULL,
    Department NVARCHAR(50) NOT NULL,
    ServiceCategory NVARCHAR(50) NOT NULL,
    ServiceType NVARCHAR(50) NULL,
    RequestTitle NVARCHAR(150) NOT NULL,
    Description NVARCHAR(MAX) NOT NULL,
    Priority NVARCHAR(20) NOT NULL DEFAULT 'Medium',
    Status NVARCHAR(20) NOT NULL DEFAULT 'Pending',
    RequestDate DATETIME NOT NULL DEFAULT GETDATE(),
    AdminRemarks NVARCHAR(250) NULL
);
```

---

## 🌐 Web API Endpoints

| Method | Endpoint | Description | Status Code |
|---|---|---|---|
| `GET` | `/api/ServiceApi` | Retrieve all service tickets | `200 OK` |
| `GET` | `/api/ServiceApi/{id}` | Retrieve ticket by ID | `200 OK` / `404 Not Found` |
| `POST` | `/api/ServiceApi` | Create new ticket | `201 Created` / `400 Bad Request` |
| `PUT` | `/api/ServiceApi/{id}` | Update existing ticket | `200 OK` / `404 Not Found` |
| `DELETE` | `/api/ServiceApi/{id}` | Delete ticket | `200 OK` / `404 Not Found` |
| `PUT` | `/api/ServiceApi/UpdateStatus/{id}` | Update status | `200 OK` / `404 Not Found` |

---

## 🛠️ Step-by-Step Execution Guide

### Prerequisites
1. Windows OS with IIS Express
2. Microsoft SQL Server LocalDB (`(localdb)\MSSQLLocalDB`)
3. Visual Studio / MSBuild

### Running with IIS Express
1. Open PowerShell and run:
   ```powershell
   & "C:\Program Files\IIS Express\iisexpress.exe" /path:"D:\Net-Technology\Experiment11" /port:59705
   ```
2. Open your browser and navigate to:
   - **Dashboard**: `http://localhost:59705/`
   - **Service Requests**: `http://localhost:59705/ServiceRequest`
   - **New Request**: `http://localhost:59705/ServiceRequest/Create`
   - **Web API Endpoint**: `http://localhost:59705/api/ServiceApi`
