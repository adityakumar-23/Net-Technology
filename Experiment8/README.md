# Experiment 8: Inventory Management System

A complete, production-ready **Inventory Management System** developed using **ASP.NET Core MVC (net10.0)**, **ADO.NET**, and **Microsoft SQL Server**.

This project strictly adheres to pure ADO.NET architecture without Entity Framework or EF Core, illustrating connected and disconnected database access patterns, parameterized queries, and transactional CRUD operations.

---

## 🌟 Key Features & Architectural Highlights

### 1. Pure ADO.NET Database Connectivity
All database operations utilize foundational ADO.NET classes from `Microsoft.Data.SqlClient`:
- **`SqlConnection`**: Manages explicit, pooled connection life-cycles to SQL Server.
- **`SqlCommand`**: Executes parameterized DDL, DML, and multi-result batch queries.
- **`SqlDataReader`**: Implements high-performance, forward-only connected reading with `ExecuteReaderAsync()`.
- **`SqlDataAdapter` & `DataTable`**: Demonstrates disconnected architecture for in-memory data tables and CSV exporting.
- **`ExecuteNonQueryAsync()`**: Performs `INSERT`, `UPDATE`, and `DELETE` operations returning affected rows.
- **`ExecuteScalarAsync()`**: Computes aggregate metrics and captures auto-generated `SCOPE_IDENTITY()`.

### 2. Executive Admin Dashboard (`/Dashboard`)
- **Total Products**: Live count of all active SKUs in the inventory.
- **Total Stock Units**: Aggregate physical units across all warehouse items.
- **Low Stock Warning Alert**: Real-time counter of items where $\text{Quantity} \le 10$, with dynamic warning banners.
- **Total Inventory Value**: Dynamically calculated as $\sum (\text{Quantity} \times \text{Price})$, formatted in Indian Rupees (₹).
- **Recent Product Activities**: Tabular list of the latest product additions.
- **Category Breakdown**: Percentage distribution of products across categories with visual progress meters.
- **Single-Query Multi-Result Batching**: The dashboard executes multiple queries in a single database roundtrip using `reader.NextResultAsync()`.

### 3. Complete Product Management (CRUD)
- **Create**: Add new products with real-time client-side valuation preview (`Quantity * Price`) and server-side validation.
- **Read (Catalog)**: Professional product table with:
  - Search by product name, supplier, or description.
  - Dropdown filter by product category.
  - Dropdown filter by stock status (*In Stock*, *Low Stock*, *Out of Stock*).
  - Calculated total stock value per item.
- **Update**: Edit existing product details with pre-populated values and concurrency safety.
- **Delete**: Confirmation page with product summary ensuring accidental deletions are prevented.
- **Export Data**: Disconnected `SqlDataAdapter` + `DataTable` export to CSV.

### 4. Robust Security & Validation
- **SQL Injection Prevention**: 100% of queries use typed `SqlParameter` objects (`@ProductName`, `@Price`, `@Quantity`, etc.).
- **Dual-Layer Validation**:
  - Model Data Annotations: `[Required]`, `[StringLength]`, `[Range]`, `[DataType]`.
  - Client-Side Validation: jQuery Validate + Unobtrusive.
- **Cross-Site Request Forgery (CSRF)**: All forms enforce `@Html.AntiForgeryToken()`.

---

## 🗄️ Database Architecture (`InventoryDB`)

### Database Table: `Products`
```sql
CREATE DATABASE InventoryDB;
GO
USE InventoryDB;
GO

CREATE TABLE Products (
    ProductID INT IDENTITY(1,1) PRIMARY KEY,
    ProductName NVARCHAR(100) NOT NULL,
    Category NVARCHAR(50) NOT NULL,
    Description NVARCHAR(500) NULL,
    Quantity INT NOT NULL DEFAULT 0,
    Price DECIMAL(18,2) NOT NULL DEFAULT 0.00,
    SupplierName NVARCHAR(100) NOT NULL,
    CreatedDate DATETIME2 NOT NULL DEFAULT GETDATE(),
    CONSTRAINT CK_Products_Quantity CHECK (Quantity >= 0),
    CONSTRAINT CK_Products_Price CHECK (Price > 0)
);
GO

CREATE NONCLUSTERED INDEX IX_Products_Category ON Products (Category);
CREATE NONCLUSTERED INDEX IX_Products_Quantity ON Products (Quantity);
GO
```

> **Note:** The application includes an automatic database initializer in `Data/DatabaseHelper.cs`. On the first run, it automatically creates `InventoryDB` and the `Products` table on SQL Server / LocalDB and seeds 10 sample products if empty!

---

## 📂 Project Directory Structure

```text
Experiment8/
│
├── Controllers/
│   ├── DashboardController.cs    # Executive metrics & summary controller
│   ├── ProductController.cs      # Complete CRUD operations & CSV export
│   └── HomeController.cs         # Default routing and error handler
│
├── Data/
│   ├── DatabaseHelper.cs         # ADO.NET connection provider & auto-initializer
│   ├── IProductRepository.cs     # Repository contract for inventory
│   └── ProductRepository.cs      # Pure ADO.NET implementation (SqlCommand, SqlDataReader, etc.)
│
├── Models/
│   ├── Product.cs                # Product entity with Data Annotations & helpers
│   ├── DashboardViewModel.cs     # Dashboard KPI aggregation model
│   └── ProductFilterViewModel.cs # Search, category, and status filter model
│
├── Views/
│   ├── Dashboard/
│   │   └── Index.cshtml          # Executive admin dashboard view
│   ├── Product/
│   │   ├── Index.cshtml          # Product catalog table with filters
│   │   ├── Create.cshtml         # Add product form with live valuation
│   │   ├── Edit.cshtml           # Edit product form
│   │   ├── Details.cshtml        # Full product specification card
│   │   └── Delete.cshtml         # Delete confirmation card
│   └── Shared/
│       ├── _Layout.cshtml        # Responsive sidebar admin layout
│       ├── _AlertMessages.cshtml # Dismissible TempData notifications
│       └── _ValidationScriptsPartial.cshtml
│
├── Database/
│   └── InventoryDB.sql           # Standalone T-SQL schema & sample data script
│
├── wwwroot/
│   ├── css/site.css              # Custom modern UI styling
│   └── js/site.js               # Sidebar toggle & active menu handling
│
├── appsettings.json              # SQL Server connection string
└── Program.cs                    # Dependency injection & pipeline configuration
```

---

## 🚀 How to Run the Project

### Prerequisites
- [.NET 10.0 SDK](https://dotnet.microsoft.com/)
- Microsoft SQL Server or SQL Server LocalDB (`(localdb)\MSSQLLocalDB`)

### Steps

1. **Navigate to the Experiment8 directory:**
   ```powershell
   cd D:\Net-Technology\Experiment8
   ```

2. **Verify connection string in `appsettings.json`:**
   ```json
   "ConnectionStrings": {
     "InventoryDB": "Server=(localdb)\\MSSQLLocalDB;Database=InventoryDB;Trusted_Connection=True;MultipleActiveResultSets=true;TrustServerCertificate=True;"
   }
   ```
   *(If using standard SQL Server Express, change to `Server=.\\SQLEXPRESS;Database=InventoryDB;Trusted_Connection=True;TrustServerCertificate=True;`)*

3. **Build the project:**
   ```powershell
   dotnet build
   ```

4. **Run the project:**
   ```powershell
   dotnet run
   ```

5. **Open your browser and navigate to:**
   ```text
   http://localhost:5000  or  https://localhost:5001
   ```

The application will automatically initialize the `InventoryDB` database and pre-populate sample products!

