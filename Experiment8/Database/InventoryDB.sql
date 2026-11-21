-- ============================================================================
-- EXPERIMENT 8: INVENTORY MANAGEMENT SYSTEM DATABASE SCRIPT
-- Database: InventoryDB
-- Technology: Microsoft SQL Server, ADO.NET, ASP.NET Core MVC
-- ============================================================================

-- 1. Create Database if not exists
USE master;
GO

IF NOT EXISTS (SELECT name FROM sys.databases WHERE name = N'InventoryDB')
BEGIN
    CREATE DATABASE InventoryDB;
    PRINT 'Database InventoryDB created successfully.';
END
ELSE
BEGIN
    PRINT 'Database InventoryDB already exists.';
END
GO

USE InventoryDB;
GO

-- 2. Create Products Table
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = N'Products')
BEGIN
    CREATE TABLE Products (
        ProductID INT IDENTITY(1,1) PRIMARY KEY,
        ProductName NVARCHAR(100) NOT NULL,
        Category NVARCHAR(50) NOT NULL,
        Description NVARCHAR(500) NULL,
        Quantity INT NOT NULL CONSTRAINT DF_Products_Quantity DEFAULT 0,
        Price DECIMAL(18,2) NOT NULL CONSTRAINT DF_Products_Price DEFAULT 0.00,
        SupplierName NVARCHAR(100) NOT NULL,
        CreatedDate DATETIME2 NOT NULL CONSTRAINT DF_Products_CreatedDate DEFAULT GETDATE(),
        CONSTRAINT CK_Products_Quantity CHECK (Quantity >= 0),
        CONSTRAINT CK_Products_Price CHECK (Price > 0)
    );
    PRINT 'Table Products created successfully with constraints.';
END
ELSE
BEGIN
    PRINT 'Table Products already exists.';
END
GO

-- 3. Create Indexes for High Performance Queries (Search & Category filters)
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name = N'IX_Products_Category' AND object_id = OBJECT_ID(N'Products'))
BEGIN
    CREATE NONCLUSTERED INDEX IX_Products_Category ON Products (Category) INCLUDE (ProductName, Quantity, Price);
END
GO

IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name = N'IX_Products_Quantity' AND object_id = OBJECT_ID(N'Products'))
BEGIN
    CREATE NONCLUSTERED INDEX IX_Products_Quantity ON Products (Quantity) INCLUDE (ProductID, ProductName, Price);
END
GO

-- 4. Seed Initial Sample Data
IF NOT EXISTS (SELECT 1 FROM Products)
BEGIN
    INSERT INTO Products (ProductName, Category, Description, Quantity, Price, SupplierName, CreatedDate) VALUES
    (N'Dell Latitude 5440 Laptop', N'Electronics', N'14-inch FHD Core i7 16GB RAM 512GB SSD Corporate Workstation', 24, 78500.00, N'Dell Direct India', DATEADD(day, -10, GETDATE())),
    (N'HP LaserJet Pro MFP 4104dw', N'Office Equipment', N'Wireless High-speed Duplex Monochrome Laser Network Printer', 8, 38900.00, N'HP Enterprise Solutions', DATEADD(day, -8, GETDATE())),
    (N'Logitech MX Master 3S Wireless Mouse', N'Accessories', N'Ergonomic Performance Bluetooth Laser Mouse with Quiet Clicks', 45, 8495.00, N'LogiTech Distribution India', DATEADD(day, -7, GETDATE())),
    (N'Samsung 27-inch 4K UHD Monitor', N'Electronics', N'IPS Panel 3840x2160 HDR10 with Borderless Stand and HDMI/DP', 5, 26999.00, N'Samsung Electronics India', DATEADD(day, -6, GETDATE())),
    (N'Keychron K2 Mechanical Keyboard', N'Accessories', N'Wireless Bluetooth Mechanical Keyboard with Gateron G Pro Brown Switches', 18, 7999.00, N'Keychron India Retail', DATEADD(day, -5, GETDATE())),
    (N'Cisco Catalyst 1000 Series Switch', N'Networking', N'24-Port Gigabit Ethernet Managed Enterprise Switch with 4x 1G SFP', 4, 45200.00, N'Cisco Systems India', DATEADD(day, -4, GETDATE())),
    (N'APC Smart-UPS 1500VA LCD', N'Office Equipment', N'Line-Interactive 230V Rack/Tower UPS with SmartConnect Cloud Monitoring', 6, 32500.00, N'Schneider Electric India', DATEADD(day, -3, GETDATE())),
    (N'Kingston FURY Beast 32GB DDR5 RAM', N'Computer Hardware', N'DDR5 5600MT/s CL36 Desktop Gaming and Workstation Memory Kit', 0, 11499.00, N'Kingston Technology', DATEADD(day, -2, GETDATE())),
    (N'Seagate IronWolf Pro 8TB NAS HDD', N'Computer Hardware', N'7200 RPM CMR 3.5-inch Internal Hard Drive for Multi-bay RAID/NAS', 12, 21990.00, N'Seagate Technology India', DATEADD(day, -1, GETDATE())),
    (N'Cat6 Gigabit Ethernet Cable (305m)', N'Networking', N'High Speed Solid Copper 24AWG UTP Bulk Ethernet Cable Pull Box', 15, 6800.00, N'D-Link India Ltd', GETDATE());

    PRINT 'Sample Products inserted successfully.';
END
GO

-- 5. Verification Queries
SELECT COUNT(*) AS [Total Products], 
       SUM(Quantity) AS [Total Stock], 
       SUM(CASE WHEN Quantity <= 10 THEN 1 ELSE 0 END) AS [Low Stock Products],
       SUM(CAST(Quantity AS DECIMAL(18,2)) * Price) AS [Total Inventory Value (Rs)]
FROM Products;

SELECT TOP 10 ProductID, ProductName, Category, Quantity, Price, SupplierName, CreatedDate 
FROM Products 
ORDER BY ProductID ASC;
GO

