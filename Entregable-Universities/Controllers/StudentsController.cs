using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Entregable_Universities.Models;
using Microsoft.EntityFrameworkCore.Migrations.Operations;
using Microsoft.DotNet.Scaffolding.Shared.Messaging;

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

        [HttpGet("GetAllListStudents")]
        public async Task<ActionResult<IEnumerable<StudentModel>>> GetStudents()
        {
            return await _context.Students.ToListAsync();
        }

        [HttpGet("GetStudentById/{id}")]
        public async Task<ActionResult<StudentModel>> GetStudentModel(int id)
        {
            var studentModel = await _context.Students.FindAsync(id);

            if (studentModel == null)
            {
                return NotFound();
            }

            return studentModel;
        }

        [HttpPut("UpdateStudent")]
        public async Task<IActionResult> PutStudentModel(StudentModel studentModel)
        {
            if (studentModel == null || studentModel.Id <= 0)
            {
                return BadRequest("Debe ingresar el Id o el Id enviado no es valido");
            }
            var existingStudent = await _context.Students.FindAsync(studentModel.Id);
            if (existingStudent == null)
            {
                return NotFound($"No se encontró un estudiante con ID: {studentModel.Id}");
            }
            _context.Entry(existingStudent).CurrentValues.SetValues(studentModel);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                return StatusCode(500, "Error de concurrencia al actualizar el estudiante.");
            }
            var data = await _context.Students.FindAsync(studentModel.Id);
            return Ok(data);
        }
        [HttpPost("CreateStudent")]
        public async Task<ActionResult<StudentModel>> PostStudentModel(StudentModel studentModel)
        {
            _context.Students.Add(studentModel);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetStudentModel", new { id = studentModel.Id }, studentModel);
        }

        [HttpDelete("DeleteStudentById/{id}")]
        public async Task<IActionResult> DeleteStudentModel(int id)
        {
            var studentModel = await _context.Students.FindAsync(id);
            if (studentModel == null)
            {
                return NotFound(new {message = $"No se encontró un estudiante con ID: {id}" });
            }
            _context.Students.Remove(studentModel);
            await _context.SaveChangesAsync();
            return Ok(studentModel);
        }
    }
}
