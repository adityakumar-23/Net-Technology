using Microsoft.Data.SqlClient;

namespace Experiment8.Data
{
    public class DatabaseHelper
    {
        private readonly string _connectionString;
        private readonly ILogger<DatabaseHelper> _logger;

        public DatabaseHelper(IConfiguration configuration, ILogger<DatabaseHelper> logger)
        {
            _connectionString = configuration.GetConnectionString("InventoryDB")
                ?? "Server=(localdb)\\MSSQLLocalDB;Database=InventoryDB;Trusted_Connection=True;MultipleActiveResultSets=true;TrustServerCertificate=True;";
            _logger = logger;
        }

        public string GetConnectionString() => _connectionString;

        public SqlConnection CreateConnection()
        {
            return new SqlConnection(_connectionString);
        }

        public async Task InitializeDatabaseAsync()
        {
            try
            {
                var builder = new SqlConnectionStringBuilder(_connectionString);
                string targetDatabase = builder.InitialCatalog;
                if (string.IsNullOrWhiteSpace(targetDatabase))
                {
                    targetDatabase = "InventoryDB";
                }

                // Connect to master database first to check/create the target database
                builder.InitialCatalog = "master";
                using (var masterConn = new SqlConnection(builder.ConnectionString))
                {
                    await masterConn.OpenAsync();
                    string checkDbSql = $@"
                        IF NOT EXISTS (SELECT name FROM sys.databases WHERE name = N'{targetDatabase}')
                        BEGIN
                            CREATE DATABASE [{targetDatabase}];
                        END";

                    using (var cmd = new SqlCommand(checkDbSql, masterConn))
                    {
                        await cmd.ExecuteNonQueryAsync();
                        _logger.LogInformation("Database {Database} verified or created successfully.", targetDatabase);
                    }
                }

                // Now connect to target database to create table & seed data
                using (var dbConn = CreateConnection())
                {
                    await dbConn.OpenAsync();

                    string createTableSql = @"
                        IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = N'Products')
                        BEGIN
                            CREATE TABLE Products (
                                ProductID INT IDENTITY(1,1) PRIMARY KEY,
                                ProductName NVARCHAR(100) NOT NULL,
                                Category NVARCHAR(50) NOT NULL,
                                Description NVARCHAR(500) NULL,
                                Quantity INT NOT NULL DEFAULT 0,
                                Price DECIMAL(18,2) NOT NULL DEFAULT 0.00,
                                SupplierName NVARCHAR(100) NOT NULL,
                                CreatedDate DATETIME2 NOT NULL DEFAULT GETDATE()
                            );
                        END";

                    using (var cmd = new SqlCommand(createTableSql, dbConn))
                    {
                        await cmd.ExecuteNonQueryAsync();
                    }

                    // Check if table is empty, then seed sample records
                    string countSql = "SELECT COUNT(*) FROM Products;";
                    int count = 0;
                    using (var countCmd = new SqlCommand(countSql, dbConn))
                    {
                        count = (int)(await countCmd.ExecuteScalarAsync() ?? 0);
                    }

                    if (count == 0)
                    {
                        string seedSql = @"
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
                            (N'Cat6 Gigabit Ethernet Cable (305m)', N'Networking', N'High Speed Solid Copper 24AWG UTP Bulk Ethernet Cable Pull Box', 15, 6800.00, N'D-Link India Ltd', GETDATE());";

                        using (var seedCmd = new SqlCommand(seedSql, dbConn))
                        {
                            await seedCmd.ExecuteNonQueryAsync();
                            _logger.LogInformation("Products table seeded with initial sample inventory items.");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred during database initialization.");
            }
        }
    }
}

