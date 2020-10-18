using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace L04.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class StudentsController : ControllerBase
    {

        private IStudentRepository _studentRepository;

        public StudentsController(IStudentRepository studentRepository)
        {
            _studentRepository = studentRepository;
        }

        // GET /students
        [HttpGet]
        public async Task<IEnumerable<StudentEntity>> Get()
        {
            return await _studentRepository.GetAllStudents();
        }


        // POST /students
        [HttpPost]
        public async Task<string> Post([FromBody] StudentEntity student)
        {
            try
            {
                await _studentRepository.CreateNewStudent(student);
                return "Adaugat cu succes!";
            }
            catch (System.Exception e)
            {
                return "Eroare: " + e.Message; 
            }

        }

        // PUT /students
        [HttpPut]
        public async Task<string> Put([FromBody] StudentEntity student)
        {
            try
            {
                await _studentRepository.EditStudent(student);
                return "Editat cu succes!";
            }
            catch (System.Exception e)
            {
                return "Eroare: " + e.Message; 
            }
        }

        // DELETE /students/5
        [HttpDelete]
        public async Task<string> Delete([FromBody] StudentEntity student)
        {
            try
            {
                await _studentRepository.DeleteStudent(student);
                return "Sters cu succes!";
            }
            catch (System.Exception e)
            {
                return "Eroare: " + e.Message; 
            }
        }
    }
}
