using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Entregable_Universities.Models;

namespace Entregable_Universities.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public StudentsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/GetAllListStudents
        [HttpGet("GetAllListStudents")]
        public async Task<ActionResult<IEnumerable<StudentModel>>> GetStudents()
        {
            return await _context.Students.ToListAsync();
        }

        // GET: api/Students/5
        [HttpGet("GetDetailStudent/{id}")]
        public async Task<ActionResult<StudentModel>> GetStudentModel(int id)
        {
            var studentModel = await _context.Students.FindAsync(id);

            if (studentModel == null)
            {
                return NotFound();
            }

            return studentModel;
        }

        // PUT: api/Students/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("UpdateStudent")]
        public async Task<IActionResult> PutStudentModel(StudentModel studentModel)
        {
            if (studentModel == null || studentModel.Id <= 0)
            {
                return BadRequest("Debe ingresar el Id o el Id enviado no es valido");
            }

            // Buscar el estudiante existente en la base de datos
            var existingStudent = await _context.Students.FindAsync(studentModel.Id);
            if (existingStudent == null)
            {
                return NotFound($"No se encontró un estudiante con ID: {studentModel.Id}");
            }

            // Actualizar las propiedades del estudiante existente
            _context.Entry(existingStudent).CurrentValues.SetValues(studentModel);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                return StatusCode(500, "Error de concurrencia al actualizar el estudiante.");
            }

            return NoContent();
        }


        // POST: api/Students
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost("CreateStudent")]
        public async Task<ActionResult<StudentModel>> PostStudentModel(StudentModel studentModel)
        {
            _context.Students.Add(studentModel);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetStudentModel", new { id = studentModel.Id }, studentModel);
        }

        // DELETE: api/Students/5
        [HttpDelete("DeleteStudentById/{id}")]
        public async Task<IActionResult> DeleteStudentModel(int id)
        {
            var studentModel = await _context.Students.FindAsync(id);
            if (studentModel == null)
            {
                return NotFound();
            }

            _context.Students.Remove(studentModel);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool StudentModelExists(int id)
        {
            return _context.Students.Any(e => e.Id == id);
        }
    }
}
