# Experiment 9: Placement Management Dashboard

A complete, production-ready **Placement Management Dashboard** developed using **ASP.NET Web Forms (C#)**, **.NET Framework (v4.7.2)**, **ADO.NET**, and **Microsoft SQL Server**.

This project provides an in-depth practical demonstration of the **five fundamental ASP.NET Data Controls** with connected database architecture and responsive Bootstrap 5 styling.

---

## 🌟 The 5 ASP.NET Data Controls Demonstrated

| # | Data Control | Page Implemented | Practical Functionality Demonstrated |
| :--- | :--- | :--- | :--- |
| **1** | **`GridView`** | `Students.aspx` | Tabular display of students, multi-column dynamic **sorting**, **paging** (5 records/page), inline **RowEditing**, **RowUpdating**, and **RowDeleting** with confirmation. |
| **2** | **`DetailsView`** | `Students.aspx` | Single-record detailed inspection, bound fields, custom status badges, and built-in **record-by-record pagination navigation**. |
| **3** | **`FormView`** | `Students.aspx` | Custom-templated record insertion (`DefaultMode="Insert"`), client and server-side validation, RadioButtonLists, and parameterized SQL INSERT. |
| **4** | **`Repeater`** | `Default.aspx` | Lightweight, template-driven feed of recent campus placement activities demonstrating `<HeaderTemplate>`, `<ItemTemplate>`, `<AlternatingItemTemplate>`, and `<FooterTemplate>`. |
| **5** | **`DataList`** | `Companies.aspx` | Responsive card-based catalog (`RepeatColumns="3"`, `RepeatLayout="Flow"`) displaying partner recruiter companies, packages, and eligibility criteria. |

---

## 🏗️ Project Architecture & Directory Structure

```text
D:\Net-Technology\Experiment9/
│
├── App_Code/
│   ├── DatabaseHelper.cs       # SQL connection manager & automatic DB/table seeding
│   ├── StudentDAL.cs          # ADO.NET Data Access Layer for Students (Paging, Sorting, CRUD)
│   ├── CompanyDAL.cs          # ADO.NET Data Access Layer for Companies (DataList queries)
│   ├── PlacementDAL.cs        # ADO.NET Data Access Layer for Dashboard KPIs & Repeater feed
│   └── Models/
│       ├── Student.cs         # Student entity
│       ├── Company.cs         # Company recruiter entity
│       ├── Placement.cs       # Offer letter mapping entity
│       └── DashboardStats.cs  # Aggregated KPI metrics model
│
├── CSS/
│   └── site.css               # Modern Bootstrap 5 admin styling & Data Controls theming
│
├── JavaScript/
│   └── site.js                # Sidebar toggle, live Indian date clock & alert dismissers
│
├── Database/
│   └── PlacementDB.sql        # Standalone T-SQL schema, constraints & seed records
│
├── Site.Master                # Master page with responsive dark sidebar navigation & notifications
├── Site.Master.cs
├── Site.Master.designer.cs
│
├── Default.aspx               # Executive Dashboard (6 KPI cards + ASP.NET Repeater Control)
├── Default.aspx.cs
├── Default.aspx.designer.cs
│
├── Students.aspx              # Tabbed showcase of GridView, DetailsView & FormView
├── Students.aspx.cs
├── Students.aspx.designer.cs
│
├── Companies.aspx             # Corporate Recruiters showcase (ASP.NET DataList Control)
├── Companies.aspx.cs
├── Companies.aspx.designer.cs
│
├── Placements.aspx            # Placement record issuance & mapping table
├── Placements.aspx.cs
├── Placements.aspx.designer.cs
│
├── Web.config                 # Connection strings & .NET Framework 4.7.2 configuration
├── Experiment9.csproj         # Visual Studio Web Application Project file
├── Experiment9.sln            # Visual Studio Solution file
└── README.md                  # Comprehensive practical documentation
```

---

## 🗄️ Database Architecture (`PlacementDB`)

The database consists of three relational tables with Foreign Keys and Check Constraints:

```sql
CREATE DATABASE PlacementDB;
GO
USE PlacementDB;
GO

-- 1. Students Table
CREATE TABLE Students (
    StudentID INT IDENTITY(1,1) PRIMARY KEY,
    StudentName NVARCHAR(100) NOT NULL,
    Email NVARCHAR(100) NOT NULL UNIQUE,
    Department NVARCHAR(50) NOT NULL,
    Semester INT NOT NULL CHECK (Semester BETWEEN 1 AND 8),
    CGPA DECIMAL(3,2) NOT NULL CHECK (CGPA BETWEEN 0.00 AND 10.00),
    Skills NVARCHAR(250) NULL,
    PlacementStatus NVARCHAR(20) NOT NULL DEFAULT 'Not Placed',
    CreatedDate DATETIME2 NOT NULL DEFAULT GETDATE(),
    CHECK (PlacementStatus IN ('Placed', 'Not Placed'))
);

-- 2. Companies Table
CREATE TABLE Companies (
    CompanyID INT IDENTITY(1,1) PRIMARY KEY,
    CompanyName NVARCHAR(100) NOT NULL,
    Location NVARCHAR(100) NOT NULL,
    JobRole NVARCHAR(100) NOT NULL,
    PackageLPA DECIMAL(5,2) NOT NULL CHECK (PackageLPA > 0),
    EligibilityCriteria NVARCHAR(250) NOT NULL,
    VisitDate DATE NOT NULL,
    CreatedDate DATETIME2 NOT NULL DEFAULT GETDATE()
);

-- 3. Placements Table
CREATE TABLE Placements (
    PlacementID INT IDENTITY(1,1) PRIMARY KEY,
    StudentID INT NOT NULL,
    CompanyID INT NOT NULL,
    OfferDate DATE NOT NULL DEFAULT GETDATE(),
    PackageOffered DECIMAL(5,2) NOT NULL CHECK (PackageOffered > 0),
    Designation NVARCHAR(100) NOT NULL,
    Status NVARCHAR(20) NOT NULL DEFAULT 'Accepted',
    FOREIGN KEY (StudentID) REFERENCES Students(StudentID) ON DELETE CASCADE,
    FOREIGN KEY (CompanyID) REFERENCES Companies(CompanyID) ON DELETE CASCADE
);
GO
```

> **Automatic DB Provisioning:** `App_Code/DatabaseHelper.cs` verifies and initializes the `PlacementDB` database and tables automatically on the first request to LocalDB, seeding 10 students, 6 top-tier recruiters (Microsoft, AWS, Infosys, Deloitte, TCS, LTIMindtree), and 7 confirmed placements!

---

## 📊 Dashboard KPI Metrics (`Default.aspx`)
The dashboard aggregates the following live metrics:
1. **Total Registered Students** (Count of active student records)
2. **Total Partner Companies** (Count of visiting recruiters)
3. **Students Placed** (Students with accepted offers)
4. **Students Not Placed** (Students seeking placement opportunities)
5. **Total Placement Offers** (Total offer letters recorded)
6. **Overall Placement Rate** ($\frac{\text{Placed}}{\text{Total}} \times 100\%$)
7. **Highest Package** (e.g. ₹24.00 LPA) & **Average CTC** (e.g. ₹11.68 LPA)

---

## 🚀 How to Run the Project

### Option A: Using IIS Express (Command Line)
1. Open PowerShell or Command Prompt.
2. Launch IIS Express pointing to the Experiment9 directory:
   ```powershell
   & "C:\Program Files\IIS Express\iisexpress.exe" /path:"D:\Net-Technology\Experiment9" /port:8080
   ```
3. Open your browser and navigate to:
   ```text
   http://localhost:8080/Default.aspx
   ```

### Option B: Using Visual Studio
1. Double-click `Experiment9.sln` or open Visual Studio and choose `File > Open > Project/Solution` &rarr; `D:\Net-Technology\Experiment9\Experiment9.sln`.
2. Press **F5** or click **IIS Express (Google Chrome / Edge)**.
3. Visual Studio will build and launch the browser automatically!

### Option C: Building with MSBuild
```powershell
& "C:\Program Files\Microsoft Visual Studio\18\Enterprise\MSBuild\Current\Bin\MSBuild.exe" "D:\Net-Technology\Experiment9\Experiment9.csproj"
```

