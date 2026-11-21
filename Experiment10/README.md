# Experiment 10: Student Management REST API

A complete, professional, and fully functional **Student Management REST API** built with **ASP.NET Core (.NET 8 Web API)**, **ADO.NET** (`Microsoft.Data.SqlClient`), and **SQL Server**.

---

## 📋 Features

- **Pure ADO.NET Architecture**: High-performance database connectivity using `SqlConnection`, `SqlCommand`, `SqlDataReader`, and parameterized asynchronous queries (`ExecuteReaderAsync()`, `ExecuteScalarAsync()`, `ExecuteNonQueryAsync()`). No ORM / Entity Framework used.
- **RESTful Endpoints**: Full CRUD operations conforming strictly to standard HTTP methods and status codes (`200 OK`, `201 Created`, `204 No Content`, `400 Bad Request`, `404 Not Found`, `500 Internal Server Error`).
- **Data Validation**: Comprehensive model validation using Data Annotations (`[Required]`, `[EmailAddress]`, `[Phone]`, `[Range]`, `[StringLength]`).
- **Custom Middleware**:
  1. **Exception Handling Middleware**: Centralized error handling returning structured JSON errors (`ApiErrorResponse`) without leaking sensitive details.
  2. **Request Logging Middleware**: Automatically measures execution time and logs incoming HTTP method, path, remote IP, and outgoing status code.
- **Auto Database Initialization**: On application start, automatically verifies the database, creates the table if missing, and seeds starter data.
- **Interactive Swagger UI**: Interactive documentation and live testing directly at `/swagger`.

---

## 🏗️ Architecture & Project Structure

```
Experiment10/
│── Controllers/
│   └── StudentsController.cs           # REST API CRUD controller
│── Models/
│   ├── Student.cs                      # Student domain model with validation
│   └── ApiResponse.cs                  # Standardized response & error models
│── Data/
│   ├── IStudentRepository.cs           # Repository contract
│   ├── StudentRepository.cs            # Pure ADO.NET implementation
│   └── DatabaseInitializer.cs          # Auto DB & schema initialization
│── Middleware/
│   ├── ExceptionHandlingMiddleware.cs  # Global exception handling
│   └── RequestLoggingMiddleware.cs     # Request & performance logger
│── Database/
│   └── StudentManagementDB.sql         # SQL Server DDL & DML script
│── appsettings.json                    # Connection string & configuration
│── Program.cs                          # Application startup & DI pipeline
└── StudentManagementAPI.csproj         # .NET 8 Web API project configuration
```

---

## 🗄️ Database Schema (`StudentManagementDB`)

```sql
CREATE TABLE Students (
    StudentId INT IDENTITY(1,1) PRIMARY KEY,
    FullName NVARCHAR(100) NOT NULL,
    Email NVARCHAR(100) NOT NULL UNIQUE,
    PhoneNumber NVARCHAR(20) NOT NULL,
    Department NVARCHAR(50) NOT NULL,
    Semester INT NOT NULL CHECK (Semester BETWEEN 1 AND 8),
    CGPA DECIMAL(4,2) NOT NULL CHECK (CGPA BETWEEN 0.00 AND 10.00),
    Address NVARCHAR(250) NULL,
    CreatedAt DATETIME NOT NULL DEFAULT GETDATE()
);
```

---

## 🚀 REST API Endpoints

| Method | Endpoint | Description | Success Code | Error Codes |
|---|---|---|---|---|
| `GET` | `/api/students` | Retrieve all registered students | `200 OK` | `500 Internal Server Error` |
| `GET` | `/api/students/{id}` | Retrieve student by ID | `200 OK` | `404 Not Found`, `500` |
| `POST` | `/api/students` | Register a new student | `201 Created` | `400 Bad Request`, `500` |
| `PUT` | `/api/students/{id}` | Update existing student details | `204 No Content` | `400 Bad Request`, `404 Not Found`, `500` |
| `DELETE` | `/api/students/{id}` | Delete student record | `204 No Content` | `404 Not Found`, `500` |

---

## 📝 Example JSON Payloads

### 1. `POST /api/students` (Create Student)
**Request Body:**
```json
{
  "fullName": "Karan Malhotra",
  "email": "karan.malhotra@example.com",
  "phoneNumber": "+91-9876501234",
  "department": "Mechanical Engineering",
  "semester": 5,
  "cgpa": 8.75,
  "address": "Sector 62, Noida, Uttar Pradesh"
}
```
**Response (201 Created):**
```json
{
  "studentId": 5,
  "fullName": "Karan Malhotra",
  "email": "karan.malhotra@example.com",
  "phoneNumber": "+91-9876501234",
  "department": "Mechanical Engineering",
  "semester": 5,
  "cgpa": 8.75,
  "address": "Sector 62, Noida, Uttar Pradesh",
  "createdAt": "2026-11-21T05:30:00Z"
}
```

### 2. `PUT /api/students/5` (Update Student)
**Request Body:**
```json
{
  "studentId": 5,
  "fullName": "Karan Malhotra",
  "email": "karan.malhotra@example.com",
  "phoneNumber": "+91-9876501234",
  "department": "Mechanical Engineering",
  "semester": 6,
  "cgpa": 8.92,
  "address": "Connaught Place, New Delhi"
}
```
**Response:** `204 No Content`

### 3. `DELETE /api/students/5` (Delete Student)
**Response:** `204 No Content`

---

## 🛠️ Step-by-Step Execution Guide

### Prerequisites
1. .NET 8 SDK or .NET 10 SDK installed (`dotnet --version`)
2. SQL Server LocalDB or SQL Server Express running

### Run the Application
1. Navigate to the project directory:
   ```powershell
   cd d:\Net-Technology\Experiment10
   ```
2. Build the project:
   ```powershell
   dotnet build
   ```
3. Run the API:
   ```powershell
   dotnet run
   ```
4. Open Swagger UI in your browser:
   ```
   http://localhost:5000/swagger  (or the designated port)
   ```
