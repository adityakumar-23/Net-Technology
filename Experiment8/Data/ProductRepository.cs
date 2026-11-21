using System.Data;
using Experiment8.Models;
using Microsoft.Data.SqlClient;

namespace Experiment8.Data
{
    public class ProductRepository : IProductRepository
    {
        private readonly DatabaseHelper _dbHelper;
        private readonly ILogger<ProductRepository> _logger;

        public ProductRepository(DatabaseHelper dbHelper, ILogger<ProductRepository> logger)
        {
            _dbHelper = dbHelper;
            _logger = logger;
        }

        // 1. READ: Display all products with search & category filter using SqlCommand + SqlDataReader
        public async Task<List<Product>> GetAllProductsAsync(string? search = null, string? category = null, string? stockStatus = null)
        {
            var products = new List<Product>();

            try
            {
                using var conn = _dbHelper.CreateConnection();
                var sql = "SELECT ProductID, ProductName, Category, Description, Quantity, Price, SupplierName, CreatedDate FROM Products WHERE 1=1";

                using var cmd = new SqlCommand();
                cmd.Connection = conn;

                if (!string.IsNullOrWhiteSpace(search))
                {
                    sql += " AND (ProductName LIKE @Search OR SupplierName LIKE @Search OR Description LIKE @Search)";
                    cmd.Parameters.Add("@Search", SqlDbType.NVarChar, 100).Value = $"%{search.Trim()}%";
                }

                if (!string.IsNullOrWhiteSpace(category) && category != "All")
                {
                    sql += " AND Category = @Category";
                    cmd.Parameters.Add("@Category", SqlDbType.NVarChar, 50).Value = category.Trim();
                }

                if (!string.IsNullOrWhiteSpace(stockStatus))
                {
                    if (stockStatus.Equals("LowStock", StringComparison.OrdinalIgnoreCase))
                    {
                        sql += " AND Quantity > 0 AND Quantity <= 10";
                    }
                    else if (stockStatus.Equals("OutOfStock", StringComparison.OrdinalIgnoreCase))
                    {
                        sql += " AND Quantity = 0";
                    }
                    else if (stockStatus.Equals("InStock", StringComparison.OrdinalIgnoreCase))
                    {
                        sql += " AND Quantity > 10";
                    }
                }

                sql += " ORDER BY ProductID DESC;";
                cmd.CommandText = sql;

                await conn.OpenAsync();
                using var reader = await cmd.ExecuteReaderAsync();

                while (await reader.ReadAsync())
                {
                    products.Add(MapProductFromReader(reader));
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error executing GetAllProductsAsync in ADO.NET");
                throw;
            }

            return products;
        }

        // 2. READ BY ID: Fetch single product details using SqlCommand + SqlDataReader
        public async Task<Product?> GetProductByIdAsync(int id)
        {
            try
            {
                using var conn = _dbHelper.CreateConnection();
                const string sql = @"
                    SELECT ProductID, ProductName, Category, Description, Quantity, Price, SupplierName, CreatedDate 
                    FROM Products 
                    WHERE ProductID = @ProductID;";

                using var cmd = new SqlCommand(sql, conn);
                cmd.Parameters.Add("@ProductID", SqlDbType.Int).Value = id;

                await conn.OpenAsync();
                using var reader = await cmd.ExecuteReaderAsync();

                if (await reader.ReadAsync())
                {
                    return MapProductFromReader(reader);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error executing GetProductByIdAsync for ProductID {ProductID}", id);
                throw;
            }

            return null;
        }

        // 3. CREATE: Insert new product using SqlCommand + Parameterized Query + ExecuteScalarAsync()
        public async Task<int> AddProductAsync(Product product)
        {
            try
            {
                using var conn = _dbHelper.CreateConnection();
                const string sql = @"
                    INSERT INTO Products (ProductName, Category, Description, Quantity, Price, SupplierName, CreatedDate)
                    VALUES (@ProductName, @Category, @Description, @Quantity, @Price, @SupplierName, GETDATE());
                    SELECT CAST(SCOPE_IDENTITY() AS INT);";

                using var cmd = new SqlCommand(sql, conn);
                cmd.Parameters.Add("@ProductName", SqlDbType.NVarChar, 100).Value = product.ProductName.Trim();
                cmd.Parameters.Add("@Category", SqlDbType.NVarChar, 50).Value = product.Category.Trim();
                cmd.Parameters.Add("@Description", SqlDbType.NVarChar, 500).Value = (object?)product.Description?.Trim() ?? DBNull.Value;
                cmd.Parameters.Add("@Quantity", SqlDbType.Int).Value = product.Quantity;
                cmd.Parameters.Add("@Price", SqlDbType.Decimal).Value = product.Price;
                cmd.Parameters.Add("@SupplierName", SqlDbType.NVarChar, 100).Value = product.SupplierName.Trim();

                await conn.OpenAsync();
                var newIdObj = await cmd.ExecuteScalarAsync();
                int newId = Convert.ToInt32(newIdObj);
                product.ProductID = newId;
                return newId;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error executing AddProductAsync in ADO.NET");
                throw;
            }
        }

        // 4. UPDATE: Modify existing product using SqlCommand + ExecuteNonQueryAsync()
        public async Task<bool> UpdateProductAsync(Product product)
        {
            try
            {
                using var conn = _dbHelper.CreateConnection();
                const string sql = @"
                    UPDATE Products 
                    SET ProductName = @ProductName,
                        Category = @Category,
                        Description = @Description,
                        Quantity = @Quantity,
                        Price = @Price,
                        SupplierName = @SupplierName
                    WHERE ProductID = @ProductID;";

                using var cmd = new SqlCommand(sql, conn);
                cmd.Parameters.Add("@ProductID", SqlDbType.Int).Value = product.ProductID;
                cmd.Parameters.Add("@ProductName", SqlDbType.NVarChar, 100).Value = product.ProductName.Trim();
                cmd.Parameters.Add("@Category", SqlDbType.NVarChar, 50).Value = product.Category.Trim();
                cmd.Parameters.Add("@Description", SqlDbType.NVarChar, 500).Value = (object?)product.Description?.Trim() ?? DBNull.Value;
                cmd.Parameters.Add("@Quantity", SqlDbType.Int).Value = product.Quantity;
                cmd.Parameters.Add("@Price", SqlDbType.Decimal).Value = product.Price;
                cmd.Parameters.Add("@SupplierName", SqlDbType.NVarChar, 100).Value = product.SupplierName.Trim();

                await conn.OpenAsync();
                int rowsAffected = await cmd.ExecuteNonQueryAsync();
                return rowsAffected > 0;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error executing UpdateProductAsync for ProductID {ProductID}", product.ProductID);
                throw;
            }
        }

        // 5. DELETE: Remove product with confirmation using SqlCommand + ExecuteNonQueryAsync()
        public async Task<bool> DeleteProductAsync(int id)
        {
            try
            {
                using var conn = _dbHelper.CreateConnection();
                const string sql = "DELETE FROM Products WHERE ProductID = @ProductID;";

                using var cmd = new SqlCommand(sql, conn);
                cmd.Parameters.Add("@ProductID", SqlDbType.Int).Value = id;

                await conn.OpenAsync();
                int rowsAffected = await cmd.ExecuteNonQueryAsync();
                return rowsAffected > 0;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error executing DeleteProductAsync for ProductID {ProductID}", id);
                throw;
            }
        }

        // 6. DASHBOARD: Aggregate metrics + recent activities in ADO.NET
        public async Task<DashboardViewModel> GetDashboardMetricsAsync()
        {
            var model = new DashboardViewModel();

            try
            {
                using var conn = _dbHelper.CreateConnection();
                const string sql = @"
                    -- Query 1: Aggregate Summary Metrics
                    SELECT 
                        COUNT(*) AS TotalProducts,
                        ISNULL(SUM(Quantity), 0) AS TotalStock,
                        ISNULL(SUM(CASE WHEN Quantity <= 10 THEN 1 ELSE 0 END), 0) AS LowStockCount,
                        ISNULL(SUM(CAST(Quantity AS DECIMAL(18,2)) * Price), 0) AS TotalInventoryValue
                    FROM Products;

                    -- Query 2: Recent 5 Products
                    SELECT TOP 5 ProductID, ProductName, Category, Description, Quantity, Price, SupplierName, CreatedDate
                    FROM Products
                    ORDER BY CreatedDate DESC;

                    -- Query 3: Low Stock Alerts List
                    SELECT ProductID, ProductName, Category, Description, Quantity, Price, SupplierName, CreatedDate
                    FROM Products
                    WHERE Quantity <= 10
                    ORDER BY Quantity ASC;

                    -- Query 4: Category Distribution
                    SELECT Category, COUNT(*) AS ProductCount
                    FROM Products
                    GROUP BY Category
                    ORDER BY ProductCount DESC;";

                using var cmd = new SqlCommand(sql, conn);
                await conn.OpenAsync();
                using var reader = await cmd.ExecuteReaderAsync();

                // 1. Summary
                if (await reader.ReadAsync())
                {
                    model.TotalProducts = reader.GetInt32(reader.GetOrdinal("TotalProducts"));
                    model.TotalStock = reader.GetInt32(reader.GetOrdinal("TotalStock"));
                    model.LowStockProductsCount = reader.GetInt32(reader.GetOrdinal("LowStockCount"));
                    model.TotalInventoryValue = reader.GetDecimal(reader.GetOrdinal("TotalInventoryValue"));
                }

                // 2. Recent Products
                if (await reader.NextResultAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        model.RecentProducts.Add(MapProductFromReader(reader));
                    }
                }

                // 3. Low Stock Products
                if (await reader.NextResultAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        model.LowStockProducts.Add(MapProductFromReader(reader));
                    }
                }

                // 4. Category Breakdown
                if (await reader.NextResultAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        string cat = reader.GetString(reader.GetOrdinal("Category"));
                        int count = reader.GetInt32(reader.GetOrdinal("ProductCount"));
                        model.CategoryCounts[cat] = count;
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving Dashboard metrics via ADO.NET");
                throw;
            }

            return model;
        }

        // 7. CATEGORIES: Distinct category list for dropdown filter
        public async Task<List<string>> GetCategoriesAsync()
        {
            var categories = new List<string>();

            try
            {
                using var conn = _dbHelper.CreateConnection();
                const string sql = "SELECT DISTINCT Category FROM Products WHERE Category IS NOT NULL AND LTRIM(RTRM(Category)) <> '' ORDER BY Category;";

                using var cmd = new SqlCommand(sql, conn);
                await conn.OpenAsync();
                using var reader = await cmd.ExecuteReaderAsync();

                while (await reader.ReadAsync())
                {
                    categories.Add(reader.GetString(0));
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving distinct categories in ADO.NET");
            }

            return categories;
        }

        // 8. DISCONNECTED ARCHITECTURE: Demonstrating SqlDataAdapter and DataTable
        public DataTable GetProductsDataTable(string? search = null, string? category = null)
        {
            var dt = new DataTable("Products");

            try
            {
                using var conn = _dbHelper.CreateConnection();
                var sql = "SELECT ProductID, ProductName, Category, Description, Quantity, Price, SupplierName, CreatedDate FROM Products WHERE 1=1";

                using var cmd = new SqlCommand();
                cmd.Connection = conn;

                if (!string.IsNullOrWhiteSpace(search))
                {
                    sql += " AND (ProductName LIKE @Search OR SupplierName LIKE @Search)";
                    cmd.Parameters.Add("@Search", SqlDbType.NVarChar, 100).Value = $"%{search.Trim()}%";
                }

                if (!string.IsNullOrWhiteSpace(category) && category != "All")
                {
                    sql += " AND Category = @Category";
                    cmd.Parameters.Add("@Category", SqlDbType.NVarChar, 50).Value = category.Trim();
                }

                sql += " ORDER BY ProductID DESC;";
                cmd.CommandText = sql;

                // Explicit use of SqlDataAdapter to fill disconnected DataTable
                using var adapter = new SqlDataAdapter(cmd);
                adapter.Fill(dt);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error filling DataTable via SqlDataAdapter");
                throw;
            }

            return dt;
        }

        private static Product MapProductFromReader(SqlDataReader reader)
        {
            return new Product
            {
                ProductID = reader.GetInt32(reader.GetOrdinal("ProductID")),
                ProductName = reader.GetString(reader.GetOrdinal("ProductName")),
                Category = reader.GetString(reader.GetOrdinal("Category")),
                Description = reader.IsDBNull(reader.GetOrdinal("Description")) ? null : reader.GetString(reader.GetOrdinal("Description")),
                Quantity = reader.GetInt32(reader.GetOrdinal("Quantity")),
                Price = reader.GetDecimal(reader.GetOrdinal("Price")),
                SupplierName = reader.GetString(reader.GetOrdinal("SupplierName")),
                CreatedDate = reader.GetDateTime(reader.GetOrdinal("CreatedDate"))
            };
        }
    }
}

