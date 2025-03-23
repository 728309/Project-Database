using C__and_Project.Models;
using Microsoft.AspNetCore.Mvc;

namespace C__and_Project.Repositories
{
    public interface IStudentRepository
    {
        List<Student> GetAllStudents();
        Student? GetStudentByID(int studentID);
        void AddStudent(Student student);
        void UpdateStudent(Student student);
        void DeleteStudent(Student student);
    }
}
