using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace L02.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class StudentsController : ControllerBase
    {
        // GET /students
        [HttpGet]
        public IEnumerable<Student> Get()
        {
            return StudentRepo.Students;
        }

        // GET /students/5
        [HttpGet("{id}")]
        public Student Get(int id)
        {
            return StudentRepo.Students.FirstOrDefault(s => s.Id == id);
        }

        // POST /students
        [HttpPost]
        public string Post([FromBody] Student student)
        {
            int id = 0;
            StudentRepo.Students.ForEach(s => {
                if(s.Id != id) {
                    student.Id = id;
                    return;
                }
                else
                    id++;   
            });
            student.Id = id;
            StudentRepo.Students.Add(student);
            StudentRepo.Students.Sort((x,y) => x.Id - y.Id);

            return "Adaugat cu succes!";
        }

        // PUT /students
        [HttpPut]
        public String Put([FromBody] Student student)
        {
            int index;
            if(StudentRepo.Students.FirstOrDefault(s => s.Id == student.Id) != null) {
                index = StudentRepo.Students.FindIndex(stud => stud.Id == student.Id);
                StudentRepo.Students[index] = student;
                return "Editat cu succes";
            }
            
            return "Studentul nu exista";
        }

        // DELETE /students/5
        [HttpDelete("{id}")]
        public String Delete(int id)
        {
            if(StudentRepo.Students.FirstOrDefault(s => s.Id == id) != null)
                StudentRepo.Students.Remove(StudentRepo.Students.FirstOrDefault(s => s.Id == id));
            return "Sters cu succes!";
        }
    }
}
