using System;
using System.Data;
using System.Data.SqlClient;
using Experiment9.Models;

namespace Experiment9.Data
{
    public static class PlacementDAL
    {
        // 1. Dashboard KPI Aggregates
        public static DashboardStats GetDashboardStats()
        {
            var stats = new DashboardStats();

            using (var conn = DatabaseHelper.GetConnection())
            {
                string sql = @"
                    SELECT 
                        (SELECT COUNT(*) FROM Students) AS TotalStudents,
                        (SELECT COUNT(*) FROM Companies) AS TotalCompanies,
                        (SELECT COUNT(*) FROM Students WHERE PlacementStatus = 'Placed') AS StudentsPlaced,
                        (SELECT COUNT(*) FROM Students WHERE PlacementStatus = 'Not Placed') AS StudentsNotPlaced,
                        (SELECT COUNT(*) FROM Placements) AS TotalPlacementOffers,
                        (SELECT ISNULL(MAX(PackageOffered), 0.0) FROM Placements) AS HighestPackageLPA,
                        (SELECT ISNULL(AVG(PackageOffered), 0.0) FROM Placements) AS AveragePackageLPA;";

                using (var cmd = new SqlCommand(sql, conn))
                {
                    conn.Open();
                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            stats.TotalStudents = Convert.ToInt32(reader["TotalStudents"]);
                            stats.TotalCompanies = Convert.ToInt32(reader["TotalCompanies"]);
                            stats.StudentsPlaced = Convert.ToInt32(reader["StudentsPlaced"]);
                            stats.StudentsNotPlaced = Convert.ToInt32(reader["StudentsNotPlaced"]);
                            stats.TotalPlacementOffers = Convert.ToInt32(reader["TotalPlacementOffers"]);
                            stats.HighestPackageLPA = Convert.ToDecimal(reader["HighestPackageLPA"]);
                            stats.AveragePackageLPA = Convert.ToDecimal(reader["AveragePackageLPA"]);
                        }
                    }
                }
            }

            return stats;
        }

        // 2. Repeater Control: Recent Placement Activities
        public static DataTable GetRecentPlacementsDataTable(int top = 6)
        {
            var dt = new DataTable("RecentPlacements");

            using (var conn = DatabaseHelper.GetConnection())
            {
                string sql = string.Format(@"
                    SELECT TOP {0} 
                        p.PlacementID,
                        s.StudentID,
                        s.StudentName,
                        s.Department,
                        c.CompanyID,
                        c.CompanyName,
                        c.Location,
                        p.OfferDate,
                        p.PackageOffered,
                        p.Designation,
                        p.Status
                    FROM Placements p
                    INNER JOIN Students s ON p.StudentID = s.StudentID
                    INNER JOIN Companies c ON p.CompanyID = c.CompanyID
                    ORDER BY p.OfferDate DESC, p.PlacementID DESC;", top);

                using (var cmd = new SqlCommand(sql, conn))
                {
                    using (var adapter = new SqlDataAdapter(cmd))
                    {
                        adapter.Fill(dt);
                    }
                }
            }

            return dt;
        }

        // 3. Placements Management Table
        public static DataTable GetAllPlacementsDataTable()
        {
            var dt = new DataTable("AllPlacements");

            using (var conn = DatabaseHelper.GetConnection())
            {
                string sql = @"
                    SELECT 
                        p.PlacementID,
                        s.StudentID,
                        s.StudentName,
                        s.Department,
                        c.CompanyID,
                        c.CompanyName,
                        p.OfferDate,
                        p.PackageOffered,
                        p.Designation,
                        p.Status
                    FROM Placements p
                    INNER JOIN Students s ON p.StudentID = s.StudentID
                    INNER JOIN Companies c ON p.CompanyID = c.CompanyID
                    ORDER BY p.PlacementID DESC;";

                using (var cmd = new SqlCommand(sql, conn))
                {
                    using (var adapter = new SqlDataAdapter(cmd))
                    {
                        adapter.Fill(dt);
                    }
                }
            }

            return dt;
        }

        // 4. Record new placement & automatically update Student's PlacementStatus
        public static int RecordPlacement(int studentId, int companyId, decimal package, string designation, string status)
        {
            using (var conn = DatabaseHelper.GetConnection())
            {
                conn.Open();
                using (var trans = conn.BeginTransaction())
                {
                    try
                    {
                        string insertSql = @"
                            INSERT INTO Placements (StudentID, CompanyID, OfferDate, PackageOffered, Designation, Status)
                            VALUES (@StudentID, @CompanyID, GETDATE(), @PackageOffered, @Designation, @Status);
                            SELECT CAST(SCOPE_IDENTITY() AS INT);";

                        int newId;
                        using (var cmd = new SqlCommand(insertSql, conn, trans))
                        {
                            cmd.Parameters.Add("@StudentID", SqlDbType.Int).Value = studentId;
                            cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = companyId;
                            cmd.Parameters.Add("@PackageOffered", SqlDbType.Decimal).Value = package;
                            cmd.Parameters.Add("@Designation", SqlDbType.NVarChar, 100).Value = designation.Trim();
                            cmd.Parameters.Add("@Status", SqlDbType.NVarChar, 20).Value = status.Trim();

                            newId = Convert.ToInt32(cmd.ExecuteScalar());
                        }

                        // Update Student PlacementStatus to 'Placed'
                        string updateStudentSql = "UPDATE Students SET PlacementStatus = 'Placed' WHERE StudentID = @StudentID;";
                        using (var updateCmd = new SqlCommand(updateStudentSql, conn, trans))
                        {
                            updateCmd.Parameters.Add("@StudentID", SqlDbType.Int).Value = studentId;
                            updateCmd.ExecuteNonQuery();
                        }

                        trans.Commit();
                        return newId;
                    }
                    catch
                    {
                        trans.Rollback();
                        throw;
                    }
                }
            }
        }

        public static bool DeletePlacement(int placementId)
        {
            using (var conn = DatabaseHelper.GetConnection())
            {
                string sql = "DELETE FROM Placements WHERE PlacementID = @PlacementID;";
                using (var cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.Add("@PlacementID", SqlDbType.Int).Value = placementId;
                    conn.Open();
                    return cmd.ExecuteNonQuery() > 0;
                }
            }
        }
    }
}

