using System.Collections.Generic;
using System.Threading.Tasks;
using L04;

public interface IStudentRepository
{
    Task<List<StudentEntity>> GetAllStudents();

    Task CreateNewStudent(StudentEntity student);

    Task EditStudent(StudentEntity student);

    Task DeleteStudent(StudentEntity student);
}