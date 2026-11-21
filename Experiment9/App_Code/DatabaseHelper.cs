using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace Experiment9.Data
{
    public static class DatabaseHelper
    {
        private static readonly string ConnectionString = ConfigurationManager.ConnectionStrings["PlacementDB"] != null
            ? ConfigurationManager.ConnectionStrings["PlacementDB"].ConnectionString
            : "Server=(localdb)\\MSSQLLocalDB;Database=PlacementDB;Integrated Security=True;MultipleActiveResultSets=true;TrustServerCertificate=True;";

        public static SqlConnection GetConnection()
        {
            return new SqlConnection(ConnectionString);
        }

        public static void InitializeDatabase()
        {
            try
            {
                var builder = new SqlConnectionStringBuilder(ConnectionString);
                string targetDb = string.IsNullOrWhiteSpace(builder.InitialCatalog) ? "PlacementDB" : builder.InitialCatalog;

                // 1. Check/Create Database on master
                builder.InitialCatalog = "master";
                using (var masterConn = new SqlConnection(builder.ConnectionString))
                {
                    masterConn.Open();
                    string checkDbSql = string.Format(@"
                        IF NOT EXISTS (SELECT name FROM sys.databases WHERE name = N'{0}')
                        BEGIN
                            CREATE DATABASE [{0}];
                        END", targetDb);

                    using (var cmd = new SqlCommand(checkDbSql, masterConn))
                    {
                        cmd.ExecuteNonQuery();
                    }
                }

                // 2. Connect to PlacementDB and ensure tables
                using (var dbConn = GetConnection())
                {
                    dbConn.Open();
                    string createTablesSql = @"
                        IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = N'Students')
                        BEGIN
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
                        END

                        IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = N'Companies')
                        BEGIN
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
                        END

                        IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = N'Placements')
                        BEGIN
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
                        END";

                    using (var cmd = new SqlCommand(createTablesSql, dbConn))
                    {
                        cmd.ExecuteNonQuery();
                    }

                    // Check if students table is empty, seed initial data
                    using (var checkCmd = new SqlCommand("SELECT COUNT(*) FROM Students;", dbConn))
                    {
                        int studentCount = Convert.ToInt32(checkCmd.ExecuteScalar());
                        if (studentCount == 0)
                        {
                            SeedInitialData(dbConn);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Database initialization error: " + ex.Message);
            }
        }

        private static void SeedInitialData(SqlConnection conn)
        {
            string seedSql = @"
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

                INSERT INTO Companies (CompanyName, Location, JobRole, PackageLPA, EligibilityCriteria, VisitDate) VALUES
                (N'Microsoft India', N'Hyderabad', N'Software Development Engineer', 24.00, N'CGPA >= 8.5, Algorithms & System Design', '2026-10-05'),
                (N'Amazon Web Services (AWS)', N'Bengaluru', N'Cloud Support Associate', 16.50, N'CGPA >= 7.5, Linux, Networking & Cloud', '2026-11-02'),
                (N'Infosys Limited', N'Bengaluru', N'Specialist Programmer', 9.50, N'CGPA >= 7.0, Strong Coding Skills', '2026-09-22'),
                (N'Deloitte USI', N'Gurugram', N'Technology Analyst', 8.25, N'CGPA >= 7.0, Analytical & Consulting Skills', '2026-11-12'),
                (N'Tata Consultancy Services (TCS)', N'Mumbai', N'Software Engineer', 7.50, N'CGPA >= 6.5, No Active Backlogs', '2026-09-15'),
                (N'LTIMindtree', N'Pune', N'Cloud Engineer', 6.50, N'CGPA >= 6.0, Networking & Cloud Basics', '2026-10-18');

                INSERT INTO Placements (StudentID, CompanyID, OfferDate, PackageOffered, Designation, Status) VALUES
                (1, 1, '2026-10-06', 24.00, N'Software Development Engineer', N'Accepted'),
                (2, 2, '2026-11-03', 16.50, N'Cloud Support Associate', N'Accepted'),
                (3, 3, '2026-09-23', 9.50, N'Specialist Programmer', N'Accepted'),
                (4, 4, '2026-11-13', 8.25, N'Technology Analyst', N'Accepted'),
                (6, 5, '2026-09-16', 7.50, N'Software Engineer', N'Accepted'),
                (8, 6, '2026-10-19', 6.50, N'Cloud Engineer', N'Accepted'),
                (10, 3, '2026-09-23', 9.50, N'Specialist Programmer', N'Accepted');";

            using (var seedCmd = new SqlCommand(seedSql, conn))
            {
                seedCmd.ExecuteNonQuery();
            }
        }
    }
}

