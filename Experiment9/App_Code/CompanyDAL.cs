using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Experiment9.Models;

namespace Experiment9.Data
{
    public static class CompanyDAL
    {
        // DataList Data Source: Renders companies in card layout
        public static DataTable GetAllCompaniesDataTable(string search = null)
        {
            var dt = new DataTable("Companies");
            using (var conn = DatabaseHelper.GetConnection())
            {
                string sql = "SELECT CompanyID, CompanyName, Location, JobRole, PackageLPA, EligibilityCriteria, VisitDate, CreatedDate FROM Companies WHERE 1=1";
                using (var cmd = new SqlCommand())
                {
                    cmd.Connection = conn;
                    if (!string.IsNullOrWhiteSpace(search))
                    {
                        sql += " AND (CompanyName LIKE @Search OR Location LIKE @Search OR JobRole LIKE @Search)";
                        cmd.Parameters.Add("@Search", SqlDbType.NVarChar, 100).Value = "%" + search.Trim() + "%";
                    }

                    sql += " ORDER BY PackageLPA DESC, CompanyName ASC;";
                    cmd.CommandText = sql;

                    using (var adapter = new SqlDataAdapter(cmd))
                    {
                        adapter.Fill(dt);
                    }
                }
            }
            return dt;
        }

        public static int InsertCompany(Company company)
        {
            using (var conn = DatabaseHelper.GetConnection())
            {
                string sql = @"
                    INSERT INTO Companies (CompanyName, Location, JobRole, PackageLPA, EligibilityCriteria, VisitDate, CreatedDate)
                    VALUES (@CompanyName, @Location, @JobRole, @PackageLPA, @EligibilityCriteria, @VisitDate, GETDATE());
                    SELECT CAST(SCOPE_IDENTITY() AS INT);";

                using (var cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.Add("@CompanyName", SqlDbType.NVarChar, 100).Value = company.CompanyName.Trim();
                    cmd.Parameters.Add("@Location", SqlDbType.NVarChar, 100).Value = company.Location.Trim();
                    cmd.Parameters.Add("@JobRole", SqlDbType.NVarChar, 100).Value = company.JobRole.Trim();
                    cmd.Parameters.Add("@PackageLPA", SqlDbType.Decimal).Value = company.PackageLPA;
                    cmd.Parameters.Add("@EligibilityCriteria", SqlDbType.NVarChar, 250).Value = company.EligibilityCriteria.Trim();
                    cmd.Parameters.Add("@VisitDate", SqlDbType.Date).Value = company.VisitDate;

                    conn.Open();
                    object result = cmd.ExecuteScalar();
                    return Convert.ToInt32(result);
                }
            }
        }

        public static bool DeleteCompany(int companyId)
        {
            using (var conn = DatabaseHelper.GetConnection())
            {
                string sql = "DELETE FROM Companies WHERE CompanyID = @CompanyID;";
                using (var cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = companyId;
                    conn.Open();
                    return cmd.ExecuteNonQuery() > 0;
                }
            }
        }
    }
}

