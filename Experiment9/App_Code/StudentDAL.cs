using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Experiment9.Models;

namespace Experiment9.Data
{
    public static class StudentDAL
    {
        // 1. GridView Data Source: Supports Filtering, Paging and Dynamic Sorting
        public static DataTable GetStudentsDataTable(string search = null, string dept = null, string sortExp = "StudentID", string sortDir = "ASC")
        {
            var dt = new DataTable("Students");

            using (var conn = DatabaseHelper.GetConnection())
            {
                string sql = "SELECT StudentID, StudentName, Email, Department, Semester, CGPA, Skills, PlacementStatus, CreatedDate FROM Students WHERE 1=1";

                using (var cmd = new SqlCommand())
                {
                    cmd.Connection = conn;

                    if (!string.IsNullOrWhiteSpace(search))
                    {
                        sql += " AND (StudentName LIKE @Search OR Email LIKE @Search OR Skills LIKE @Search)";
                        cmd.Parameters.Add("@Search", SqlDbType.NVarChar, 100).Value = "%" + search.Trim() + "%";
                    }

                    if (!string.IsNullOrWhiteSpace(dept) && dept != "All")
                    {
                        sql += " AND Department = @Department";
                        cmd.Parameters.Add("@Department", SqlDbType.NVarChar, 50).Value = dept.Trim();
                    }

                    // Safe Sorting whitelist
                    string safeSort = "StudentID";
                    switch (sortExp)
                    {
                        case "StudentName": safeSort = "StudentName"; break;
                        case "Email": safeSort = "Email"; break;
                        case "Department": safeSort = "Department"; break;
                        case "Semester": safeSort = "Semester"; break;
                        case "CGPA": safeSort = "CGPA"; break;
                        case "PlacementStatus": safeSort = "PlacementStatus"; break;
                        default: safeSort = "StudentID"; break;
                    }
                    string safeDir = sortDir.ToUpper() == "DESC" ? "DESC" : "ASC";
                    sql += string.Format(" ORDER BY {0} {1}", safeSort, safeDir);

                    cmd.CommandText = sql;
                    using (var adapter = new SqlDataAdapter(cmd))
                    {
                        adapter.Fill(dt);
                    }
                }
            }

            return dt;
        }

        // 2. DetailsView / FormView: Fetch Single Record by StudentID
        public static DataTable GetStudentByIdDataTable(int studentId)
        {
            var dt = new DataTable("StudentDetails");
            using (var conn = DatabaseHelper.GetConnection())
            {
                string sql = "SELECT StudentID, StudentName, Email, Department, Semester, CGPA, Skills, PlacementStatus, CreatedDate FROM Students WHERE StudentID = @StudentID;";
                using (var cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.Add("@StudentID", SqlDbType.Int).Value = studentId;
                    using (var adapter = new SqlDataAdapter(cmd))
                    {
                        adapter.Fill(dt);
                    }
                }
            }
            return dt;
        }

        // 3. FormView / Add Student: Parameterized INSERT returning new StudentID
        public static int InsertStudent(Student student)
        {
            using (var conn = DatabaseHelper.GetConnection())
            {
                string sql = @"
                    INSERT INTO Students (StudentName, Email, Department, Semester, CGPA, Skills, PlacementStatus, CreatedDate)
                    VALUES (@StudentName, @Email, @Department, @Semester, @CGPA, @Skills, @PlacementStatus, GETDATE());
                    SELECT CAST(SCOPE_IDENTITY() AS INT);";

                using (var cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.Add("@StudentName", SqlDbType.NVarChar, 100).Value = student.StudentName.Trim();
                    cmd.Parameters.Add("@Email", SqlDbType.NVarChar, 100).Value = student.Email.Trim();
                    cmd.Parameters.Add("@Department", SqlDbType.NVarChar, 50).Value = student.Department.Trim();
                    cmd.Parameters.Add("@Semester", SqlDbType.Int).Value = student.Semester;
                    cmd.Parameters.Add("@CGPA", SqlDbType.Decimal).Value = student.CGPA;
                    cmd.Parameters.Add("@Skills", SqlDbType.NVarChar, 250).Value = (object)student.Skills ?? DBNull.Value;
                    cmd.Parameters.Add("@PlacementStatus", SqlDbType.NVarChar, 20).Value = string.IsNullOrWhiteSpace(student.PlacementStatus) ? "Not Placed" : student.PlacementStatus;

                    conn.Open();
                    object result = cmd.ExecuteScalar();
                    return Convert.ToInt32(result);
                }
            }
        }

        // 4. GridView / FormView: Parameterized UPDATE
        public static bool UpdateStudent(Student student)
        {
            using (var conn = DatabaseHelper.GetConnection())
            {
                string sql = @"
                    UPDATE Students 
                    SET StudentName = @StudentName,
                        Email = @Email,
                        Department = @Department,
                        Semester = @Semester,
                        CGPA = @CGPA,
                        Skills = @Skills,
                        PlacementStatus = @PlacementStatus
                    WHERE StudentID = @StudentID;";

                using (var cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.Add("@StudentID", SqlDbType.Int).Value = student.StudentID;
                    cmd.Parameters.Add("@StudentName", SqlDbType.NVarChar, 100).Value = student.StudentName.Trim();
                    cmd.Parameters.Add("@Email", SqlDbType.NVarChar, 100).Value = student.Email.Trim();
                    cmd.Parameters.Add("@Department", SqlDbType.NVarChar, 50).Value = student.Department.Trim();
                    cmd.Parameters.Add("@Semester", SqlDbType.Int).Value = student.Semester;
                    cmd.Parameters.Add("@CGPA", SqlDbType.Decimal).Value = student.CGPA;
                    cmd.Parameters.Add("@Skills", SqlDbType.NVarChar, 250).Value = (object)student.Skills ?? DBNull.Value;
                    cmd.Parameters.Add("@PlacementStatus", SqlDbType.NVarChar, 20).Value = student.PlacementStatus;

                    conn.Open();
                    int rows = cmd.ExecuteNonQuery();
                    return rows > 0;
                }
            }
        }

        // 5. GridView: Parameterized DELETE
        public static bool DeleteStudent(int studentId)
        {
            using (var conn = DatabaseHelper.GetConnection())
            {
                string sql = "DELETE FROM Students WHERE StudentID = @StudentID;";
                using (var cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.Add("@StudentID", SqlDbType.Int).Value = studentId;
                    conn.Open();
                    int rows = cmd.ExecuteNonQuery();
                    return rows > 0;
                }
            }
        }

        public static List<string> GetDepartments()
        {
            var list = new List<string>();
            using (var conn = DatabaseHelper.GetConnection())
            {
                string sql = "SELECT DISTINCT Department FROM Students ORDER BY Department;";
                using (var cmd = new SqlCommand(sql, conn))
                {
                    conn.Open();
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            list.Add(reader.GetString(0));
                        }
                    }
                }
            }
            return list;
        }
    }
}

