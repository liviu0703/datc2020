using System.Collections.Generic;

namespace L02
{
    public static class StudentRepo
    {
        public static List<Student> Students = new List<Student>() {
            new Student{Nume="Ionescu",Prenume="Ion",Facultate="AC",AnStudiu=2,Id=0},
            new Student{Nume="Popescu",Prenume="Dan",Facultate="CT",AnStudiu=1,Id=1}
        };
        
        
    }
}