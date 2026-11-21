-- ============================================================================
-- EXPERIMENT 9: PLACEMENT MANAGEMENT SYSTEM DATABASE SCRIPT
-- Database: PlacementDB
-- Technology: ASP.NET Web Forms, ADO.NET, Microsoft SQL Server
-- ============================================================================

USE master;
GO

IF NOT EXISTS (SELECT name FROM sys.databases WHERE name = N'PlacementDB')
BEGIN
    CREATE DATABASE PlacementDB;
    PRINT 'Database PlacementDB created successfully.';
END
ELSE
BEGIN
    PRINT 'Database PlacementDB already exists.';
END
GO

USE PlacementDB;
GO

-- 1. Create Students Table
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = N'Students')
BEGIN
    CREATE TABLE Students (
        StudentID INT IDENTITY(1,1) PRIMARY KEY,
        StudentName NVARCHAR(100) NOT NULL,
        Email NVARCHAR(100) NOT NULL UNIQUE,
        Department NVARCHAR(50) NOT NULL,
        Semester INT NOT NULL CONSTRAINT CK_Students_Semester CHECK (Semester BETWEEN 1 AND 8),
        CGPA DECIMAL(3,2) NOT NULL CONSTRAINT CK_Students_CGPA CHECK (CGPA BETWEEN 0.00 AND 10.00),
        Skills NVARCHAR(250) NULL,
        PlacementStatus NVARCHAR(20) NOT NULL CONSTRAINT DF_Students_Status DEFAULT 'Not Placed',
        CreatedDate DATETIME2 NOT NULL CONSTRAINT DF_Students_Created DEFAULT GETDATE(),
        CONSTRAINT CK_Students_Status CHECK (PlacementStatus IN ('Placed', 'Not Placed'))
    );
    PRINT 'Table Students created successfully.';
END
GO

-- 2. Create Companies Table
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = N'Companies')
BEGIN
    CREATE TABLE Companies (
        CompanyID INT IDENTITY(1,1) PRIMARY KEY,
        CompanyName NVARCHAR(100) NOT NULL,
        Location NVARCHAR(100) NOT NULL,
        JobRole NVARCHAR(100) NOT NULL,
        PackageLPA DECIMAL(5,2) NOT NULL CONSTRAINT CK_Companies_Package CHECK (PackageLPA > 0),
        EligibilityCriteria NVARCHAR(250) NOT NULL,
        VisitDate DATE NOT NULL,
        CreatedDate DATETIME2 NOT NULL CONSTRAINT DF_Companies_Created DEFAULT GETDATE()
    );
    PRINT 'Table Companies created successfully.';
END
GO

-- 3. Create Placements Table
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = N'Placements')
BEGIN
    CREATE TABLE Placements (
        PlacementID INT IDENTITY(1,1) PRIMARY KEY,
        StudentID INT NOT NULL,
        CompanyID INT NOT NULL,
        OfferDate DATE NOT NULL CONSTRAINT DF_Placements_OfferDate DEFAULT GETDATE(),
        PackageOffered DECIMAL(5,2) NOT NULL CONSTRAINT CK_Placements_Package CHECK (PackageOffered > 0),
        Designation NVARCHAR(100) NOT NULL,
        Status NVARCHAR(20) NOT NULL CONSTRAINT DF_Placements_Status DEFAULT 'Accepted',
        CONSTRAINT FK_Placements_Student FOREIGN KEY (StudentID) REFERENCES Students(StudentID) ON DELETE CASCADE,
        CONSTRAINT FK_Placements_Company FOREIGN KEY (CompanyID) REFERENCES Companies(CompanyID) ON DELETE CASCADE
    );
    PRINT 'Table Placements created successfully.';
END
GO

-- 4. Create Indexes
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name = N'IX_Students_Dept_Status' AND object_id = OBJECT_ID(N'Students'))
BEGIN
    CREATE NONCLUSTERED INDEX IX_Students_Dept_Status ON Students (Department, PlacementStatus);
END
GO

