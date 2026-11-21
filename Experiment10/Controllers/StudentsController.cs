using Microsoft.AspNetCore.Mvc;
using StudentManagementAPI.Data;
using StudentManagementAPI.Models;

namespace StudentManagementAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Produces("application/json")]
    public class StudentsController : ControllerBase
    {
        private readonly IStudentRepository _repository;
        private readonly ILogger<StudentsController> _logger;

        public StudentsController(IStudentRepository repository, ILogger<StudentsController> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        /// <summary>
        /// Retrieves all registered students.
        /// </summary>
        /// <response code="200">Returns list of all students.</response>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<Student>))]
        public async Task<IActionResult> GetStudents()
        {
            _logger.LogInformation("Fetching all students from database.");
            var students = await _repository.GetAllStudentsAsync();
            return Ok(students);
        }

        /// <summary>
        /// Retrieves a specific student by ID.
        /// </summary>
        /// <param name="id">Student ID.</param>
        /// <response code="200">Returns the matching student record.</response>
        /// <response code="404">Student with the specified ID was not found.</response>
        [HttpGet("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Student))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetStudent(int id)
        {
            _logger.LogInformation("Fetching student with ID {Id}.", id);
            var student = await _repository.GetStudentByIdAsync(id);

            if (student == null)
            {
                _logger.LogWarning("Student with ID {Id} not found.", id);
                return NotFound(new { message = $"Student with ID {id} was not found." });
            }

            return Ok(student);
        }

        /// <summary>
        /// Registers a new student record.
        /// </summary>
        /// <param name="student">Student creation data.</param>
        /// <response code="201">Student created successfully.</response>
        /// <response code="400">Invalid model data.</response>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(Student))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> AddStudent([FromBody] Student student)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _logger.LogInformation("Creating new student record for {FullName}.", student.FullName);
            int newId = await _repository.AddStudentAsync(student);

            return CreatedAtAction(nameof(GetStudent), new { id = newId }, student);
        }

        /// <summary>
        /// Updates an existing student record by ID.
        /// </summary>
        /// <param name="id">Student ID.</param>
        /// <param name="student">Updated student information.</param>
        /// <response code="204">Student updated successfully.</response>
        /// <response code="400">ID mismatch or invalid model.</response>
        /// <response code="404">Student not found.</response>
        [HttpPut("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateStudent(int id, [FromBody] Student student)
        {
            if (student.StudentId != 0 && student.StudentId != id)
            {
                return BadRequest(new { message = "Path ID and request body StudentId do not match." });
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _logger.LogInformation("Updating student record for ID {Id}.", id);
            bool updated = await _repository.UpdateStudentAsync(id, student);

            if (!updated)
            {
                _logger.LogWarning("Update failed. Student with ID {Id} not found.", id);
                return NotFound(new { message = $"Student with ID {id} was not found." });
            }

            return NoContent();
        }

        /// <summary>
        /// Deletes a student record by ID.
        /// </summary>
        /// <param name="id">Student ID.</param>
        /// <response code="204">Student deleted successfully.</response>
        /// <response code="404">Student not found.</response>
        [HttpDelete("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteStudent(int id)
        {
            _logger.LogInformation("Deleting student record for ID {Id}.", id);
            bool deleted = await _repository.DeleteStudentAsync(id);

            if (!deleted)
            {
                _logger.LogWarning("Delete failed. Student with ID {Id} not found.", id);
                return NotFound(new { message = $"Student with ID {id} was not found." });
            }

            return NoContent();
        }
    }
}
