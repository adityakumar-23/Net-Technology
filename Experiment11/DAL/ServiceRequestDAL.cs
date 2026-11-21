using SmartCampusPortal.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace SmartCampusPortal.DAL
{
    public class ServiceRequestDAL
    {
        private readonly string cs =
            ConfigurationManager.ConnectionStrings["CampusConnection"] != null
                ? ConfigurationManager.ConnectionStrings["CampusConnection"].ConnectionString
                : "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=SmartCampusDB;Integrated Security=True";

        // Automated Database & Table Initialization
        public static void EnsureDatabaseAndTable()
        {
            try
            {
                string masterCs = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=master;Integrated Security=True";

                // Step 1: Ensure database exists
                using (SqlConnection masterConn = new SqlConnection(masterCs))
                {
                    masterConn.Open();
                    string dbSql = @"
                        IF NOT EXISTS (SELECT 1 FROM sys.databases WHERE name = 'SmartCampusDB')
                        BEGIN
                            CREATE DATABASE SmartCampusDB;
                        END";
                    using (SqlCommand cmd = new SqlCommand(dbSql, masterConn))
                    {
                        cmd.ExecuteNonQuery();
                    }
                }

                // Step 2: Ensure table exists in SmartCampusDB
                string targetCs = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=SmartCampusDB;Integrated Security=True";
                using (SqlConnection dbConn = new SqlConnection(targetCs))
                {
                    dbConn.Open();
                    string tableSql = @"
                        IF NOT EXISTS (SELECT 1 FROM sys.tables WHERE name = 'ServiceRequests')
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
                                Priority NVARCHAR(20) NOT NULL DEFAULT 'Medium',
                                Status NVARCHAR(20) NOT NULL DEFAULT 'Pending',
                                RequestDate DATETIME NOT NULL DEFAULT GETDATE(),
                                AdminRemarks NVARCHAR(250) NULL
                            );

                            INSERT INTO ServiceRequests (StudentName, StudentEmail, Department, ServiceCategory, ServiceType, RequestTitle, Description, Priority, Status, RequestDate, AdminRemarks)
                            VALUES
                            ('Aditya Kumar', 'aditya.k@campus.edu', 'Computer Science & Engineering', 'IT Support', 'IT Support', 'Wi-Fi connectivity issue in CS Lab 3', 'Frequent disconnects and DNS resolution timeouts on machines 12 through 20.', 'High', 'In Progress', DATEADD(HOUR, -5, GETDATE()), 'Router restarted; inspecting switch port.'),
                            ('Priya Sharma', 'priya.s@campus.edu', 'Information Technology', 'Classroom Maintenance', 'Classroom Maintenance', 'Projector not working in Room 204', 'HDMI input not detected and display lamp flickers constantly.', 'Medium', 'Pending', DATEADD(HOUR, -8, GETDATE()), NULL),
                            ('Rahul Verma', 'rahul.v@campus.edu', 'Electronics & Communication', 'Hostel Maintenance', 'Hostel Maintenance', 'AC unit leaking water in Hostel Block B', 'Water dripping on study desk from split AC unit in Room 108.', 'High', 'Completed', DATEADD(DAY, -1, GETDATE()), 'Filter cleaned and condensate drainage pipe cleared.'),
                            ('Ananya Gupta', 'ananya.g@campus.edu', 'Electrical Engineering', 'Electrical Maintenance', 'Electrical Maintenance', 'Defective tube lights in 2nd Floor Corridor', 'Two tube lights blinking continuously near the elevator lobby.', 'Low', 'Pending', DATEADD(DAY, -2, GETDATE()), NULL),
                            ('Sneha Patil', 'sneha.p@campus.edu', 'Computer Science & Engineering', 'Library Services', 'Library Services', 'Digital Library RFID scanner malfunction', 'The self-checkout kiosk card reader fails to scan student ID cards.', 'Medium', 'Completed', DATEADD(DAY, -3, GETDATE()), 'Replaced USB RFID reader driver and tested.'),
                            ('Rohan Mehta', 'rohan.m@campus.edu', 'Mechanical Engineering', 'Hostel Maintenance', 'Hostel Maintenance', 'Water cooler cooling coil issue', 'Water cooler in Block C 1st floor dispensing room temperature water.', 'Medium', 'Rejected', DATEADD(DAY, -4, GETDATE()), 'Equipment under external warranty; forwarded to Estate Office.');
                        END";
                    using (SqlCommand cmd = new SqlCommand(tableSql, dbConn))
                    {
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception)
            {
                // Silently allow continuation if database was already initialized
            }
        }

        // Get all requests with optional search, category, and status filtering
        public List<ServiceRequest> GetAll(string search = null, string category = null, string status = null)
        {
            List<ServiceRequest> list = new List<ServiceRequest>();

            using (SqlConnection con = new SqlConnection(cs))
            {
                string query = "SELECT * FROM ServiceRequests WHERE 1=1";

                if (!string.IsNullOrEmpty(search))
                {
                    query += " AND (StudentName LIKE @Search OR RequestTitle LIKE @Search OR Description LIKE @Search OR Department LIKE @Search)";
                }

                if (!string.IsNullOrEmpty(category) && category != "All")
                {
                    query += " AND (ServiceCategory = @Category OR ServiceType = @Category)";
                }

                if (!string.IsNullOrEmpty(status) && status != "All")
                {
                    query += " AND Status = @Status";
                }

                query += " ORDER BY RequestId DESC";

                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    if (!string.IsNullOrEmpty(search))
                    {
                        cmd.Parameters.AddWithValue("@Search", "%" + search.Trim() + "%");
                    }

                    if (!string.IsNullOrEmpty(category) && category != "All")
                    {
                        cmd.Parameters.AddWithValue("@Category", category.Trim());
                    }

                    if (!string.IsNullOrEmpty(status) && status != "All")
                    {
                        cmd.Parameters.AddWithValue("@Status", status.Trim());
                    }

                    con.Open();
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            list.Add(MapReaderToServiceRequest(dr));
                        }
                    }
                }
            }

            return list;
        }

        // Get single request by ID
        public ServiceRequest GetById(int id)
        {
            ServiceRequest request = null;

            using (SqlConnection con = new SqlConnection(cs))
            {
                string query = "SELECT * FROM ServiceRequests WHERE RequestId = @RequestId";

                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@RequestId", id);

                    con.Open();
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        if (dr.Read())
                        {
                            request = MapReaderToServiceRequest(dr);
                        }
                    }
                }
            }

            return request;
        }

        // Insert new request (returns generated RequestId)
        public int Insert(ServiceRequest request)
        {
            using (SqlConnection con = new SqlConnection(cs))
            {
                string cat = !string.IsNullOrEmpty(request.ServiceCategory)
                    ? request.ServiceCategory
                    : (!string.IsNullOrEmpty(request.ServiceType) ? request.ServiceType : "IT Support");

                string title = !string.IsNullOrEmpty(request.RequestTitle)
                    ? request.RequestTitle
                    : (cat + " Request by " + request.StudentName);

                string email = !string.IsNullOrEmpty(request.StudentEmail)
                    ? request.StudentEmail
                    : "student@campus.edu";

                string query = @"
                    INSERT INTO ServiceRequests
                    (StudentName, StudentEmail, Department, ServiceCategory, ServiceType,
                     RequestTitle, Description, Priority, Status, RequestDate, AdminRemarks)
                    VALUES
                    (@StudentName, @StudentEmail, @Department, @ServiceCategory, @ServiceType,
                     @RequestTitle, @Description, @Priority, @Status, @RequestDate, @AdminRemarks);
                    SELECT CAST(SCOPE_IDENTITY() AS INT);";

                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@StudentName", request.StudentName);
                    cmd.Parameters.AddWithValue("@StudentEmail", email);
                    cmd.Parameters.AddWithValue("@Department", request.Department);
                    cmd.Parameters.AddWithValue("@ServiceCategory", cat);
                    cmd.Parameters.AddWithValue("@ServiceType", cat);
                    cmd.Parameters.AddWithValue("@RequestTitle", title);
                    cmd.Parameters.AddWithValue("@Description", request.Description);
                    cmd.Parameters.AddWithValue("@Priority", string.IsNullOrEmpty(request.Priority) ? "Medium" : request.Priority);
                    cmd.Parameters.AddWithValue("@Status", string.IsNullOrEmpty(request.Status) ? "Pending" : request.Status);
                    cmd.Parameters.AddWithValue("@RequestDate", request.RequestDate == default(DateTime) ? DateTime.Now : request.RequestDate);
                    cmd.Parameters.AddWithValue("@AdminRemarks", (object)request.AdminRemarks ?? DBNull.Value);

                    con.Open();
                    object result = cmd.ExecuteScalar();
                    if (result != null && result != DBNull.Value)
                    {
                        request.RequestId = Convert.ToInt32(result);
                    }
                }
            }

            return request.RequestId;
        }

        // Update full request details
        public bool Update(ServiceRequest request)
        {
            using (SqlConnection con = new SqlConnection(cs))
            {
                string cat = !string.IsNullOrEmpty(request.ServiceCategory)
                    ? request.ServiceCategory
                    : (!string.IsNullOrEmpty(request.ServiceType) ? request.ServiceType : "IT Support");

                string title = !string.IsNullOrEmpty(request.RequestTitle)
                    ? request.RequestTitle
                    : (cat + " Request by " + request.StudentName);

                string email = !string.IsNullOrEmpty(request.StudentEmail)
                    ? request.StudentEmail
                    : "student@campus.edu";

                string query = @"
                    UPDATE ServiceRequests
                    SET StudentName = @StudentName,
                        StudentEmail = @StudentEmail,
                        Department = @Department,
                        ServiceCategory = @ServiceCategory,
                        ServiceType = @ServiceType,
                        RequestTitle = @RequestTitle,
                        Description = @Description,
                        Priority = @Priority,
                        Status = @Status,
                        AdminRemarks = @AdminRemarks
                    WHERE RequestId = @RequestId";

                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@RequestId", request.RequestId);
                    cmd.Parameters.AddWithValue("@StudentName", request.StudentName);
                    cmd.Parameters.AddWithValue("@StudentEmail", email);
                    cmd.Parameters.AddWithValue("@Department", request.Department);
                    cmd.Parameters.AddWithValue("@ServiceCategory", cat);
                    cmd.Parameters.AddWithValue("@ServiceType", cat);
                    cmd.Parameters.AddWithValue("@RequestTitle", title);
                    cmd.Parameters.AddWithValue("@Description", request.Description);
                    cmd.Parameters.AddWithValue("@Priority", string.IsNullOrEmpty(request.Priority) ? "Medium" : request.Priority);
                    cmd.Parameters.AddWithValue("@Status", string.IsNullOrEmpty(request.Status) ? "Pending" : request.Status);
                    cmd.Parameters.AddWithValue("@AdminRemarks", (object)request.AdminRemarks ?? DBNull.Value);

                    con.Open();
                    int rows = cmd.ExecuteNonQuery();
                    return rows > 0;
                }
            }
        }

        // Update request status (Preserves exact method signature from reference project)
        public void UpdateStatus(int id, string status)
        {
            UpdateStatus(id, status, null);
        }

        // Overload to update status with optional AdminRemarks
        public bool UpdateStatus(int id, string status, string adminRemarks)
        {
            using (SqlConnection con = new SqlConnection(cs))
            {
                string query = @"
                    UPDATE ServiceRequests
                    SET Status = @Status,
                        AdminRemarks = CASE WHEN @AdminRemarks IS NOT NULL THEN @AdminRemarks ELSE AdminRemarks END
                    WHERE RequestId = @RequestId";

                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@Status", status);
                    cmd.Parameters.AddWithValue("@AdminRemarks", (object)adminRemarks ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@RequestId", id);

                    con.Open();
                    int rows = cmd.ExecuteNonQuery();
                    return rows > 0;
                }
            }
        }

        // Delete request by ID
        public bool Delete(int id)
        {
            using (SqlConnection con = new SqlConnection(cs))
            {
                string query = "DELETE FROM ServiceRequests WHERE RequestId = @RequestId";

                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@RequestId", id);

                    con.Open();
                    int rows = cmd.ExecuteNonQuery();
                    return rows > 0;
                }
            }
        }

        // Get Dashboard Statistics
        public DashboardViewModel GetDashboardStats()
        {
            DashboardViewModel model = new DashboardViewModel();
            List<ServiceRequest> all = GetAll();

            model.TotalRequests = all.Count;
            model.RecentRequests = all.Count > 6 ? all.GetRange(0, 6) : all;

            foreach (var req in all)
            {
                // Status counts
                string st = req.Status ?? "Pending";
                if (!model.StatusCounts.ContainsKey(st))
                {
                    model.StatusCounts[st] = 0;
                }
                model.StatusCounts[st]++;

                if (string.Equals(st, "Pending", StringComparison.OrdinalIgnoreCase))
                {
                    model.PendingRequests++;
                }
                else if (string.Equals(st, "In Progress", StringComparison.OrdinalIgnoreCase))
                {
                    model.InProgressRequests++;
                }
                else if (string.Equals(st, "Completed", StringComparison.OrdinalIgnoreCase))
                {
                    model.CompletedRequests++;
                }
                else if (string.Equals(st, "Rejected", StringComparison.OrdinalIgnoreCase))
                {
                    model.RejectedRequests++;
                }

                // Category counts
                string cat = req.ServiceCategory ?? "Other Services";
                if (!model.CategoryCounts.ContainsKey(cat))
                {
                    model.CategoryCounts[cat] = 0;
                }
                model.CategoryCounts[cat]++;
            }

            return model;
        }

        // Helper method to safely read columns from SqlDataReader
        private ServiceRequest MapReaderToServiceRequest(SqlDataReader dr)
        {
            ServiceRequest req = new ServiceRequest();

            req.RequestId = Convert.ToInt32(dr["RequestId"]);
            req.StudentName = dr["StudentName"] != DBNull.Value ? dr["StudentName"].ToString() : "";
            req.Department = dr["Department"] != DBNull.Value ? dr["Department"].ToString() : "";
            req.Description = dr["Description"] != DBNull.Value ? dr["Description"].ToString() : "";
            req.Priority = dr["Priority"] != DBNull.Value ? dr["Priority"].ToString() : "Medium";
            req.Status = dr["Status"] != DBNull.Value ? dr["Status"].ToString() : "Pending";
            req.RequestDate = dr["RequestDate"] != DBNull.Value ? Convert.ToDateTime(dr["RequestDate"]) : DateTime.Now;

            // Safe checks for extended fields
            if (HasColumn(dr, "ServiceCategory") && dr["ServiceCategory"] != DBNull.Value)
            {
                req.ServiceCategory = dr["ServiceCategory"].ToString();
            }
            else if (HasColumn(dr, "ServiceType") && dr["ServiceType"] != DBNull.Value)
            {
                req.ServiceCategory = dr["ServiceType"].ToString();
            }
            else
            {
                req.ServiceCategory = "IT Support";
            }

            if (HasColumn(dr, "StudentEmail") && dr["StudentEmail"] != DBNull.Value)
            {
                req.StudentEmail = dr["StudentEmail"].ToString();
            }
            else
            {
                req.StudentEmail = "student@campus.edu";
            }

            if (HasColumn(dr, "RequestTitle") && dr["RequestTitle"] != DBNull.Value)
            {
                req.RequestTitle = dr["RequestTitle"].ToString();
            }
            else
            {
                req.RequestTitle = req.ServiceCategory + " Request";
            }

            if (HasColumn(dr, "AdminRemarks") && dr["AdminRemarks"] != DBNull.Value)
            {
                req.AdminRemarks = dr["AdminRemarks"].ToString();
            }

            return req;
        }

        private bool HasColumn(SqlDataReader reader, string columnName)
        {
            for (int i = 0; i < reader.FieldCount; i++)
            {
                if (string.Equals(reader.GetName(i), columnName, StringComparison.OrdinalIgnoreCase))
                {
                    return true;
                }
            }
            return false;
        }
    }
}