-- 5. Seed Initial Data
IF NOT EXISTS (SELECT 1 FROM Students)
BEGIN
    INSERT INTO Students (StudentName, Email, Department, Semester, CGPA, Skills, PlacementStatus) VALUES
    (N'Aditya Kumar', N'aditya.kumar@marwadi.edu', N'Computer Engineering', 5, 8.85, N'C#, .NET, SQL, React, Python', N'Placed'),
    (N'Ananya Sharma', N'ananya.sharma@marwadi.edu', N'Computer Engineering', 7, 9.20, N'Java, Spring Boot, Microservices, AWS', N'Placed'),
    (N'Rahul Patel', N'rahul.patel@marwadi.edu', N'Information Technology', 7, 7.80, N'Python, Django, Machine Learning, SQL', N'Placed'),
    (N'Sneha Desai', N'sneha.desai@marwadi.edu', N'AI & Data Science', 7, 8.95, N'TensorFlow, PyTorch, NLP, Python', N'Placed'),
    (N'Harsh Mehta', N'harsh.mehta@marwadi.edu', N'Computer Engineering', 7, 6.90, N'JavaScript, Node.js, Express, MongoDB', N'Not Placed'),
    (N'Priya Nair', N'priya.nair@marwadi.edu', N'Electronics & Communication', 7, 8.10, N'Embedded C, IoT, Verilog, Python', N'Placed'),
    (N'Devendra Joshi', N'devendra.joshi@marwadi.edu', N'Information Technology', 5, 7.45, N'HTML, CSS, Bootstrap, PHP, MySQL', N'Not Placed'),
    (N'Pooja Varma', N'pooja.varma@marwadi.edu', N'Computer Engineering', 7, 8.60, N'Full Stack Web, Cloud, Docker, Git', N'Placed'),
    (N'Rohit Shah', N'rohit.shah@marwadi.edu', N'Computer Engineering', 7, 7.15, N'C++, Data Structures, Algorithms, SQL', N'Not Placed'),
    (N'Neha Gupta', N'neha.gupta@marwadi.edu', N'AI & Data Science', 7, 9.40, N'Data Analytics, Tableau, PowerBI, Python', N'Placed');
END
GO

IF NOT EXISTS (SELECT 1 FROM Companies)
BEGIN
    INSERT INTO Companies (CompanyName, Location, JobRole, PackageLPA, EligibilityCriteria, VisitDate) VALUES
    (N'Microsoft India', N'Hyderabad', N'Software Development Engineer', 24.00, N'CGPA >= 8.5, Algorithms & System Design', '2026-10-05'),
    (N'Amazon Web Services (AWS)', N'Bengaluru', N'Cloud Support Associate', 16.50, N'CGPA >= 7.5, Linux, Networking & Cloud', '2026-11-02'),
    (N'Infosys Limited', N'Bengaluru', N'Specialist Programmer', 9.50, N'CGPA >= 7.0, Strong Coding Skills', '2026-09-22'),
    (N'Deloitte USI', N'Gurugram', N'Technology Analyst', 8.25, N'CGPA >= 7.0, Analytical & Consulting Skills', '2026-11-12'),
    (N'Tata Consultancy Services (TCS)', N'Mumbai', N'Software Engineer', 7.50, N'CGPA >= 6.5, No Active Backlogs', '2026-09-15'),
    (N'LTIMindtree', N'Pune', N'Cloud Engineer', 6.50, N'CGPA >= 6.0, Networking & Cloud Basics', '2026-10-18');
END
GO

IF NOT EXISTS (SELECT 1 FROM Placements)
BEGIN
    INSERT INTO Placements (StudentID, CompanyID, OfferDate, PackageOffered, Designation, Status) VALUES
    (1, 1, '2026-10-06', 24.00, N'Software Development Engineer', N'Accepted'),
    (2, 2, '2026-11-03', 16.50, N'Cloud Support Associate', N'Accepted'),
    (3, 3, '2026-09-23', 9.50, N'Specialist Programmer', N'Accepted'),
    (4, 4, '2026-11-13', 8.25, N'Technology Analyst', N'Accepted'),
    (6, 5, '2026-09-16', 7.50, N'Software Engineer', N'Accepted'),
    (8, 6, '2026-10-19', 6.50, N'Cloud Engineer', N'Accepted'),
    (10, 3, '2026-09-23', 9.50, N'Specialist Programmer', N'Accepted');
END
GO

-- 6. Verification Summary
SELECT COUNT(*) AS [Total Students],
       SUM(CASE WHEN PlacementStatus = 'Placed' THEN 1 ELSE 0 END) AS [Placed Students],
       SUM(CASE WHEN PlacementStatus = 'Not Placed' THEN 1 ELSE 0 END) AS [Unplaced Students]
FROM Students;

SELECT COUNT(*) AS [Total Companies] FROM Companies;
SELECT COUNT(*) AS [Total Offers], MAX(PackageOffered) AS [Highest Package (LPA)], AVG(PackageOffered) AS [Avg Package (LPA)] FROM Placements;
GO

