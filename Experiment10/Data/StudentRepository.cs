using System.Data;
using Microsoft.Data.SqlClient;
using StudentManagementAPI.Models;

namespace StudentManagementAPI.Data
{
    public class StudentRepository : IStudentRepository
    {
        private readonly string _connectionString;
        private readonly ILogger<StudentRepository> _logger;

        public StudentRepository(IConfiguration configuration, ILogger<StudentRepository> logger)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection")
                ?? throw new InvalidOperationException("Connection string 'DefaultConnection' was not found.");
            _logger = logger;
        }

        private SqlConnection CreateConnection() => new SqlConnection(_connectionString);

        public async Task<IEnumerable<Student>> GetAllStudentsAsync()
        {
            var students = new List<Student>();
            const string query = @"
                SELECT StudentId, FullName, Email, PhoneNumber, Department, Semester, CGPA, Address, CreatedAt
                FROM Students
                ORDER BY StudentId ASC;";

            using var connection = CreateConnection();
            using var command = new SqlCommand(query, connection);

            await connection.OpenAsync();
            using var reader = await command.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                students.Add(MapStudent(reader));
            }

            return students;
        }

        public async Task<Student?> GetStudentByIdAsync(int id)
        {
            const string query = @"
                SELECT StudentId, FullName, Email, PhoneNumber, Department, Semester, CGPA, Address, CreatedAt
                FROM Students
                WHERE StudentId = @StudentId;";

            using var connection = CreateConnection();
            using var command = new SqlCommand(query, connection);
            command.Parameters.Add("@StudentId", SqlDbType.Int).Value = id;

            await connection.OpenAsync();
            using var reader = await command.ExecuteReaderAsync();

            if (await reader.ReadAsync())
            {
                return MapStudent(reader);
            }

            return null;
        }

        public async Task<int> AddStudentAsync(Student student)
        {
            const string query = @"
                INSERT INTO Students (FullName, Email, PhoneNumber, Department, Semester, CGPA, Address, CreatedAt)
                VALUES (@FullName, @Email, @PhoneNumber, @Department, @Semester, @CGPA, @Address, @CreatedAt);
                SELECT CAST(SCOPE_IDENTITY() AS INT);";

            using var connection = CreateConnection();
            using var command = new SqlCommand(query, connection);

            command.Parameters.Add("@FullName", SqlDbType.NVarChar, 100).Value = student.FullName;
            command.Parameters.Add("@Email", SqlDbType.NVarChar, 100).Value = student.Email;
            command.Parameters.Add("@PhoneNumber", SqlDbType.NVarChar, 20).Value = student.PhoneNumber;
            command.Parameters.Add("@Department", SqlDbType.NVarChar, 50).Value = student.Department;
            command.Parameters.Add("@Semester", SqlDbType.Int).Value = student.Semester;
            command.Parameters.Add("@CGPA", SqlDbType.Decimal).Value = student.CGPA;
            command.Parameters.Add("@Address", SqlDbType.NVarChar, 250).Value = (object?)student.Address ?? DBNull.Value;
            command.Parameters.Add("@CreatedAt", SqlDbType.DateTime).Value = student.CreatedAt == default ? DateTime.UtcNow : student.CreatedAt;

            await connection.OpenAsync();
            var result = await command.ExecuteScalarAsync();

            student.StudentId = Convert.ToInt32(result);
            return student.StudentId;
        }

        public async Task<bool> UpdateStudentAsync(int id, Student student)
        {
            const string query = @"
                UPDATE Students
                SET FullName = @FullName,
                    Email = @Email,
                    PhoneNumber = @PhoneNumber,
                    Department = @Department,
                    Semester = @Semester,
                    CGPA = @CGPA,
                    Address = @Address
                WHERE StudentId = @StudentId;";

            using var connection = CreateConnection();
            using var command = new SqlCommand(query, connection);

            command.Parameters.Add("@StudentId", SqlDbType.Int).Value = id;
            command.Parameters.Add("@FullName", SqlDbType.NVarChar, 100).Value = student.FullName;
            command.Parameters.Add("@Email", SqlDbType.NVarChar, 100).Value = student.Email;
            command.Parameters.Add("@PhoneNumber", SqlDbType.NVarChar, 20).Value = student.PhoneNumber;
            command.Parameters.Add("@Department", SqlDbType.NVarChar, 50).Value = student.Department;
            command.Parameters.Add("@Semester", SqlDbType.Int).Value = student.Semester;
            command.Parameters.Add("@CGPA", SqlDbType.Decimal).Value = student.CGPA;
            command.Parameters.Add("@Address", SqlDbType.NVarChar, 250).Value = (object?)student.Address ?? DBNull.Value;

            await connection.OpenAsync();
            int rowsAffected = await command.ExecuteNonQueryAsync();

            return rowsAffected > 0;
        }

        public async Task<bool> DeleteStudentAsync(int id)
        {
            const string query = "DELETE FROM Students WHERE StudentId = @StudentId;";

            using var connection = CreateConnection();
            using var command = new SqlCommand(query, connection);
            command.Parameters.Add("@StudentId", SqlDbType.Int).Value = id;

            await connection.OpenAsync();
            int rowsAffected = await command.ExecuteNonQueryAsync();

            return rowsAffected > 0;
        }

        private static Student MapStudent(SqlDataReader reader)
        {
            return new Student
            {
                StudentId = reader.GetInt32(reader.GetOrdinal("StudentId")),
                FullName = reader.GetString(reader.GetOrdinal("FullName")),
                Email = reader.GetString(reader.GetOrdinal("Email")),
                PhoneNumber = reader.GetString(reader.GetOrdinal("PhoneNumber")),
                Department = reader.GetString(reader.GetOrdinal("Department")),
                Semester = reader.GetInt32(reader.GetOrdinal("Semester")),
                CGPA = reader.GetDecimal(reader.GetOrdinal("CGPA")),
                Address = reader.IsDBNull(reader.GetOrdinal("Address")) ? null : reader.GetString(reader.GetOrdinal("Address")),
                CreatedAt = reader.IsDBNull(reader.GetOrdinal("CreatedAt")) ? DateTime.UtcNow : reader.GetDateTime(reader.GetOrdinal("CreatedAt"))
            };
        }
    }
}
