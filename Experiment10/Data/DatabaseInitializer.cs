using Microsoft.Data.SqlClient;

namespace StudentManagementAPI.Data
{
    public static class DatabaseInitializer
    {
        public static void Initialize(IConfiguration configuration, ILogger logger)
        {
            try
            {
                string connStr = configuration.GetConnectionString("DefaultConnection")
                    ?? "Server=(localdb)\\MSSQLLocalDB;Database=StudentManagementDB;Trusted_Connection=True;MultipleActiveResultSets=true;TrustServerCertificate=True";

                var builder = new SqlConnectionStringBuilder(connStr);
                string targetDb = string.IsNullOrEmpty(builder.InitialCatalog) ? "StudentManagementDB" : builder.InitialCatalog;

                // Step 1: Ensure Database exists on master
                builder.InitialCatalog = "master";
                using (var masterConn = new SqlConnection(builder.ConnectionString))
                {
                    masterConn.Open();
                    string checkDbSql = $@"
                        IF NOT EXISTS (SELECT 1 FROM sys.databases WHERE name = '{targetDb}')
                        BEGIN
                            CREATE DATABASE [{targetDb}];
                        END;";
                    using var cmd = new SqlCommand(checkDbSql, masterConn);
                    cmd.ExecuteNonQuery();
                }

                // Step 2: Ensure Table and Seed Data exist
                builder.InitialCatalog = targetDb;
                using (var dbConn = new SqlConnection(builder.ConnectionString))
                {
                    dbConn.Open();
                    string createTableSql = @"
                        IF NOT EXISTS (SELECT 1 FROM sys.tables WHERE name = 'Students')
                        BEGIN
                            CREATE TABLE Students (
                                StudentId INT IDENTITY(1,1) PRIMARY KEY,
                                FullName NVARCHAR(100) NOT NULL,
                                Email NVARCHAR(100) NOT NULL UNIQUE,
                                PhoneNumber NVARCHAR(20) NOT NULL,
                                Department NVARCHAR(50) NOT NULL,
                                Semester INT NOT NULL,
                                CGPA DECIMAL(4,2) NOT NULL,
                                Address NVARCHAR(250) NULL,
                                CreatedAt DATETIME NOT NULL DEFAULT GETDATE()
                            );

                            INSERT INTO Students (FullName, Email, PhoneNumber, Department, Semester, CGPA, Address, CreatedAt)
                            VALUES 
                            ('Aditya Kumar', 'aditya.kumar@example.com', '+91-9876543210', 'Computer Science & Engineering', 6, 9.15, 'New Delhi, India', GETDATE()),
                            ('Priya Sharma', 'priya.sharma@example.com', '+91-9812345678', 'Information Technology', 6, 8.85, 'Mumbai, Maharashtra', GETDATE()),
                            ('Rahul Verma', 'rahul.verma@example.com', '+91-9988776655', 'Electronics & Communication', 4, 8.40, 'Bangalore, Karnataka', GETDATE()),
                            ('Ananya Gupta', 'ananya.gupta@example.com', '+91-9765432109', 'Computer Science & Engineering', 8, 9.45, 'Pune, Maharashtra', GETDATE());
                        END;";

                    using var cmd = new SqlCommand(createTableSql, dbConn);
                    cmd.ExecuteNonQuery();
                }

                logger.LogInformation("Database [{Database}] and Students table verified successfully.", targetDb);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred during database initialization.");
            }
        }
    }
}
