-- ========================================================
-- Experiment 11: Smart Campus Service Portal Database Script
-- Technology: Microsoft SQL Server, ADO.NET, ASP.NET MVC 5, Web API 2
-- Database: SmartCampusDB
-- Author: Aditya Kumar
-- ========================================================

-- Step 1: Create Database if not exists
IF NOT EXISTS (SELECT name FROM sys.databases WHERE name = 'SmartCampusDB')
BEGIN
    CREATE DATABASE SmartCampusDB;
    PRINT 'Database SmartCampusDB created successfully.';
END
GO

USE SmartCampusDB;
GO

-- Step 2: Create ServiceRequests Table
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'ServiceRequests')
BEGIN
    CREATE TABLE ServiceRequests (
        RequestId INT IDENTITY(1,1) PRIMARY KEY,
        StudentName NVARCHAR(100) NOT NULL,
        StudentEmail NVARCHAR(100) NOT NULL DEFAULT 'student@campus.edu',
        Department NVARCHAR(50) NOT NULL,
        ServiceCategory NVARCHAR(50) NOT NULL DEFAULT 'IT Support',
        ServiceType NVARCHAR(50) NULL,
        RequestTitle NVARCHAR(150) NOT NULL DEFAULT 'Service Request',
        Description NVARCHAR(MAX) NOT NULL,
        Priority NVARCHAR(20) NOT NULL DEFAULT 'Medium' CHECK (Priority IN ('Low', 'Medium', 'High')),
        Status NVARCHAR(20) NOT NULL DEFAULT 'Pending' CHECK (Status IN ('Pending', 'In Progress', 'Completed', 'Rejected')),
        RequestDate DATETIME NOT NULL DEFAULT GETDATE(),
        AdminRemarks NVARCHAR(250) NULL
    );
    PRINT 'ServiceRequests table created successfully.';
END
GO

-- Step 3: Insert Realistic Seed Records
IF NOT EXISTS (SELECT 1 FROM ServiceRequests)
BEGIN
    INSERT INTO ServiceRequests (StudentName, StudentEmail, Department, ServiceCategory, ServiceType, RequestTitle, Description, Priority, Status, RequestDate, AdminRemarks)
    VALUES
    ('Aditya Kumar', 'aditya.k@campus.edu', 'Computer Science & Engineering', 'IT Support', 'IT Support', 'Wi-Fi connectivity issue in CS Lab 3', 'Frequent disconnects and DNS resolution timeouts on machines 12 through 20.', 'High', 'In Progress', DATEADD(HOUR, -5, GETDATE()), 'Router restarted; inspecting switch port.'),
    ('Priya Sharma', 'priya.s@campus.edu', 'Information Technology', 'Classroom Maintenance', 'Classroom Maintenance', 'Projector not working in Room 204', 'HDMI input not detected and display lamp flickers constantly.', 'Medium', 'Pending', DATEADD(HOUR, -8, GETDATE()), NULL),
    ('Rahul Verma', 'rahul.v@campus.edu', 'Electronics & Communication', 'Hostel Maintenance', 'Hostel Maintenance', 'AC unit leaking water in Hostel Block B', 'Water dripping on study desk from split AC unit in Room 108.', 'High', 'Completed', DATEADD(DAY, -1, GETDATE()), 'Filter cleaned and condensate drainage pipe cleared.'),
    ('Ananya Gupta', 'ananya.g@campus.edu', 'Electrical Engineering', 'Electrical Maintenance', 'Electrical Maintenance', 'Defective tube lights in 2nd Floor Corridor', 'Two tube lights blinking continuously near the elevator lobby.', 'Low', 'Pending', DATEADD(DAY, -2, GETDATE()), NULL),
    ('Sneha Patil', 'sneha.p@campus.edu', 'Computer Science & Engineering', 'Library Services', 'Library Services', 'Digital Library RFID scanner malfunction', 'The self-checkout kiosk card reader fails to scan student ID cards.', 'Medium', 'Completed', DATEADD(DAY, -3, GETDATE()), 'Replaced USB RFID reader driver and tested.'),
    ('Rohan Mehta', 'rohan.m@campus.edu', 'Mechanical Engineering', 'Hostel Maintenance', 'Hostel Maintenance', 'Water cooler cooling coil issue', 'Water cooler in Block C 1st floor dispensing room temperature water.', 'Medium', 'Rejected', DATEADD(DAY, -4, GETDATE()), 'Equipment under external warranty; forwarded to Estate Office.');

    PRINT 'Initial seed records inserted successfully.';
END
GO

-- Step 4: Verification Query
SELECT 
    RequestId,
    StudentName,
    StudentEmail,
    Department,
    ServiceCategory,
    RequestTitle,
    Priority,
    Status,
    RequestDate,
    AdminRemarks
FROM ServiceRequests
ORDER BY RequestId DESC;
GO
