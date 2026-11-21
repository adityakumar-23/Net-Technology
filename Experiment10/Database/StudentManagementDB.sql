-- ========================================================
-- Experiment 10: Student Management REST API Database Script
-- Technology: Microsoft SQL Server, ADO.NET, ASP.NET Core Web API (.NET 8)
-- Database: StudentManagementDB
-- Author: Aditya Kumar
-- ========================================================

-- Step 1: Create Database if it does not exist
IF NOT EXISTS (SELECT name FROM sys.databases WHERE name = 'StudentManagementDB')
BEGIN
    CREATE DATABASE StudentManagementDB;
    PRINT 'Database StudentManagementDB created successfully.';
END
GO

USE StudentManagementDB;
GO

-- Step 2: Create Students Table
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Students')
BEGIN
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
    PRINT 'Students table created successfully.';
END
GO

-- Step 3: Insert Sample Seed Records
IF NOT EXISTS (SELECT 1 FROM Students)
BEGIN
    INSERT INTO Students (FullName, Email, PhoneNumber, Department, Semester, CGPA, Address, CreatedAt)
    VALUES 
    ('Aditya Kumar', 'aditya.kumar@example.com', '+91-9876543210', 'Computer Science & Engineering', 6, 9.15, 'New Delhi, India', GETDATE()),
    ('Priya Sharma', 'priya.sharma@example.com', '+91-9812345678', 'Information Technology', 6, 8.85, 'Mumbai, Maharashtra', GETDATE()),
    ('Rahul Verma', 'rahul.verma@example.com', '+91-9988776655', 'Electronics & Communication', 4, 8.40, 'Bangalore, Karnataka', GETDATE()),
    ('Ananya Gupta', 'ananya.gupta@example.com', '+91-9765432109', 'Computer Science & Engineering', 8, 9.45, 'Pune, Maharashtra', GETDATE());
    
    PRINT 'Initial seed records inserted successfully.';
END
GO

-- Step 4: Verification Query
SELECT 
    StudentId,
    FullName,
    Email,
    PhoneNumber,
    Department,
    Semester,
    CGPA,
    Address,
    CreatedAt
FROM Students;
GO
