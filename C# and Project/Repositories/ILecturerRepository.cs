using C__and_Project.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace C__and_Project.Repositories
{
    public interface ILecturerRepository
    {
        List<Lecturer> GetAllLecturers();
        Lecturer? GetLecturerByLastName(string LastName);
        void AddLecturer(Lecturer lectuer);
        void UpdateLecturer(Lecturer lecturer);
        void DeleteLecturer(Lecturer lecturer);
    }
}